using System;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[Serializable]
	public class AnchorPlayerLatch
	{
		private Transform _attach;

		public bool IsLatched => _attach != null;

		public void Latch(Transform attachTarget, AnchorThrowSphere sphere, AnchorPendant pendant, Action clearPullGrabbers, ref bool physGrabReeling)
		{
			if (!(attachTarget == null))
			{
				_attach = attachTarget;
				physGrabReeling = false;
				clearPullGrabbers?.Invoke();
				pendant.SetReturnSpring(0f, 0f, 1f);
				AnchorPhysicsUtil.ZeroVelocity(sphere.Rigidbody);
				AnchorPhysicsUtil.ZeroVelocity(pendant.Rigidbody);
				if (sphere.Rigidbody != null)
				{
					sphere.Rigidbody.isKinematic = true;
				}
				if (pendant.Rigidbody != null)
				{
					pendant.Rigidbody.isKinematic = true;
				}
				Snap(sphere, pendant);
			}
		}

		public void Clear()
		{
			_attach = null;
		}

		public void Snap(AnchorThrowSphere sphere, AnchorPendant pendant)
		{
			if (!(_attach == null))
			{
				Vector3 position = _attach.position;
				Quaternion rotation = _attach.rotation;
				sphere.ApplyPose(position, rotation);
				AnchorPhysicsUtil.ZeroVelocity(sphere.Rigidbody);
				pendant.SnapFlightRelativeTo(position, rotation);
				AnchorPhysicsUtil.ZeroVelocity(pendant.Rigidbody);
			}
		}
	}
}
