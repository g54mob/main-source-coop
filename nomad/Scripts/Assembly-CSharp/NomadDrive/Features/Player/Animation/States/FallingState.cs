namespace NomadDrive.Features.Player.Animation.States
{
	public class FallingState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 8);
		}
	}
}
