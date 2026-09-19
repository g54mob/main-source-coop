using Features.AIModuleStateMachine.Scripts.Data;
using Features.DamageableTrackModule.Scripts;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.Services
{
	public class EnemyPlayerAttackabilityService : IEnemyPlayerAttackabilityService
	{
		private readonly PlayerEnemyInteractionBlocksModel _interactionBlocksModel;

		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		public EnemyPlayerAttackabilityService(PlayerEnemyInteractionBlocksModel interactionBlocksModel, PlayerDamageablesTrackModel playerDamageablesTrackModel)
		{
			_interactionBlocksModel = interactionBlocksModel;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
		}

		public bool CanEnemyTargetPlayer(int playerId)
		{
			if (IsPlayerConnectedAndSpawned(playerId))
			{
				return !IsPlayerBlockedForEnemies(playerId);
			}
			return false;
		}

		public bool CanEnemyAttackPlayer(int playerId)
		{
			if (IsPlayerConnectedAndSpawned(playerId))
			{
				return !IsPlayerBlockedForEnemies(playerId);
			}
			return false;
		}

		private bool IsPlayerConnectedAndSpawned(int playerId)
		{
			if (!_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(playerId, out var value))
			{
				return false;
			}
			if (value is NetworkBehaviour networkBehaviour && networkBehaviour.Object != null)
			{
				return networkBehaviour.Object.IsValid;
			}
			return false;
		}

		private bool IsPlayerBlockedForEnemies(int playerId)
		{
			return _interactionBlocksModel.IsBlocked(playerId);
		}
	}
}
