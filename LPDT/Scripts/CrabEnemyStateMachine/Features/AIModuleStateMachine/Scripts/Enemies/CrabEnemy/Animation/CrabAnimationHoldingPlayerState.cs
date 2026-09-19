using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Animation
{
	public class CrabAnimationHoldingPlayerState : StateBase<CrabAnimationStateId>
	{
		private readonly CrabAnimatorPresenter _presenter;

		private CrabClawSide _ownedClaw;

		public CrabAnimationHoldingPlayerState(CrabAnimatorPresenter presenter)
			: base(false, false)
		{
			_presenter = presenter;
		}

		public override void OnEnter()
		{
			_ownedClaw = _presenter.ActiveClaw;
			_presenter.SetHandClosedForClaw(_ownedClaw, closed: true);
			if (_ownedClaw == CrabClawSide.Left)
			{
				_presenter.SetRightHandClosed(closed: true);
			}
		}

		public override void OnLogic()
		{
			_presenter.ApplyLocomotionParams();
		}

		public override void OnExit()
		{
			_presenter.SetHandClosedForClaw(_ownedClaw, closed: false);
			if (_ownedClaw == CrabClawSide.Left)
			{
				_presenter.SetRightHandClosed(closed: false);
			}
			_ownedClaw = CrabClawSide.None;
		}
	}
}
