using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct GenerateTriangleMeshContactsJob : IJobParallelFor
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
		public NativeArray<BurstAabb> simplexBounds;

		[ReadOnly]
		public NativeArray<BurstAffineTransform> transforms;

		[ReadOnly]
		public NativeArray<BurstColliderShape> shapes;

		[ReadOnly]
		public NativeArray<BurstRigidbody> rigidbodies;

		[ReadOnly]
		public NativeArray<TriangleMeshHeader> triangleMeshHeaders;

		[ReadOnly]
		public NativeArray<BIHNode> bihNodes;

		[ReadOnly]
		public NativeArray<Triangle> triangles;

		[ReadOnly]
		public NativeArray<float3> vertices;

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
			BurstColliderShape shape = shapes[bodyB];
			if (shape.dataIndex < 0)
			{
				return;
			}
			int rigidbodyIndex = shape.rigidbodyIndex;
			TriangleMeshHeader triangleMeshHeader = triangleMeshHeaders[shape.dataIndex];
			int size;
			int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(bodyA, out size);
			BurstAabb burstAabb = simplexBounds[bodyA];
			BurstAffineTransform colliderToSolver = worldToSolver * transforms[bodyB];
			BurstAabb bounds = burstAabb.Transformed(math.inverse(float4x4.TRS(colliderToSolver.translation.xyz, colliderToSolver.rotation, colliderToSolver.scale.xyz)));
			float4 margin = new float4((shape.contactOffset + parameters.collisionMargin) / colliderToSolver.scale.xyz, 0f);
			BurstTriangleMesh function = new BurstTriangleMesh
			{
				colliderToSolver = colliderToSolver,
				shape = shape
			};
			NativeQueue<int> nativeQueue = new NativeQueue<int>(Allocator.Temp);
			nativeQueue.Enqueue(0);
			while (!nativeQueue.IsEmpty())
			{
				int num = nativeQueue.Dequeue();
				BIHNode bIHNode = bihNodes[triangleMeshHeader.firstNode + num];
				if (bIHNode.firstChild < 0)
				{
					for (int j = bIHNode.start; j < bIHNode.start + bIHNode.count; j++)
					{
						Triangle triangle = triangles[triangleMeshHeader.firstTriangle + j];
						float4 float5 = new float4(vertices[triangleMeshHeader.firstVertex + triangle.i1], 0f);
						float4 float6 = new float4(vertices[triangleMeshHeader.firstVertex + triangle.i2], 0f);
						float4 float7 = new float4(vertices[triangleMeshHeader.firstVertex + triangle.i3], 0f);
						if (new BurstAabb(float5, float6, float7, margin).IntersectsAabb(in bounds, shape.is2D))
						{
							float4 convexBary = BurstMath.BarycenterForSimplexOfSize(size);
							function.tri.Cache(float5 * colliderToSolver.scale, float6 * colliderToSolver.scale, float7 * colliderToSolver.scale);
							float4 convexPoint;
							BurstLocalOptimization.SurfacePoint surfacePoint = BurstLocalOptimization.Optimize(ref function, positions, orientations, radii, simplices, simplexStartAndSize, size, ref convexBary, out convexPoint, parameters.surfaceCollisionIterations, parameters.surfaceCollisionTolerance);
							float4 zero = float4.zero;
							float num2 = 0f;
							for (int k = 0; k < size; k++)
							{
								int index = simplices[simplexStartAndSize + k];
								num2 += radii[index].x * convexBary[k];
								zero += velocities[index] * convexBary[k];
							}
							float4 float8 = float4.zero;
							if (rigidbodyIndex >= 0)
							{
								float8 = BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, surfacePoint.point, rigidbodies, solverToWorld);
							}
							float num3 = math.dot(convexPoint - surfacePoint.point, surfacePoint.normal);
							if (math.dot(zero - float8, surfacePoint.normal) * deltaTime + num3 <= num2 + shape.contactOffset + parameters.collisionMargin)
							{
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
				else
				{
					if (bounds.min[bIHNode.axis] <= bIHNode.min)
					{
						nativeQueue.Enqueue(bIHNode.firstChild);
					}
					if (bounds.max[bIHNode.axis] >= bIHNode.max)
					{
						nativeQueue.Enqueue(bIHNode.firstChild + 1);
					}
				}
			}
		}
	}
}
