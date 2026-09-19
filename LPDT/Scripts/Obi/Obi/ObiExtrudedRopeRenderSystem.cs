using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public abstract class ObiExtrudedRopeRenderSystem : RenderSystem<ObiRopeExtrudedRenderer>, IRenderSystem
	{
		protected List<ObiRopeExtrudedRenderer> sortedRenderers = new List<ObiRopeExtrudedRenderer>();

		protected VertexAttributeDescriptor[] layout = new VertexAttributeDescriptor[5]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
			new VertexAttributeDescriptor(VertexAttribute.Normal),
			new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
		};

		protected static ProfilerMarker m_SetupRenderMarker = new ProfilerMarker("SetupExtrudedRopeRendering");

		protected static ProfilerMarker m_RenderMarker = new ProfilerMarker("ExtrudedRopeRendering");

		protected ObiSolver m_Solver;

		protected SubMeshDescriptor subMeshDescriptor = new SubMeshDescriptor(0, 0);

		protected List<ProceduralRenderBatch<ProceduralRopeVertex>> batchList = new List<ProceduralRenderBatch<ProceduralRopeVertex>>();

		protected ObiNativeList<BurstExtrudedMeshData> rendererData;

		protected ObiNativeList<int> pathSmootherIndices;

		protected Dictionary<ObiRopeSection, int> sectionToIndex = new Dictionary<ObiRopeSection, int>();

		protected ObiNativeVector2List sectionData;

		protected ObiNativeList<int> sectionOffsets;

		protected ObiNativeList<int> sectionIndices;

		protected ObiNativeList<int> vertexOffsets;

		protected ObiNativeList<int> triangleOffsets;

		protected ObiNativeList<int> vertexCounts;

		protected ObiNativeList<int> triangleCounts;

		protected ObiPathSmootherRenderSystem pathSmootherSystem;

		public Oni.RenderingSystemType typeEnum => Oni.RenderingSystemType.ExtrudedRope;

		public RendererSet<ObiRopeExtrudedRenderer> renderers { get; } = new RendererSet<ObiRopeExtrudedRenderer>();

		public ObiExtrudedRopeRenderSystem(ObiSolver solver)
		{
			m_Solver = solver;
			rendererData = new ObiNativeList<BurstExtrudedMeshData>();
			pathSmootherIndices = new ObiNativeList<int>();
			sectionData = new ObiNativeVector2List();
			sectionOffsets = new ObiNativeList<int>();
			sectionIndices = new ObiNativeList<int>();
			vertexOffsets = new ObiNativeList<int>();
			triangleOffsets = new ObiNativeList<int>();
			vertexCounts = new ObiNativeList<int>();
			triangleCounts = new ObiNativeList<int>();
		}

		public void Dispose()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Dispose();
			}
			batchList.Clear();
			if (rendererData != null)
			{
				rendererData.Dispose();
			}
			if (pathSmootherIndices != null)
			{
				pathSmootherIndices.Dispose();
			}
			if (sectionData != null)
			{
				sectionData.Dispose();
			}
			if (sectionOffsets != null)
			{
				sectionOffsets.Dispose();
			}
			if (sectionIndices != null)
			{
				sectionIndices.Dispose();
			}
			if (vertexOffsets != null)
			{
				vertexOffsets.Dispose();
			}
			if (triangleOffsets != null)
			{
				triangleOffsets.Dispose();
			}
			if (vertexCounts != null)
			{
				vertexCounts.Dispose();
			}
			if (triangleCounts != null)
			{
				triangleCounts.Dispose();
			}
		}

		private void Clear()
		{
			rendererData.Clear();
			pathSmootherIndices.Clear();
			sectionData.Clear();
			sectionToIndex.Clear();
			sectionOffsets.Clear();
			sectionIndices.Clear();
			vertexOffsets.Clear();
			triangleOffsets.Clear();
			vertexCounts.Clear();
			triangleCounts.Clear();
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Dispose();
			}
			batchList.Clear();
		}

		private void CreateBatches()
		{
			sortedRenderers.Clear();
			for (int i = 0; i < renderers.Count; i++)
			{
				if (renderers[i].section != null && renderers[i].TryGetComponent<ObiPathSmoother>(out var component) && component.enabled)
				{
					renderers[i].renderParameters.layer = renderers[i].gameObject.layer;
					batchList.Add(new ProceduralRenderBatch<ProceduralRopeVertex>(i, renderers[i].material, renderers[i].renderParameters));
					sortedRenderers.Add(renderers[i]);
				}
			}
			vertexOffsets.ResizeUninitialized(sortedRenderers.Count);
			triangleOffsets.ResizeUninitialized(sortedRenderers.Count);
			batchList.Sort();
			sortedRenderers.Clear();
			for (int j = 0; j < batchList.Count; j++)
			{
				ProceduralRenderBatch<ProceduralRopeVertex> proceduralRenderBatch = batchList[j];
				sortedRenderers.Add(renderers[proceduralRenderBatch.firstRenderer]);
				proceduralRenderBatch.firstRenderer = j;
				int indexInSystem = sortedRenderers[j].GetComponent<ObiPathSmoother>().indexInSystem;
				pathSmootherIndices.Add(indexInSystem);
				if (!sectionToIndex.TryGetValue(sortedRenderers[j].section, out var value))
				{
					value = sectionOffsets.count;
					sectionToIndex[sortedRenderers[j].section] = value;
					sectionOffsets.Add(sectionData.count);
					sectionData.AddRange(sortedRenderers[j].section.vertices);
				}
				sectionIndices.Add(value);
				int num = pathSmootherSystem.chunkOffsets[indexInSystem];
				int num2 = pathSmootherSystem.chunkOffsets[indexInSystem + 1] - num;
				for (int k = num; k < num + num2; k++)
				{
					int num3 = pathSmootherSystem.smoothFrameCounts[k];
					proceduralRenderBatch.vertexCount += num3 * sortedRenderers[j].section.vertices.Count;
					proceduralRenderBatch.triangleCount += (num3 - 1) * (sortedRenderers[j].section.vertices.Count - 1) * 2;
				}
				vertexCounts.Add(proceduralRenderBatch.vertexCount);
				triangleCounts.Add(proceduralRenderBatch.triangleCount);
				rendererData.Add(new BurstExtrudedMeshData(sortedRenderers[j]));
			}
			sectionOffsets.Add(sectionData.count);
		}

		private void CalculateMeshOffsets()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				ProceduralRenderBatch<ProceduralRopeVertex> proceduralRenderBatch = batchList[i];
				int num = 0;
				int num2 = 0;
				for (int j = 0; j < proceduralRenderBatch.rendererCount; j++)
				{
					int index = proceduralRenderBatch.firstRenderer + j;
					vertexOffsets[index] = num;
					triangleOffsets[index] = num2;
					num += vertexCounts[index];
					num2 += triangleCounts[index];
				}
			}
		}

		public virtual void Setup()
		{
			pathSmootherSystem = m_Solver.GetRenderSystem<ObiPathSmoother>() as ObiPathSmootherRenderSystem;
			if (pathSmootherSystem == null)
			{
				return;
			}
			using (m_SetupRenderMarker.Auto())
			{
				Clear();
				CreateBatches();
				ObiUtils.MergeBatches(batchList);
				CalculateMeshOffsets();
			}
		}

		public abstract void Render();

		public void Step()
		{
		}

		public void BakeMesh(ObiRopeExtrudedRenderer renderer, ref Mesh mesh, bool transformToActorLocalSpace = false)
		{
			int num = sortedRenderers.IndexOf(renderer);
			for (int i = 0; i < batchList.Count; i++)
			{
				ProceduralRenderBatch<ProceduralRopeVertex> proceduralRenderBatch = batchList[i];
				if (num >= proceduralRenderBatch.firstRenderer && num < proceduralRenderBatch.firstRenderer + proceduralRenderBatch.rendererCount)
				{
					proceduralRenderBatch.BakeMesh(vertexOffsets[num], vertexCounts[num], triangleOffsets[num], triangleCounts[num], renderer.actor.actorSolverToLocalMatrix, ref mesh, transformToActorLocalSpace);
					break;
				}
			}
		}
	}
}
