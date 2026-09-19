using Features.AIModuleStateMachine.Scripts.Core.Systems.Damage;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobDamageProcessSystem : EnemyDamageReactionSystemBase
	{
		private ParrotMobEnemy _enemy;

		private ParrotMobEnemyContext _context;

		[Inject]
		public void InjectDependencies(ParrotMobEnemy enemy, ParrotMobEnemyContext context, SpawnedPlayersModel spawnedPlayersModel)
		{
			_enemy = enemy;
			_context = context;
			InjectBaseDependencies(spawnedPlayersModel);
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority)
			{
				_context.StatHealthController.OnEnemyDead += OnEnemyDeadHandler;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (base.HasStateAuthority)
			{
				_context.StatHealthController.OnEnemyDead -= OnEnemyDeadHandler;
			}
			base.Despawned(runner, hasState);
		}

		protected override void React(PlayerDataHolder attacker, DamageData damageData)
		{
			if (!_context.IsDead && !_context.IsFearing)
			{
				_context.SetPriorityPlayer(attacker);
				_context.IsZoneEngagementActive = true;
				if (_enemy.CurrentStateId != ParrotMobStateId.Scream)
				{
					_enemy.TriggerEvent(ParrotMobEvent.OnDamage);
				}
			}
		}

		private void OnEnemyDeadHandler()
		{
			_context.IsDead = true;
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
