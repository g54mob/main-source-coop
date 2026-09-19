using UnityEngine;

namespace Obi
{
	public class ComputeChainRopeRenderSystem : ObiChainRopeRenderSystem
	{
		private ComputeShader ropeShader;

		private int updateKernel;

		private uint[] args = new uint[5];

		public ComputeChainRopeRenderSystem(ObiSolver solver)
			: base(solver)
		{
			ropeShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/RopeChainRendering"));
			updateKernel = ropeShader.FindKernel("UpdateChainMesh");
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
			rendererData.AsComputeBuffer<ChainRendererData>();
			chunkData.AsComputeBuffer<ChunkData>();
			modifiers.SafeAsComputeBuffer<ObiRopeChainRenderer.LinkModifier>();
			elements.AsComputeBuffer<Vector2Int>();
			instanceTransforms.AsComputeBuffer<Matrix4x4>();
			invInstanceTransforms.AsComputeBuffer<Matrix4x4>();
			instanceColors.AsComputeBuffer<Vector4>();
		}

		public override void Render()
		{
			using (ObiChainRopeRenderSystem.m_RenderMarker.Auto())
			{
				ComputeSolverImpl computeSolverImpl = m_Solver.implementation as ComputeSolverImpl;
				if (computeSolverImpl.renderablePositionsBuffer == null || computeSolverImpl.renderablePositionsBuffer.count <= 0 || elements.count <= 0)
				{
					return;
				}
				ropeShader.SetBuffer(updateKernel, "rendererData", rendererData.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "chunksData", chunkData.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "modifiers", modifiers.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "elements", elements.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "renderablePositions", computeSolverImpl.renderablePositionsBuffer);
				ropeShader.SetBuffer(updateKernel, "renderableOrientations", computeSolverImpl.renderableOrientationsBuffer);
				ropeShader.SetBuffer(updateKernel, "principalRadii", computeSolverImpl.renderableRadiiBuffer);
				ropeShader.SetBuffer(updateKernel, "colors", computeSolverImpl.colorsBuffer);
				ropeShader.SetBuffer(updateKernel, "instanceTransforms", instanceTransforms.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "invInstanceTransforms", invInstanceTransforms.computeBuffer);
				ropeShader.SetBuffer(updateKernel, "instanceColors", instanceColors.computeBuffer);
				ropeShader.SetMatrix("solverToWorld", m_Solver.transform.localToWorldMatrix);
				ropeShader.SetInt("chunkCount", chunkData.count);
				int threadGroupsX = ComputeMath.ThreadGroupCount(chunkData.count, 32);
				ropeShader.Dispatch(updateKernel, threadGroupsX, 1, 1);
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
