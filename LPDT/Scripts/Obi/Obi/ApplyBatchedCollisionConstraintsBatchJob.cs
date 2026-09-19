using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	public struct ApplyBatchedCollisionConstraintsBatchJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<BurstContact> contacts;

		[ReadOnly]
		public NativeArray<int> simplices;

		[ReadOnly]
		public SimplexCounts simplexCounts;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> positions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> deltas;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> counts;

		[NativeDisableParallelForRestriction]
		public NativeArray<quaternion> orientations;

		[NativeDisableParallelForRestriction]
		public NativeArray<quaternion> orientationDeltas;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> orientationCounts;

		[ReadOnly]
		public Oni.ConstraintParameters constraintParameters;

		[ReadOnly]
		public BatchData batchData;

		public void Execute(int workItemIndex)
		{
			batchData.GetConstraintRange(workItemIndex, out var start, out var end);
			for (int i = start; i < end; i++)
			{
				int size;
				int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(contacts[i].bodyA, out size);
				int size2;
				int simplexStartAndSize2 = simplexCounts.GetSimplexStartAndSize(contacts[i].bodyB, out size2);
				for (int j = 0; j < size; j++)
				{
					int particleIndex = simplices[simplexStartAndSize + j];
					BurstConstraintsBatchImpl.ApplyPositionDelta(particleIndex, constraintParameters.SORFactor, ref positions, ref deltas, ref counts);
					BurstConstraintsBatchImpl.ApplyOrientationDelta(particleIndex, constraintParameters.SORFactor, ref orientations, ref orientationDeltas, ref orientationCounts);
				}
				for (int k = 0; k < size2; k++)
				{
					int particleIndex2 = simplices[simplexStartAndSize2 + k];
					BurstConstraintsBatchImpl.ApplyPositionDelta(particleIndex2, constraintParameters.SORFactor, ref positions, ref deltas, ref counts);
					BurstConstraintsBatchImpl.ApplyOrientationDelta(particleIndex2, constraintParameters.SORFactor, ref orientations, ref orientationDeltas, ref orientationCounts);
				}
			}
		}
	}
}
