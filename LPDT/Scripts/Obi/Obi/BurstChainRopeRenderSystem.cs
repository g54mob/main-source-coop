using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Obi
{
	public class BurstChainRopeRenderSystem : ObiChainRopeRenderSystem
	{
		[BurstCompile]
		private struct InstanceTransforms : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<ChainRendererData> rendererData;

			[ReadOnly]
			public NativeArray<ChunkData> chunkData;

			[ReadOnly]
			public NativeArray<ObiRopeChainRenderer.LinkModifier> modifiers;

			[ReadOnly]
			public NativeArray<int2> elements;

			[ReadOnly]
			public NativeArray<float4> renderablePositions;

			[ReadOnly]
			public NativeArray<quaternion> renderableOrientations;

			[ReadOnly]
			public NativeArray<float4> principalRadii;

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
				int num = ((i > 0) ? chunkData[i - 1].offset : 0);
				int num2 = chunkData[i].offset - num;
				int rendererIndex = chunkData[i].rendererIndex;
				ChainRendererData chainRendererData = rendererData[rendererIndex];
				float3 xyz = ((float4)chainRendererData.scale).xyz;
				int num3 = ((rendererIndex > 0) ? rendererData[rendererIndex - 1].modifierOffset : 0);
				int num4 = chainRendererData.modifierOffset - num3;
				ObiRopeChainRenderer.LinkModifier linkModifier = default(ObiRopeChainRenderer.LinkModifier);
				linkModifier.Clear();
				BurstPathFrame burstPathFrame = default(BurstPathFrame);
				burstPathFrame.Reset();
				float num5 = (0f - chainRendererData.twist) * (float)num2 * chainRendererData.twistAnchor;
				burstPathFrame.SetTwist(num5);
				for (int j = 0; j < num2; j++)
				{
					if (num4 > 0)
					{
						linkModifier = modifiers[num3 + j % num4];
					}
					int index = num + j;
					float4 float5 = renderablePositions[elements[index].x];
					float4 float6 = renderablePositions[elements[index].y];
					float4 float7 = float6 - float5;
					float3 newTangent = math.normalizesafe(float7.xyz);
					if (chainRendererData.usesOrientedParticles == 1)
					{
						burstPathFrame.Transport(float6.xyz, newTangent, math.rotate(renderableOrientations[elements[index].x], new float3(0f, 1f, 0f)), num5);
						num5 += chainRendererData.twist;
					}
					else
					{
						burstPathFrame.Transport(float6.xyz, newTangent, chainRendererData.twist);
					}
					quaternion quaternion2 = quaternion.LookRotationSafe(burstPathFrame.tangent, burstPathFrame.normal);
					float3 translation = (float5 + float7 * 0.5f).xyz + math.mul(quaternion2, linkModifier.translation);
					float3 scale = principalRadii[elements[index].x].x * 2f * xyz * linkModifier.scale;
					quaternion2 = math.mul(quaternion2, quaternion.Euler(math.radians(linkModifier.rotation)));
					instanceTransforms[index] = math.mul(solverToWorld, float4x4.TRS(translation, quaternion2, scale));
					instanceColors[index] = (colors[elements[index].x] + colors[elements[index].y]) * 0.5f;
				}
			}
		}

		protected Matrix4x4[] transformsArray = new Matrix4x4[1023];

		public BurstChainRopeRenderSystem(ObiSolver solver)
			: base(solver)
		{
		}

		public override void Setup()
		{
			base.Setup();
		}

		public override void Render()
		{
			using (ObiChainRopeRenderSystem.m_RenderMarker.Auto())
			{
				IJobParallelForExtensions.Schedule(new InstanceTransforms
				{
					rendererData = rendererData.AsNativeArray<ChainRendererData>(),
					chunkData = chunkData.AsNativeArray<ChunkData>(),
					modifiers = modifiers.AsNativeArray<ObiRopeChainRenderer.LinkModifier>(),
					elements = elements.AsNativeArray<int2>(),
					instanceTransforms = instanceTransforms.AsNativeArray<float4x4>(),
					instanceColors = instanceColors.AsNativeArray<float4>(),
					renderablePositions = m_Solver.renderablePositions.AsNativeArray<float4>(),
					renderableOrientations = m_Solver.renderableOrientations.AsNativeArray<quaternion>(),
					principalRadii = m_Solver.principalRadii.AsNativeArray<float4>(),
					colors = m_Solver.colors.AsNativeArray<float4>(),
					solverToWorld = m_Solver.transform.localToWorldMatrix
				}, chunkData.count, 8).Complete();
				for (int i = 0; i < batchList.Count; i++)
				{
					InstancedRenderBatch instancedRenderBatch = batchList[i];
					RenderParams renderParams = instancedRenderBatch.renderParams;
					renderParams.material = instancedRenderBatch.material;
					renderParams.worldBounds = m_Solver.bounds;
					Graphics.RenderMeshInstanced(renderParams, instancedRenderBatch.mesh, 0, instanceTransforms.AsNativeArray<Matrix4x4>(), instancedRenderBatch.instanceCount, instancedRenderBatch.firstInstance);
				}
			}
		}
	}
}
