using System.Collections.Generic;
using Features.DeadPartsModule.Scripts;
using Features.KrakenModule.Scripts.Data;
using Features.LevelGatesModule.Data;
using Features.LevelGatesModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.KrakenModule.Scripts.Core
{
	public class DeadPartThrowTargetService : IDeadPartThrowTargetService
	{
		private readonly PlayerDeadPartModel _playerDeadPartModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		private readonly IGateNavigationService _gateNavigationService;

		private readonly PlayerDeadPartsConfiguration _playerDeadPartsConfiguration;

		private readonly KrakenHelpThrowConfiguration _krakenHelpThrowConfiguration;

		public DeadPartThrowTargetService(PlayerDeadPartModel playerDeadPartModel, IPlayerStateService playerStateService, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel, IGateNavigationService gateNavigationService, PlayerDeadPartsConfiguration playerDeadPartsConfiguration, KrakenHelpThrowConfiguration krakenHelpThrowConfiguration)
		{
			_playerDeadPartModel = playerDeadPartModel;
			_playerStateService = playerStateService;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
			_gateNavigationService = gateNavigationService;
			_playerDeadPartsConfiguration = playerDeadPartsConfiguration;
			_krakenHelpThrowConfiguration = krakenHelpThrowConfiguration;
		}

		public bool HasDeadPlayers(NetworkRunner runner)
		{
			if (runner == null)
			{
				return false;
			}
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

		public bool TryFindNearestBeachAliveTarget(Vector3 fromPosition, out KrakenDeadPartThrowTarget target)
		{
			target = default(KrakenDeadPartThrowTarget);
			float num = float.PositiveInfinity;
			float targetResurrectionDistance = _playerDeadPartsConfiguration.TargetResurrectionDistance;
			PlayerRef playerRef = PlayerRef.None;
			Vector3 targetPosition = Vector3.zero;
			foreach (KeyValuePair<PlayerRef, PlayerAlivePart> allPlayersAlivePart in _playerDeadPartModel.AllPlayersAliveParts)
			{
				PlayerRef key = allPlayersAlivePart.Key;
				PlayerAlivePart value = allPlayersAlivePart.Value;
				if (!(value == null) && _playerStateService.IsPlayerAlive(key.PlayerId) && IsPlayerOnBeach(key.PlayerId, value) && !_playerDeadPartModel.HasFreeDeadPartNearAlive(value, targetResurrectionDistance) && TryGetAlivePartTargetPosition(value, out var targetPosition2))
				{
					float sqrMagnitude = (targetPosition2 - fromPosition).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						playerRef = key;
						targetPosition = targetPosition2;
					}
				}
			}
			if (playerRef == PlayerRef.None)
			{
				return false;
			}
			target = new KrakenDeadPartThrowTarget(playerRef, targetPosition);
			return true;
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

		private Vector3 GetAlivePartPosition(PlayerAlivePart alivePart)
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

		private bool TryGetAlivePartTargetPosition(PlayerAlivePart alivePart, out Vector3 targetPosition)
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
