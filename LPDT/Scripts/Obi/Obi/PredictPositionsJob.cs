using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct PredictPositionsJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> activeParticles;

		[ReadOnly]
		public NativeArray<int> phases;

		[ReadOnly]
		public NativeArray<float4> buoyancies;

		[ReadOnly]
		public NativeArray<float4> externalForces;

		[ReadOnly]
		public NativeArray<float> inverseMasses;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> previousPositions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> positions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> velocities;

		[ReadOnly]
		public NativeArray<float4> externalTorques;

		[ReadOnly]
		public NativeArray<float> inverseRotationalMasses;

		[NativeDisableParallelForRestriction]
		public NativeArray<quaternion> previousOrientations;

		[NativeDisableParallelForRestriction]
		public NativeArray<quaternion> orientations;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> angularVelocities;

		[ReadOnly]
		public float4 gravity;

		[ReadOnly]
		public float deltaTime;

		[ReadOnly]
		public bool is2D;

		public void Execute(int index)
		{
			int index2 = activeParticles[index];
			previousPositions[index2] = positions[index2];
			previousOrientations[index2] = orientations[index2];
			if (inverseMasses[index2] > 0f)
			{
				float4 float5 = gravity;
				if ((phases[index2] & 0x2000000) != 0)
				{
					float5 *= 0f - buoyancies[index2].z;
				}
				float4 value = velocities[index2] + (inverseMasses[index2] * externalForces[index2] + float5) * deltaTime;
				if (is2D)
				{
					value[3] = 0f;
				}
				velocities[index2] = value;
			}
			if (inverseRotationalMasses[index2] > 0f)
			{
				float3 float6 = angularVelocities[index2].xyz + inverseRotationalMasses[index2] * externalTorques[index2].xyz * deltaTime;
				if (is2D)
				{
					float6 = float6.project(new float3(0f, 0f, 1f));
				}
				angularVelocities[index2] = new float4(float6, angularVelocities[index2].w);
			}
			positions[index2] = BurstIntegration.IntegrateLinear(positions[index2], velocities[index2], deltaTime);
			orientations[index2] = BurstIntegration.IntegrateAngular(orientations[index2], angularVelocities[index2], deltaTime);
		}
	}
}
