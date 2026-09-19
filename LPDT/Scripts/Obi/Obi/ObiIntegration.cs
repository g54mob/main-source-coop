using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	public static class ObiIntegration
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 IntegrateLinear(Vector4 position, Vector4 velocity, float dt)
		{
			return position + velocity * dt;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 DifferentiateLinear(Vector4 position, Vector4 prevPosition, float dt)
		{
			return (position - prevPosition) / dt;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Quaternion AngularVelocityToSpinQuaternion(Quaternion rotation, Vector4 angularVelocity, float dt)
		{
			Quaternion quaternion = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0f) * rotation;
			Vector4 vector = new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w) * 0.5f * dt;
			return new Quaternion(vector.x, vector.y, vector.z, vector.w);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Quaternion IntegrateAngular(Quaternion rotation, Vector4 angularVelocity, float dt)
		{
			Quaternion quaternion = AngularVelocityToSpinQuaternion(rotation, angularVelocity, dt);
			rotation.x += quaternion.x;
			rotation.y += quaternion.y;
			rotation.z += quaternion.z;
			rotation.w += quaternion.w;
			return rotation.normalized;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 DifferentiateAngular(Quaternion rotation, Quaternion prevRotation, float dt)
		{
			Quaternion quaternion = rotation * Quaternion.Inverse(prevRotation);
			return new Vector4(quaternion.x, quaternion.y, quaternion.z, 0f) * 2f / dt;
		}
	}
}
