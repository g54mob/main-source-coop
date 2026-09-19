using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct DecimateChunksJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> inputFrameOffsets;

		[NativeDisableParallelForRestriction]
		public NativeArray<BurstPathFrame> inputFrames;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> outputFrameCounts;

		[ReadOnly]
		public NativeArray<BurstPathSmootherData> pathData;

		public void Execute(int i)
		{
			int num = ((i > 0) ? inputFrameOffsets[i - 1] : 0);
			int num2 = inputFrameOffsets[i] - num;
			if (pathData[i].decimation < 1E-05f || num2 < 3)
			{
				outputFrameCounts[i] = num2;
				return;
			}
			float num3 = pathData[i].decimation * pathData[i].decimation * 0.01f;
			int num4 = 0;
			int num5 = num2 - 1;
			outputFrameCounts[i] = 0;
			while (num4 < num5)
			{
				inputFrames[num + outputFrameCounts[i]++] = inputFrames[num + num4];
				int num6 = num5;
				while (true)
				{
					int num7 = 0;
					float num8 = 0f;
					for (int j = num4 + 1; j < num6; j++)
					{
						float mu;
						float num9 = math.lengthsq(BurstMath.NearestPointOnEdge(inputFrames[num + num4].position, inputFrames[num + num6].position, inputFrames[num + j].position, out mu) - inputFrames[num + j].position);
						if (num9 > num8)
						{
							num7 = j;
							num8 = num9;
						}
					}
					if (num8 <= num3)
					{
						break;
					}
					num6 = num7;
				}
				num4 = num6;
			}
			inputFrames[num + outputFrameCounts[i]++] = inputFrames[num + num5];
		}
	}
}
