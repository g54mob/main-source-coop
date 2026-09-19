using UnityEngine;

namespace Obi
{
	public class ComputeLineRopeRenderSystem : ObiLineRopeRenderSystem
	{
		private ComputeShader ropeShader;

		private int updateKernel;

		public ComputeLineRopeRenderSystem(ObiSolver solver)
			: base(solver)
		{
			ropeShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/RopeLineRendering"));
			updateKernel = ropeShader.FindKernel("UpdateLineMesh");
		}

		public override void Setup()
		{
			base.Setup();
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Initialize(layout, gpu: true);
			}
			vertexOffsets.AsComputeBuffer<int>();
			triangleOffsets.AsComputeBuffer<int>();
			triangleCounts.AsComputeBuffer<int>();
			pathSmootherIndices.AsComputeBuffer<int>();
			rendererData.AsComputeBuffer<BurstLineMeshData>();
			pathSmootherSystem.chunkOffsets.AsComputeBuffer<int>();
		}

		public override void Render()
		{
		}

		public override void RenderFromCamera(Camera camera)
		{
			using (ObiLineRopeRenderSystem.m_RenderMarker.Auto())
			{
				if (pathSmootherSystem != null && pathSmootherSystem.chunkOffsets != null && pathSmootherSystem.chunkOffsets.count > 0)
				{
					ropeShader.SetBuffer(updateKernel, "pathSmootherIndices", pathSmootherIndices.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "chunkOffsets", pathSmootherSystem.chunkOffsets.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "frames", pathSmootherSystem.smoothFrames.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "frameOffsets", pathSmootherSystem.smoothFrameOffsets.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "frameCounts", pathSmootherSystem.smoothFrameCounts.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "vertexOffsets", vertexOffsets.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "triangleOffsets", triangleOffsets.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "triangleCounts", triangleCounts.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "rendererData", rendererData.computeBuffer);
					ropeShader.SetBuffer(updateKernel, "pathData", pathSmootherSystem.pathData.computeBuffer);
					ropeShader.SetVector("localSpaceCamera", m_Solver.transform.InverseTransformPoint(camera.transform.position));
					for (int i = 0; i < batchList.Count; i++)
					{
						ProceduralRenderBatch<ProceduralRopeVertex> proceduralRenderBatch = batchList[i];
						int threadGroupsX = ComputeMath.ThreadGroupCount(proceduralRenderBatch.rendererCount, 128);
						ropeShader.SetInt("firstRenderer", proceduralRenderBatch.firstRenderer);
						ropeShader.SetInt("rendererCount", proceduralRenderBatch.rendererCount);
						ropeShader.SetBuffer(updateKernel, "vertices", proceduralRenderBatch.gpuVertexBuffer);
						ropeShader.SetBuffer(updateKernel, "tris", proceduralRenderBatch.gpuIndexBuffer);
						ropeShader.Dispatch(updateKernel, threadGroupsX, 1, 1);
						RenderParams rparams = proceduralRenderBatch.renderParams;
						rparams.worldBounds = m_Solver.bounds;
						rparams.camera = camera;
						Graphics.RenderMesh(in rparams, proceduralRenderBatch.mesh, 0, m_Solver.transform.localToWorldMatrix, m_Solver.transform.localToWorldMatrix);
					}
				}
			}
		}
	}
}
