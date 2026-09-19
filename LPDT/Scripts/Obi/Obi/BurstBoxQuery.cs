using Unity.Collections;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstBoxQuery : BurstLocalOptimization.IDistanceFunction
	{
		public BurstQueryShape shape;

		public BurstAffineTransform colliderToSolver;

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			float4 float5 = shape.center * colliderToSolver.scale;
			float4 float6 = shape.size * colliderToSolver.scale * 0.5f;
			point = colliderToSolver.InverseTransformPointUnscaled(point) - float5;
			float4 float7 = float6 - math.abs(point);
			if (float7.x >= 0f && float7.y >= 0f && float7.z >= 0f)
			{
				float num = float.MaxValue;
				int index = 0;
				for (int i = 0; i < 3; i++)
				{
					if (float7[i] < num)
					{
						num = float7[i];
						index = i;
					}
				}
				projectedPoint.normal = float4.zero;
				projectedPoint.point = point;
				projectedPoint.normal[index] = ((point[index] > 0f) ? 1 : (-1));
				projectedPoint.point[index] = float6[index] * projectedPoint.normal[index];
			}
			else
			{
				projectedPoint.point = math.clamp(point, -float6, float6);
				projectedPoint.normal = math.normalizesafe(point - projectedPoint.point);
			}
			projectedPoint.point = colliderToSolver.TransformPointUnscaled(projectedPoint.point + float5 + projectedPoint.normal * shape.contactOffset);
			projectedPoint.normal = colliderToSolver.TransformDirection(projectedPoint.normal);
		}

		public void Query(int shapeIndex, NativeArray<float4> positions, NativeArray<quaternion> orientations, NativeArray<float4> radii, NativeArray<int> simplices, int simplexIndex, int simplexStart, int simplexSize, NativeQueue<BurstQueryResult>.ParallelWriter results, int optimizationIterations, float optimizationTolerance)
		{
			BurstQueryResult value = new BurstQueryResult
			{
				simplexIndex = simplexIndex,
				queryIndex = shapeIndex
			};
			float4 convexBary = BurstMath.BarycenterForSimplexOfSize(simplexSize);
			float4 convexPoint;
			BurstLocalOptimization.SurfacePoint surfacePoint = BurstLocalOptimization.Optimize(ref this, positions, orientations, radii, simplices, simplexStart, simplexSize, ref convexBary, out convexPoint, optimizationIterations, optimizationTolerance);
			float4 zero = float4.zero;
			float num = 0f;
			for (int i = 0; i < simplexSize; i++)
			{
				int index = simplices[simplexStart + i];
				zero += positions[index] * convexBary[i];
				num += BurstMath.EllipsoidRadius(surfacePoint.normal, orientations[index], radii[index].xyz) * convexBary[i];
			}
			value.queryPoint = surfacePoint.point;
			value.normal = surfacePoint.normal;
			value.simplexBary = convexBary;
			value.distance = math.dot(zero - surfacePoint.point, surfacePoint.normal) - num;
			if (value.distance <= shape.maxDistance)
			{
				results.Enqueue(value);
			}
		}
	}
}
