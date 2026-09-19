using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct GenerateDistanceFieldContactsJob : IJobParallelFor
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

		[ReadOnly]
		public NativeArray<BurstRigidbody> rigidbodies;

		[ReadOnly]
		public NativeArray<DistanceFieldHeader> distanceFieldHeaders;

		[ReadOnly]
		public NativeArray<BurstDFNode> distanceFieldNodes;

		[WriteOnly]
		[NativeDisableParallelForRestriction]
		public NativeQueue<BurstContact>.ParallelWriter contactsQueue;

		[ReadOnly]
		public int firstPair;

		[ReadOnly]
		public BurstInertialFrame solverToWorld;

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
			int rigidbodyIndex = shapes[bodyB].rigidbodyIndex;
			if (shapes[bodyB].dataIndex >= 0)
			{
				int size;
				int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(bodyA, out size);
				BurstAffineTransform colliderToSolver = worldToSolver * transforms[bodyB];
				BurstDistanceField function = new BurstDistanceField
				{
					colliderToSolver = colliderToSolver,
					shape = shapes[bodyB],
					distanceFieldHeaders = distanceFieldHeaders,
					dfNodes = distanceFieldNodes
				};
				float4 convexBary = BurstMath.BarycenterForSimplexOfSize(size);
				float4 convexPoint;
				BurstLocalOptimization.SurfacePoint surfacePoint = BurstLocalOptimization.Optimize(ref function, positions, orientations, radii, simplices, simplexStartAndSize, size, ref convexBary, out convexPoint, parameters.surfaceCollisionIterations, parameters.surfaceCollisionTolerance);
				float4 zero = float4.zero;
				float num = 0f;
				for (int j = 0; j < size; j++)
				{
					int index = simplices[simplexStartAndSize + j];
					num += radii[index].x * convexBary[j];
					zero += velocities[index] * convexBary[j];
				}
				float4 float5 = float4.zero;
				if (rigidbodyIndex >= 0)
				{
					float5 = BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, surfacePoint.point, rigidbodies, solverToWorld);
				}
				math.dot(convexPoint - surfacePoint.point, surfacePoint.normal);
				math.dot(zero - float5, surfacePoint.normal);
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
}
