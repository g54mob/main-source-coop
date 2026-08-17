using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace NomadDrive.Features.EvilRoads.Jobs
{
	[BurstCompile]
	public struct RoadTerrainDeformJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<float3> WorldVerts;

		[ReadOnly]
		public NativeArray<int> Triangles;

		[ReadOnly]
		public NativeArray<float> InHeights;

		[WriteOnly]
		public NativeArray<float> OutHeights;

		[WriteOnly]
		public NativeArray<byte> Zone;

		public int SubWidth;

		public int MinX;

		public int MinZ;

		public int HeightmapResolution;

		public float3 TerrainPosition;

		public float3 TerrainSize;

		public float RoadSurfaceAdjustment;

		public float InfluenceDistance;

		public float BlendingSmoothness;

		public void Execute(int index)
		{
			int num = index % SubWidth;
			int num2 = index / SubWidth;
			int num3 = MinX + num;
			int num4 = MinZ + num2;
			float x = TerrainPosition.x + (float)num3 / (float)(HeightmapResolution - 1) * TerrainSize.x;
			float y = TerrainPosition.z + (float)num4 / (float)(HeightmapResolution - 1) * TerrainSize.z;
			float2 p = new float2(x, y);
			float num5 = 3.4028235E+38f;
			float num6 = 0f;
			bool flag = false;
			int length = Triangles.Length;
			for (int i = 0; i + 2 < length; i += 3)
			{
				float3 float5 = WorldVerts[Triangles[i]];
				float3 float6 = WorldVerts[Triangles[i + 1]];
				float3 float7 = WorldVerts[Triangles[i + 2]];
				float2 a = new float2(float5.x, float5.z);
				float2 b = new float2(float6.x, float6.z);
				float2 c = new float2(float7.x, float7.z);
				if (RoadJobMath.PointInTriangle(p, a, b, c))
				{
					num6 = RoadJobMath.InterpolateY(p, a, b, c, float5.y, float6.y, float7.y);
					num5 = 0f;
					flag = true;
					break;
				}
				float num7 = RoadJobMath.DistanceToTriangle(p, a, b, c);
				if (num7 < num5)
				{
					num5 = num7;
					num6 = RoadJobMath.InterpolateY(p, a, b, c, float5.y, float6.y, float7.y);
				}
			}
			float num8 = InHeights[index];
			if (!flag && num5 > InfluenceDistance)
			{
				OutHeights[index] = num8;
				Zone[index] = 0;
				return;
			}
			float end = num8 * TerrainSize.y + TerrainPosition.y;
			float num9 = num6 + RoadSurfaceAdjustment;
			float num10;
			byte value;
			if (flag)
			{
				num10 = num9;
				value = 2;
			}
			else
			{
				float num11 = math.saturate(num5 / InfluenceDistance);
				float t = math.pow(num11 * num11 * (3f - 2f * num11), BlendingSmoothness);
				num10 = math.lerp(num9, end, t);
				value = 1;
			}
			OutHeights[index] = math.saturate((num10 - TerrainPosition.y) / TerrainSize.y);
			Zone[index] = value;
		}
	}
}
