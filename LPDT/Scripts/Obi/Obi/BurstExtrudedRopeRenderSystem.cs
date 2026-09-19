using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class BurstExtrudedRopeRenderSystem : ObiExtrudedRopeRenderSystem
	{
		[BurstCompile]
		private struct BuildExtrudedMesh : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> pathSmootherIndices;

			[ReadOnly]
			public NativeArray<int> chunkOffsets;

			[ReadOnly]
			public NativeArray<BurstPathFrame> frames;

			[ReadOnly]
			public NativeArray<int> frameOffsets;

			[ReadOnly]
			public NativeArray<int> frameCounts;

			[ReadOnly]
			public NativeArray<float2> sectionData;

			[ReadOnly]
			public NativeArray<int> sectionOffsets;

			[ReadOnly]
			public NativeArray<int> sectionIndices;

			[ReadOnly]
			public NativeArray<int> vertexOffsets;

			[ReadOnly]
			public NativeArray<int> triangleOffsets;

			[ReadOnly]
			public NativeArray<int> triangleCounts;

			[ReadOnly]
			public NativeArray<BurstExtrudedMeshData> rendererData;

			[ReadOnly]
			public NativeArray<BurstPathSmootherData> pathData;

			[NativeDisableParallelForRestriction]
			public NativeArray<ProceduralRopeVertex> vertices;

			[NativeDisableParallelForRestriction]
			public NativeArray<int> tris;

			[ReadOnly]
			public int firstRenderer;

			public void Execute(int u)
			{
				int index = firstRenderer + u;
				int num = pathSmootherIndices[index];
				float3 zero = float3.zero;
				float3 zero2 = float3.zero;
				float4 zero3 = float4.zero;
				int num2 = 0;
				int num3 = 0;
				int num4 = sectionOffsets[sectionIndices[index]];
				int num5 = sectionOffsets[sectionIndices[index] + 1] - num4 - 1;
				int num6 = num5 + 1;
				float num7 = 0f;
				for (int i = chunkOffsets[num]; i < chunkOffsets[num + 1]; i++)
				{
					num7 += pathData[i].smoothLength;
				}
				float num8 = (0f - rendererData[index].uvScale.y) * pathData[chunkOffsets[num]].restLength * rendererData[index].uvAnchor;
				float num9 = num7 / pathData[chunkOffsets[num]].restLength;
				int num10 = vertexOffsets[index];
				int num11 = triangleOffsets[index];
				for (int j = num11; j < num11 + triangleCounts[index]; j++)
				{
					int num12 = j * 3;
					tris[num12] = 0;
					tris[num12 + 1] = 0;
					tris[num12 + 2] = 0;
				}
				for (int k = chunkOffsets[num]; k < chunkOffsets[num + 1]; k++)
				{
					int num13 = frameOffsets[k];
					int num14 = frameCounts[k];
					for (int l = 0; l < num14; l++)
					{
						int index2 = num13 + math.max(l - 1, 0);
						int index3 = num13 + l;
						num8 += rendererData[index].uvScale.y * (math.distance(frames[index3].position, frames[index2].position) / ((rendererData[index].normalizeV == 1) ? num7 : num9));
						float num15 = frames[index3].thickness * rendererData[index].thicknessScale;
						int num16 = num3 + 1;
						for (int m = 0; m <= num5; m++)
						{
							float2 float5 = sectionData[num4 + m];
							zero2.x = (float5.x * frames[index3].normal.x + float5.y * frames[index3].binormal.x) * num15;
							zero2.y = (float5.x * frames[index3].normal.y + float5.y * frames[index3].binormal.y) * num15;
							zero2.z = (float5.x * frames[index3].normal.z + float5.y * frames[index3].binormal.z) * num15;
							zero.x = frames[index3].position.x + zero2.x;
							zero.y = frames[index3].position.y + zero2.y;
							zero.z = frames[index3].position.z + zero2.z;
							zero3.xyz = math.cross(zero2, frames[index3].tangent);
							zero3.w = -1f;
							vertices[num10 + num3 * num6 + m] = new ProceduralRopeVertex
							{
								pos = zero,
								normal = zero2,
								tangent = zero3,
								color = frames[index3].color,
								uv = new float2((float)m / (float)num5 * rendererData[index].uvScale.x, num8)
							};
							if (m < num5 && l < num14 - 1)
							{
								int num17 = num11 * 3;
								tris[num17 + num2++] = num10 + num3 * num6 + m;
								tris[num17 + num2++] = num10 + num16 * num6 + m;
								tris[num17 + num2++] = num10 + num3 * num6 + (m + 1);
								tris[num17 + num2++] = num10 + num3 * num6 + (m + 1);
								tris[num17 + num2++] = num10 + num16 * num6 + m;
								tris[num17 + num2++] = num10 + num16 * num6 + (m + 1);
							}
						}
						num3++;
					}
				}
			}
		}

		public BurstExtrudedRopeRenderSystem(ObiSolver solver)
			: base(solver)
		{
		}

		public override void Setup()
		{
			base.Setup();
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Initialize(layout);
			}
		}

		public override void Render()
		{
			if (pathSmootherSystem == null)
			{
				return;
			}
			using (ObiExtrudedRopeRenderSystem.m_RenderMarker.Auto())
			{
				NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(batchList.Count, Allocator.Temp);
				for (int i = 0; i < batchList.Count; i++)
				{
					ProceduralRenderBatch<ProceduralRopeVertex> proceduralRenderBatch = batchList[i];
					BuildExtrudedMesh jobData = new BuildExtrudedMesh
					{
						pathSmootherIndices = pathSmootherIndices.AsNativeArray<int>(),
						chunkOffsets = pathSmootherSystem.chunkOffsets.AsNativeArray<int>(),
						frames = pathSmootherSystem.smoothFrames.AsNativeArray<BurstPathFrame>(),
						frameOffsets = pathSmootherSystem.smoothFrameOffsets.AsNativeArray<int>(),
						frameCounts = pathSmootherSystem.smoothFrameCounts.AsNativeArray<int>(),
						sectionData = sectionData.AsNativeArray<float2>(),
						sectionOffsets = sectionOffsets.AsNativeArray<int>(),
						sectionIndices = sectionIndices.AsNativeArray<int>(),
						vertexOffsets = vertexOffsets.AsNativeArray<int>(),
						triangleOffsets = triangleOffsets.AsNativeArray<int>(),
						triangleCounts = triangleCounts.AsNativeArray<int>(),
						pathData = pathSmootherSystem.pathData.AsNativeArray<BurstPathSmootherData>(),
						rendererData = rendererData.AsNativeArray<BurstExtrudedMeshData>(),
						vertices = proceduralRenderBatch.vertices,
						tris = proceduralRenderBatch.triangles,
						firstRenderer = proceduralRenderBatch.firstRenderer
					};
					jobs[i] = IJobParallelForExtensions.Schedule(jobData, proceduralRenderBatch.rendererCount, 1);
				}
				JobHandle.CombineDependencies(jobs).Complete();
				jobs.Dispose();
				for (int j = 0; j < batchList.Count; j++)
				{
					ProceduralRenderBatch<ProceduralRopeVertex> proceduralRenderBatch2 = batchList[j];
					proceduralRenderBatch2.mesh.SetVertexBufferData(proceduralRenderBatch2.vertices, 0, 0, proceduralRenderBatch2.vertexCount, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
					proceduralRenderBatch2.mesh.SetIndexBufferData(proceduralRenderBatch2.triangles, 0, 0, proceduralRenderBatch2.triangleCount * 3, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
					RenderParams rparams = proceduralRenderBatch2.renderParams;
					rparams.worldBounds = m_Solver.bounds;
					Graphics.RenderMesh(in rparams, proceduralRenderBatch2.mesh, 0, m_Solver.transform.localToWorldMatrix, m_Solver.transform.localToWorldMatrix);
				}
			}
		}
	}
}
