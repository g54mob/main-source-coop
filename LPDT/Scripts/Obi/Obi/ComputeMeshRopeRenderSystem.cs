using UnityEngine;

namespace Obi
{
	public class ComputeMeshRopeRenderSystem : ObiMeshRopeRenderSystem
	{
		private ComputeShader ropeShader;

		private int updateKernel;

		public ComputeMeshRopeRenderSystem(ObiSolver solver)
			: base(solver)
		{
			ropeShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/RopeMeshRendering"));
			updateKernel = ropeShader.FindKernel("UpdateRopeMesh");
		}

		protected override void CloseBatches()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Initialize(sortedRenderers, meshData, meshIndices, layout, gpu: true);
			}
			meshData.PrepareForCompute();
			meshIndices.AsComputeBuffer<int>();
			sortedIndices.AsComputeBuffer<int>();
			sortedOffsets.AsComputeBuffer<int>();
			vertexOffsets.AsComputeBuffer<int>();
			pathSmootherIndices.AsComputeBuffer<int>();
			rendererData.AsComputeBuffer<BurstMeshData>();
			pathSmootherSystem.chunkOffsets.AsComputeBuffer<int>();
			base.CloseBatches();
		}

		public override void Render()
		{
			using (ObiMeshRopeRenderSystem.m_RenderMarker.Auto())
			{
				if (pathSmootherSystem == null || pathSmootherSystem.chunkOffsets == null || pathSmootherSystem.chunkOffsets.count <= 0)
				{
					return;
				}
				ropeShader.SetBuffer(updateKernel, "chunkOffsets", pathSmootherSystem.chunkOffsets.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "pathSmootherIndices", pathSmootherIndices.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "frames", pathSmootherSystem.smoothFrames.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "frameOffsets", pathSmootherSystem.smoothFrameOffsets.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "frameCounts", pathSmootherSystem.smoothFrameCounts.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "vertexOffsets", vertexOffsets.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "meshIndices", meshIndices.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "meshData", meshData.meshData.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "rendererData", rendererData.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "pathData", pathSmootherSystem.pathData.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "sortedIndices", sortedIndices.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "sortedOffsets", sortedOffsets.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "positions", meshData.restPositions.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "normals", meshData.restNormals.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "tangents", meshData.restTangents.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "colors", meshData.restColors.computeBuffer);
				for (int i = 0; i < batchList.Count; i++)
				{
					DynamicRenderBatch<ObiRopeMeshRenderer> dynamicRenderBatch = batchList[i];
					int threadGroupsX = ComputeMath.ThreadGroupCount(dynamicRenderBatch.rendererCount, 16);
					ropeShader.SetInt("firstRenderer", dynamicRenderBatch.firstRenderer);
					ropeShader.SetInt("rendererCount", dynamicRenderBatch.rendererCount);
					ropeShader.SetBuffer(updateKernel, "vertices", dynamicRenderBatch.gpuVertexBuffer);
					ropeShader.Dispatch(updateKernel, threadGroupsX, 1, 1);
					RenderParams rparams = dynamicRenderBatch.renderParams;
					rparams.worldBounds = m_Solver.bounds;
					for (int j = 0; j < dynamicRenderBatch.materials.Length; j++)
					{
						rparams.material = dynamicRenderBatch.materials[j];
						Graphics.RenderMesh(in rparams, dynamicRenderBatch.mesh, j, m_Solver.transform.localToWorldMatrix, m_Solver.transform.localToWorldMatrix);
					}
				}
			}
		}
	}
}
