using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Obi
{
	public static class BurstIntegration
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 IntegrateLinear(float4 position, float4 velocity, float dt)
		{
			return position + velocity * dt;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 DifferentiateLinear(float4 position, float4 prevPosition, float dt)
		{
			return (position - prevPosition) / dt;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion AngularVelocityToSpinQuaternion(quaternion rotation, float4 angularVelocity, float dt)
		{
			quaternion a = new quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0f);
			return new quaternion(0.5f * math.mul(a, rotation).value * dt);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion IntegrateAngular(quaternion rotation, float4 angularVelocity, float dt)
		{
			rotation.value += AngularVelocityToSpinQuaternion(rotation, angularVelocity, dt).value;
			return math.normalize(rotation);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 DifferentiateAngular(quaternion rotation, quaternion prevRotation, float dt)
		{
			quaternion quaternion2 = math.mul(rotation, math.inverse(prevRotation));
			return new float4(((quaternion2.value.w >= 0f) ? 1 : (-1)) * quaternion2.value.xyz * 2f / dt, 0f);
		}
	}
}
