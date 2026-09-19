using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Animation
{
	public class CrabAnimationCarryingItemState : StateBase<CrabAnimationStateId>
	{
		private readonly CrabAnimatorPresenter _presenter;

		public CrabAnimationCarryingItemState(CrabAnimatorPresenter presenter)
			: base(false, false)
		{
			_presenter = presenter;
		}

		public override void OnEnter()
		{
			_presenter.SetHandClosed(1f);
			_presenter.SetHandClosedForClaw(CrabClawSide.Left, closed: true);
			_presenter.SetHandClosedForClaw(CrabClawSide.Right, closed: true);
		}

		public override void OnLogic()
		{
			_presenter.ApplyLocomotionParams();
		}

		public override void OnExit()
		{
			_presenter.SetHandClosed(0f);
			_presenter.SetHandClosedForClaw(CrabClawSide.Left, closed: false);
			_presenter.SetHandClosedForClaw(CrabClawSide.Right, closed: false);
		}
	}
}
