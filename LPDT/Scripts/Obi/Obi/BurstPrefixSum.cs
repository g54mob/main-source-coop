using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstPrefixSum
	{
		[BurstCompile]
		private struct BlockSumJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> input;

			[NativeDisableParallelForRestriction]
			public NativeArray<int> output;

			public NativeArray<int> blocks;

			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe int* count;

			public unsafe void Execute(int block)
			{
				int num = *count + 1;
				int num2 = (int)math.ceil((float)num / 8f);
				int num3 = block * num2;
				int num4 = math.min(num3 + num2, num);
				output[num3] = 0;
				if (num2 == 0)
				{
					blocks[block] = 0;
					return;
				}
				for (int i = num3 + 1; i < num4; i++)
				{
					output[i] = output[i - 1] + input[i - 1];
				}
				blocks[block] = output[num4 - 1] + input[num4 - 1];
			}
		}

		[BurstCompile]
		private struct BlockSum : IJob
		{
			public NativeArray<int> blocks;

			public void Execute()
			{
				int num = blocks[0];
				blocks[0] = 0;
				for (int i = 1; i < blocks.Length; i++)
				{
					int num2 = blocks[i];
					blocks[i] = blocks[i - 1] + num;
					num = num2;
				}
			}
		}

		[BurstCompile]
		private struct PrefixSumJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> prefixBlocks;

			[NativeDisableParallelForRestriction]
			public NativeArray<int> output;

			[ReadOnly]
			[NativeDisableUnsafePtrRestriction]
			public unsafe int* count;

			public unsafe void Execute(int block)
			{
				int num = *count + 1;
				int num2 = (int)math.ceil((float)num / 8f);
				int num3 = block * num2;
				int num4 = math.min(num3 + num2, num);
				for (int i = num3; i < num4; i++)
				{
					output[i] += prefixBlocks[block];
				}
			}
		}

		private int inputSize;

		private const int numBlocks = 8;

		private NativeArray<int> blockSums;

		public BurstPrefixSum(int inputSize)
		{
			this.inputSize = inputSize;
			blockSums = new NativeArray<int>(8, Allocator.Persistent);
		}

		public void Dispose()
		{
			if (blockSums.IsCreated)
			{
				blockSums.Dispose();
			}
		}

		public unsafe JobHandle Sum(NativeArray<int> input, NativeArray<int> result, int* count, JobHandle inputDeps)
		{
			inputDeps = IJobParallelForExtensions.Schedule(new BlockSumJob
			{
				input = input,
				output = result,
				blocks = blockSums,
				count = count
			}, 8, 1, inputDeps);
			inputDeps = new BlockSum
			{
				blocks = blockSums
			}.Schedule(inputDeps);
			return IJobParallelForExtensions.Schedule(new PrefixSumJob
			{
				prefixBlocks = blockSums,
				output = result,
				count = count
			}, 8, 1, inputDeps);
		}
	}
}
