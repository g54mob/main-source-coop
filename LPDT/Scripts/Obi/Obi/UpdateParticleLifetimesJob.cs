using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Obi
{
	[BurstCompile]
	internal struct UpdateParticleLifetimesJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> activeParticles;

		[NativeDisableParallelForRestriction]
		public NativeArray<float> life;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> deadParticles;

		[NativeDisableContainerSafetyRestriction]
		public NativeReference<int> deadParticleCount;

		[ReadOnly]
		public float dt;

		public unsafe void Execute(int i)
		{
			int num = activeParticles[i];
			life[num] -= dt;
			if (life[num] <= 0f)
			{
				int index = Interlocked.Increment(ref *deadParticleCount.GetUnsafePtr()) - 1;
				deadParticles[index] = num;
				life[num] = 0f;
			}
		}
	}
}
