using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.EnemyFearingModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobFearSystem : MonoSystem
	{
		private ParrotMobEnemy _enemy;

		private ParrotMobEnemyContext _context;

		private EnemyFearListenerModel _enemyFearListenerModel;

		public override bool IsEnabled => true;

		[Inject]
		public void InjectDependencies(ParrotMobEnemy enemy, ParrotMobEnemyContext context, EnemyFearListenerModel enemyFearListenerModel)
		{
			_enemy = enemy;
			_context = context;
			_enemyFearListenerModel = enemyFearListenerModel;
		}

		public override void Enable()
		{
		}

		public override void Disable()
		{
		}

		public override void Clear()
		{
		}

		public override void Spawned()
		{
			base.Spawned();
			_enemyFearListenerModel.RegisterFearListener(_enemy.EnemyType, _enemy.EnemyInstants, _enemy);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_enemyFearListenerModel.UnregisterFearListener(_enemy.EnemyType, _enemy.EnemyInstants);
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Initialized && base.HasStateAuthority && _context.IsFearing)
			{
				_enemy.PositionOnFearEnd = _enemy.transform.position;
				_enemy.FearCompleted = true;
				_context.IsFearing = false;
				if (_context.IsDespawnAfterFear)
				{
					base.Object.DespawnHierarchy();
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
