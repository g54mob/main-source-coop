using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Animation
{
	public class CrabAnimationDetachingLeftState : StateBase<CrabAnimationStateId>
	{
		private readonly CrabAnimatorPresenter _presenter;

		public CrabAnimationDetachingLeftState(CrabAnimatorPresenter presenter)
			: base(false, false)
		{
			_presenter = presenter;
		}

		public override void OnEnter()
		{
			_presenter.SetRightHandClosed(closed: true);
			_presenter.SetLeftHandClosed(closed: true);
			_presenter.SetDetachingLeft(active: true);
			_presenter.ApplyStoppedLocomotionParams();
			_presenter.PlayDetachingSound();
		}

		public override void OnLogic()
		{
			_presenter.ApplyStoppedLocomotionParams();
		}

		public override void OnExit()
		{
			_presenter.SetDetachingLeft(active: false);
			_presenter.SetLeftHandClosed(closed: false);
			_presenter.SetRightHandClosed(closed: false);
		}
	}
}
