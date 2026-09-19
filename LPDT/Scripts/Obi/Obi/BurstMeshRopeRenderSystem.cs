using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class BurstMeshRopeRenderSystem : ObiMeshRopeRenderSystem
	{
		[BurstCompile]
		private struct BuildRopeMeshJob : IJobParallelFor
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
			public NativeArray<int> meshIndices;

			[ReadOnly]
			public NativeArray<MeshDataBatch.MeshData> meshData;

			[ReadOnly]
			public NativeArray<BurstMeshData> rendererData;

			[ReadOnly]
			public NativeArray<BurstPathSmootherData> pathData;

			[ReadOnly]
			public NativeArray<int> sortedIndices;

			[ReadOnly]
			public NativeArray<int> sortedOffsets;

			[ReadOnly]
			public NativeArray<float3> positions;

			[ReadOnly]
			public NativeArray<float3> normals;

			[ReadOnly]
			public NativeArray<float4> tangents;

			[ReadOnly]
			public NativeArray<float4> colors;

			[NativeDisableParallelForRestriction]
			public NativeArray<RopeMeshVertex> vertices;

			[ReadOnly]
			public int firstRenderer;

			public void Execute(int i)
			{
				int index = firstRenderer + i;
				int index2 = pathSmootherIndices[index];
				BurstMeshData burstMeshData = rendererData[index];
				MeshDataBatch.MeshData meshData = this.meshData[meshIndices[index]];
				int num = sortedOffsets[index];
				int num2 = vertexOffsets[index];
				int index3 = chunkOffsets[index2];
				int num3 = frameOffsets[index3];
				int num4 = num3 + frameCounts[index3] - 1;
				int axis = (int)burstMeshData.axis;
				float3 float5 = (Vector3)burstMeshData.scale;
				float num5 = ((burstMeshData.stretchWithRope == 1) ? (pathData[index3].smoothLength / pathData[index3].restLength) : 1f);
				float num6 = math.clamp(1f + burstMeshData.volumeScaling * (1f / math.max(num5, 0.01f) - 1f), 0.01f, 2f);
				if (burstMeshData.spanEntireLength == 1)
				{
					float num7 = burstMeshData.meshSizeAlongAxis * (float)burstMeshData.instances;
					float num8 = burstMeshData.instanceSpacing * (float)(burstMeshData.instances - 1);
					float5[axis] = pathData[index3].restLength / (num7 + num8);
				}
				float5[axis] *= num5;
				float num9 = burstMeshData.offset;
				int num10 = num3;
				int num11 = num3 + 1;
				int num12 = num3;
				float num13 = math.distance(frames[num10].position, frames[num11].position);
				float num14 = num13;
				for (int j = 0; j < burstMeshData.instances; j++)
				{
					for (int k = 0; k < meshData.vertexCount; k++)
					{
						int index4 = meshData.firstVertex + sortedIndices[num + k];
						int index5 = meshData.firstVertex + sortedIndices[num + math.max(0, k - 1)];
						num9 += (positions[index4][axis] - positions[index5][axis]) * float5[axis];
						BurstPathFrame burstPathFrame;
						if (num9 < 0f)
						{
							while (0f - num9 > num14 && num10 > num3)
							{
								num9 += num14;
								num10 = math.max(num10 - 1, num3);
								num11 = math.min(num10 + 1, num4);
								num12 = math.max(num10 - 1, num3);
								num13 = math.distance(frames[num10].position, frames[num11].position);
								num14 = math.distance(frames[num10].position, frames[num12].position);
							}
							float3 float6 = float3.zero;
							if (num10 == num12)
							{
								float6 = frames[num10].position - frames[num11].position;
								num14 = math.length(float6);
							}
							burstPathFrame = InterpolateFrames(frames[num10], frames[num12], float6, (0f - num9) / num14);
						}
						else
						{
							while (num9 > num13 && num10 < num4)
							{
								num9 -= num13;
								num10 = math.min(num10 + 1, num4);
								num11 = math.min(num10 + 1, num4);
								num12 = math.max(num10 - 1, num3);
								num13 = math.distance(frames[num10].position, frames[num11].position);
								num14 = math.distance(frames[num10].position, frames[num12].position);
							}
							float3 float7 = float3.zero;
							if (num10 == num11)
							{
								float7 = frames[num10].position - frames[num12].position;
								num13 = math.length(float7);
							}
							burstPathFrame = InterpolateFrames(frames[num10], frames[num11], float7, num9 / num13);
						}
						float3x3 a = burstPathFrame.ToMatrix(axis);
						float3 b = positions[index4] * float5 * burstPathFrame.thickness * num6;
						b[axis] = 0f;
						vertices[num2 + sortedIndices[num + k]] = new RopeMeshVertex
						{
							pos = burstPathFrame.position + math.mul(a, b),
							normal = math.mul(a, normals[index4]),
							tangent = new float4(math.mul(a, tangents[index4].xyz), tangents[index4].w),
							color = colors[index4] * burstPathFrame.color
						};
					}
					num2 += meshData.vertexCount;
					num9 += burstMeshData.instanceSpacing * float5[axis];
				}
			}

			private BurstPathFrame InterpolateFrames(BurstPathFrame a, BurstPathFrame b, float3 bOffset, float t)
			{
				b.position += bOffset;
				BurstPathFrame result = (1f - t) * a + t * b;
				result.normal = math.normalize(result.normal);
				result.binormal = math.normalize(result.binormal);
				return result;
			}
		}

		public BurstMeshRopeRenderSystem(ObiSolver solver)
			: base(solver)
		{
		}

		protected override void CloseBatches()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Initialize(sortedRenderers, meshData, meshIndices, layout);
			}
			base.CloseBatches();
		}

		public override void Render()
		{
			if (pathSmootherSystem == null)
			{
				return;
			}
			using (ObiMeshRopeRenderSystem.m_RenderMarker.Auto())
			{
				NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(batchList.Count, Allocator.Temp);
				for (int i = 0; i < batchList.Count; i++)
				{
					DynamicRenderBatch<ObiRopeMeshRenderer> dynamicRenderBatch = batchList[i];
					BuildRopeMeshJob jobData = new BuildRopeMeshJob
					{
						chunkOffsets = pathSmootherSystem.chunkOffsets.AsNativeArray<int>(),
						pathSmootherIndices = pathSmootherIndices.AsNativeArray<int>(),
						frames = pathSmootherSystem.smoothFrames.AsNativeArray<BurstPathFrame>(),
						frameOffsets = pathSmootherSystem.smoothFrameOffsets.AsNativeArray<int>(),
						frameCounts = pathSmootherSystem.smoothFrameCounts.AsNativeArray<int>(),
						vertexOffsets = vertexOffsets.AsNativeArray<int>(),
						meshIndices = meshIndices.AsNativeArray<int>(),
						meshData = meshData.meshData.AsNativeArray<MeshDataBatch.MeshData>(),
						rendererData = rendererData.AsNativeArray<BurstMeshData>(),
						pathData = pathSmootherSystem.pathData.AsNativeArray<BurstPathSmootherData>(),
						sortedIndices = sortedIndices.AsNativeArray<int>(),
						sortedOffsets = sortedOffsets.AsNativeArray<int>(),
						positions = meshData.restPositions.AsNativeArray<float3>(),
						normals = meshData.restNormals.AsNativeArray<float3>(),
						tangents = meshData.restTangents.AsNativeArray<float4>(),
						colors = meshData.restColors.AsNativeArray<float4>(),
						vertices = dynamicRenderBatch.dynamicVertexData.AsNativeArray<RopeMeshVertex>(),
						firstRenderer = dynamicRenderBatch.firstRenderer
					};
					jobs[i] = IJobParallelForExtensions.Schedule(jobData, dynamicRenderBatch.rendererCount, 1);
				}
				JobHandle.CombineDependencies(jobs).Complete();
				jobs.Dispose();
				for (int j = 0; j < batchList.Count; j++)
				{
					DynamicRenderBatch<ObiRopeMeshRenderer> dynamicRenderBatch2 = batchList[j];
					dynamicRenderBatch2.mesh.SetVertexBufferData(dynamicRenderBatch2.dynamicVertexData.AsNativeArray<DynamicBatchVertex>(), 0, 0, dynamicRenderBatch2.vertexCount, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
					RenderParams rparams = dynamicRenderBatch2.renderParams;
					rparams.worldBounds = m_Solver.bounds;
					for (int k = 0; k < dynamicRenderBatch2.materials.Length; k++)
					{
						rparams.material = dynamicRenderBatch2.materials[k];
						Graphics.RenderMesh(in rparams, dynamicRenderBatch2.mesh, k, m_Solver.transform.localToWorldMatrix, m_Solver.transform.localToWorldMatrix);
					}
				}
			}
		}
	}
}
