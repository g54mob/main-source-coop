namespace NomadDrive.Features.Player.Animation.States
{
	public class DownedSittingState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 12);
		}
	}
}
