using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Obi
{
	public class ComputeFoamRenderSystem : ObiFoamRenderSystem
	{
		private ComputeShader foamShader;

		private int sortKernel;

		private int clearMeshKernel;

		private int buildMeshKernel;

		protected Material thickness_Material;

		protected Material color_Material;

		protected LocalKeyword shader2DFeature;

		public ComputeFoamRenderSystem(ObiSolver solver)
			: base(solver)
		{
			foamShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/FluidFoam"));
			sortKernel = foamShader.FindKernel("Sort");
			clearMeshKernel = foamShader.FindKernel("ClearMesh");
			buildMeshKernel = foamShader.FindKernel("BuildMesh");
			if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset)
			{
				renderBatch = new ProceduralRenderBatch<DiffuseParticleVertex>(0, Resources.Load<Material>("ObiMaterials/URP/Fluid/FoamParticlesURP"), new RenderBatchParams(receiveShadow: true));
			}
			else
			{
				renderBatch = new ProceduralRenderBatch<DiffuseParticleVertex>(0, Resources.Load<Material>("ObiMaterials/Fluid/FoamParticles"), new RenderBatchParams(receiveShadow: true));
			}
			renderBatch.vertexCount = (int)(m_Solver.maxFoamParticles * 4);
			renderBatch.triangleCount = (int)(m_Solver.maxFoamParticles * 2);
			renderBatch.Initialize(layout, gpu: true);
		}

		private void ReallocateParticleBuffers()
		{
			if (m_Solver.foamPositions.count * 4 != renderBatch.vertexCount)
			{
				renderBatch.Dispose();
				renderBatch.vertexCount = m_Solver.foamPositions.count * 4;
				renderBatch.triangleCount = m_Solver.foamPositions.count * 2;
				renderBatch.Initialize(layout, gpu: true);
			}
		}

		public override void Setup()
		{
			using (ObiFoamRenderSystem.m_SetupRenderMarker.Auto())
			{
				for (int i = 0; i < base.renderers.Count; i++)
				{
					base.renderers[i].actor.solverIndices.AsComputeBuffer<int>();
				}
			}
		}

		public override void Step()
		{
			for (int i = 0; i < base.renderers.Count; i++)
			{
				base.renderers[i].actor.solverIndices.Upload();
			}
		}

		public override void Render()
		{
			ComputeSolverImpl computeSolverImpl = m_Solver.implementation as ComputeSolverImpl;
			if (!Application.isPlaying)
			{
				return;
			}
			ReallocateParticleBuffers();
			if (computeSolverImpl.activeParticlesBuffer == null || computeSolverImpl.abstraction.foamPositions.computeBuffer == null)
			{
				return;
			}
			foreach (Camera camera in cameras)
			{
				if (camera == null)
				{
					continue;
				}
				foamShader.SetVector("sortAxis", computeSolverImpl.abstraction.transform.InverseTransformDirection(camera.transform.forward));
				foamShader.SetBuffer(sortKernel, "outputPositions", computeSolverImpl.abstraction.foamPositions.computeBuffer);
				foamShader.SetBuffer(sortKernel, "outputVelocities", computeSolverImpl.abstraction.foamVelocities.computeBuffer);
				foamShader.SetBuffer(sortKernel, "outputColors", computeSolverImpl.abstraction.foamColors.computeBuffer);
				foamShader.SetBuffer(sortKernel, "outputAttributes", computeSolverImpl.abstraction.foamAttributes.computeBuffer);
				foamShader.SetBuffer(sortKernel, "dispatch", computeSolverImpl.abstraction.foamCount.computeBuffer);
				int num = m_Solver.foamPositions.count.CeilToPowerOfTwo() / 2;
				int num2 = (int)Mathf.Log(num * 2, 2f);
				int threadGroupsX = ComputeMath.ThreadGroupCount(num, 128);
				for (int i = 0; i < num2; i++)
				{
					for (int j = 0; j < i + 1; j++)
					{
						int num3 = 1 << i - j;
						int val = 2 * num3 - 1;
						foamShader.SetInt("groupWidth", num3);
						foamShader.SetInt("groupHeight", val);
						foamShader.SetInt("stepIndex", j);
						foamShader.Dispatch(sortKernel, threadGroupsX, 1, 1);
					}
				}
				int threadGroupsX2 = ComputeMath.ThreadGroupCount(m_Solver.foamPositions.count, 128);
				foamShader.SetInt("maxFoamParticles", m_Solver.foamPositions.count);
				foamShader.SetBuffer(clearMeshKernel, "indices", renderBatch.gpuIndexBuffer);
				foamShader.Dispatch(clearMeshKernel, threadGroupsX2, 1, 1);
				foamShader.SetBuffer(buildMeshKernel, "inputPositions", computeSolverImpl.abstraction.foamPositions.computeBuffer);
				foamShader.SetBuffer(buildMeshKernel, "inputVelocities", computeSolverImpl.abstraction.foamVelocities.computeBuffer);
				foamShader.SetBuffer(buildMeshKernel, "inputColors", computeSolverImpl.abstraction.foamColors.computeBuffer);
				foamShader.SetBuffer(buildMeshKernel, "inputAttributes", computeSolverImpl.abstraction.foamAttributes.computeBuffer);
				foamShader.SetBuffer(buildMeshKernel, "vertices", renderBatch.gpuVertexBuffer);
				foamShader.SetBuffer(buildMeshKernel, "indices", renderBatch.gpuIndexBuffer);
				foamShader.SetBuffer(buildMeshKernel, "dispatch", computeSolverImpl.abstraction.foamCount.computeBuffer);
				foamShader.DispatchIndirect(buildMeshKernel, computeSolverImpl.abstraction.foamCount.computeBuffer);
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
				rparams.shadowCastingMode = ShadowCastingMode.Off;
				Graphics.RenderMesh(in rparams, renderBatch.mesh, 0, m_Solver.transform.localToWorldMatrix, m_Solver.transform.localToWorldMatrix);
			}
		}
	}
}
