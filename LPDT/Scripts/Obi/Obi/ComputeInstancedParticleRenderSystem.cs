using UnityEngine;

namespace Obi
{
	public class ComputeInstancedParticleRenderSystem : ObiInstancedParticleRenderSystem
	{
		private ComputeShader instanceShader;

		private int updateKernel;

		private uint[] args = new uint[5];

		public ComputeInstancedParticleRenderSystem(ObiSolver solver)
			: base(solver)
		{
			instanceShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/InstancedParticleRendering"));
			updateKernel = instanceShader.FindKernel("UpdateParticleInstances");
		}

		protected override void CloseBatches()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Initialize(gpu: true);
			}
		}

		public override void Setup()
		{
			base.Setup();
			activeParticles.AsComputeBuffer<int>();
			rendererData.AsComputeBuffer<ParticleRendererData>();
			rendererIndex.AsComputeBuffer<int>();
			instanceTransforms.AsComputeBuffer<Matrix4x4>();
			invInstanceTransforms.AsComputeBuffer<Matrix4x4>();
			instanceColors.AsComputeBuffer<Vector4>();
		}

		public override void Render()
		{
			using (ObiInstancedParticleRenderSystem.m_RenderMarker.Auto())
			{
				ComputeSolverImpl computeSolverImpl = m_Solver.implementation as ComputeSolverImpl;
				if (computeSolverImpl.renderablePositionsBuffer == null || computeSolverImpl.renderablePositionsBuffer.count <= 0 || activeParticles.count <= 0)
				{
					return;
				}
				instanceShader.SetBuffer(updateKernel, "activeParticles", activeParticles.computeBuffer);
				instanceShader.SetBuffer(updateKernel, "rendererData", rendererData.computeBuffer);
				instanceShader.SetBuffer(updateKernel, "rendererIndex", rendererIndex.computeBuffer);
				instanceShader.SetBuffer(updateKernel, "renderablePositions", computeSolverImpl.renderablePositionsBuffer);
				instanceShader.SetBuffer(updateKernel, "renderableOrientations", computeSolverImpl.renderableOrientationsBuffer);
				instanceShader.SetBuffer(updateKernel, "renderableRadii", computeSolverImpl.renderableRadiiBuffer);
				instanceShader.SetBuffer(updateKernel, "colors", computeSolverImpl.colorsBuffer);
				instanceShader.SetBuffer(updateKernel, "instanceTransforms", instanceTransforms.computeBuffer);
				instanceShader.SetBuffer(updateKernel, "invInstanceTransforms", invInstanceTransforms.computeBuffer);
				instanceShader.SetBuffer(updateKernel, "instanceColors", instanceColors.computeBuffer);
				instanceShader.SetMatrix("solverToWorld", m_Solver.transform.localToWorldMatrix);
				instanceShader.SetInt("particleCount", activeParticles.count);
				int threadGroupsX = ComputeMath.ThreadGroupCount(activeParticles.count, 128);
				instanceShader.Dispatch(updateKernel, threadGroupsX, 1, 1);
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				materialPropertyBlock.SetBuffer("_InstanceTransforms", instanceTransforms.computeBuffer);
				materialPropertyBlock.SetBuffer("_InvInstanceTransforms", invInstanceTransforms.computeBuffer);
				materialPropertyBlock.SetBuffer("_Colors", instanceColors.computeBuffer);
				for (int i = 0; i < batchList.Count; i++)
				{
					InstancedRenderBatch instancedRenderBatch = batchList[i];
					if (!(instancedRenderBatch.mesh == null))
					{
						args[0] = instancedRenderBatch.mesh.GetIndexCount(0);
						args[1] = (uint)instancedRenderBatch.instanceCount;
						args[2] = instancedRenderBatch.mesh.GetIndexStart(0);
						args[3] = instancedRenderBatch.mesh.GetBaseVertex(0);
						args[4] = (uint)instancedRenderBatch.firstInstance;
						instancedRenderBatch.argsBuffer.SetData(args);
						RenderParams rparams = instancedRenderBatch.renderParams;
						rparams.material = instancedRenderBatch.material;
						rparams.worldBounds = m_Solver.bounds;
						rparams.matProps = materialPropertyBlock;
						Graphics.RenderMeshIndirect(in rparams, instancedRenderBatch.mesh, instancedRenderBatch.argsBuffer);
					}
				}
			}
		}
	}
}
