using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class BurstParticleRenderSystem : ObiParticleRenderSystem
	{
		public BurstParticleRenderSystem(ObiSolver solver)
			: base(solver)
		{
			m_Solver = solver;
		}

		public override void Render()
		{
			using (ObiParticleRenderSystem.m_RenderMarker.Auto())
			{
				for (int i = 0; i < batchList.Count; i++)
				{
					ProceduralRenderBatch<ParticleVertex> proceduralRenderBatch = batchList[i];
					IJobParallelForExtensions.Schedule(new BuildParticleMeshDataJob
					{
						particleIndices = activeParticles.AsNativeArray<int>(),
						rendererIndices = rendererIndex.AsNativeArray<int>(),
						rendererData = rendererData.AsNativeArray<ParticleRendererData>(),
						renderablePositions = m_Solver.renderablePositions.AsNativeArray<float4>(),
						renderableOrientations = m_Solver.renderableOrientations.AsNativeArray<quaternion>(),
						renderableRadii = m_Solver.renderableRadii.AsNativeArray<float4>(),
						colors = m_Solver.colors.AsNativeArray<float4>(),
						vertices = proceduralRenderBatch.vertices,
						indices = proceduralRenderBatch.triangles,
						firstParticle = proceduralRenderBatch.firstParticle
					}, proceduralRenderBatch.vertexCount / 4, 32).Complete();
					proceduralRenderBatch.mesh.SetVertexBufferData(proceduralRenderBatch.vertices, 0, 0, proceduralRenderBatch.vertexCount, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
					proceduralRenderBatch.mesh.SetIndexBufferData(proceduralRenderBatch.triangles, 0, 0, proceduralRenderBatch.triangleCount * 3, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
					RenderParams rparams = proceduralRenderBatch.renderParams;
					rparams.worldBounds = m_Solver.bounds;
					Graphics.RenderMesh(in rparams, proceduralRenderBatch.mesh, 0, m_Solver.transform.localToWorldMatrix, m_Solver.transform.localToWorldMatrix);
				}
			}
		}
	}
}
