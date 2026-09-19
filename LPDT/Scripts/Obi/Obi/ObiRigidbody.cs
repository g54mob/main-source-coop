using UnityEngine;

namespace Obi
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Rigidbody))]
	public class ObiRigidbody : ObiRigidbodyBase
	{
		private Quaternion prevRotation;

		private Vector3 prevPosition;

		public Rigidbody unityRigidbody { get; private set; }

		public Vector3 position => unityRigidbody.position;

		public Quaternion rotation => unityRigidbody.rotation;

		public Vector3 linearVelocity { get; protected set; }

		public Vector3 angularVelocity { get; protected set; }

		protected override void OnEnable()
		{
			unityRigidbody = GetComponent<Rigidbody>();
			ResetPosition();
			base.OnEnable();
		}

		public void ResetPosition()
		{
			prevPosition = unityRigidbody.position;
			prevRotation = unityRigidbody.rotation;
			linearVelocity = unityRigidbody.linearVelocity;
			angularVelocity = unityRigidbody.angularVelocity;
		}

		private void CacheVelocities(float stepTime)
		{
			if (unityRigidbody.isKinematic && stepTime > 0f)
			{
				linearVelocity = (unityRigidbody.position - prevPosition) / stepTime;
				Quaternion quaternion = unityRigidbody.rotation * Quaternion.Inverse(prevRotation);
				angularVelocity = new Vector3(quaternion.x, quaternion.y, quaternion.z) * 2f / stepTime;
			}
			else
			{
				linearVelocity = unityRigidbody.linearVelocity;
				angularVelocity = unityRigidbody.angularVelocity;
			}
			prevPosition = unityRigidbody.position;
			prevRotation = unityRigidbody.rotation;
		}

		public override void UpdateIfNeeded(float stepTime)
		{
			if (!(unityRigidbody == null) && base.Handle.isValid)
			{
				CacheVelocities(stepTime);
				ObiColliderWorld instance = ObiColliderWorld.GetInstance();
				ColliderRigidbody value = instance.rigidbodies[base.Handle.index];
				value.FromRigidbody(this);
				instance.rigidbodies[base.Handle.index] = value;
			}
		}

		public override void UpdateVelocities(Vector3 linearDelta, Vector3 angularDelta)
		{
			if (!(unityRigidbody == null) && Application.isPlaying && !unityRigidbody.isKinematic && !kinematicForParticles && (Vector3.SqrMagnitude(linearDelta) > 1E-05f || Vector3.SqrMagnitude(angularDelta) > 1E-05f))
			{
				unityRigidbody.linearVelocity += linearDelta;
				unityRigidbody.angularVelocity += angularDelta;
			}
		}
	}
}
