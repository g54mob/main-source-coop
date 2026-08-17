using UnityEngine;

namespace NomadDrive.Features.Player.Animation
{
	public abstract class AnimationState
	{
		protected Animator Animator;

		protected static readonly int StateIdHash = Animator.StringToHash("StateId");

		protected static readonly int MoveDirectionHash = Animator.StringToHash("MoveDirection");

		protected static readonly int MoveStrafeHash = Animator.StringToHash("MoveStrafe");

		protected static readonly int SpeedHash = Animator.StringToHash("Speed");

		protected const float MoveParamDampTime = 0.06f;

		public virtual void Initialize(Animator animator)
		{
			Animator = animator;
		}

		protected void WriteMoveParameters(float moveDirection, float moveStrafe, float speed)
		{
			Animator.SetFloat(MoveDirectionHash, moveDirection, 0.06f, Time.deltaTime);
			Animator.SetFloat(MoveStrafeHash, moveStrafe, 0.06f, Time.deltaTime);
			Animator.SetFloat(SpeedHash, speed);
		}

		public virtual void Enter()
		{
		}

		public virtual void UpdateParameters(float moveDirection, float moveStrafe, float speed)
		{
		}

		public virtual void Exit()
		{
		}
	}
}
