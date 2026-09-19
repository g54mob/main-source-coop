using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.EnemyFearingModule.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperFearSystem : MonoSystem
	{
		private SleeperEnemy _enemy;

		private EnemyFearListenerModel _enemyFearListenerModel;

		public override bool IsEnabled => true;

		[Inject]
		public void InjectDependencies(SleeperEnemy enemy, EnemyFearListenerModel enemyFearListenerModel)
		{
			_enemy = enemy;
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
