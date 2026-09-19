using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class ObiFoamRenderSystem : RenderSystem<ObiFoamGenerator>, IRenderSystem
	{
		protected VertexAttributeDescriptor[] layout = new VertexAttributeDescriptor[5]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.Normal),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 4)
		};

		protected static ProfilerMarker m_SetupRenderMarker = new ProfilerMarker("SetupSurfaceMeshing");

		protected static ProfilerMarker m_RenderMarker = new ProfilerMarker("SurfaceMeshing");

		protected HashSet<Camera> cameras = new HashSet<Camera>();

		protected MaterialPropertyBlock matProps;

		protected ObiSolver m_Solver;

		public ProceduralRenderBatch<DiffuseParticleVertex> renderBatch;

		private Action<ScriptableRenderContext, Camera> renderCallback;

		public Oni.RenderingSystemType typeEnum => Oni.RenderingSystemType.FoamParticles;

		public RendererSet<ObiFoamGenerator> renderers { get; } = new RendererSet<ObiFoamGenerator>();

		public bool isSetup => true;

		public uint tier => 0u;

		public ObiFoamRenderSystem(ObiSolver solver)
		{
			m_Solver = solver;
			matProps = new MaterialPropertyBlock();
			renderCallback = delegate(ScriptableRenderContext cntxt, Camera cam)
			{
				RenderFromCamera(cam);
			};
			RenderPipelineManager.beginCameraRendering += renderCallback;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(RenderFromCamera));
		}

		public virtual void Dispose()
		{
			RenderPipelineManager.beginCameraRendering -= renderCallback;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(RenderFromCamera));
			renderBatch.Dispose();
			cameras.Clear();
		}

		public void RenderFromCamera(Camera camera)
		{
			cameras.Add(camera);
		}

		public virtual void Setup()
		{
		}

		public virtual void Step()
		{
		}

		public virtual void Render()
		{
		}
	}
}
