using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.States
{
	public class BartenderWaitingPaymentState : StateBase<BartenderStateId>
	{
		private readonly BartenderEnemy _enemy;

		private readonly BartenderEnemyContext _context;

		public BartenderWaitingPaymentState(BartenderEnemy enemy, BartenderEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(BartenderStateId.WaitingPayment);
			_context.SetIsDancing(value: false);
			_context.SetActiveDanceIndex(-1);
			_context.SetIsReacting(value: false);
			_context.SetIsAggressive(value: false);
			_context.CurrentStateTime = 0f;
			_context.AnimationSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
			_enemy.FaceCounterLookTarget();
		}
	}
}
