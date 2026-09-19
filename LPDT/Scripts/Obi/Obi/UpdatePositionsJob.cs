using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct UpdatePositionsJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> activeParticles;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> positions;

		[ReadOnly]
		public NativeArray<float4> previousPositions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> velocities;

		[NativeDisableParallelForRestriction]
		public NativeArray<quaternion> orientations;

		[ReadOnly]
		public NativeArray<quaternion> previousOrientations;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> angularVelocities;

		[ReadOnly]
		public float velocityScale;

		[ReadOnly]
		public float sleepThreshold;

		[ReadOnly]
		public float maxVelocity;

		[ReadOnly]
		public float maxAngularVelocity;

		public void Execute(int index)
		{
			int index2 = activeParticles[index];
			float4 float5 = velocities[index2];
			float4 value = angularVelocities[index2];
			float5 *= velocityScale;
			value.xyz *= velocityScale;
			float num = math.length(float5);
			float num2 = math.length(value.xyz);
			if (num > 1E-07f)
			{
				float5 *= math.min(maxVelocity, num) / num;
			}
			if (num2 > 1E-07f)
			{
				value.xyz *= math.min(maxAngularVelocity, num2) / num2;
			}
			if (num * num * 0.5f + num2 * num2 * 0.5f <= sleepThreshold)
			{
				positions[index2] = previousPositions[index2];
				orientations[index2] = previousOrientations[index2];
				float5 = float4.zero;
				value.xyz = float3.zero;
			}
			velocities[index2] = float5;
			angularVelocities[index2] = value;
		}
	}
}
