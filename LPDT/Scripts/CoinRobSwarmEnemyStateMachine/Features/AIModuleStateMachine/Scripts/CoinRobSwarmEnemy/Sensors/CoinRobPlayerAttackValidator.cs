using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors
{
	public class CoinRobPlayerAttackValidator
	{
		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobCombatSettings _combatSettings;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly INavigationService _navigationService;

		private readonly IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		public CoinRobPlayerAttackValidator(CoinRobSwarmEnemyContext context, CoinRobCombatSettings combatSettings, SpawnedPlayersModel spawnedPlayersModel, INavigationService navigationService, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_context = context;
			_combatSettings = combatSettings;
			_spawnedPlayersModel = spawnedPlayersModel;
			_navigationService = navigationService;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public bool TryGetPlayerPosition(PlayerRef player, out Vector3 playerPosition)
		{
			playerPosition = Vector3.zero;
			if (player == PlayerRef.None)
			{
				return false;
			}
			if (!_spawnedPlayersModel.Players.TryGetValue(player, out var value) || value.NetworkObject == null)
			{
				return false;
			}
			playerPosition = value.NetworkObject.transform.position;
			return true;
		}

		public bool CanEngagePlayerAttack(PlayerRef player)
		{
			return CanEngagePlayerAttack(player, _context.SwarmKing);
		}

		public bool CanEngagePlayerAttack(PlayerRef player, CoinRobBehaviour king)
		{
			if (CanTargetPlayer(player) && TryGetPlayerPosition(player, out var playerPosition))
			{
				return CanEngagePlayerAttack(playerPosition, king);
			}
			return false;
		}

		public bool CanEngagePlayerAttack(Vector3 playerPosition, CoinRobBehaviour king)
		{
			if (IsPlayerInSafeZone(playerPosition))
			{
				return false;
			}
			if (IsKingAbovePlayer(king, playerPosition))
			{
				return false;
			}
			return true;
		}

		public bool CanEngageReactivePlayerAttack(PlayerRef player)
		{
			if (CanTargetPlayer(player) && TryGetPlayerPosition(player, out var playerPosition))
			{
				return !IsPlayerInSafeZone(playerPosition);
			}
			return false;
		}

		public bool IsPlayerProtectedBySafeZone(PlayerRef player)
		{
			if (TryGetPlayerPosition(player, out var playerPosition))
			{
				return IsPlayerInSafeZone(playerPosition);
			}
			return false;
		}

		public bool CanKingAttackPlayer(Vector3 playerPosition, CoinRobBehaviour king)
		{
			if (IsPlayerInSafeZone(playerPosition))
			{
				return false;
			}
			if (king == null)
			{
				return false;
			}
			if (IsKingAbovePlayer(king, playerPosition))
			{
				return false;
			}
			return true;
		}

		public bool CanKingAttackPlayer(PlayerRef player, CoinRobBehaviour king)
		{
			if (CanTargetPlayer(player) && TryGetPlayerPosition(player, out var playerPosition))
			{
				return CanKingAttackPlayer(playerPosition, king);
			}
			return false;
		}

		public bool IsKingAbovePlayer(CoinRobBehaviour king, Vector3 playerPosition)
		{
			if (king == null)
			{
				return false;
			}
			float playerGroundY = GetPlayerGroundY(playerPosition);
			float y = king.transform.position.y;
			float kingAbovePlayerBlockThreshold = GetKingAbovePlayerBlockThreshold();
			return y > playerGroundY + kingAbovePlayerBlockThreshold;
		}

		public float GetPlayerGroundY(Vector3 playerPosition)
		{
			if (_navigationService.IsPointOnNavMeshProjected(playerPosition, out var hit))
			{
				return hit.position.y;
			}
			return playerPosition.y;
		}

		private bool CanTargetPlayer(PlayerRef player)
		{
			if (player != PlayerRef.None)
			{
				return _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(player.PlayerId);
			}
			return false;
		}

		private float GetKingAbovePlayerBlockThreshold()
		{
			float minKingAbovePlayerForBlockedAttack = _combatSettings.MinKingAbovePlayerForBlockedAttack;
			if (!(minKingAbovePlayerForBlockedAttack > 0f))
			{
				return 0.35f;
			}
			return minKingAbovePlayerForBlockedAttack;
		}

		private bool IsPlayerInSafeZone(Vector3 playerPosition)
		{
			EnemySafeZoneAttackDetector safeZoneAttackDetector = _context.SafeZoneAttackDetector;
			PlayerSafeZone safeZone;
			if (safeZoneAttackDetector != null)
			{
				return safeZoneAttackDetector.TryFindPlayerSafeZone(playerPosition, out safeZone);
			}
			return false;
		}
	}
}
