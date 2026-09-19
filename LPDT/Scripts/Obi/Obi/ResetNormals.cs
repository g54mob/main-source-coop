using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct ResetNormals : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> phases;

		public NativeArray<float4> normals;

		[WriteOnly]
		public NativeArray<float4> tangents;

		public void Execute(int i)
		{
			if ((phases[i] & 0x2000000) == 0 && normals[i].w >= 0f)
			{
				normals[i] = float4.zero;
				tangents[i] = float4.zero;
			}
		}
	}
}
