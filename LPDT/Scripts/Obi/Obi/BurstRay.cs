using Unity.Collections;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstRay : BurstLocalOptimization.IDistanceFunction
	{
		public BurstQueryShape shape;

		public BurstAffineTransform colliderToSolver;

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			float4x4 float4x5 = float4x4.TRS(point.xyz, orientation, radii.xyz);
			float4x4 a = math.mul(math.inverse(float4x5), float4x4.TRS(colliderToSolver.translation.xyz, colliderToSolver.rotation, colliderToSolver.scale.xyz));
			float4 float5 = math.mul(a, new float4(shape.center.xyz, 1f));
			float4 float6 = math.normalizesafe(math.mul(a, new float4(shape.size.xyz, 0f)));
			float num = ObiUtils.RaySphereIntersection(float5.xyz, float6.xyz, float3.zero, 1f);
			if (num < 0f)
			{
				point = colliderToSolver.InverseTransformPointUnscaled(point);
				float mu;
				float4 float7 = BurstMath.NearestPointOnEdge(shape.center * colliderToSolver.scale, (shape.center + shape.size) * colliderToSolver.scale, point, out mu);
				float4 obj = point - float7;
				float num2 = math.length(obj);
				float4 float8 = obj / (num2 + 1E-07f);
				projectedPoint.point = colliderToSolver.TransformPointUnscaled(float7 + float8 * shape.contactOffset);
				projectedPoint.normal = colliderToSolver.TransformDirection(float8);
			}
			else
			{
				float4 float9 = math.mul(float4x5, new float4((float5 + float6 * num).xyz, 1f));
				float4 float10 = math.normalizesafe(new float4((point - float9).xyz, 0f));
				projectedPoint.point = float9 + float10 * shape.contactOffset;
				projectedPoint.normal = float10;
			}
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
				value.distanceAlongRay = math.dot((surfacePoint.point + surfacePoint.normal * value.distance).xyz - shape.center.xyz, math.normalizesafe(shape.size.xyz));
				results.Enqueue(value);
			}
		}
	}
}
