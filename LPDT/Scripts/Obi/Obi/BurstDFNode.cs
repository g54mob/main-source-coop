using Unity.Mathematics;

namespace Obi
{
	public struct BurstDFNode
	{
		public float4 distancesA;

		public float4 distancesB;

		public float4 center;

		public int firstChild;

		private int pad0;

		private int pad1;

		private int pad2;

		public float4 SampleWithGradient(float4 position)
		{
			float4 normalizedPos = GetNormalizedPos(position);
			float4 float5 = distancesA + (distancesB - distancesA) * normalizedPos[0];
			float2 float6 = float5.xy + (float5.zw - float5.xy) * normalizedPos[1];
			float w = float6[0] + (float6[1] - float6[0]) * normalizedPos[2];
			float2 float7 = distancesA.xy + (distancesA.zw - distancesA.xy) * normalizedPos[1];
			float num = float7[0] + (float7[1] - float7[0]) * normalizedPos[2];
			float7 = distancesB.xy + (distancesB.zw - distancesB.xy) * normalizedPos[1];
			float num2 = float7[0] + (float7[1] - float7[0]) * normalizedPos[2];
			float num3 = float5[0] + (float5[1] - float5[0]) * normalizedPos[2];
			float num4 = float5[2] + (float5[3] - float5[2]) * normalizedPos[2];
			return new float4(num2 - num, num4 - num3, float6[1] - float6[0], w);
		}

		public float4 GetNormalizedPos(float4 position)
		{
			float4 float5 = center - new float4(center[3]);
			return (position - float5) / (center[3] * 2f);
		}

		public int GetOctant(float4 position)
		{
			int num = 0;
			if (position[0] > center[0])
			{
				num |= 4;
			}
			if (position[1] > center[1])
			{
				num |= 2;
			}
			if (position[2] > center[2])
			{
				num |= 1;
			}
			return num;
		}
	}
}
