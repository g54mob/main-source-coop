using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct CopyJob : IJobParallelForDefer
	{
		[ReadOnly]
		public NativeArray<float4> inputPositions;

		[ReadOnly]
		public NativeArray<float4> inputVelocities;

		[ReadOnly]
		public NativeArray<float4> inputColors;

		[ReadOnly]
		public NativeArray<float4> inputAttributes;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> outputPositions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> outputVelocities;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> outputColors;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> outputAttributes;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> dispatchBuffer;

		public void Execute(int i)
		{
			if (i == 0)
			{
				dispatchBuffer[3] = dispatchBuffer[7];
				dispatchBuffer[7] = 0;
			}
			outputPositions[i] = inputPositions[i];
			outputVelocities[i] = inputVelocities[i];
			outputColors[i] = inputColors[i];
			outputAttributes[i] = inputAttributes[i];
		}
	}
}
