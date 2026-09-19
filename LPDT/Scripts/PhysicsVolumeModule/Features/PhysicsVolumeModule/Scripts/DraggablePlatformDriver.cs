using System.Collections.Generic;
using Features.GrabModule.Scripts.PhysGrab;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class DraggablePlatformDriver : GrabObjectBase
	{
		[SerializeField]
		private Rigidbody _rb;

		[SerializeField]
		private List<PhysGrabber> _playerGrabbing = new List<PhysGrabber>();

		[SerializeField]
		private float _stiffness = 100f;

		[SerializeField]
		private float _damping = 5f;

		[SerializeField]
		private float _maxSpeed = 2f;

		[Range(0.1f, 100f)]
		[SerializeField]
		private float _forceMultiplier = 10f;

		[SerializeField]
		private float _handleTrailDistance = 1.2f;

		public override Rigidbody Rigidbody => _rb;

		public override List<PhysGrabber> Grabbers => _playerGrabbing;

		public override float GrabStrengthMultiplier { get; set; }

		public override ICartItemsContainer CartItemsGrabber => null;

		public override bool GrabbingPhysicsBlocked { get; set; }

		public void FixedUpdate()
		{
			GrabbingPhysics();
		}

		private void GrabbingPhysics()
		{
			if (GrabbingPhysicsBlocked || Grabbers.Count == 0 || _rb.isKinematic || base.Object.StateAuthority != _playerGrabbing[0].Object.StateAuthority)
			{
				return;
			}
			PhysGrabber physGrabber = Grabbers[0];
			if (physGrabber.physGrabPoints.TryGetValue(this, out var value) && !(value == null))
			{
				Vector3 vector = physGrabber.physGrabPointPullerPosition.position - value.position;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (magnitude > 0.001f)
				{
					vector -= vector / magnitude * Mathf.Min(_handleTrailDistance, magnitude);
				}
				vector = Vector3.ClampMagnitude(vector, 2f);
				Vector3 force = new Vector3(vector.x * _stiffness - _rb.linearVelocity.x * _damping, 0f, vector.z * _stiffness - _rb.linearVelocity.z * _damping) * _rb.mass;
				float num = _rb.mass * _forceMultiplier;
				if (force.magnitude > num)
				{
					force = force.normalized * num;
				}
				if (_rb.linearVelocity.magnitude > _maxSpeed)
				{
					force *= 0.5f;
				}
				_rb.AddForceAtPosition(force, value.position, ForceMode.Force);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
