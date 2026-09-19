using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.DeadPartsModule.Scripts;
using Features.LevelGatesModule.Data;
using Features.LevelGatesModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts
{
	public class KrakenDeadPartThrowSystem : MonoBehaviour
	{
		[SerializeField]
		private KrakenController _krakenController;

		[SerializeField]
		private float _checkInterval = 0.25f;

		[SerializeField]
		private Transform _deadPartSpawnPoint;

		private PlayerDeadPartModel _playerDeadPartModel;

		private IPlayerStateService _playerStateService;

		private PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		private IGateNavigationService _gateNavigationService;

		private PlayerDeadPartsConfiguration _playerDeadPartsConfiguration;

		private KrakenHelpThrowModel _krakenHelpThrowModel;

		private KrakenHelpThrowConfiguration _krakenHelpThrowConfiguration;

		private MultiplayerModel _multiplayerModel;

		private float _checkTimer;

		private bool _isHelpThrowInProgress;

		[Inject]
		private void InjectDependencies(PlayerDeadPartModel playerDeadPartModel, IPlayerStateService playerStateService, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel, IGateNavigationService gateNavigationService, PlayerDeadPartsConfiguration playerDeadPartsConfiguration, KrakenHelpThrowModel krakenHelpThrowModel, KrakenHelpThrowConfiguration krakenHelpThrowConfiguration, MultiplayerModel multiplayerModel)
		{
			_playerDeadPartModel = playerDeadPartModel;
			_playerStateService = playerStateService;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
			_gateNavigationService = gateNavigationService;
			_playerDeadPartsConfiguration = playerDeadPartsConfiguration;
			_krakenHelpThrowModel = krakenHelpThrowModel;
			_krakenHelpThrowConfiguration = krakenHelpThrowConfiguration;
			_multiplayerModel = multiplayerModel;
		}

		private void Update()
		{
			if (base.enabled || _krakenController == null || !_krakenController.HasStateAuthority || _isHelpThrowInProgress)
			{
				return;
			}
			_checkTimer -= Time.deltaTime;
			if (!(_checkTimer > 0f))
			{
				_checkTimer = _checkInterval;
				if (_krakenController.IsIdle && TryEvaluateHelpThrow(out var request))
				{
					TryStartHelpThrowAsync(request).Forget();
				}
			}
		}

		private async UniTaskVoid TryStartHelpThrowAsync(KrakenHelpThrowRequest request)
		{
			_isHelpThrowInProgress = true;
			try
			{
				if (await _krakenController.RequestHelpThrowAsync(request))
				{
					_krakenHelpThrowModel.TryConsumeHelpThrow();
				}
			}
			finally
			{
				_isHelpThrowInProgress = false;
			}
		}

		private bool TryEvaluateHelpThrow(out KrakenHelpThrowRequest request)
		{
			request = default(KrakenHelpThrowRequest);
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return false;
			}
			if (_krakenHelpThrowModel.RemainingHelpThrows <= 0)
			{
				return false;
			}
			if (_playerDeadPartModel.HasAnyFreeSpawnedDeadPart())
			{
				return false;
			}
			if (!HasDeadPlayers(networkRunner))
			{
				return false;
			}
			if (!TryFindNearestBeachAliveTarget(out var closestPlayer, out var targetPosition))
			{
				return false;
			}
			Vector3 position = _deadPartSpawnPoint.position;
			request = new KrakenHelpThrowRequest(closestPlayer, targetPosition, position);
			return true;
		}

		private bool HasDeadPlayers(NetworkRunner runner)
		{
			foreach (PlayerRef activePlayer in runner.ActivePlayers)
			{
				int playerId = activePlayer.PlayerId;
				if (_playerStateService.IsPlayerDead(playerId) || _playerStateService.GetPlayerState(playerId) == PlayerState.PreDeadCrouch)
				{
					return true;
				}
			}
			return false;
		}

		private bool TryFindNearestBeachAliveTarget(out PlayerRef closestPlayer, out Vector3 targetPosition)
		{
			closestPlayer = PlayerRef.None;
			targetPosition = Vector3.zero;
			Vector3 position = _krakenController.transform.position;
			float num = float.PositiveInfinity;
			float targetResurrectionDistance = _playerDeadPartsConfiguration.TargetResurrectionDistance;
			foreach (KeyValuePair<PlayerRef, PlayerAlivePart> allPlayersAlivePart in _playerDeadPartModel.AllPlayersAliveParts)
			{
				PlayerRef key = allPlayersAlivePart.Key;
				PlayerAlivePart value = allPlayersAlivePart.Value;
				if (!(value == null) && _playerStateService.IsPlayerAlive(key.PlayerId) && IsPlayerOnBeach(key.PlayerId, value) && !_playerDeadPartModel.HasFreeDeadPartNearAlive(value, targetResurrectionDistance) && TryGetAlivePartTargetPosition(value, out var targetPosition2))
				{
					float sqrMagnitude = (targetPosition2 - position).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						closestPlayer = key;
						targetPosition = targetPosition2;
					}
				}
			}
			return closestPlayer != PlayerRef.None;
		}

		private bool IsPlayerOnBeach(int playerId, PlayerAlivePart alivePart)
		{
			if (_playersGatesModelSynchronizedModel.TryGetPlayerState(playerId, out var state) && state.PlayerInsideGate)
			{
				return false;
			}
			Vector3 alivePartPosition = GetAlivePartPosition(alivePart);
			if (!_gateNavigationService.TryGetClosestExitGate(alivePartPosition, out var closestGate))
			{
				return false;
			}
			float maxBeachDistanceFromExitGate = _krakenHelpThrowConfiguration.MaxBeachDistanceFromExitGate;
			return Vector3.Distance(alivePartPosition, closestGate.Position) <= maxBeachDistanceFromExitGate;
		}

		private static Vector3 GetAlivePartPosition(PlayerAlivePart alivePart)
		{
			if (alivePart.DeadPartTarget != null)
			{
				return alivePart.DeadPartTarget.position;
			}
			if (alivePart.DownPos != null)
			{
				return alivePart.DownPos.position;
			}
			return alivePart.transform.position;
		}

		private static bool TryGetAlivePartTargetPosition(PlayerAlivePart alivePart, out Vector3 targetPosition)
		{
			targetPosition = Vector3.zero;
			if (alivePart == null)
			{
				return false;
			}
			if (alivePart.DeadPartTarget != null)
			{
				targetPosition = alivePart.DeadPartTarget.position;
				return true;
			}
			if (alivePart.DownPos != null)
			{
				targetPosition = alivePart.DownPos.position;
				return true;
			}
			return false;
		}
	}
}
