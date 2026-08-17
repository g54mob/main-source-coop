namespace NomadDrive.Features.Player.Animation.States
{
	public class CrouchedIdleState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 3);
		}
	}
}
