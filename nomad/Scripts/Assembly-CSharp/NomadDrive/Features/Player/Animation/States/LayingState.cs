namespace NomadDrive.Features.Player.Animation.States
{
	public class LayingState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 10);
		}
	}
}
