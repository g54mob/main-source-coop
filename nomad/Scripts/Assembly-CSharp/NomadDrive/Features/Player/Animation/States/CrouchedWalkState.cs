namespace NomadDrive.Features.Player.Animation.States
{
	public class CrouchedWalkState : AnimationState
	{
		public override void Enter()
		{
			Animator.SetInteger(AnimationState.StateIdHash, 4);
		}

		public override void UpdateParameters(float moveDirection, float moveStrafe, float speed)
		{
			WriteMoveParameters(moveDirection, moveStrafe, speed);
		}
	}
}
