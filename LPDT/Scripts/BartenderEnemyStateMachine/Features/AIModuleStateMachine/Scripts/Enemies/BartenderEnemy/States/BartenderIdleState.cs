using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.States
{
	public class BartenderIdleState : StateBase<BartenderStateId>
	{
		private readonly BartenderEnemy _enemy;

		private readonly BartenderEnemyContext _context;

		public BartenderIdleState(BartenderEnemy enemy, BartenderEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(BartenderStateId.Idle);
			_context.SetIsReacting(value: false);
			_context.SetIsAggressive(value: false);
			_context.CurrentStateTime = 0f;
			_context.AnimationSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
			_enemy.FaceCounterLookTarget();
			_enemy.EnsureHomeStateForStock();
			_enemy.RefreshDancing();
		}

		public override void OnLogic()
		{
			_enemy.RefreshDancing();
		}
	}
}
