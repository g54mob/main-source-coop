using UnityEngine;

namespace Obi
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Rigidbody2D))]
	public class ObiRigidbody2D : ObiRigidbodyBase
	{
		private Quaternion prevRotation;

		private Vector3 prevPosition;

		public Rigidbody2D unityRigidbody { get; private set; }

		public Vector2 position => unityRigidbody.position;

		public float rotation => unityRigidbody.rotation;

		public Vector2 linearVelocity { get; protected set; }

		public float angularVelocity { get; protected set; }

		protected override void OnEnable()
		{
			unityRigidbody = GetComponent<Rigidbody2D>();
			ResetPosition();
			base.OnEnable();
		}

		public void ResetPosition()
		{
			prevPosition = unityRigidbody.position;
			prevRotation = Quaternion.AngleAxis(unityRigidbody.rotation, Vector3.forward);
			linearVelocity = unityRigidbody.linearVelocity;
			angularVelocity = unityRigidbody.angularVelocity;
		}

		private void CacheVelocities(float stepTime)
		{
			if (unityRigidbody.isKinematic && stepTime > 0f)
			{
				linearVelocity = (base.transform.position - prevPosition) / stepTime;
				angularVelocity = (base.transform.rotation * Quaternion.Inverse(prevRotation)).z * 57.29578f * 2f / stepTime;
			}
			else
			{
				linearVelocity = unityRigidbody.linearVelocity;
				angularVelocity = unityRigidbody.angularVelocity;
			}
			prevPosition = base.transform.position;
			prevRotation = base.transform.rotation;
		}

		public override void UpdateIfNeeded(float stepTime)
		{
			if (!(unityRigidbody == null))
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
			if (!(unityRigidbody == null) && Application.isPlaying && !unityRigidbody.isKinematic && !kinematicForParticles)
			{
				unityRigidbody.linearVelocity += new Vector2(linearDelta.x, linearDelta.y);
				unityRigidbody.angularVelocity += angularDelta[2] * 57.29578f;
			}
		}
	}
}
