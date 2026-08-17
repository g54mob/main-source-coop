namespace NomadDrive.Features.Player.Animation.States
{
	public class JumpState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 7);
		}
	}
}
