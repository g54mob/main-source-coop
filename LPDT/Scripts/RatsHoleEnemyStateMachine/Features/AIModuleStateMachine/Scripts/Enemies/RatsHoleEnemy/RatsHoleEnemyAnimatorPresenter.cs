using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	public class RatsHoleEnemyAnimatorPresenter : MonoBehaviour
	{
		private readonly int _attackTriggerHash = Animator.StringToHash("Attacking");

		private readonly int _speedHash = Animator.StringToHash("Speed");

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private float _speedDampTime = 0.12f;

		public void SyncLocomotionFromVelocity(RatsHoleEnemyContext context)
		{
			if (!(context == null))
			{
				if (context.VisualState == RatsHoleEnemyVisualState.Attack || context.IsAttackPerforming)
				{
					SetSpeed(0f);
					return;
				}
				if (context.VisualState == RatsHoleEnemyVisualState.Idle)
				{
					SetSpeed(0f);
					return;
				}
				float num = Mathf.Max(context.MoveSpeed, context.RunVelocityThreshold, 0.01f);
				float speed = Mathf.Clamp01(context.SmoothedVelocity / num);
				SetSpeed(speed);
			}
		}

		public void TriggerAttack()
		{
			_animator.ResetTrigger(_attackTriggerHash);
			_animator.SetTrigger(_attackTriggerHash);
		}

		private void SetSpeed(float speed)
		{
			_animator.SetFloat(_speedHash, speed, _speedDampTime, Time.deltaTime);
		}
	}
}
