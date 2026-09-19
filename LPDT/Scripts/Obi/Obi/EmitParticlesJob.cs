using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct EmitParticlesJob : IJobParallelFor
	{
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

		public uint emitterShape;

		public float4 emitterPosition;

		public quaternion emitterRotation;

		public float4 emitterSize;

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
			int num = Interlocked.Add(ref unsafePtr[3], 1) - 1;
			if (num < outputPositions.Length)
			{
				float4 position;
				float3 velocity;
				if (emitterShape == 0)
				{
					BurstMath.RandomInCylinder(randomSeed + (float)i, -new float4(0f, 1f, 0f, 0f) * emitterSize.y * 0.5f, new float4(0f, 1f, 0f, 0f), emitterSize.y, math.max(emitterSize.x, emitterSize.z) * 0.5f, out position, out velocity);
				}
				else
				{
					BurstMath.RandomInBox(randomSeed + (float)i, float4.zero, emitterSize, out position, out velocity);
				}
				float2 float5 = BurstMath.Hash21(randomSeed - (float)i);
				float num2 = math.max(0f, lifetime - lifetime * float5.x * lifetimeRandom);
				float z = particleSize - particleSize * float5.y * sizeRandom;
				outputPositions[num] = new float4(emitterPosition.xyz + math.rotate(emitterRotation, position.xyz), 0f);
				outputVelocities[num] = new float4(0f, 0f, 0f, buoyancy);
				outputColors[num] = foamColor;
				outputAttributes[num] = new float4(1f, 1f / num2, z, BurstMath.PackFloatRGBA(new float4(airAging / 50f, airdrag, drag, isosurface)));
			}
		}
	}
}
