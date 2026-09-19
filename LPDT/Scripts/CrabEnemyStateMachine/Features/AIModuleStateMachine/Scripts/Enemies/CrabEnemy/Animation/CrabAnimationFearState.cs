using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Animation
{
	public class CrabAnimationFearState : StateBase<CrabAnimationStateId>
	{
		private readonly CrabAnimatorPresenter _presenter;

		public CrabAnimationFearState(CrabAnimatorPresenter presenter)
			: base(false, false)
		{
			_presenter = presenter;
		}

		public override void OnLogic()
		{
			_presenter.ApplyLocomotionParams();
		}
	}
}
