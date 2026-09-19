using Features.AIModuleStateMachine.Scripts.Core.Systems.Damage;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeDamageReactionSystem : EnemyDamageReactionSystemBase
	{
		private SnakeEnemy _enemy;

		private SnakeEnemyContext _context;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		[Inject]
		public void InjectDependencies(SnakeEnemy enemy, SnakeEnemyContext context, SpawnedPlayersModel spawnedPlayersModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_enemy = enemy;
			_context = context;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			InjectBaseDependencies(spawnedPlayersModel);
		}

		protected override void React(PlayerDataHolder attacker, DamageData damageData)
		{
			if (!(attacker?.NetworkObject == null) && _enemy.CurrentStateId != SnakeStateId.SafeZoneApproach)
			{
				int playerId = attacker.NetworkObject.InputAuthority.PlayerId;
				if (_enemyPlayerAttackabilityService.CanEnemyTargetPlayer(playerId) && _context.CanTargetPlayerForWrap(playerId) && !_context.IsPlayerOutsideGate(playerId))
				{
					_context.ClearWrapEscapeCooldown();
					_context.SetPriorityPlayer(attacker);
					_context.ApplyStepAggroMoveSpeed();
					_enemy.TriggerEvent(SnakeEvent.OnDamaged);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
