using Features.LevelLightModule.Scripts;
using UnityEngine;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine.States
{
	public class LocalPlayerAliveState : LocalPlayerStateBase
	{
		private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

		public override PlayerState StateEnum => PlayerState.Alive;

		protected override void OnEnter()
		{
			LightObjectsGroupService.SetLightObjectGroupState(LightObjectGroup.Player, state: true);
			PlayerMovableModel.LocalMovable.IsAutomaticForwardMovement = false;
			PlayerMovableModel.LocalMovable.TryCrouchImmediately(isCrouching: true);
			if (PlayerMovableModel.NetworkedAnimator.CompositeAnimator.GetBool(IsDeadHash))
			{
				PlayerMovableModel.NetworkedAnimator.SetBool(IsDeadHash, boolValue: false);
			}
			SetCommonHudVisible(isVisible: true);
			SetDefaultLineArmController();
			RunAliveSequence(base.RequestMouseHidden);
		}
	}
}
