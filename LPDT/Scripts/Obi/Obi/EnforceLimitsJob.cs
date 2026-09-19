using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct EnforceLimitsJob : IJobParallelFor
	{
		[NativeDisableParallelForRestriction]
		public NativeArray<float4> positions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> prevPositions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float> life;

		[ReadOnly]
		public NativeArray<int> phases;

		[ReadOnly]
		public NativeArray<int> activeParticles;

		[ReadOnly]
		public bool killOffLimits;

		[ReadOnly]
		public BurstAabb boundaryLimits;

		public void Execute(int index)
		{
			int index2 = activeParticles[index];
			float4 float5 = positions[index2];
			float4 float6 = prevPositions[index2];
			bool flag = math.any(math.step(float5, boundaryLimits.min).xyz + math.step(boundaryLimits.max, float5).xyz);
			if ((phases[index2] & 0x8000000) != 0)
			{
				life[index2] = ((flag && killOffLimits) ? 0f : life[index2]);
			}
			float5.xyz = math.clamp(float5, boundaryLimits.min, boundaryLimits.max).xyz;
			float6.xyz = math.clamp(float6, boundaryLimits.min, boundaryLimits.max).xyz;
			positions[index2] = float5;
			prevPositions[index2] = float6;
		}
	}
}
