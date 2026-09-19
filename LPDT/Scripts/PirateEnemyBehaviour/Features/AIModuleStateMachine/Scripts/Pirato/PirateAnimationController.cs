using System;
using System.Collections;
using Features.AnimationModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	[NetworkBehaviourWeaved(1)]
	public class PirateAnimationController : NetworkBehaviour
	{
		[SerializeField]
		private NetworkedAnimationControllerBase _animationController;

		[SerializeField]
		private PirateEnemyContext _pirateEnemyContext;

		private bool _checkForAttackEnd;

		[WeaverGenerated]
		[DefaultForProperty("Velocity", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Velocity;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe float Velocity
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PirateAnimationController.Velocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PirateAnimationController.Velocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)((byte*)Ptr + 0) = value;
			}
		}

		public event Action OnAttackEnded;

		public event Action OnAttack;

		public event Action OnMeleeAttackStarted;

		public event Action OnPistolAttackStarted;

		public event Action OnWoodenStep;

		public event Action OnStep;

		public void TriggerMeleeAttack()
		{
			_animationController.PlayAnimation(AnimationType.MeleeAttack);
			StartCoroutine(StartCheckForAttackEnd());
		}

		public void TriggerRangeAttack()
		{
			_animationController.PlayAnimation(_pirateEnemyContext.PirateAttackController.DefaultAnimationType);
			StartCoroutine(StartCheckForAttackEnd());
		}

		public void TriggerSafeZoneAttack()
		{
			_animationController.PlayAnimation(_pirateEnemyContext.PirateAttackController.BottomAnimationType);
			StartCoroutine(StartCheckForAttackEnd());
		}

		public void TriggerSafeZoneMeleeAttack()
		{
			_animationController.PlayAnimation(AnimationType.SafeZoneMeleeAttack);
			StartCoroutine(StartCheckForAttackEnd());
		}

		private void TriggerAttack()
		{
			this.OnAttack?.Invoke();
		}

		private IEnumerator StartCheckForAttackEnd()
		{
			yield return null;
			_checkForAttackEnd = true;
		}

		private void CheckAttackEnd()
		{
			if (_checkForAttackEnd)
			{
				AnimatorStateInfo currentAnimatorStateInfo = _animationController.GetCurrentAnimatorStateInfo("Attack");
				AnimatorStateInfo nextAnimatorStateInfo = _animationController.GetNextAnimatorStateInfo("Attack");
				if (!currentAnimatorStateInfo.IsTag("Attack") && !nextAnimatorStateInfo.IsTag("Attack"))
				{
					this.OnAttackEnded?.Invoke();
					_checkForAttackEnd = false;
				}
			}
		}

		private void Update()
		{
			if (base.HasStateAuthority)
			{
				CheckAttackEnd();
				Velocity = _pirateEnemyContext.EnemyMovableBase.GetVelocity().magnitude;
			}
			_animationController.SetFloat("Velocity", Mathf.Lerp(_animationController.GetFloat("Velocity"), Velocity, Time.deltaTime * 2f));
		}

		private void InvokeOnMeleeAttackStarted()
		{
			this.OnMeleeAttackStarted?.Invoke();
		}

		private void InvokeOnPistolAttackStarted()
		{
			this.OnPistolAttackStarted?.Invoke();
		}

		private void InvokeOnWoodenStep()
		{
			this.OnWoodenStep?.Invoke();
		}

		private void InvokeOnStep()
		{
			this.OnStep?.Invoke();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Velocity = _Velocity;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Velocity = Velocity;
		}
	}
}
