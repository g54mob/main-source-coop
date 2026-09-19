using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.Rendering;

namespace Obi
{
	public abstract class ObiParticleRenderSystem : RenderSystem<ObiParticleRenderer>, IRenderSystem
	{
		protected VertexAttributeDescriptor[] layout = new VertexAttributeDescriptor[6]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.Normal),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord2, VertexAttributeFormat.Float32, 4)
		};

		protected static ProfilerMarker m_SetupRenderMarker = new ProfilerMarker("SetupParticleRendering");

		protected static ProfilerMarker m_RenderMarker = new ProfilerMarker("ParticleRendering");

		protected ObiSolver m_Solver;

		protected List<ProceduralRenderBatch<ParticleVertex>> batchList = new List<ProceduralRenderBatch<ParticleVertex>>();

		protected ObiNativeList<int> activeParticles;

		protected ObiNativeList<int> rendererIndex;

		protected ObiNativeList<ParticleRendererData> rendererData;

		public Oni.RenderingSystemType typeEnum => Oni.RenderingSystemType.Particles;

		public RendererSet<ObiParticleRenderer> renderers { get; } = new RendererSet<ObiParticleRenderer>();

		public bool isSetup => activeParticles != null;

		public ObiParticleRenderSystem(ObiSolver solver)
		{
			m_Solver = solver;
			activeParticles = new ObiNativeList<int>();
			rendererIndex = new ObiNativeList<int>();
			rendererData = new ObiNativeList<ParticleRendererData>();
		}

		public virtual void Dispose()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Dispose();
			}
			batchList.Clear();
			if (activeParticles != null)
			{
				activeParticles.Dispose();
			}
			if (rendererData != null)
			{
				rendererData.Dispose();
			}
			if (rendererIndex != null)
			{
				rendererIndex.Dispose();
			}
		}

		protected virtual void Clear()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Dispose();
			}
			batchList.Clear();
			activeParticles.Clear();
			rendererData.Clear();
			rendererIndex.Clear();
		}

		protected virtual void CreateBatches()
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				renderers[i].renderParameters.layer = renderers[i].gameObject.layer;
				batchList.Add(new ProceduralRenderBatch<ParticleVertex>(i, renderers[i].material, renderers[i].renderParameters));
			}
			batchList.Sort();
			int num = 0;
			for (int j = 0; j < batchList.Count; j++)
			{
				ProceduralRenderBatch<ParticleVertex> proceduralRenderBatch = batchList[j];
				ObiParticleRenderer obiParticleRenderer = renderers[proceduralRenderBatch.firstRenderer];
				int particleCount = obiParticleRenderer.actor.particleCount;
				proceduralRenderBatch.vertexCount += particleCount * 4;
				proceduralRenderBatch.triangleCount += particleCount * 2;
				proceduralRenderBatch.firstParticle = num;
				num += particleCount;
				activeParticles.AddRange(obiParticleRenderer.actor.solverIndices, particleCount);
				rendererData.Add(new ParticleRendererData(obiParticleRenderer.particleColor, obiParticleRenderer.radiusScale));
				rendererIndex.AddReplicate(j, particleCount);
			}
		}

		protected virtual void CloseBatches()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Initialize(layout);
			}
		}

		public virtual void Setup()
		{
			using (m_SetupRenderMarker.Auto())
			{
				Clear();
				CreateBatches();
				ObiUtils.MergeBatches(batchList);
				CloseBatches();
			}
		}

		public virtual void Step()
		{
		}

		public virtual void Render()
		{
		}
	}
}
