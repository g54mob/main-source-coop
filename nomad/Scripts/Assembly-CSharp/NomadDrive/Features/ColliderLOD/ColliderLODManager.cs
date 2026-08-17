using System.Collections.Generic;
using EvilCore.Extensions;
using EvilCore.Networking;
using Mirror;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Player;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.ColliderLOD
{
	public class ColliderLODManager : MonoBehaviour, IColliderLODManager, IFloatingOriginShiftable
	{
		private struct PlayerProximity
		{
			public Vector3 Position;

			public float VelocityMargin;
		}

		[SerializeField]
		private ColliderLodConfig config;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IPlayerService _playerService;

		private static ColliderLODManager _instance;

		private readonly Dictionary<IColliderLodTarget, ColliderLodEntry> _entries = new Dictionary<IColliderLodTarget, ColliderLodEntry>();

		private readonly HashSet<ColliderLodEntry> _activeEntries = new HashSet<ColliderLodEntry>();

		private readonly List<ColliderLodEntry> _movingEntries = new List<ColliderLodEntry>();

		private ColliderLodSpatialGrid _grid;

		private float _maxEntryReach;

		private readonly HashSet<ColliderLodEntry> _candidates = new HashSet<ColliderLodEntry>();

		private readonly List<ColliderLodEntry> _activeSnapshot = new List<ColliderLodEntry>();

		private readonly Queue<ColliderLodEntry> _disableQueue = new Queue<ColliderLodEntry>();

		private readonly List<PlayerProximity> _players = new List<PlayerProximity>();

		private readonly Dictionary<uint, Vector3> _prevPlayerPositions = new Dictionary<uint, Vector3>();

		private readonly HashSet<uint> _seenPlayerNetIds = new HashSet<uint>();

		private readonly List<uint> _prevPlayerKeysScratch = new List<uint>();

		private float _nextCycleTime;

		private bool _isEnabled = true;

		private int _activeCount;

		private int _lastLoggedActive = -1;

		public bool IsEnabled => _isEnabled;

		public int ManagedCount => _entries.Count;

		public int ActiveCount => _activeCount;

		public static ColliderLODManager Instance => _instance;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			_instance = this;
			_ = config == null;
			_isEnabled = config != null && config.masterEnabled;
			_grid = new ColliderLodSpatialGrid((config != null) ? config.gridCellSize : 12f);
		}

		private void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		private void OnEnable()
		{
			ColliderLodRegistry.OnRegistered += AddTarget;
			ColliderLodRegistry.OnUnregistered += RemoveTarget;
			FloatingOriginManager.RegisterShiftable(this);
			RebuildRegistry();
		}

		private void OnDisable()
		{
			ColliderLodRegistry.OnRegistered -= AddTarget;
			ColliderLodRegistry.OnUnregistered -= RemoveTarget;
			FloatingOriginManager.UnregisterShiftable(this);
			RebuildRegistry(clearOnly: true);
		}

		private void RebuildRegistry(bool clearOnly = false)
		{
			_entries.Clear();
			_activeEntries.Clear();
			_movingEntries.Clear();
			_disableQueue.Clear();
			_grid.Clear();
			_maxEntryReach = 0f;
			if (clearOnly)
			{
				return;
			}
			foreach (IColliderLodTarget target in ColliderLodRegistry.Targets)
			{
				AddTarget(target);
			}
		}

		private void AddTarget(IColliderLodTarget target)
		{
			if (target != null && target.ParticipatesInColliderLod && !_entries.ContainsKey(target))
			{
				ColliderLodEntry colliderLodEntry = new ColliderLodEntry(target);
				colliderLodEntry.ActivationRadius = ResolveActivationRadius(target);
				colliderLodEntry.DeactivationRadius = colliderLodEntry.ActivationRadius + ((config != null) ? config.hysteresisBuffer : 1.5f);
				_entries[target] = colliderLodEntry;
				colliderLodEntry.LodActive = true;
				_activeEntries.Add(colliderLodEntry);
				_grid.Insert(colliderLodEntry);
				if (target.LodPositionMayChange)
				{
					_movingEntries.Add(colliderLodEntry);
				}
				float num = colliderLodEntry.DeactivationRadius + colliderLodEntry.BoundsRadius;
				if (num > _maxEntryReach)
				{
					_maxEntryReach = num;
				}
			}
		}

		private float ResolveActivationRadius(IColliderLodTarget target)
		{
			float result = ((config != null) ? config.activationRadius : 5f);
			if (config == null || config.typeOverrides == null)
			{
				return result;
			}
			string text = target.GetType().Name;
			for (int i = 0; i < config.typeOverrides.Count; i++)
			{
				if (config.typeOverrides[i].componentTypeName == text)
				{
					return config.typeOverrides[i].activationRadius;
				}
			}
			return result;
		}

		private void RemoveTarget(IColliderLodTarget target)
		{
			if (target != null && _entries.TryGetValue(target, out var value))
			{
				value.Removed = true;
				_grid.Remove(value);
				_activeEntries.Remove(value);
				_movingEntries.Remove(value);
				_entries.Remove(target);
				if (!value.LodActive && value.IsAlive)
				{
					target.SetLodColliderActive(active: true);
				}
			}
		}

		public void OnOriginShift(Vector3 delta)
		{
			_grid.Clear();
			foreach (ColliderLodEntry value in _entries.Values)
			{
				if (value.IsAlive)
				{
					if (value.Transform != null)
					{
						value.LastBucketedPosition = value.Transform.position;
					}
					_grid.Insert(value);
				}
			}
			if (_prevPlayerPositions.Count > 0)
			{
				_prevPlayerKeysScratch.Clear();
				_prevPlayerKeysScratch.AddRange(_prevPlayerPositions.Keys);
				foreach (uint item in _prevPlayerKeysScratch)
				{
					_prevPlayerPositions[item] += delta;
				}
			}
			_nextCycleTime = 0f;
		}

		private void LateUpdate()
		{
			if (config == null)
			{
				return;
			}
			DrainDisableQueue();
			if (!(Time.unscaledTime < _nextCycleTime))
			{
				_nextCycleTime = Time.unscaledTime + config.evaluationInterval;
				if (_isEnabled)
				{
					RefreshPlayers();
					EvaluateCycle();
				}
			}
		}

		private void EvaluateCycle()
		{
			for (int i = 0; i < _movingEntries.Count; i++)
			{
				ColliderLodEntry colliderLodEntry = _movingEntries[i];
				if (colliderLodEntry.IsAlive)
				{
					Vector3 position = colliderLodEntry.Transform.position;
					if ((position - colliderLodEntry.LastBucketedPosition).sqrMagnitude > config.rebucketThreshold * config.rebucketThreshold)
					{
						colliderLodEntry.LastBucketedPosition = position;
						_grid.Rebucket(colliderLodEntry);
					}
				}
			}
			if (_players.Count == 0)
			{
				if (config.failSafeKeepActive)
				{
					ReenableAll();
				}
				return;
			}
			_candidates.Clear();
			float num = 0f;
			for (int j = 0; j < _players.Count; j++)
			{
				if (_players[j].VelocityMargin > num)
				{
					num = _players[j].VelocityMargin;
				}
			}
			for (int k = 0; k < _players.Count; k++)
			{
				float radius = _maxEntryReach + _players[k].VelocityMargin;
				_grid.QueryRing(_players[k].Position, radius, _candidates);
			}
			foreach (ColliderLodEntry candidate in _candidates)
			{
				if (candidate.IsAlive)
				{
					if (candidate.LodActive)
					{
						candidate.PendingDisable = false;
					}
					else if (IsInsideAnyEnableBand(candidate))
					{
						candidate.LodActive = true;
						candidate.PendingDisable = false;
						_activeEntries.Add(candidate);
						candidate.Target.SetLodColliderActive(active: true);
					}
				}
			}
			_activeSnapshot.Clear();
			_activeSnapshot.AddRange(_activeEntries);
			for (int l = 0; l < _activeSnapshot.Count; l++)
			{
				ColliderLodEntry colliderLodEntry2 = _activeSnapshot[l];
				if (!colliderLodEntry2.IsAlive)
				{
					continue;
				}
				if (IsOutsideAllDisableBands(colliderLodEntry2))
				{
					if (!colliderLodEntry2.PendingDisable)
					{
						colliderLodEntry2.PendingDisable = true;
						_disableQueue.Enqueue(colliderLodEntry2);
					}
				}
				else
				{
					colliderLodEntry2.PendingDisable = false;
				}
			}
			_activeCount = _activeEntries.Count;
			if (_activeCount != _lastLoggedActive)
			{
				_lastLoggedActive = _activeCount;
			}
		}

		private void ReenableAll()
		{
			foreach (ColliderLodEntry value in _entries.Values)
			{
				value.PendingDisable = false;
				if (!value.LodActive && value.IsAlive)
				{
					value.LodActive = true;
					_activeEntries.Add(value);
					value.Target.SetLodColliderActive(active: true);
				}
			}
			_activeCount = _activeEntries.Count;
		}

		private bool IsInsideAnyEnableBand(ColliderLodEntry entry)
		{
			Vector3 worldCenter = entry.WorldCenter;
			for (int i = 0; i < _players.Count; i++)
			{
				float num = entry.ActivationRadius + entry.BoundsRadius + _players[i].VelocityMargin;
				if ((_players[i].Position - worldCenter).sqrMagnitude <= num * num)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsOutsideAllDisableBands(ColliderLodEntry entry)
		{
			Vector3 worldCenter = entry.WorldCenter;
			for (int i = 0; i < _players.Count; i++)
			{
				float num = entry.DeactivationRadius + entry.BoundsRadius + _players[i].VelocityMargin;
				if ((_players[i].Position - worldCenter).sqrMagnitude <= num * num)
				{
					return false;
				}
			}
			return true;
		}

		private void DrainDisableQueue()
		{
			int num = ((config != null) ? config.maxDisableTogglesPerFrame : 50);
			int num2 = 0;
			while (num2 < num && _disableQueue.Count > 0)
			{
				ColliderLodEntry colliderLodEntry = _disableQueue.Dequeue();
				if (colliderLodEntry != null && !colliderLodEntry.Removed && colliderLodEntry.PendingDisable)
				{
					if (!colliderLodEntry.LodActive)
					{
						colliderLodEntry.PendingDisable = false;
						continue;
					}
					if (!colliderLodEntry.IsAlive)
					{
						colliderLodEntry.PendingDisable = false;
						continue;
					}
					colliderLodEntry.LodActive = false;
					colliderLodEntry.PendingDisable = false;
					_activeEntries.Remove(colliderLodEntry);
					colliderLodEntry.Target.SetLodColliderActive(active: false);
					num2++;
				}
			}
		}

		private void RefreshPlayers()
		{
			_players.Clear();
			_seenPlayerNetIds.Clear();
			if (_networkManager != null)
			{
				foreach (INetworkPlayer connectedPlayer in _networkManager.ConnectedPlayers)
				{
					if (connectedPlayer != null)
					{
						uint netId = connectedPlayer.NetId;
						if (_seenPlayerNetIds.Add(netId) && NetworkClient.spawned.TryGetValue(netId, out var value) && value != null)
						{
							AddPlayer(netId, value.transform.position);
						}
					}
				}
			}
			if (_playerService != null && _playerService.IsPlayerSpawned && _playerService.LocalPlayer != null)
			{
				uint netId2 = _playerService.LocalPlayer.netId;
				if (_seenPlayerNetIds.Add(netId2))
				{
					AddPlayer(netId2, _playerService.LocalPlayer.transform.position);
				}
			}
		}

		private void AddPlayer(uint netId, Vector3 position)
		{
			float num = 0f;
			if (_prevPlayerPositions.TryGetValue(netId, out var value))
			{
				float evaluationInterval = config.evaluationInterval;
				if (evaluationInterval > 0.0001f)
				{
					num = (position - value).magnitude / evaluationInterval;
				}
			}
			_prevPlayerPositions[netId] = position;
			_players.Add(new PlayerProximity
			{
				Position = position,
				VelocityMargin = num * config.velocityLookaheadSeconds
			});
		}

		public void SetEnabled(bool enabled)
		{
			if (_isEnabled != enabled)
			{
				_isEnabled = enabled;
				if (!_isEnabled)
				{
					_disableQueue.Clear();
					ReenableAll();
				}
				else
				{
					_nextCycleTime = 0f;
				}
			}
		}

		public string GetStatsString()
		{
			return $"managed={_entries.Count} active={_activeCount} players={_players.Count} enabled={_isEnabled}";
		}

		public void SetActivationRadiusRuntime(float radius)
		{
			if (config == null)
			{
				return;
			}
			config.activationRadius = Mathf.Max(0f, radius);
			_maxEntryReach = 0f;
			foreach (ColliderLodEntry value in _entries.Values)
			{
				value.ActivationRadius = ResolveActivationRadius(value.Target);
				value.DeactivationRadius = value.ActivationRadius + config.hysteresisBuffer;
				float num = value.DeactivationRadius + value.BoundsRadius;
				if (num > _maxEntryReach)
				{
					_maxEntryReach = num;
				}
			}
			_nextCycleTime = 0f;
		}

		private void OnDrawGizmos()
		{
			if (config == null || !config.drawGizmos || !Application.isPlaying)
			{
				return;
			}
			for (int i = 0; i < _players.Count; i++)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(_players[i].Position, config.activationRadius + _players[i].VelocityMargin);
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(_players[i].Position, config.DeactivationRadius + _players[i].VelocityMargin);
			}
			int num = 0;
			foreach (ColliderLodEntry value in _entries.Values)
			{
				if (num >= config.maxGizmoEntries)
				{
					break;
				}
				if (value.IsAlive)
				{
					Gizmos.color = (value.LodActive ? Color.cyan : new Color(1f, 0.3f, 0.3f, 0.5f));
					Gizmos.DrawWireSphere(value.WorldCenter, Mathf.Max(0.2f, value.BoundsRadius));
					num++;
				}
			}
		}
	}
}
