using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstCapsule : BurstLocalOptimization.IDistanceFunction
	{
		public BurstColliderShape shape;

		public BurstAffineTransform colliderToSolver;

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			float4 float5 = shape.center * colliderToSolver.scale;
			point = colliderToSolver.InverseTransformPointUnscaled(point) - float5;
			if (shape.is2D)
			{
				point[2] = 0f;
			}
			int num = (int)shape.size.z;
			float num2 = shape.size.x * math.max(colliderToSolver.scale[(num + 1) % 3], colliderToSolver.scale[(num + 2) % 3]);
			float num3 = math.max(num2, shape.size.y * 0.5f * colliderToSolver.scale[num]);
			float4 zero = float4.zero;
			zero[num] = num3 - num2;
			float mu;
			float4 float6 = BurstMath.NearestPointOnEdge(-zero, zero, point, out mu);
			float4 obj = point - float6;
			float num4 = math.length(obj);
			float4 float7 = obj / (num4 + 1E-07f);
			projectedPoint.point = colliderToSolver.TransformPointUnscaled(float5 + float6 + float7 * (num2 + shape.contactOffset));
			projectedPoint.normal = colliderToSolver.TransformDirection(float7);
		}

		public static JobHandle GenerateContacts(ObiColliderWorld world, BurstSolverImpl solver, NativeList<Oni.ContactPair> contactPairs, NativeQueue<BurstContact> contactQueue, NativeArray<int> contactOffsetsPerType, float deltaTime, JobHandle inputDeps)
		{
			int num = contactOffsetsPerType[3] - contactOffsetsPerType[2];
			if (num == 0)
			{
				return inputDeps;
			}
			inputDeps = IJobParallelForExtensions.Schedule(new GenerateCapsuleContactsJob
			{
				contactPairs = contactPairs,
				positions = solver.positions,
				orientations = solver.orientations,
				velocities = solver.velocities,
				invMasses = solver.invMasses,
				radii = solver.principalRadii,
				simplices = solver.simplices,
				simplexCounts = solver.simplexCounts,
				transforms = world.colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				shapes = world.colliderShapes.AsNativeArray<BurstColliderShape>(),
				contactsQueue = contactQueue.AsParallelWriter(),
				worldToSolver = solver.worldToSolver,
				deltaTime = deltaTime,
				parameters = solver.abstraction.parameters,
				firstPair = contactOffsetsPerType[2]
			}, num, 8, inputDeps);
			return inputDeps;
		}
	}
}
