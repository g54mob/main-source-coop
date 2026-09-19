using UnityEngine;

namespace Obi
{
	public class ComputeParticleRenderSystem : ObiParticleRenderSystem
	{
		public ComputeShader meshComputeShader;

		private int buildMeshKernel;

		public ComputeParticleRenderSystem(ObiSolver solver)
			: base(solver)
		{
			meshComputeShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/ParticleMeshBuilding"));
			buildMeshKernel = meshComputeShader.FindKernel("BuildMesh");
		}

		protected override void CloseBatches()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Initialize(layout, gpu: true);
			}
			activeParticles.AsComputeBuffer<int>();
			rendererIndex.AsComputeBuffer<int>();
			rendererData.AsComputeBuffer<ParticleRendererData>();
		}

		public override void Render()
		{
			using (ObiParticleRenderSystem.m_RenderMarker.Auto())
			{
				ComputeSolverImpl computeSolverImpl = m_Solver.implementation as ComputeSolverImpl;
				if (computeSolverImpl.renderablePositionsBuffer != null && activeParticles.computeBuffer != null && computeSolverImpl.renderablePositionsBuffer.count > 0)
				{
					meshComputeShader.SetBuffer(buildMeshKernel, "particleIndices", activeParticles.computeBuffer);
					meshComputeShader.SetBuffer(buildMeshKernel, "positions", computeSolverImpl.renderablePositionsBuffer);
					meshComputeShader.SetBuffer(buildMeshKernel, "orientations", computeSolverImpl.renderableOrientationsBuffer);
					meshComputeShader.SetBuffer(buildMeshKernel, "radii", computeSolverImpl.renderableRadiiBuffer);
					meshComputeShader.SetBuffer(buildMeshKernel, "colors", computeSolverImpl.colorsBuffer);
					meshComputeShader.SetBuffer(buildMeshKernel, "rendererIndices", rendererIndex.computeBuffer);
					meshComputeShader.SetBuffer(buildMeshKernel, "rendererData", rendererData.computeBuffer);
					for (int i = 0; i < batchList.Count; i++)
					{
						ProceduralRenderBatch<ParticleVertex> proceduralRenderBatch = batchList[i];
						int threadGroupsX = ComputeMath.ThreadGroupCount(proceduralRenderBatch.vertexCount / 4, 128);
						meshComputeShader.SetInt("firstParticle", proceduralRenderBatch.firstParticle);
						meshComputeShader.SetInt("particleCount", proceduralRenderBatch.vertexCount / 4);
						meshComputeShader.SetBuffer(buildMeshKernel, "vertices", proceduralRenderBatch.gpuVertexBuffer);
						meshComputeShader.SetBuffer(buildMeshKernel, "indices", proceduralRenderBatch.gpuIndexBuffer);
						meshComputeShader.Dispatch(buildMeshKernel, threadGroupsX, 1, 1);
						RenderParams rparams = proceduralRenderBatch.renderParams;
						rparams.worldBounds = m_Solver.bounds;
						Graphics.RenderMesh(in rparams, proceduralRenderBatch.mesh, 0, m_Solver.transform.localToWorldMatrix, m_Solver.transform.localToWorldMatrix);
					}
				}
			}
		}
	}
}
