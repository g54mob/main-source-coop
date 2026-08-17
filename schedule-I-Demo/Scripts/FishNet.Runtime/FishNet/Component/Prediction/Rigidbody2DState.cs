using UnityEngine;

namespace FishNet.Component.Prediction
{
	public struct Rigidbody2DState
	{
		public uint LocalTick;

		public Vector3 Position;

		public Quaternion Rotation;

		public Vector2 Velocity;

		public float AngularVelocity;

		public bool Simulated;

		public bool IsKinematic;

		public Rigidbody2DState(Rigidbody2D rb, bool simulated, uint tick)
		{
			Position = rb.transform.position;
			Rotation = rb.transform.rotation;
			Velocity = rb.velocity;
			AngularVelocity = rb.angularVelocity;
			Simulated = simulated;
			IsKinematic = rb.isKinematic;
			LocalTick = tick;
		}

		public Rigidbody2DState(Rigidbody2D rb, uint tick)
		{
			Position = rb.transform.position;
			Rotation = rb.transform.rotation;
			Velocity = rb.velocity;
			AngularVelocity = rb.angularVelocity;
			Simulated = rb.simulated;
			IsKinematic = rb.isKinematic;
			LocalTick = tick;
		}
	}
}
