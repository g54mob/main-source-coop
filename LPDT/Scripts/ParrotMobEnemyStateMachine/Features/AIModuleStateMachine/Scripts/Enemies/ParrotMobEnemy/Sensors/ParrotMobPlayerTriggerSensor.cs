using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Sensors
{
	public class ParrotMobPlayerTriggerSensor : MonoBehaviour
	{
		private const float OutOfRangeSlack = 0.25f;

		private const float PlayerLosHeightOffset = 1.2f;

		[SerializeField]
		private SphereCollider _triggerCollider;

		[SerializeField]
		private EnemyLineOfSightDetector _lineOfSightDetector;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private float _detectionRange = 8f;

		private readonly Dictionary<Collider, PlayerDataHolder> _playerCollidersInside = new Dictionary<Collider, PlayerDataHolder>();

		private readonly HashSet<PlayerDataHolder> _playersInside = new HashSet<PlayerDataHolder>();

		private readonly List<Collider> _collidersToRemove = new List<Collider>();

		private PlayerDataHolder _visiblePlayer;

		public PlayerDataHolder PlayerInsideTrigger => _visiblePlayer;

		public bool HasPlayerInside
		{
			get
			{
				if (_visiblePlayer != null)
				{
					return _visiblePlayer.NetworkObject != null;
				}
				return false;
			}
		}

		[Inject]
		private void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public void Configure(float detectionRange)
		{
			_detectionRange = Mathf.Max(0.1f, detectionRange);
			if (!(_triggerCollider == null))
			{
				_triggerCollider.isTrigger = true;
				_triggerCollider.radius = _detectionRange;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (TryResolvePlayer(other, out var player))
			{
				_playerCollidersInside[other] = player;
				_playersInside.Add(player);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (_playerCollidersInside.Remove(other, out var value))
			{
				RemovePlayerIfNoCollidersLeft(value);
			}
		}

		private void LateUpdate()
		{
			PrunePlayersInside();
			_visiblePlayer = ResolveClosestVisiblePlayer();
		}

		private void PrunePlayersInside()
		{
			if (_playerCollidersInside.Count == 0)
			{
				return;
			}
			float num = _detectionRange + 0.25f;
			float num2 = num * num;
			Vector3 vector = ((_triggerCollider != null) ? _triggerCollider.transform.TransformPoint(_triggerCollider.center) : base.transform.position);
			_collidersToRemove.Clear();
			foreach (KeyValuePair<Collider, PlayerDataHolder> item in _playerCollidersInside)
			{
				Collider key = item.Key;
				PlayerDataHolder value = item.Value;
				if (key == null || !key.enabled || !key.gameObject.activeInHierarchy || value == null || value.NetworkObject == null)
				{
					_collidersToRemove.Add(key);
				}
				else if ((key.ClosestPoint(vector) - vector).sqrMagnitude > num2)
				{
					_collidersToRemove.Add(key);
				}
			}
			foreach (Collider item2 in _collidersToRemove)
			{
				if (_playerCollidersInside.Remove(item2, out var value2))
				{
					RemovePlayerIfNoCollidersLeft(value2);
				}
			}
		}

		private void RemovePlayerIfNoCollidersLeft(PlayerDataHolder player)
		{
			if (!_playerCollidersInside.Values.Any((PlayerDataHolder insidePlayer) => insidePlayer == player))
			{
				_playersInside.Remove(player);
			}
		}

		private PlayerDataHolder ResolveClosestVisiblePlayer()
		{
			if (_playersInside.Count == 0)
			{
				return null;
			}
			Vector3 position = base.transform.position;
			PlayerDataHolder result = null;
			float num = float.MaxValue;
			foreach (PlayerDataHolder item in _playersInside)
			{
				if (item == null || item.NetworkObject == null)
				{
					continue;
				}
				Vector3 position2 = item.NetworkObject.transform.position;
				Vector3 targetPosition = position2 + Vector3.up * 1.2f;
				if (HasLineOfSightTo(targetPosition))
				{
					float sqrMagnitude = (position2 - position).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						result = item;
					}
				}
			}
			return result;
		}

		private bool HasLineOfSightTo(Vector3 targetPosition)
		{
			if (!(_lineOfSightDetector == null))
			{
				return _lineOfSightDetector.HasLineOfSight(targetPosition);
			}
			return true;
		}

		private bool TryResolvePlayer(Collider other, out PlayerDataHolder player)
		{
			player = null;
			if (_spawnedPlayersModel == null || other == null)
			{
				return false;
			}
			NetworkObject componentInParent = other.GetComponentInParent<NetworkObject>();
			if (componentInParent == null)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player2 in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player2.Value;
				if (value != null && !(value.NetworkObject == null) && !(value.NetworkObject != componentInParent))
				{
					player = value;
					return true;
				}
			}
			return false;
		}
	}
}
