namespace NomadDrive.Features.Player.Animation.States
{
	public class SittingState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 6);
		}
	}
}
