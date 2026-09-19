using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstHeightField : BurstLocalOptimization.IDistanceFunction
	{
		public BurstColliderShape shape;

		public BurstAffineTransform colliderToSolver;

		public BurstMath.CachedTri tri;

		public float4 triNormal;

		public HeightFieldHeader header;

		public NativeArray<float> heightFieldSamples;

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			point = colliderToSolver.InverseTransformPoint(point);
			float4 bary;
			float4 float5 = BurstMath.NearestPointOnTri(in tri, point, out bary);
			float4 float6 = math.normalizesafe(point - float5);
			projectedPoint.point = colliderToSolver.TransformPoint(float5 + float6 * shape.contactOffset);
			projectedPoint.normal = colliderToSolver.TransformDirection(float6);
		}

		public static JobHandle GenerateContacts(ObiColliderWorld world, BurstSolverImpl solver, NativeList<Oni.ContactPair> contactPairs, NativeQueue<BurstContact> contactQueue, NativeArray<int> contactOffsetsPerType, float deltaTime, JobHandle inputDeps)
		{
			int num = contactOffsetsPerType[4] - contactOffsetsPerType[3];
			if (num == 0)
			{
				return inputDeps;
			}
			inputDeps = IJobParallelForExtensions.Schedule(new GenerateHeightFieldContactsJob
			{
				contactPairs = contactPairs,
				positions = solver.positions,
				orientations = solver.orientations,
				velocities = solver.velocities,
				invMasses = solver.invMasses,
				radii = solver.principalRadii,
				simplices = solver.simplices,
				simplexCounts = solver.simplexCounts,
				simplexBounds = solver.simplexBounds,
				transforms = world.colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				shapes = world.colliderShapes.AsNativeArray<BurstColliderShape>(),
				rigidbodies = world.rigidbodies.AsNativeArray<BurstRigidbody>(),
				heightFieldHeaders = world.heightFieldContainer.headers.AsNativeArray<HeightFieldHeader>(),
				heightFieldSamples = world.heightFieldContainer.samples.AsNativeArray<float>(),
				contactsQueue = contactQueue.AsParallelWriter(),
				solverToWorld = solver.inertialFrame,
				worldToSolver = solver.worldToSolver,
				deltaTime = deltaTime,
				parameters = solver.abstraction.parameters,
				firstPair = contactOffsetsPerType[3]
			}, num, 1, inputDeps);
			return inputDeps;
		}
	}
}
