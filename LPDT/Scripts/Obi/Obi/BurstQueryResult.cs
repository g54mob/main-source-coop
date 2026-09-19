using Unity.Mathematics;

namespace Obi
{
	public struct BurstQueryResult
	{
		public float4 simplexBary;

		public float4 queryPoint;

		public float4 normal;

		public float distance;

		public float distanceAlongRay;

		public int simplexIndex;

		public int queryIndex;
	}
}
