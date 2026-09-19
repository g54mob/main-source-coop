using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct UpdateTriangleNormalsJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> deformableTriangles;

		[ReadOnly]
		public NativeArray<float2> deformableTriangleUVs;

		[ReadOnly]
		public NativeArray<float4> renderPositions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> normals;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> tangents;

		public void Execute(int i)
		{
			int num = deformableTriangles[i * 3];
			int num2 = deformableTriangles[i * 3 + 1];
			int num3 = deformableTriangles[i * 3 + 2];
			float3 xyz = (renderPositions[num2] - renderPositions[num]).xyz;
			float3 xyz2 = (renderPositions[num3] - renderPositions[num]).xyz;
			float2 float5 = deformableTriangleUVs[i * 3 + 1] - deformableTriangleUVs[i * 3];
			float2 float6 = deformableTriangleUVs[i * 3 + 2] - deformableTriangleUVs[i * 3];
			float4 data = new float4(math.cross(xyz, xyz2), 0f);
			float4 data2 = float4.zero;
			float num4 = float5.x * float6.y - float6.x * float5.y;
			if (math.abs(num4) > 1E-07f)
			{
				data2 = new float4(float6.y * xyz.x - float5.y * xyz2.x, float6.y * xyz.y - float5.y * xyz2.y, float6.y * xyz.z - float5.y * xyz2.z, 0f) / num4;
			}
			BurstMath.AtomicAdd(normals, num, data);
			BurstMath.AtomicAdd(normals, num2, data);
			BurstMath.AtomicAdd(normals, num3, data);
			BurstMath.AtomicAdd(tangents, num, data2);
			BurstMath.AtomicAdd(tangents, num2, data2);
			BurstMath.AtomicAdd(tangents, num3, data2);
		}
	}
}
