namespace NomadDrive.Features.Player.Animation.States
{
	public class SprintState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 2);
		}

		public override void UpdateParameters(float moveDirection, float moveStrafe, float speed)
		{
			WriteMoveParameters(moveDirection, moveStrafe, speed);
		}
	}
}
