namespace NomadDrive.Features.Player.Animation.States
{
	public class IdleState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 0);
		}
	}
}
