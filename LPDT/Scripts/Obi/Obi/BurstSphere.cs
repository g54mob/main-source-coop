using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstSphere : BurstLocalOptimization.IDistanceFunction
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
			float num = shape.size.x * math.cmax(colliderToSolver.scale.xyz);
			float num2 = math.length(point);
			float4 float6 = point / (num2 + 1E-07f);
			projectedPoint.point = colliderToSolver.TransformPointUnscaled(float5 + float6 * (num + shape.contactOffset));
			projectedPoint.normal = colliderToSolver.TransformDirection(float6);
		}

		public static JobHandle GenerateContacts(ObiColliderWorld world, BurstSolverImpl solver, NativeList<Oni.ContactPair> contactPairs, NativeQueue<BurstContact> contactQueue, NativeArray<int> contactOffsetsPerType, float deltaTime, JobHandle inputDeps)
		{
			int num = contactOffsetsPerType[1] - contactOffsetsPerType[0];
			if (num == 0)
			{
				return inputDeps;
			}
			inputDeps = IJobParallelForExtensions.Schedule(new GenerateSphereContactsJob
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
				firstPair = contactOffsetsPerType[0]
			}, num, 8, inputDeps);
			return inputDeps;
		}
	}
}
