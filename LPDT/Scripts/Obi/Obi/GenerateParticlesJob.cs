using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct GenerateParticlesJob : IJobParallelFor
	{
		[ReadOnly]
		[DeallocateOnJobCompletion]
		public NativeArray<int> activeParticles;

		[ReadOnly]
		public NativeArray<float4> positions;

		[ReadOnly]
		public NativeArray<float4> velocities;

		[ReadOnly]
		public NativeArray<float4> principalRadii;

		[ReadOnly]
		public NativeArray<float4> fluidData;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> angularVelocities;

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

		public float2 vorticityRange;

		public float2 velocityRange;

		public float potentialIncrease;

		public float potentialDiffusion;

		public float foamGenerationRate;

		public float lifetime;

		public float lifetimeRandom;

		public float particleSize;

		public float sizeRandom;

		public float buoyancy;

		public float drag;

		public float airdrag;

		public float airAging;

		public float isosurface;

		public float4 foamColor;

		public float randomSeed;

		public float deltaTime;

		public unsafe void Execute(int i)
		{
			int* unsafePtr = (int*)dispatchBuffer.GetUnsafePtr();
			int num = activeParticles[i];
			float4 value = angularVelocities[num];
			float2 enc = BurstMath.UnpackFloatRG(value.w);
			float num2 = BurstMath.Remap01(fluidData[num].z, vorticityRange.x, vorticityRange.y);
			float num3 = BurstMath.Remap01(math.length(velocities[num].xyz), velocityRange.x, velocityRange.y);
			float num4 = num3 * num2 * deltaTime * potentialIncrease;
			enc.y = math.saturate(enc.y * potentialDiffusion + num4);
			enc.x += foamGenerationRate * enc.y * deltaTime;
			int num5 = (int)enc.x;
			enc.x -= num5;
			for (int j = 0; j < num5; j++)
			{
				int num6 = Interlocked.Add(ref unsafePtr[3], 1) - 1;
				if (num6 < outputPositions.Length)
				{
					BurstMath.RandomInCylinder(randomSeed + (float)num + (float)j, positions[num], math.normalizesafe(velocities[num]), math.length(velocities[num]) * deltaTime, principalRadii[num].x, out var position, out var velocity);
					float2 float5 = BurstMath.Hash21(randomSeed - (float)num - (float)j);
					float num7 = num3 * (lifetime - lifetime * float5.x * lifetimeRandom);
					float z = particleSize - particleSize * float5.y * sizeRandom;
					outputPositions[num6] = position;
					outputVelocities[num6] = velocities[num] + new float4(velocity, buoyancy);
					outputColors[num6] = foamColor;
					outputAttributes[num6] = new float4(1f, 1f / num7, z, BurstMath.PackFloatRGBA(new float4(airAging / 50f, airdrag, drag, isosurface)));
				}
			}
			value.w = BurstMath.PackFloatRG(enc);
			angularVelocities[num] = value;
		}
	}
}
