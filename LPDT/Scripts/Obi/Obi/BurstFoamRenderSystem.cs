using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Obi
{
	public class BurstFoamRenderSystem : ObiFoamRenderSystem
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		protected struct SortHandleComparer : IComparer<float2>
		{
			public int Compare(float2 a, float2 b)
			{
				return b.y.CompareTo(a.y);
			}
		}

		[BurstCompile]
		private struct ProjectOnSortAxisJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> inputPositions;

			[NativeDisableParallelForRestriction]
			public NativeArray<float2> sortHandles;

			public float3 sortAxis;

			public void Execute(int i)
			{
				sortHandles[i] = new float2(i, math.dot(inputPositions[i].xyz, sortAxis));
			}
		}

		[BurstCompile]
		private struct SortParticles : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float2> sortHandles;

			[ReadOnly]
			public NativeArray<float4> inputPositions;

			[ReadOnly]
			public NativeArray<float4> inputVelocities;

			[ReadOnly]
			public NativeArray<float4> inputColors;

			[ReadOnly]
			public NativeArray<float4> inputAttributes;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> outputPositions;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> outputVelocities;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> outputColors;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> outputAttributes;

			public void Execute(int i)
			{
				int index = (int)sortHandles[i].x;
				outputPositions[i] = inputPositions[index];
				outputVelocities[i] = inputVelocities[index];
				outputColors[i] = inputColors[index];
				outputAttributes[i] = inputAttributes[index];
			}
		}

		[BurstCompile]
		private struct BuildFoamMeshDataJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> inputPositions;

			[ReadOnly]
			public NativeArray<float4> inputVelocities;

			[ReadOnly]
			public NativeArray<float4> inputColors;

			[ReadOnly]
			public NativeArray<float4> inputAttributes;

			[NativeDisableParallelForRestriction]
			public NativeArray<DiffuseParticleVertex> vertices;

			[NativeDisableParallelForRestriction]
			public NativeArray<int> indices;

			public void Execute(int i)
			{
				DiffuseParticleVertex value = new DiffuseParticleVertex
				{
					pos = new float4(inputPositions[i].xyz, 1f),
					color = inputColors[i],
					velocity = inputVelocities[i],
					attributes = inputAttributes[i],
					offset = new float3(1f, 1f, 0f)
				};
				vertices[i * 4] = value;
				value.offset = new float3(-1f, 1f, 0f);
				vertices[i * 4 + 1] = value;
				value.offset = new float3(-1f, -1f, 0f);
				vertices[i * 4 + 2] = value;
				value.offset = new float3(1f, -1f, 0f);
				vertices[i * 4 + 3] = value;
				indices[i * 6] = i * 4 + 2;
				indices[i * 6 + 1] = i * 4 + 1;
				indices[i * 6 + 2] = i * 4;
				indices[i * 6 + 3] = i * 4 + 3;
				indices[i * 6 + 4] = i * 4 + 2;
				indices[i * 6 + 5] = i * 4;
			}
		}

		protected NativeArray<float2> sortHandles;

		protected SortHandleComparer comparer;

		public BurstFoamRenderSystem(ObiSolver solver)
			: base(solver)
		{
			if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset)
			{
				renderBatch = new ProceduralRenderBatch<DiffuseParticleVertex>(0, Resources.Load<Material>("ObiMaterials/URP/Fluid/FoamParticlesURP"), new RenderBatchParams(receiveShadow: true));
			}
			else
			{
				renderBatch = new ProceduralRenderBatch<DiffuseParticleVertex>(0, Resources.Load<Material>("ObiMaterials/Fluid/FoamParticles"), new RenderBatchParams(receiveShadow: true));
			}
			ReallocateRenderBatch();
		}

		public override void Dispose()
		{
			base.Dispose();
			if (sortHandles.IsCreated)
			{
				sortHandles.Dispose();
			}
		}

		private void ReallocateRenderBatch()
		{
			if (!sortHandles.IsCreated || m_Solver.foamPositions.count * 4 != renderBatch.vertexCount)
			{
				renderBatch.Dispose();
				renderBatch.vertexCount = m_Solver.foamPositions.count * 4;
				renderBatch.triangleCount = m_Solver.foamPositions.count * 2;
				renderBatch.Initialize(layout);
				if (sortHandles.IsCreated)
				{
					sortHandles.Dispose();
				}
				sortHandles = new NativeArray<float2>(m_Solver.foamPositions.count, Allocator.Persistent);
			}
		}

		public override void Setup()
		{
		}

		public override void Step()
		{
		}

		public unsafe override void Render()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			BurstSolverImpl burstSolverImpl = m_Solver.implementation as BurstSolverImpl;
			ReallocateRenderBatch();
			foreach (Camera camera in cameras)
			{
				if (!(camera == null))
				{
					JobHandle dependsOn = default(JobHandle);
					SortJob<float2, SortHandleComparer> sortJob = sortHandles.Slice(0, m_Solver.foamCount[3]).SortJob(comparer);
					UnsafeUtility.MemClear(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(renderBatch.triangles), UnsafeUtility.SizeOf<int>() * renderBatch.triangles.Length);
					dependsOn = IJobParallelForExtensions.Schedule(new ProjectOnSortAxisJob
					{
						inputPositions = burstSolverImpl.abstraction.foamPositions.AsNativeArray<float4>(),
						sortHandles = sortHandles,
						sortAxis = burstSolverImpl.abstraction.transform.InverseTransformDirection(camera.transform.forward)
					}, m_Solver.foamCount[3], 256, dependsOn);
					dependsOn = sortJob.Schedule(dependsOn);
					dependsOn = IJobParallelForExtensions.Schedule(new SortParticles
					{
						sortHandles = sortHandles,
						inputPositions = burstSolverImpl.abstraction.foamPositions.AsNativeArray<float4>(),
						inputVelocities = burstSolverImpl.abstraction.foamVelocities.AsNativeArray<float4>(),
						inputColors = burstSolverImpl.abstraction.foamColors.AsNativeArray<float4>(),
						inputAttributes = burstSolverImpl.abstraction.foamAttributes.AsNativeArray<float4>(),
						outputPositions = burstSolverImpl.auxPositions,
						outputVelocities = burstSolverImpl.auxVelocities,
						outputColors = burstSolverImpl.auxColors,
						outputAttributes = burstSolverImpl.auxAttributes
					}, m_Solver.foamCount[3], 256, dependsOn);
					IJobParallelForExtensions.Schedule(new BuildFoamMeshDataJob
					{
						inputPositions = burstSolverImpl.auxPositions,
						inputVelocities = burstSolverImpl.auxVelocities,
						inputColors = burstSolverImpl.auxColors,
						inputAttributes = burstSolverImpl.auxAttributes,
						vertices = renderBatch.vertices,
						indices = renderBatch.triangles
					}, m_Solver.foamCount[3], 128, dependsOn).Complete();
					renderBatch.mesh.SetVertexBufferData(renderBatch.vertices, 0, 0, renderBatch.vertexCount, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
					renderBatch.mesh.SetIndexBufferData(renderBatch.triangles, 0, 0, renderBatch.triangleCount * 3, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
					matProps.SetFloat("_FadeDepth", 0f);
					matProps.SetFloat("_VelocityStretching", m_Solver.maxFoamVelocityStretch);
					matProps.SetFloat("_RadiusScale", m_Solver.foamRadiusScale);
					matProps.SetFloat("_FadeIn", m_Solver.foamFade.x);
					matProps.SetFloat("_FadeOut", m_Solver.foamFade.y);
					matProps.SetFloat("_ScatterDensity", m_Solver.foamVolumeDensity);
					matProps.SetFloat("_AmbientDensity", m_Solver.foamAmbientDensity);
					matProps.SetColor("_ScatterColor", m_Solver.foamScatterColor);
					matProps.SetColor("_AmbientColor", m_Solver.foamAmbientColor);
					RenderParams rparams = renderBatch.renderParams;
					rparams.worldBounds = m_Solver.bounds;
					rparams.camera = camera;
					rparams.matProps = matProps;
					Graphics.RenderMesh(in rparams, renderBatch.mesh, 0, m_Solver.transform.localToWorldMatrix, m_Solver.transform.localToWorldMatrix);
				}
			}
		}
	}
}
