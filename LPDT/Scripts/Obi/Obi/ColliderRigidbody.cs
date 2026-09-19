using System;
using UnityEngine;

namespace Obi
{
	public struct ColliderRigidbody
	{
		public Matrix4x4 inverseInertiaTensor;

		public Vector4 velocity;

		public Vector4 angularVelocity;

		public Vector4 com;

		public float inverseMass;

		public int constraintCount;

		private int pad1;

		private int pad2;

		public void FromRigidbody(ObiRigidbody rb)
		{
			bool flag = !Application.isPlaying || rb.unityRigidbody.isKinematic || rb.kinematicForParticles;
			velocity = (rb.kinematicForParticles ? Vector3.zero : (rb.linearVelocity + (rb.unityRigidbody.useGravity ? (Physics.gravity * Time.fixedDeltaTime) : Vector3.zero)));
			angularVelocity = (rb.kinematicForParticles ? Vector3.zero : rb.angularVelocity);
			com = rb.unityRigidbody.position + rb.unityRigidbody.rotation * rb.unityRigidbody.centerOfMass;
			Vector3 vector = new Vector3(((rb.unityRigidbody.constraints & RigidbodyConstraints.FreezeRotationX) != RigidbodyConstraints.None) ? 0f : (1f / rb.unityRigidbody.inertiaTensor.x), ((rb.unityRigidbody.constraints & RigidbodyConstraints.FreezeRotationY) != RigidbodyConstraints.None) ? 0f : (1f / rb.unityRigidbody.inertiaTensor.y), ((rb.unityRigidbody.constraints & RigidbodyConstraints.FreezeRotationZ) != RigidbodyConstraints.None) ? 0f : (1f / rb.unityRigidbody.inertiaTensor.z));
			Vector3 vector2 = (flag ? Vector3.zero : vector);
			Matrix4x4 matrix4x = Matrix4x4.Rotate(rb.unityRigidbody.rotation * rb.unityRigidbody.inertiaTensorRotation);
			inverseInertiaTensor = matrix4x * Matrix4x4.Scale(vector2) * matrix4x.transpose;
			inverseMass = (flag ? 0f : (1f / rb.unityRigidbody.mass));
		}

		public void FromRigidbody(ObiRigidbody2D rb)
		{
			bool flag = !Application.isPlaying || rb.unityRigidbody.isKinematic || rb.kinematicForParticles;
			velocity = rb.linearVelocity;
			angularVelocity = new Vector4(0f, 0f, rb.angularVelocity * (MathF.PI / 180f), 0f);
			com = rb.transform.position + rb.transform.rotation * rb.unityRigidbody.centerOfMass;
			Vector3 vector = (flag ? Vector3.zero : new Vector3(0f, 0f, ((rb.unityRigidbody.constraints & RigidbodyConstraints2D.FreezeRotation) != RigidbodyConstraints2D.None) ? 0f : (1f / rb.unityRigidbody.inertia)));
			Matrix4x4 matrix4x = Matrix4x4.Rotate(Quaternion.AngleAxis(rb.rotation, Vector3.forward));
			inverseInertiaTensor = matrix4x * Matrix4x4.Scale(vector) * matrix4x.transpose;
			inverseMass = (flag ? 0f : (1f / rb.unityRigidbody.mass));
		}
	}
}
