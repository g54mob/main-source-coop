using UnityEngine;

namespace FishNet.Component.Prediction
{
	public struct RigidbodyState
	{
		public uint LocalTick;

		public Vector3 Position;

		public Quaternion Rotation;

		public bool IsKinematic;

		public Vector3 Velocity;

		public Vector3 AngularVelocity;

		public RigidbodyState(Rigidbody rb, bool isKinematic, uint tick)
			: this(rb, tick)
		{
			Position = rb.transform.position;
			Rotation = rb.transform.rotation;
			IsKinematic = isKinematic;
			Velocity = rb.velocity;
			AngularVelocity = rb.angularVelocity;
			LocalTick = tick;
		}

		public RigidbodyState(Rigidbody rb, uint tick)
		{
			Position = rb.transform.position;
			Rotation = rb.transform.rotation;
			IsKinematic = rb.isKinematic;
			Velocity = rb.velocity;
			AngularVelocity = rb.angularVelocity;
			LocalTick = tick;
		}
	}
}
