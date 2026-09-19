using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.States
{
	public class DespawnState : StateBase<MimicStateId>
	{
		private readonly MimicEnemy _enemy;

		private readonly MimicEnemyContext _context;

		public DespawnState(MimicEnemy enemy, MimicEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(MimicStateId.Despawn);
			_enemy.SetVisualState(MimicVisualState.Despawn);
			_context.MimicFearDestroySystem.Enable();
			_enemy.RequestDespawnAfterFear();
		}

		public override void OnExit()
		{
			_context.MimicFearDestroySystem.Disable();
		}
	}
}
