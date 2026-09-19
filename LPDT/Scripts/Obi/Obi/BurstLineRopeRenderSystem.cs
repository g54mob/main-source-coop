using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class BurstLineRopeRenderSystem : ObiLineRopeRenderSystem
	{
		[BurstCompile]
		private struct BuildLineMesh : IJobParallelFor
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
			public NativeArray<int> vertexOffsets;

			[ReadOnly]
			public NativeArray<int> triangleOffsets;

			[ReadOnly]
			public NativeArray<int> triangleCounts;

			[ReadOnly]
			public NativeArray<BurstLineMeshData> rendererData;

			[ReadOnly]
			public NativeArray<BurstPathSmootherData> pathData;

			[NativeDisableParallelForRestriction]
			public NativeArray<ProceduralRopeVertex> vertices;

			[NativeDisableParallelForRestriction]
			public NativeArray<int> tris;

			[ReadOnly]
			public int firstRenderer;

			[ReadOnly]
			public float3 localSpaceCamera;

			public void Execute(int u)
			{
				int index = firstRenderer + u;
				int num = pathSmootherIndices[index];
				float3 zero = float3.zero;
				float3 float5 = float3.zero;
				float4 zero2 = float4.zero;
				int num2 = 0;
				int num3 = 0;
				int num4 = vertexOffsets[index];
				int num5 = triangleOffsets[index];
				float num6 = 0f;
				for (int i = chunkOffsets[num]; i < chunkOffsets[num + 1]; i++)
				{
					num6 += pathData[i].smoothLength;
				}
				float num7 = (0f - rendererData[index].uvScale.y) * pathData[chunkOffsets[num]].restLength * rendererData[index].uvAnchor;
				float num8 = num6 / pathData[chunkOffsets[num]].restLength;
				for (int j = num5; j < num5 + triangleCounts[index]; j++)
				{
					int num9 = j * 3;
					tris[num9] = 0;
					tris[num9 + 1] = 0;
					tris[num9 + 2] = 0;
				}
				for (int k = chunkOffsets[num]; k < chunkOffsets[num + 1]; k++)
				{
					int num10 = frameOffsets[k];
					int num11 = frameCounts[k];
					for (int l = 0; l < num11; l++)
					{
						int index2 = num10 + math.max(l - 1, 0);
						int index3 = num10 + l;
						num7 += rendererData[index].uvScale.y * (math.distance(frames[index3].position, frames[index2].position) / ((rendererData[index].normalizeV == 1) ? num6 : num8));
						float num12 = frames[index3].thickness * rendererData[index].thicknessScale;
						float5.x = frames[index3].position.x - localSpaceCamera.x;
						float5.y = frames[index3].position.y - localSpaceCamera.y;
						float5.z = frames[index3].position.z - localSpaceCamera.z;
						float5 = math.normalize(float5);
						zero2.x = 0f - (float5.y * frames[index3].tangent.z - float5.z * frames[index3].tangent.y);
						zero2.y = 0f - (float5.z * frames[index3].tangent.x - float5.x * frames[index3].tangent.z);
						zero2.z = 0f - (float5.x * frames[index3].tangent.y - float5.y * frames[index3].tangent.x);
						zero2.xyz = math.normalize(zero2.xyz);
						zero2.w = 1f;
						zero.x = frames[index3].position.x - zero2.x * num12;
						zero.y = frames[index3].position.y - zero2.y * num12;
						zero.z = frames[index3].position.z - zero2.z * num12;
						vertices[num4 + num3 * 2] = new ProceduralRopeVertex
						{
							pos = zero,
							normal = -float5,
							tangent = zero2,
							color = frames[index3].color,
							uv = new float2(0f, num7)
						};
						zero.x = frames[index3].position.x + zero2.x * num12;
						zero.y = frames[index3].position.y + zero2.y * num12;
						zero.z = frames[index3].position.z + zero2.z * num12;
						vertices[num4 + num3 * 2 + 1] = new ProceduralRopeVertex
						{
							pos = zero,
							normal = -float5,
							tangent = zero2,
							color = frames[index3].color,
							uv = new float2(1f, num7)
						};
						if (l < num11 - 1)
						{
							int num13 = num5 * 3;
							tris[num13 + num2++] = num4 + num3 * 2;
							tris[num13 + num2++] = num4 + (num3 + 1) * 2;
							tris[num13 + num2++] = num4 + num3 * 2 + 1;
							tris[num13 + num2++] = num4 + num3 * 2 + 1;
							tris[num13 + num2++] = num4 + (num3 + 1) * 2;
							tris[num13 + num2++] = num4 + (num3 + 1) * 2 + 1;
						}
						num3++;
					}
				}
			}
		}

		public BurstLineRopeRenderSystem(ObiSolver solver)
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
		}

		public override void RenderFromCamera(Camera camera)
		{
			if (pathSmootherSystem == null)
			{
				return;
			}
			using (ObiLineRopeRenderSystem.m_RenderMarker.Auto())
			{
				NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(batchList.Count, Allocator.Temp);
				for (int i = 0; i < batchList.Count; i++)
				{
					ProceduralRenderBatch<ProceduralRopeVertex> proceduralRenderBatch = batchList[i];
					BuildLineMesh jobData = new BuildLineMesh
					{
						pathSmootherIndices = pathSmootherIndices.AsNativeArray<int>(),
						chunkOffsets = pathSmootherSystem.chunkOffsets.AsNativeArray<int>(),
						frames = pathSmootherSystem.smoothFrames.AsNativeArray<BurstPathFrame>(),
						frameOffsets = pathSmootherSystem.smoothFrameOffsets.AsNativeArray<int>(),
						frameCounts = pathSmootherSystem.smoothFrameCounts.AsNativeArray<int>(),
						vertexOffsets = vertexOffsets.AsNativeArray<int>(),
						triangleOffsets = triangleOffsets.AsNativeArray<int>(),
						triangleCounts = triangleCounts.AsNativeArray<int>(),
						pathData = pathSmootherSystem.pathData.AsNativeArray<BurstPathSmootherData>(),
						rendererData = rendererData.AsNativeArray<BurstLineMeshData>(),
						vertices = proceduralRenderBatch.vertices,
						tris = proceduralRenderBatch.triangles,
						firstRenderer = proceduralRenderBatch.firstRenderer,
						localSpaceCamera = m_Solver.transform.InverseTransformPoint(camera.transform.position)
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
