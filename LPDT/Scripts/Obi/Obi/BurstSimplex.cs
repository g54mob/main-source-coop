using Unity.Collections;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstSimplex : BurstLocalOptimization.IDistanceFunction
	{
		public NativeArray<float4> positions;

		public NativeArray<float4> radii;

		public NativeArray<int> simplices;

		public int simplexStart;

		public int simplexSize;

		private BurstMath.CachedTri tri;

		public void CacheData()
		{
			if (simplexSize == 3)
			{
				tri.Cache(new float4(positions[simplices[simplexStart]].xyz, 0f), new float4(positions[simplices[simplexStart + 1]].xyz, 0f), new float4(positions[simplices[simplexStart + 2]].xyz, 0f));
			}
		}

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			switch (simplexSize)
			{
			default:
			{
				float4 point2 = positions[simplices[simplexStart]];
				point2.w = 0f;
				projectedPoint.bary = new float4(1f, 0f, 0f, 0f);
				projectedPoint.point = point2;
				break;
			}
			case 2:
			{
				float4 float5 = positions[simplices[simplexStart]];
				float5.w = 0f;
				float4 float6 = positions[simplices[simplexStart + 1]];
				float6.w = 0f;
				BurstMath.NearestPointOnEdge(float5, float6, point, out var mu);
				projectedPoint.bary = new float4(1f - mu, mu, 0f, 0f);
				projectedPoint.point = float5 * projectedPoint.bary[0] + float6 * projectedPoint.bary[1];
				break;
			}
			case 3:
				projectedPoint.point = BurstMath.NearestPointOnTri(in tri, point, out projectedPoint.bary);
				break;
			}
			projectedPoint.normal = math.normalizesafe(point - projectedPoint.point);
		}
	}
}
