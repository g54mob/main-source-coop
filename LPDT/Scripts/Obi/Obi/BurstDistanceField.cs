using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstDistanceField : BurstLocalOptimization.IDistanceFunction
	{
		public BurstColliderShape shape;

		public BurstAffineTransform colliderToSolver;

		public NativeArray<DistanceFieldHeader> distanceFieldHeaders;

		public NativeArray<BurstDFNode> dfNodes;

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			point = colliderToSolver.InverseTransformPoint(point);
			if (shape.is2D)
			{
				point[2] = 0f;
			}
			DistanceFieldHeader header = distanceFieldHeaders[shape.dataIndex];
			float4 float5 = DFTraverse(point, in header);
			float4 float6 = new float4(math.normalize(float5.xyz), 0f);
			projectedPoint.point = colliderToSolver.TransformPoint(point - float6 * (float5[3] - shape.contactOffset));
			projectedPoint.normal = colliderToSolver.TransformDirection(float6);
		}

		private float4 DFTraverse(float4 particlePosition, in DistanceFieldHeader header)
		{
			NativeArray<int> nativeArray = new NativeArray<int>(12, Allocator.Temp);
			int num = 0;
			nativeArray[num++] = 0;
			while (num > 0)
			{
				int num2 = nativeArray[--num];
				BurstDFNode burstDFNode = dfNodes[header.firstNode + num2];
				if (burstDFNode.firstChild >= 0)
				{
					nativeArray[num++] = burstDFNode.firstChild + burstDFNode.GetOctant(particlePosition);
					continue;
				}
				return burstDFNode.SampleWithGradient(particlePosition);
			}
			return float4.zero;
		}

		public static JobHandle GenerateContacts(ObiColliderWorld world, BurstSolverImpl solver, NativeList<Oni.ContactPair> contactPairs, NativeQueue<BurstContact> contactQueue, NativeArray<int> contactOffsetsPerType, float deltaTime, JobHandle inputDeps)
		{
			int num = contactOffsetsPerType[7] - contactOffsetsPerType[6];
			if (num == 0)
			{
				return inputDeps;
			}
			inputDeps = IJobParallelForExtensions.Schedule(new GenerateDistanceFieldContactsJob
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
				rigidbodies = world.rigidbodies.AsNativeArray<BurstRigidbody>(),
				distanceFieldHeaders = world.distanceFieldContainer.headers.AsNativeArray<DistanceFieldHeader>(),
				distanceFieldNodes = world.distanceFieldContainer.dfNodes.AsNativeArray<BurstDFNode>(),
				contactsQueue = contactQueue.AsParallelWriter(),
				solverToWorld = solver.inertialFrame,
				worldToSolver = solver.worldToSolver,
				deltaTime = deltaTime,
				parameters = solver.abstraction.parameters,
				firstPair = contactOffsetsPerType[6]
			}, num, 1, inputDeps);
			return inputDeps;
		}
	}
}
