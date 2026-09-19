using Features.LevelLightModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine.States
{
	public class LocalPlayerPreDeadCrouchState : LocalPlayerStateBase
	{
		private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

		private static readonly int IsPermanentStunInProgressHash = Animator.StringToHash("IsPermanentStunInProgress");

		private static readonly int IsPermanentStunTransitionForceHash = Animator.StringToHash("IsPermanentStunTransitionForce");

		private static readonly int PermanentStunHash = Animator.StringToHash("PermanentStun");

		public override PlayerState StateEnum => PlayerState.PreDeadCrouch;

		protected override void OnEnter()
		{
			LightObjectsGroupService.SetLightObjectGroupState(LightObjectGroup.Player, state: true);
			PlayerMovableModel.LocalMovable.IsAutomaticForwardMovement = false;
			if (PlayerMovableModel.NetworkedAnimator.CompositeAnimator.GetBool(IsDeadHash))
			{
				PlayerMovableModel.NetworkedAnimator.SetBool(IsDeadHash, boolValue: false);
			}
			PlayerMovableModel.NetworkedAnimator.SetBool(IsPermanentStunInProgressHash, boolValue: true);
			PlayerMovableModel.NetworkedAnimator.SetTrigger(PermanentStunHash);
			PlayerMovableModel.LocalMovable.ForceCrouch(isCrouching: true);
			SetCommonHudVisible(isVisible: true);
			RunAliveSequence(delegate
			{
				RequestMouseHidden();
				SetDefaultLineArmController();
			});
		}

		protected override void OnExit()
		{
			PlayerCharacterMovableBase localMovable = PlayerMovableModel.LocalMovable;
			if (localMovable == null)
			{
				return;
			}
			localMovable.ForceCrouch(isCrouching: false);
			NetworkedCompositeAnimator networkedAnimator = PlayerMovableModel.NetworkedAnimator;
			if (!(networkedAnimator == null))
			{
				if (localMovable.CanStandUp())
				{
					networkedAnimator.SetBool(IsPermanentStunTransitionForceHash, boolValue: false);
					networkedAnimator.SetBool(IsPermanentStunInProgressHash, boolValue: false);
				}
				else
				{
					networkedAnimator.SetBool(IsPermanentStunTransitionForceHash, boolValue: true);
					networkedAnimator.SetBool(IsPermanentStunInProgressHash, boolValue: false);
				}
			}
		}
	}
}
