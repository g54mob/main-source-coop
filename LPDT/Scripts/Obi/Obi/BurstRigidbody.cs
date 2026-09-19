using Unity.Mathematics;

namespace Obi
{
	public struct BurstRigidbody
	{
		public float4x4 inverseInertiaTensor;

		public float4 velocity;

		public float4 angularVelocity;

		public float4 com;

		public float inverseMass;

		public int constraintCount;

		private int pad1;

		private int pad2;
	}
}
