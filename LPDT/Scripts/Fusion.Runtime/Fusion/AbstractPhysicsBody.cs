using UnityEngine;

namespace Fusion
{
	public abstract class AbstractPhysicsBody
	{
		public enum BodyInterpolation
		{
			None = 0,
			Interpolate = 1,
			Extrapolate = 2
		}

		public bool WasColliding;

		public abstract Vector3 LinearVelocity { get; set; }

		public abstract Vector3 AngularVelocity { get; set; }

		public abstract Vector3 Position { get; set; }

		public abstract Quaternion Rotation { get; set; }

		public abstract Vector3 Gravity { get; }

		public abstract float Drag { get; set; }

		public abstract float AngularDrag { get; set; }

		public abstract float Mass { get; set; }

		public abstract int EncodedConstraints { get; set; }

		public abstract NetworkRigidbodyFlags Flags { get; }

		public abstract bool Kinematic { get; set; }

		public abstract bool Sleeping { get; set; }

		public abstract Vector3 InertiaTensor { get; set; }

		public abstract BodyInterpolation Interpolation { get; set; }

		public abstract Transform Transform { get; }

		public abstract Component Component { get; }

		public abstract bool Valid { get; }

		public abstract void AddForce(Vector3 force);

		public abstract void AddTorque(Vector3 torque);
	}
}
