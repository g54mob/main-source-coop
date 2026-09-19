using System;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[RequireComponent(typeof(Rigidbody))]
	[NetworkBehaviourWeaved(0)]
	public class RotationRestorer : NetworkBehaviour
	{
		[Header("Target Rotation")]
		[Tooltip("The rotation the object should return to. Leave at default (0,0,0) or set your desired rest rotation.")]
		public Quaternion TargetRotation = Quaternion.identity;

		[Header("Spring Settings")]
		[Tooltip("How strongly the object is pulled back to the target rotation.")]
		public float SpringStrength = 10f;

		[Tooltip("How much angular velocity is damped to prevent oscillation.")]
		public float AngularDamping = 5f;

		[SerializeField]
		private Rigidbody _rb;

		private void FixedUpdate()
		{
			if (!(base.Object == null) && base.HasStateAuthority)
			{
				ApplyRestoreForce();
			}
		}

		private void ApplyRestoreForce()
		{
			(TargetRotation * Quaternion.Inverse(_rb.rotation)).ToAngleAxis(out var angle, out var axis);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			if (!(Mathf.Abs(angle) < 0.01f))
			{
				Vector3 vector = axis.normalized * (angle * (MathF.PI / 180f) * SpringStrength);
				Vector3 vector2 = -_rb.angularVelocity * AngularDamping;
				_rb.AddTorque(vector + vector2, ForceMode.Acceleration);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
