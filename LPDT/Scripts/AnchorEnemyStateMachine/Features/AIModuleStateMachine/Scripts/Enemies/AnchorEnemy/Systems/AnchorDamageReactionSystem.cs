using Features.AIModuleStateMachine.Scripts.Core.Systems.Damage;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorDamageReactionSystem : EnemyDamageReactionSystemBase
	{
		private AnchorEnemy _anchorEnemy;

		private AnchorEnemyContext _context;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		[Inject]
		public void InjectDependencies(AnchorEnemy anchorEnemy, AnchorEnemyContext context, SpawnedPlayersModel spawnedPlayersModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_anchorEnemy = anchorEnemy;
			_context = context;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			InjectBaseDependencies(spawnedPlayersModel);
		}

		protected override void React(PlayerDataHolder attacker, DamageData damageData)
		{
			if (!(attacker?.NetworkObject == null))
			{
				int playerId = attacker.NetworkObject.InputAuthority.PlayerId;
				if (_enemyPlayerAttackabilityService.CanEnemyTargetPlayer(playerId))
				{
					_context.SetPriorityPlayer(attacker);
					_anchorEnemy.TriggerEvent(AnchorEvent.OnDamaged);
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
