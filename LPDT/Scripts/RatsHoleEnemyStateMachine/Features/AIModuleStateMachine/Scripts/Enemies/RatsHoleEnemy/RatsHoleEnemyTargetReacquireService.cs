using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.LevelGatesModule.Data;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	public class RatsHoleEnemyTargetReacquireService
	{
		private const float TARGET_PATH_SAMPLE_RADIUS = 1f;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly PlayersGatesModelSynchronizedModel _playersGatesModel;

		private readonly IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private readonly INavigationService _navigationService;

		public RatsHoleEnemyTargetReacquireService(SpawnedPlayersModel spawnedPlayersModel, PlayersGatesModelSynchronizedModel playersGatesModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, INavigationService navigationService)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_playersGatesModel = playersGatesModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_navigationService = navigationService;
		}

		public bool TryFindNearest(RatsHoleEnemy enemy, RatsHoleEnemyContext context, float range, bool fromHome, PlayerDataHolder ignoredPlayer, out PlayerDataHolder player)
		{
			player = null;
			if (_spawnedPlayersModel?.Players == null || range <= 0f)
			{
				return false;
			}
			Vector3 origin = ((fromHome && (bool)context.HasHomePosition) ? context.HomePosition : enemy.transform.position);
			float rangeSqr = range * range;
			float bestDistanceSqr = float.MaxValue;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player2 in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player2.Value;
				if (value != ignoredPlayer && CanUsePlayer(value) && !IsPlayerOutsideGate(player2.Key.PlayerId) && _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(player2.Key.PlayerId) && !IsPlayerInSafeZone(enemy, context, value) && IsPlayerCloserThanBest(value, origin, rangeSqr, bestDistanceSqr, out var distanceSqr) && HasCompletePathToPlayer(context, player2.Key, value))
				{
					bestDistanceSqr = distanceSqr;
					player = value;
				}
			}
			return player != null;
		}

		public bool IsPlayerInSafeZone(RatsHoleEnemy enemy, RatsHoleEnemyContext context, PlayerDataHolder holder)
		{
			if (!CanUsePlayer(holder))
			{
				return false;
			}
			Vector3 playerPosition = holder.NetworkObject.transform.position;
			PlayerRef inputAuthority = holder.NetworkObject.InputAuthority;
			if (_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out var position))
			{
				playerPosition = position;
			}
			if (context.SafeZoneBlocker != null)
			{
				return context.SafeZoneBlocker.IsPlayerInSafeZone(enemy.transform.position, playerPosition);
			}
			if (context.SafeZoneAttackDetector != null)
			{
				return context.SafeZoneAttackDetector.IsPlayerInSafeZone(playerPosition);
			}
			return false;
		}

		private static bool CanUsePlayer(PlayerDataHolder holder)
		{
			if (holder?.NetworkObject != null)
			{
				return holder.NetworkObject.IsValid;
			}
			return false;
		}

		private static bool IsPlayerCloserThanBest(PlayerDataHolder holder, Vector3 origin, float rangeSqr, float bestDistanceSqr, out float distanceSqr)
		{
			Vector3 vector = holder.NetworkObject.transform.position - origin;
			distanceSqr = new Vector2(vector.x, vector.z).sqrMagnitude;
			if (distanceSqr <= rangeSqr)
			{
				return distanceSqr < bestDistanceSqr;
			}
			return false;
		}

		private bool HasCompletePathToPlayer(RatsHoleEnemyContext context, PlayerRef playerRef, PlayerDataHolder holder)
		{
			if (context.NavMeshAgent == null)
			{
				return false;
			}
			Vector3 goalWorldPos = holder.NetworkObject.transform.position;
			if (_navigationService.TryGetPlayerTrackingPosition(playerRef, out var position))
			{
				goalWorldPos = position;
			}
			NavMeshPath path;
			return _navigationService.TryGetCompletePath(context.NavMeshAgent, goalWorldPos, 1f, out path);
		}

		private bool IsPlayerOutsideGate(int playerId)
		{
			if (_playersGatesModel != null && _playersGatesModel.TryGetPlayerState(playerId, out var state))
			{
				return !state.PlayerInsideGate;
			}
			return false;
		}
	}
}
