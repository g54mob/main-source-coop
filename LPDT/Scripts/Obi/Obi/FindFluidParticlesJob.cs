using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Obi
{
	[BurstCompile]
	internal struct FindFluidParticlesJob : IJob
	{
		[ReadOnly]
		public NativeArray<int> activeParticles;

		[ReadOnly]
		public NativeArray<int> phases;

		public NativeList<int> fluidParticles;

		public void Execute()
		{
			fluidParticles.Clear();
			for (int i = 0; i < activeParticles.Length; i++)
			{
				int value = activeParticles[i];
				if ((phases[value] & 0x2000000) != 0)
				{
					fluidParticles.Add(in value);
				}
			}
		}
	}
}
