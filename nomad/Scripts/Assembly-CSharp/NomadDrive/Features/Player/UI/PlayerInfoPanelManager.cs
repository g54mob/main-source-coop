using System;
using System.Collections.Generic;
using EvilCore.DynamicCasting;
using EvilCore.Networking;
using NomadDrive.Features.Player.Downed;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.UI
{
	public sealed class PlayerInfoPanelManager : MonoBehaviour
	{
		[SerializeField]
		private PlayerInfoPanel panelPrefab;

		[SerializeField]
		private PlayerInfoPanelConfig config;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private ICastingManager _castingManager;

		private readonly Dictionary<uint, PlayerInfoPanel> _panels = new Dictionary<uint, PlayerInfoPanel>();

		private readonly Dictionary<uint, Action<bool>> _speakingHandlers = new Dictionary<uint, Action<bool>>();

		private readonly Dictionary<uint, PlayerDeathController> _deathControllers = new Dictionary<uint, PlayerDeathController>();

		private readonly HashSet<uint> _scratchActive = new HashSet<uint>();

		private bool _subscribed;

		private Transform _cameraTransform;

		private readonly Dictionary<uint, bool> _occludedCache = new Dictionary<uint, bool>();

		private int _occlusionCursor;

		private void OnEnable()
		{
			Subscribe();
			TryRefresh();
		}

		private void Start()
		{
			Subscribe();
			TryRefresh();
		}

		private void OnDisable()
		{
			Unsubscribe();
			DespawnAll();
		}

		private void Subscribe()
		{
			if (!_subscribed && _playerService != null && _networkManager != null)
			{
				_playerService.OnPlayerRegistered += OnLocalPlayerRegistered;
				_playerService.OnPlayerCleared += OnLocalPlayerCleared;
				_networkManager.OnConnectedPlayersChanged += OnConnectedPlayersChanged;
				_subscribed = true;
			}
		}

		private void Unsubscribe()
		{
			if (_subscribed)
			{
				if (_playerService != null)
				{
					_playerService.OnPlayerRegistered -= OnLocalPlayerRegistered;
					_playerService.OnPlayerCleared -= OnLocalPlayerCleared;
				}
				if (_networkManager != null)
				{
					_networkManager.OnConnectedPlayersChanged -= OnConnectedPlayersChanged;
				}
				_subscribed = false;
			}
		}

		private void OnLocalPlayerRegistered()
		{
			_cameraTransform = ((_playerService != null) ? _playerService.CameraTransform : null);
			TryRefresh();
		}

		private void OnLocalPlayerCleared()
		{
			_cameraTransform = null;
			DespawnAll();
		}

		private void OnConnectedPlayersChanged()
		{
			TryRefresh();
		}

		private void TryRefresh()
		{
			if (panelPrefab == null || config == null || _playerService == null || _networkManager == null || _playerService.LocalPlayer == null)
			{
				return;
			}
			if (_cameraTransform == null)
			{
				_cameraTransform = _playerService.CameraTransform;
			}
			_scratchActive.Clear();
			uint netId = _playerService.LocalPlayer.netId;
			IReadOnlyList<INetworkPlayer> connectedPlayers = _networkManager.ConnectedPlayers;
			for (int i = 0; i < connectedPlayers.Count; i++)
			{
				INetworkPlayer networkPlayer = connectedPlayers[i];
				if (networkPlayer != null && networkPlayer.NetId != netId)
				{
					_scratchActive.Add(networkPlayer.NetId);
					if (!_panels.ContainsKey(networkPlayer.NetId))
					{
						SpawnPanelFor(networkPlayer);
					}
				}
			}
			if (_panels.Count <= 0)
			{
				return;
			}
			List<uint> list = null;
			foreach (KeyValuePair<uint, PlayerInfoPanel> panel in _panels)
			{
				if (!_scratchActive.Contains(panel.Key))
				{
					if (list == null)
					{
						list = new List<uint>();
					}
					list.Add(panel.Key);
				}
			}
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					DespawnPanelFor(list[j]);
				}
			}
		}

		private void SpawnPanelFor(INetworkPlayer netPlayer)
		{
			if (netPlayer is Player { transform: var followTarget } player)
			{
				PlayerInfoPanel playerInfoPanel = UnityEngine.Object.Instantiate(panelPrefab);
				playerInfoPanel.gameObject.name = "PlayerInfoPanel_" + player.DisplayName;
				playerInfoPanel.Bind(player, followTarget, _cameraTransform, config);
				_panels[player.netId] = playerInfoPanel;
				_deathControllers[player.netId] = player.GetComponent<PlayerDeathController>();
				Action<bool> value = playerInfoPanel.SetSpeakingState;
				player.OnSpeakingChanged += value;
				_speakingHandlers[player.netId] = value;
				if (player.IsSpeaking)
				{
					playerInfoPanel.SetSpeakingState(isSpeaking: true);
				}
			}
		}

		private void DespawnPanelFor(uint netId)
		{
			if (!_panels.TryGetValue(netId, out var value))
			{
				return;
			}
			if (_speakingHandlers.TryGetValue(netId, out var value2))
			{
				if (value != null && value.TrackedPlayer != null)
				{
					value.TrackedPlayer.OnSpeakingChanged -= value2;
				}
				_speakingHandlers.Remove(netId);
			}
			if (value != null)
			{
				value.Unbind();
				UnityEngine.Object.Destroy(value.gameObject);
			}
			_panels.Remove(netId);
			_deathControllers.Remove(netId);
			_occludedCache.Remove(netId);
		}

		private void DespawnAll()
		{
			if (_panels.Count == 0)
			{
				_speakingHandlers.Clear();
				_deathControllers.Clear();
				_occludedCache.Clear();
				return;
			}
			List<uint> list = new List<uint>(_panels.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				DespawnPanelFor(list[i]);
			}
			_speakingHandlers.Clear();
			_deathControllers.Clear();
			_occludedCache.Clear();
		}

		private void LateUpdate()
		{
			if (config == null || _panels.Count == 0)
			{
				return;
			}
			if (_cameraTransform == null)
			{
				_cameraTransform = ((_playerService != null) ? _playerService.CameraTransform : null);
				if (_cameraTransform == null)
				{
					return;
				}
			}
			Vector3 position = _cameraTransform.position;
			Vector3 forward = _cameraTransform.forward;
			bool flag = config.occlusionCheckEnabled && _castingManager != null;
			int num = -1;
			if (flag && _panels.Count > 0)
			{
				_occlusionCursor++;
				num = _occlusionCursor % _panels.Count;
			}
			int num2 = -1;
			foreach (KeyValuePair<uint, PlayerInfoPanel> panel in _panels)
			{
				num2++;
				PlayerInfoPanel value = panel.Value;
				if (value == null || value.TrackedPlayer == null)
				{
					continue;
				}
				Transform transform = value.TrackedPlayer.transform;
				if (_deathControllers.TryGetValue(panel.Key, out var value2) && value2 != null && value2.IsDowned && value2.ChickenNetId != 0 && _networkManager.TryGetNetworkObjectById(value2.ChickenNetId, out var networkObject) && networkObject != null)
				{
					transform = networkObject.transform;
				}
				if (value.FollowTarget != transform)
				{
					value.SetFollowTarget(transform);
				}
				if (!(value.FollowTarget == null))
				{
					Vector3 worldAnchorPosition = value.GetWorldAnchorPosition();
					float num3 = Vector3.Distance(position, worldAnchorPosition);
					bool value3;
					if (!flag)
					{
						value3 = false;
					}
					else if (num2 == num && num3 > 0.5f && num3 < config.maxVisibleDistance && Vector3.Dot(forward, worldAnchorPosition - position) > 0f)
					{
						value3 = IsOccluded(position, worldAnchorPosition, num3);
						_occludedCache[panel.Key] = value3;
					}
					else
					{
						_occludedCache.TryGetValue(panel.Key, out value3);
					}
					value.Tick(num3, value3);
				}
			}
		}

		private bool IsOccluded(Vector3 camPos, Vector3 headPos, float distance)
		{
			Vector3 direction = headPos - camPos;
			if (direction.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			direction.Normalize();
			float num = Mathf.Max(0f, distance - 0.4f);
			if (num <= 0f)
			{
				return false;
			}
			CastRequest request = CastRequest.Ray(_cameraTransform, direction, num, config.occluderLayerMask, useTransformForward: false);
			return _castingManager.CastImmediate(request).DidHit;
		}
	}
}
