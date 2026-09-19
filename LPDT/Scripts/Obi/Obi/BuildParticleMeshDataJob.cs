using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Obi
{
	[BurstCompile]
	internal struct BuildParticleMeshDataJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<int> particleIndices;

		[ReadOnly]
		public NativeArray<int> rendererIndices;

		[ReadOnly]
		public NativeArray<ParticleRendererData> rendererData;

		[ReadOnly]
		public NativeArray<float4> renderablePositions;

		[ReadOnly]
		public NativeArray<quaternion> renderableOrientations;

		[ReadOnly]
		public NativeArray<float4> renderableRadii;

		[ReadOnly]
		public NativeArray<float4> colors;

		[NativeDisableParallelForRestriction]
		public NativeArray<ParticleVertex> vertices;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> indices;

		[ReadOnly]
		public int firstParticle;

		public void Execute(int i)
		{
			int index = particleIndices[firstParticle + i];
			int index2 = rendererIndices[firstParticle + i];
			ParticleVertex value = new ParticleVertex
			{
				pos = new float4(renderablePositions[index].xyz, 1f),
				color = colors[index] * (Vector4)rendererData[index2].color,
				b1 = new float4(math.mul(renderableOrientations[index], new float3(1f, 0f, 0f)), renderableRadii[index][0] * renderableRadii[index][3] * rendererData[index2].radiusScale),
				b2 = new float4(math.mul(renderableOrientations[index], new float3(0f, 1f, 0f)), renderableRadii[index][1] * renderableRadii[index][3] * rendererData[index2].radiusScale),
				b3 = new float4(math.mul(renderableOrientations[index], new float3(0f, 0f, 1f)), renderableRadii[index][2] * renderableRadii[index][3] * rendererData[index2].radiusScale),
				offset = new float3(1f, 1f, 0f)
			};
			vertices[i * 4] = value;
			value.offset = new float3(-1f, 1f, 0f);
			vertices[i * 4 + 1] = value;
			value.offset = new float3(-1f, -1f, 0f);
			vertices[i * 4 + 2] = value;
			value.offset = new float3(1f, -1f, 0f);
			vertices[i * 4 + 3] = value;
			indices[i * 6] = i * 4 + 2;
			indices[i * 6 + 1] = i * 4 + 1;
			indices[i * 6 + 2] = i * 4;
			indices[i * 6 + 3] = i * 4 + 3;
			indices[i * 6 + 4] = i * 4 + 2;
			indices[i * 6 + 5] = i * 4;
		}
	}
}
