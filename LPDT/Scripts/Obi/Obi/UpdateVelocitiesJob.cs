using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct UpdateVelocitiesJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> activeParticles;

		[ReadOnly]
		public NativeArray<float> inverseMasses;

		[ReadOnly]
		public NativeArray<float4> previousPositions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> positions;

		[NativeDisableParallelForRestriction]
		[WriteOnly]
		public NativeArray<float4> velocities;

		[ReadOnly]
		public NativeArray<float> inverseRotationalMasses;

		[ReadOnly]
		public NativeArray<quaternion> previousOrientations;

		[NativeDisableParallelForRestriction]
		public NativeArray<quaternion> orientations;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> angularVelocities;

		[ReadOnly]
		public float deltaTime;

		[ReadOnly]
		public bool is2D;

		public void Execute(int index)
		{
			int index2 = activeParticles[index];
			if (is2D)
			{
				float4 value = positions[index2];
				value[2] = previousPositions[index2][2];
				positions[index2] = value;
			}
			if (inverseMasses[index2] > 0f)
			{
				velocities[index2] = BurstIntegration.DifferentiateLinear(positions[index2], previousPositions[index2], deltaTime);
			}
			else
			{
				velocities[index2] = float4.zero;
			}
			if (inverseRotationalMasses[index2] > 0f)
			{
				angularVelocities[index2] = new float4(BurstIntegration.DifferentiateAngular(orientations[index2], previousOrientations[index2], deltaTime).xyz, angularVelocities[index2].w);
			}
			else
			{
				angularVelocities[index2] = float4.zero;
			}
		}
	}
}
