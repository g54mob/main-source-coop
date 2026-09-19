using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstBox : BurstLocalOptimization.IDistanceFunction
	{
		public BurstColliderShape shape;

		public BurstAffineTransform colliderToSolver;

		public void Evaluate(float4 point, float4 radii, quaternion orientation, ref BurstLocalOptimization.SurfacePoint projectedPoint)
		{
			float4 float5 = shape.center * colliderToSolver.scale;
			float4 float6 = shape.size * colliderToSolver.scale * 0.5f;
			point = colliderToSolver.InverseTransformPointUnscaled(point) - float5;
			if (shape.is2D)
			{
				point[2] = 0f;
			}
			float4 float7 = float6 - math.abs(point);
			if (float7.x >= 0f && float7.y >= 0f && float7.z >= 0f)
			{
				float num = float.MaxValue;
				int index = 0;
				for (int i = 0; i < 3; i++)
				{
					if (float7[i] < num)
					{
						num = float7[i];
						index = i;
					}
				}
				projectedPoint.normal = float4.zero;
				projectedPoint.point = point;
				projectedPoint.normal[index] = ((point[index] > 0f) ? 1 : (-1));
				projectedPoint.point[index] = float6[index] * projectedPoint.normal[index];
			}
			else
			{
				projectedPoint.point = math.clamp(point, -float6, float6);
				projectedPoint.normal = math.normalizesafe(point - projectedPoint.point);
			}
			projectedPoint.point = colliderToSolver.TransformPointUnscaled(projectedPoint.point + float5 + projectedPoint.normal * shape.contactOffset);
			projectedPoint.normal = colliderToSolver.TransformDirection(projectedPoint.normal);
		}

		public static JobHandle GenerateContacts(ObiColliderWorld world, BurstSolverImpl solver, NativeList<Oni.ContactPair> contactPairs, NativeQueue<BurstContact> contactQueue, NativeArray<int> contactOffsetsPerType, float deltaTime, JobHandle inputDeps)
		{
			int num = contactOffsetsPerType[2] - contactOffsetsPerType[1];
			if (num == 0)
			{
				return inputDeps;
			}
			inputDeps = IJobParallelForExtensions.Schedule(new GenerateBoxContactsJob
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
				firstPair = contactOffsetsPerType[1]
			}, num, 8, inputDeps);
			return inputDeps;
		}
	}
}
