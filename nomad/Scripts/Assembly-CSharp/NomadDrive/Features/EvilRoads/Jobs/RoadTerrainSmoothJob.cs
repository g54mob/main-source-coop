using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace NomadDrive.Features.EvilRoads.Jobs
{
	[BurstCompile]
	public struct RoadTerrainSmoothJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<float> Source;

		[ReadOnly]
		public NativeArray<byte> Zone;

		[WriteOnly]
		public NativeArray<float> Output;

		public int SubWidth;

		public int SubHeight;

		public void Execute(int index)
		{
			if (Zone[index] != 1)
			{
				Output[index] = Source[index];
				return;
			}
			int num = index % SubWidth;
			int num2 = index / SubWidth;
			float num3 = 0f;
			int num4 = 0;
			for (int i = -1; i <= 1; i++)
			{
				int num5 = num2 + i;
				if (num5 < 0 || num5 >= SubHeight)
				{
					continue;
				}
				for (int j = -1; j <= 1; j++)
				{
					int num6 = num + j;
					if (num6 >= 0 && num6 < SubWidth)
					{
						num3 += Source[num5 * SubWidth + num6];
						num4++;
					}
				}
			}
			float num7 = Source[index];
			if (num4 > 0)
			{
				float y = math.lerp(num7, num3 / (float)num4, 0.4f);
				Output[index] = math.min(num7, y);
			}
			else
			{
				Output[index] = num7;
			}
		}
	}
}
