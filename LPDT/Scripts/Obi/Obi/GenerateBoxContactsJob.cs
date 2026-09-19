using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct GenerateBoxContactsJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeList<Oni.ContactPair> contactPairs;

		[ReadOnly]
		public NativeArray<float4> velocities;

		[ReadOnly]
		public NativeArray<float4> positions;

		[ReadOnly]
		public NativeArray<quaternion> orientations;

		[ReadOnly]
		public NativeArray<float> invMasses;

		[ReadOnly]
		public NativeArray<float4> radii;

		[ReadOnly]
		public NativeArray<int> simplices;

		[ReadOnly]
		public SimplexCounts simplexCounts;

		[ReadOnly]
		public NativeArray<BurstAffineTransform> transforms;

		[ReadOnly]
		public NativeArray<BurstColliderShape> shapes;

		[WriteOnly]
		[NativeDisableParallelForRestriction]
		public NativeQueue<BurstContact>.ParallelWriter contactsQueue;

		[ReadOnly]
		public int firstPair;

		[ReadOnly]
		public BurstAffineTransform worldToSolver;

		[ReadOnly]
		public float deltaTime;

		[ReadOnly]
		public Oni.SolverParameters parameters;

		public void Execute(int i)
		{
			int bodyA = contactPairs[firstPair + i].bodyA;
			int bodyB = contactPairs[firstPair + i].bodyB;
			int size;
			int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(bodyA, out size);
			BurstAffineTransform colliderToSolver = worldToSolver * transforms[bodyB];
			BurstBox function = new BurstBox
			{
				colliderToSolver = colliderToSolver,
				shape = shapes[bodyB]
			};
			float4 convexBary = BurstMath.BarycenterForSimplexOfSize(size);
			float4 convexPoint;
			BurstLocalOptimization.SurfacePoint surfacePoint = BurstLocalOptimization.Optimize(ref function, positions, orientations, radii, simplices, simplexStartAndSize, size, ref convexBary, out convexPoint, parameters.surfaceCollisionIterations, parameters.surfaceCollisionTolerance);
			contactsQueue.Enqueue(new BurstContact
			{
				bodyA = bodyA,
				bodyB = bodyB,
				pointA = convexBary,
				pointB = surfacePoint.point,
				normal = surfacePoint.normal * function.shape.sign
			});
		}
	}
}
