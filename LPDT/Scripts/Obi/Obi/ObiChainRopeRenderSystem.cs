using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public abstract class ObiChainRopeRenderSystem : RenderSystem<ObiRopeChainRenderer>, IRenderSystem
	{
		protected VertexAttributeDescriptor[] layout = new VertexAttributeDescriptor[5]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
			new VertexAttributeDescriptor(VertexAttribute.Normal),
			new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
		};

		protected static ProfilerMarker m_SetupRenderMarker = new ProfilerMarker("SetupChainRopeRendering");

		protected static ProfilerMarker m_RenderMarker = new ProfilerMarker("ChainRopeRendering");

		protected ObiSolver m_Solver;

		protected List<InstancedRenderBatch> batchList = new List<InstancedRenderBatch>();

		protected ObiNativeList<ChainRendererData> rendererData;

		protected ObiNativeList<ChunkData> chunkData;

		protected ObiNativeList<ObiRopeChainRenderer.LinkModifier> modifiers;

		protected ObiNativeList<Vector2Int> elements;

		protected ObiNativeList<Matrix4x4> instanceTransforms;

		protected ObiNativeList<Matrix4x4> invInstanceTransforms;

		protected ObiNativeList<Vector4> instanceColors;

		public Oni.RenderingSystemType typeEnum => Oni.RenderingSystemType.ChainRope;

		public RendererSet<ObiRopeChainRenderer> renderers { get; } = new RendererSet<ObiRopeChainRenderer>();

		public ObiChainRopeRenderSystem(ObiSolver solver)
		{
			m_Solver = solver;
		}

		public virtual void Dispose()
		{
			CleanupBatches();
			DestroyLists();
		}

		private void DestroyLists()
		{
			if (instanceTransforms != null)
			{
				instanceTransforms.Dispose();
			}
			if (invInstanceTransforms != null)
			{
				invInstanceTransforms.Dispose();
			}
			if (instanceColors != null)
			{
				instanceColors.Dispose();
			}
			if (elements != null)
			{
				elements.Dispose();
			}
			if (chunkData != null)
			{
				chunkData.Dispose();
			}
			if (rendererData != null)
			{
				rendererData.Dispose();
			}
			if (modifiers != null)
			{
				modifiers.Dispose();
			}
		}

		private void CreateListsIfNecessary()
		{
			DestroyLists();
			instanceTransforms = new ObiNativeList<Matrix4x4>();
			invInstanceTransforms = new ObiNativeList<Matrix4x4>();
			instanceColors = new ObiNativeList<Vector4>();
			elements = new ObiNativeList<Vector2Int>();
			chunkData = new ObiNativeList<ChunkData>();
			rendererData = new ObiNativeList<ChainRendererData>();
			modifiers = new ObiNativeList<ObiRopeChainRenderer.LinkModifier>();
		}

		private void CleanupBatches()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Dispose();
			}
			batchList.Clear();
		}

		private void GenerateBatches()
		{
			instanceTransforms.Clear();
			invInstanceTransforms.Clear();
			instanceColors.Clear();
			elements.Clear();
			rendererData.Clear();
			chunkData.Clear();
			modifiers.Clear();
			for (int i = 0; i < renderers.Count; i++)
			{
				ObiRopeChainRenderer obiRopeChainRenderer = renderers[i];
				if (obiRopeChainRenderer.linkMesh != null && obiRopeChainRenderer.linkMaterial != null)
				{
					obiRopeChainRenderer.renderParameters.layer = obiRopeChainRenderer.gameObject.layer;
					batchList.Add(new InstancedRenderBatch(i, obiRopeChainRenderer.linkMesh, obiRopeChainRenderer.linkMaterial, obiRopeChainRenderer.renderParameters));
				}
			}
			batchList.Sort();
			for (int j = 0; j < batchList.Count; j++)
			{
				ObiRopeChainRenderer obiRopeChainRenderer2 = renderers[batchList[j].firstRenderer];
				ObiRopeBase obiRopeBase = obiRopeChainRenderer2.actor as ObiRopeBase;
				modifiers.AddRange(obiRopeChainRenderer2.linkModifiers);
				rendererData.Add(new ChainRendererData(modifiers.count, obiRopeChainRenderer2.twistAnchor, obiRopeChainRenderer2.linkTwist, obiRopeChainRenderer2.linkScale, obiRopeBase.usesOrientedParticles));
				batchList[j].firstInstance = elements.count;
				batchList[j].instanceCount = obiRopeBase.elements.Count;
				for (int k = 0; k < obiRopeBase.elements.Count; k++)
				{
					elements.Add(new Vector2Int(obiRopeBase.elements[k].particle1, obiRopeBase.elements[k].particle2));
					if (k < obiRopeBase.elements.Count - 1 && obiRopeBase.elements[k].particle2 != obiRopeBase.elements[k + 1].particle1)
					{
						chunkData.Add(new ChunkData(rendererData.count - 1, elements.count));
					}
				}
				chunkData.Add(new ChunkData(rendererData.count - 1, elements.count));
			}
			instanceTransforms.ResizeUninitialized(elements.count);
			invInstanceTransforms.ResizeUninitialized(elements.count);
			instanceColors.ResizeUninitialized(elements.count);
		}

		protected virtual void CloseBatches()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Initialize();
			}
		}

		public virtual void Setup()
		{
			using (m_SetupRenderMarker.Auto())
			{
				CreateListsIfNecessary();
				CleanupBatches();
				GenerateBatches();
				ObiUtils.MergeBatches(batchList);
				CloseBatches();
			}
		}

		public abstract void Render();

		public void Step()
		{
		}

		public void BakeMesh(ObiRopeChainRenderer renderer, ref Mesh mesh, bool transformToActorLocalSpace = false)
		{
			int num = renderers.IndexOf(renderer);
			for (int i = 0; i < batchList.Count; i++)
			{
				InstancedRenderBatch instancedRenderBatch = batchList[i];
				if (num >= instancedRenderBatch.firstRenderer && num < instancedRenderBatch.firstRenderer + instancedRenderBatch.rendererCount)
				{
					instancedRenderBatch.BakeMesh(renderers, renderer, chunkData, instanceTransforms, renderer.actor.actorSolverToLocalMatrix, ref mesh, transformToActorLocalSpace);
					break;
				}
			}
		}
	}
}
