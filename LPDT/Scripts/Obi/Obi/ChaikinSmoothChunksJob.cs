using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct ChaikinSmoothChunksJob : IJobParallelFor
	{
		[NativeDisableParallelForRestriction]
		public NativeArray<BurstPathFrame> inputFrames;

		[ReadOnly]
		public NativeArray<int> inputFrameOffsets;

		[ReadOnly]
		public NativeArray<int> inputFrameCounts;

		[NativeDisableParallelForRestriction]
		public NativeArray<BurstPathFrame> outputFrames;

		[ReadOnly]
		public NativeArray<int> outputFrameOffsets;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> outputFrameCounts;

		[NativeDisableParallelForRestriction]
		public NativeArray<BurstPathSmootherData> pathData;

		public void Execute(int i)
		{
			int num = ((i > 0) ? inputFrameOffsets[i - 1] : 0);
			int num2 = inputFrameCounts[i];
			int num3 = outputFrameOffsets[i];
			int smoothing = (int)pathData[i].smoothing;
			if (smoothing == 0)
			{
				outputFrameCounts[i] = num2;
				for (int j = 0; j < num2; j++)
				{
					outputFrames[num3 + j] = inputFrames[num + j];
				}
			}
			else
			{
				int num4 = (int)math.pow(2f, smoothing);
				int num5 = num2 - 1;
				float num6 = math.pow(2f, -(smoothing + 1));
				float num7 = math.pow(2f, -smoothing);
				float num8 = math.pow(2f, -2 * smoothing);
				float num9 = math.pow(2f, -2 * smoothing - 1);
				outputFrameCounts[i] = (num2 - 2) * num4 + 2;
				outputFrames[num3] = (0.5f + num6) * inputFrames[num] + (0.5f - num6) * inputFrames[num + 1];
				outputFrames[num3 + num4 * num5 - num4 + 1] = (0.5f - num6) * inputFrames[num + num5 - 1] + (0.5f + num6) * inputFrames[num + num5];
				for (int k = 1; k <= num4; k++)
				{
					float w = 0.5f - num6 - (float)(k - 1) * (num7 - (float)k * num9);
					float w2 = 0.5f + num6 + (float)(k - 1) * (num7 - (float)k * num8);
					float w3 = (float)((k - 1) * k) * num9;
					for (int l = 1; l < num5; l++)
					{
						BurstPathFrame.WeightedSum(w, w2, w3, in GetElementAsRef(inputFrames, num + l - 1), in GetElementAsRef(inputFrames, num + l), in GetElementAsRef(inputFrames, num + l + 1), ref GetElementAsRef(outputFrames, num3 + (l - 1) * num4 + k));
					}
				}
				outputFrames[num3] = inputFrames[num];
				outputFrames[num3 + outputFrameCounts[i] - 1] = inputFrames[num + num2 - 1];
			}
			BurstPathSmootherData value = pathData[i];
			value.smoothLength = 0f;
			for (int m = num3 + 1; m < num3 + outputFrameCounts[i]; m++)
			{
				value.smoothLength += math.distance(outputFrames[m - 1].position, outputFrames[m].position);
			}
			pathData[i] = value;
		}

		private unsafe static ref T GetElementAsRef<T>(NativeArray<T> array, int index) where T : unmanaged
		{
			return ref UnsafeUtility.ArrayElementAsRef<T>(array.GetUnsafePtr(), index);
		}
	}
}
