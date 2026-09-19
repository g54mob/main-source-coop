using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Obi
{
	public class BurstInstancedParticleRenderSystem : ObiInstancedParticleRenderSystem
	{
		[BurstCompile]
		private struct InstancedParticleTransforms : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> activeParticles;

			[ReadOnly]
			public NativeArray<ParticleRendererData> rendererData;

			[ReadOnly]
			public NativeArray<int> rendererIndex;

			[ReadOnly]
			public NativeArray<float4> renderablePositions;

			[ReadOnly]
			public NativeArray<quaternion> renderableOrientations;

			[ReadOnly]
			public NativeArray<float4> renderableRadii;

			[ReadOnly]
			public NativeArray<float4> colors;

			[ReadOnly]
			public float4x4 solverToWorld;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> instanceTransforms;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> instanceColors;

			public void Execute(int i)
			{
				int index = activeParticles[i];
				Matrix4x4 matrix4x = float4x4.TRS(renderablePositions[index].xyz, renderableOrientations[index], renderableRadii[index].xyz * renderableRadii[index][3] * rendererData[rendererIndex[i]].radiusScale);
				instanceTransforms[i] = math.mul(solverToWorld, matrix4x);
				instanceColors[i] = colors[index] * (Vector4)rendererData[rendererIndex[i]].color;
			}
		}

		public BurstInstancedParticleRenderSystem(ObiSolver solver)
			: base(solver)
		{
		}

		public override void Render()
		{
			using (ObiInstancedParticleRenderSystem.m_RenderMarker.Auto())
			{
				IJobParallelForExtensions.Schedule(new InstancedParticleTransforms
				{
					activeParticles = activeParticles.AsNativeArray<int>(),
					rendererData = rendererData.AsNativeArray<ParticleRendererData>(),
					rendererIndex = rendererIndex.AsNativeArray<int>(),
					instanceTransforms = instanceTransforms.AsNativeArray<float4x4>(),
					instanceColors = instanceColors.AsNativeArray<float4>(),
					renderablePositions = m_Solver.renderablePositions.AsNativeArray<float4>(),
					renderableOrientations = m_Solver.renderableOrientations.AsNativeArray<quaternion>(),
					renderableRadii = m_Solver.renderableRadii.AsNativeArray<float4>(),
					colors = m_Solver.colors.AsNativeArray<float4>(),
					solverToWorld = m_Solver.transform.localToWorldMatrix
				}, activeParticles.count, 32).Complete();
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				for (int i = 0; i < batchList.Count; i++)
				{
					InstancedRenderBatch instancedRenderBatch = batchList[i];
					if (instancedRenderBatch.instanceCount > 0)
					{
						materialPropertyBlock.SetVectorArray("_Colors", instanceColors.AsNativeArray<Vector4>().Slice(instancedRenderBatch.firstInstance, instancedRenderBatch.instanceCount).ToArray());
						RenderParams renderParams = instancedRenderBatch.renderParams;
						renderParams.material = instancedRenderBatch.material;
						renderParams.worldBounds = m_Solver.bounds;
						renderParams.matProps = materialPropertyBlock;
						Graphics.RenderMeshInstanced(renderParams, instancedRenderBatch.mesh, 0, instanceTransforms.AsNativeArray<Matrix4x4>(), instancedRenderBatch.instanceCount, instancedRenderBatch.firstInstance);
					}
				}
			}
		}
	}
}
