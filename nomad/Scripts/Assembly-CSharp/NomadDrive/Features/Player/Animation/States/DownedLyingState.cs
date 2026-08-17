namespace NomadDrive.Features.Player.Animation.States
{
	public class DownedLyingState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 11);
		}
	}
}
