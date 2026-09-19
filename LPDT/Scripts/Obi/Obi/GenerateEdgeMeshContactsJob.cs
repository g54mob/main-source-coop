using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct GenerateEdgeMeshContactsJob : IJobParallelFor
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
		public NativeArray<EdgeMeshHeader> edgeMeshHeaders;

		[ReadOnly]
		public NativeArray<BIHNode> edgeBihNodes;

		[ReadOnly]
		public NativeArray<Edge> edges;

		[ReadOnly]
		public NativeArray<float2> edgeVertices;

		[WriteOnly]
		[NativeDisableParallelForRestriction]
		public NativeQueue<BurstContact>.ParallelWriter contactsQueue;

		[ReadOnly]
		public int firstPair;

		[ReadOnly]
		public BurstAffineTransform solverToWorld;

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
			EdgeMeshHeader header = edgeMeshHeaders[shape.dataIndex];
			int size;
			int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(bodyA, out size);
			BurstAabb burstAabb = simplexBounds[bodyA];
			BurstAffineTransform colliderToSolver = worldToSolver * transforms[bodyB];
			BurstAabb bounds = burstAabb.Transformed(math.inverse(float4x4.TRS(colliderToSolver.translation.xyz, colliderToSolver.rotation, colliderToSolver.scale.xyz)));
			float4 margin = new float4((shape.contactOffset + parameters.collisionMargin) / colliderToSolver.scale.xyz, 0f);
			BurstEdgeMesh function = new BurstEdgeMesh
			{
				colliderToSolver = colliderToSolver,
				shape = shape,
				header = header,
				edgeBihNodes = edgeBihNodes,
				edges = edges,
				vertices = edgeVertices
			};
			NativeQueue<int> nativeQueue = new NativeQueue<int>(Allocator.Temp);
			nativeQueue.Enqueue(0);
			while (!nativeQueue.IsEmpty())
			{
				int num = nativeQueue.Dequeue();
				BIHNode bIHNode = edgeBihNodes[header.firstNode + num];
				if (bIHNode.firstChild < 0)
				{
					for (int j = bIHNode.start; j < bIHNode.start + bIHNode.count; j++)
					{
						Edge edge = edges[header.firstEdge + j];
						float4 v = new float4(edgeVertices[header.firstVertex + edge.i1], 0f, 0f) + shape.center;
						float4 v2 = new float4(edgeVertices[header.firstVertex + edge.i2], 0f, 0f) + shape.center;
						if (new BurstAabb(v, v2, margin).IntersectsAabb(in bounds, shape.is2D))
						{
							float4 convexBary = BurstMath.BarycenterForSimplexOfSize(size);
							function.dataOffset = j;
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
