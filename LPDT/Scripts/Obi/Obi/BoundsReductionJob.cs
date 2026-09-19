using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Obi
{
	[BurstCompile]
	internal struct BoundsReductionJob : IJobParallelFor
	{
		[NativeDisableParallelForRestriction]
		public NativeArray<BurstAabb> bounds;

		[ReadOnly]
		public int stride;

		[ReadOnly]
		public int size;

		public void Execute(int first)
		{
			int num = first * size;
			for (int i = 1; i < size; i++)
			{
				int index = num * stride;
				int num2 = (num + i) * stride;
				if (num2 < bounds.Length)
				{
					BurstAabb value = bounds[index];
					value.EncapsulateBounds(bounds[num2]);
					bounds[index] = value;
				}
			}
		}
	}
}
