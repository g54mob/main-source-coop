using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstEdgeMesh : BurstLocalOptimization.IDistanceFunction
	{
		public BurstColliderShape shape;

		public BurstAffineTransform colliderToSolver;

		public int dataOffset;

		public EdgeMeshHeader header;

		public NativeArray<BIHNode> edgeBihNodes;

		public NativeArray<Edge> edges;

		public NativeArray<float2> vertices;

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			point = colliderToSolver.InverseTransformPointUnscaled(point);
			if (shape.is2D)
			{
				point[2] = 0f;
			}
			Edge edge = edges[header.firstEdge + dataOffset];
			float4 a = (new float4(vertices[header.firstVertex + edge.i1], 0f, 0f) + shape.center) * colliderToSolver.scale;
			float4 b = (new float4(vertices[header.firstVertex + edge.i2], 0f, 0f) + shape.center) * colliderToSolver.scale;
			float mu;
			float4 float5 = BurstMath.NearestPointOnEdge(a, b, point, out mu);
			float4 float6 = math.normalizesafe(point - float5);
			projectedPoint.normal = colliderToSolver.TransformDirection(float6);
			projectedPoint.point = colliderToSolver.TransformPointUnscaled(float5 + float6 * shape.contactOffset);
		}

		public static JobHandle GenerateContacts(ObiColliderWorld world, BurstSolverImpl solver, NativeList<Oni.ContactPair> contactPairs, NativeQueue<BurstContact> contactQueue, NativeArray<int> contactOffsetsPerType, float deltaTime, JobHandle inputDeps)
		{
			int num = contactOffsetsPerType[6] - contactOffsetsPerType[5];
			if (num == 0)
			{
				return inputDeps;
			}
			inputDeps = IJobParallelForExtensions.Schedule(new GenerateEdgeMeshContactsJob
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
				edgeMeshHeaders = world.edgeMeshContainer.headers.AsNativeArray<EdgeMeshHeader>(),
				edgeBihNodes = world.edgeMeshContainer.bihNodes.AsNativeArray<BIHNode>(),
				edges = world.edgeMeshContainer.edges.AsNativeArray<Edge>(),
				edgeVertices = world.edgeMeshContainer.vertices.AsNativeArray<float2>(),
				contactsQueue = contactQueue.AsParallelWriter(),
				solverToWorld = solver.solverToWorld,
				worldToSolver = solver.worldToSolver,
				deltaTime = deltaTime,
				parameters = solver.abstraction.parameters,
				firstPair = contactOffsetsPerType[4]
			}, num, 1, inputDeps);
			return inputDeps;
		}
	}
}
