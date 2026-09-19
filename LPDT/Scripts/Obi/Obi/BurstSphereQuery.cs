using Unity.Collections;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstSphereQuery : BurstLocalOptimization.IDistanceFunction
	{
		public BurstQueryShape shape;

		public BurstAffineTransform colliderToSolver;

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			float4 float5 = shape.center * colliderToSolver.scale;
			point = colliderToSolver.InverseTransformPointUnscaled(point) - float5;
			float num = shape.size.x * math.cmax(colliderToSolver.scale.xyz);
			float num2 = math.length(point);
			float4 float6 = point / (num2 + 1E-07f);
			projectedPoint.point = colliderToSolver.TransformPointUnscaled(float5 + float6 * (num + shape.contactOffset));
			projectedPoint.normal = colliderToSolver.TransformDirection(float6);
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
