using System;
using Fusion;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	[NetworkBehaviourWeaved(1)]
	public class PirateAimController : NetworkBehaviour
	{
		[SerializeField]
		private PirateEnemyContext _pirateEnemyContext;

		[SerializeField]
		private Rig _rig;

		[SerializeField]
		private MultiAimConstraint _multiAimConstraintGun;

		[SerializeField]
		private Transform _targetBone;

		[SerializeField]
		private Transform _rotationTransform;

		[SerializeField]
		private float _rotationSpeed = 5f;

		[SerializeField]
		private float _positionLerpSpeed = 5f;

		[SerializeField]
		private float _weightLerpSpeed = 3f;

		[WeaverGenerated]
		[DefaultForProperty("_targetWeight", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float __targetWeight;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe float _targetWeight
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PirateAimController._targetWeight. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PirateAimController._targetWeight. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)((byte*)Ptr + 0) = value;
			}
		}

		private void Update()
		{
			if (_pirateEnemyContext.TryGetTargetPosition(out var position))
			{
				_rig.weight = Mathf.Lerp(_rig.weight, _targetWeight, Time.deltaTime * _weightLerpSpeed);
				RotateTowardsTarget(position);
			}
		}

		public void RotateTowardsTarget(Vector3 targetPosition)
		{
			_rotationTransform.forward = (targetPosition - _targetBone.position).normalized;
			_rotationTransform.position = _targetBone.position;
		}

		public void EnableAim()
		{
			if (base.HasStateAuthority)
			{
				_targetWeight = 1f;
			}
		}

		public void DisableAim()
		{
			if (base.HasStateAuthority)
			{
				_targetWeight = 0f;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			_targetWeight = __targetWeight;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			__targetWeight = _targetWeight;
		}
	}
}
