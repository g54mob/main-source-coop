using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace NomadDrive.Features.EvilRoads.Jobs
{
	[BurstCompile]
	public struct TreeCullJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<float2> TreePositions;

		[ReadOnly]
		public NativeArray<float2> RoadPoints;

		[WriteOnly]
		public NativeArray<bool> Remove;

		public float ClearRadiusSqr;

		public float4 Bounds;

		public void Execute(int index)
		{
			float2 float5 = TreePositions[index];
			if (float5.x < Bounds.x || float5.x > Bounds.y || float5.y < Bounds.z || float5.y > Bounds.w)
			{
				Remove[index] = false;
				return;
			}
			bool value = false;
			int length = RoadPoints.Length;
			for (int i = 0; i < length; i++)
			{
				if (math.lengthsq(float5 - RoadPoints[i]) <= ClearRadiusSqr)
				{
					value = true;
					break;
				}
			}
			Remove[index] = value;
		}
	}
}
