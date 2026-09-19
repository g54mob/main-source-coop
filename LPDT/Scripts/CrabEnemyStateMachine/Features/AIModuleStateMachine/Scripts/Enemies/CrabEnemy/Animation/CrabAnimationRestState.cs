using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Animation
{
	public class CrabAnimationRestState : StateBase<CrabAnimationStateId>
	{
		private readonly CrabAnimatorPresenter _presenter;

		public CrabAnimationRestState(CrabAnimatorPresenter presenter)
			: base(false, false)
		{
			_presenter = presenter;
		}

		public override void OnLogic()
		{
			_presenter.ApplyStoppedLocomotionParams();
		}
	}
}
