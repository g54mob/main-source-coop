using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct UpdateEdgeNormalsJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> deformableEdges;

		[ReadOnly]
		public NativeArray<float4> velocities;

		[ReadOnly]
		public NativeArray<float4> wind;

		[ReadOnly]
		public NativeArray<float4> renderPositions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> normals;

		public void Execute(int i)
		{
			int num = deformableEdges[i * 2];
			int num2 = deformableEdges[i * 2 + 1];
			float4 ontoB = renderPositions[num2] - renderPositions[num];
			float4 obj = (velocities[num] + velocities[num2]) * 0.5f - (wind[num] + wind[num2]) * 0.5f;
			float4 data = obj - math.projectsafe(obj, ontoB);
			BurstMath.AtomicAdd(normals, num, data);
			BurstMath.AtomicAdd(normals, num2, data);
		}
	}
}
