using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	public struct ApplyCollisionConstraintsBatchJob : IJob
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

		public void Execute()
		{
			for (int i = 0; i < contacts.Length; i++)
			{
				int size;
				int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(contacts[i].bodyA, out size);
				for (int j = 0; j < size; j++)
				{
					int particleIndex = simplices[simplexStartAndSize + j];
					BurstConstraintsBatchImpl.ApplyPositionDelta(particleIndex, constraintParameters.SORFactor, ref positions, ref deltas, ref counts);
					BurstConstraintsBatchImpl.ApplyOrientationDelta(particleIndex, constraintParameters.SORFactor, ref orientations, ref orientationDeltas, ref orientationCounts);
				}
			}
		}
	}
}
