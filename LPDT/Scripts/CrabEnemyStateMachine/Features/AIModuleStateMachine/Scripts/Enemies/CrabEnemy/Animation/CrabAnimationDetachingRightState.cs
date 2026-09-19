using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Animation
{
	public class CrabAnimationDetachingRightState : StateBase<CrabAnimationStateId>
	{
		private readonly CrabAnimatorPresenter _presenter;

		public CrabAnimationDetachingRightState(CrabAnimatorPresenter presenter)
			: base(false, false)
		{
			_presenter = presenter;
		}

		public override void OnEnter()
		{
			_presenter.SetRightHandClosed(closed: true);
			_presenter.SetDetachingRight(active: true);
			_presenter.ApplyStoppedLocomotionParams();
			_presenter.PlayDetachingSound();
		}

		public override void OnLogic()
		{
			_presenter.ApplyStoppedLocomotionParams();
		}

		public override void OnExit()
		{
			_presenter.SetDetachingRight(active: false);
			_presenter.SetRightHandClosed(closed: false);
		}
	}
}
