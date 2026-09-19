using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public abstract class ObiMeshRopeRenderSystem : RenderSystem<ObiRopeMeshRenderer>, IRenderSystem
	{
		protected List<ObiRopeMeshRenderer> sortedRenderers = new List<ObiRopeMeshRenderer>();

		protected static ProfilerMarker m_SetupRenderMarker = new ProfilerMarker("SetupMeshRopeRendering");

		protected static ProfilerMarker m_RenderMarker = new ProfilerMarker("MeshRopeRendering");

		protected VertexAttributeDescriptor[] layout = new VertexAttributeDescriptor[8]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
			new VertexAttributeDescriptor(VertexAttribute.Normal),
			new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 1),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 2, 1),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord2, VertexAttributeFormat.Float32, 2, 1),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord3, VertexAttributeFormat.Float32, 2, 1)
		};

		protected ObiSolver m_Solver;

		protected List<DynamicRenderBatch<ObiRopeMeshRenderer>> batchList = new List<DynamicRenderBatch<ObiRopeMeshRenderer>>();

		protected MeshDataBatch meshData;

		protected ObiNativeList<int> meshIndices;

		protected ObiNativeList<int> pathSmootherIndices;

		protected ObiNativeList<BurstMeshData> rendererData;

		protected ObiNativeList<int> sortedIndices;

		protected ObiNativeList<int> sortedOffsets;

		protected ObiNativeList<int> vertexOffsets;

		protected ObiNativeList<int> vertexCounts;

		protected ObiPathSmootherRenderSystem pathSmootherSystem;

		public Oni.RenderingSystemType typeEnum => Oni.RenderingSystemType.MeshRope;

		public RendererSet<ObiRopeMeshRenderer> renderers { get; } = new RendererSet<ObiRopeMeshRenderer>();

		public ObiMeshRopeRenderSystem(ObiSolver solver)
		{
			m_Solver = solver;
			meshData = new MeshDataBatch();
			meshIndices = new ObiNativeList<int>();
			pathSmootherIndices = new ObiNativeList<int>();
			rendererData = new ObiNativeList<BurstMeshData>();
			sortedIndices = new ObiNativeList<int>();
			sortedOffsets = new ObiNativeList<int>();
			vertexOffsets = new ObiNativeList<int>();
			vertexCounts = new ObiNativeList<int>();
		}

		public void Dispose()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Dispose();
			}
			batchList.Clear();
			meshData.Dispose();
			if (pathSmootherIndices != null)
			{
				pathSmootherIndices.Dispose();
			}
			if (meshIndices != null)
			{
				meshIndices.Dispose();
			}
			if (sortedIndices != null)
			{
				sortedIndices.Dispose();
			}
			if (sortedOffsets != null)
			{
				sortedOffsets.Dispose();
			}
			if (vertexOffsets != null)
			{
				vertexOffsets.Dispose();
			}
			if (vertexCounts != null)
			{
				vertexCounts.Dispose();
			}
			if (rendererData != null)
			{
				rendererData.Dispose();
			}
		}

		private void Clear()
		{
			meshData.Clear();
			meshIndices.Clear();
			pathSmootherIndices.Clear();
			rendererData.Clear();
			vertexOffsets.Clear();
			vertexCounts.Clear();
			sortedIndices.Clear();
			sortedOffsets.Clear();
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Dispose();
			}
			batchList.Clear();
			meshData.InitializeStaticData();
			meshData.InitializeTempData();
		}

		private void CreateBatches()
		{
			sortedRenderers.Clear();
			for (int i = 0; i < renderers.Count; i++)
			{
				if (renderers[i].sourceMesh != null && renderers[i].TryGetComponent<ObiPathSmoother>(out var component) && component.enabled)
				{
					int vertexCount = renderers[i].vertexCount * (int)renderers[i].meshInstances;
					renderers[i].renderParameters.layer = renderers[i].gameObject.layer;
					batchList.Add(new DynamicRenderBatch<ObiRopeMeshRenderer>(i, vertexCount, renderers[i].materials, renderers[i].renderParameters));
					sortedRenderers.Add(renderers[i]);
				}
			}
			vertexOffsets.ResizeUninitialized(sortedRenderers.Count);
			batchList.Sort();
			sortedRenderers.Clear();
			for (int j = 0; j < batchList.Count; j++)
			{
				DynamicRenderBatch<ObiRopeMeshRenderer> dynamicRenderBatch = batchList[j];
				vertexCounts.Add(dynamicRenderBatch.vertexCount);
				sortedRenderers.Add(renderers[dynamicRenderBatch.firstRenderer]);
				dynamicRenderBatch.firstRenderer = j;
				pathSmootherIndices.Add(sortedRenderers[j].GetComponent<ObiPathSmoother>().indexInSystem);
				rendererData.Add(new BurstMeshData(sortedRenderers[j]));
			}
		}

		protected virtual void PopulateBatches()
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < sortedRenderers.Count; i++)
			{
				sortedRenderers[i].GetVertices(list);
				float[] array = new float[sortedRenderers[i].vertexCount];
				int[] array2 = new int[sortedRenderers[i].vertexCount];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = list[j][(int)sortedRenderers[i].axis];
					array2[j] = j;
				}
				Array.Sort(array, array2);
				sortedOffsets.Add(sortedIndices.count);
				sortedIndices.AddRange(array2);
				meshIndices.Add(meshData.AddMesh(sortedRenderers[i]));
			}
		}

		private void CalculateMeshOffsets()
		{
			for (int i = 0; i < batchList.Count; i++)
			{
				DynamicRenderBatch<ObiRopeMeshRenderer> dynamicRenderBatch = batchList[i];
				int num = 0;
				for (int j = 0; j < dynamicRenderBatch.rendererCount; j++)
				{
					int index = dynamicRenderBatch.firstRenderer + j;
					vertexOffsets[index] = num;
					num += vertexCounts[index];
				}
			}
		}

		protected virtual void CloseBatches()
		{
			meshData.DisposeOfStaticData();
			meshData.DisposeOfTempData();
		}

		public void Setup()
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
				PopulateBatches();
				ObiUtils.MergeBatches(batchList);
				CalculateMeshOffsets();
				CloseBatches();
			}
		}

		public void Step()
		{
		}

		public virtual void Render()
		{
		}

		public void BakeMesh(ObiRopeMeshRenderer renderer, ref Mesh mesh, bool transformToActorLocalSpace = false)
		{
			int num = sortedRenderers.IndexOf(renderer);
			for (int i = 0; i < batchList.Count; i++)
			{
				DynamicRenderBatch<ObiRopeMeshRenderer> dynamicRenderBatch = batchList[i];
				if (num >= dynamicRenderBatch.firstRenderer && num < dynamicRenderBatch.firstRenderer + dynamicRenderBatch.rendererCount)
				{
					dynamicRenderBatch.BakeMesh(sortedRenderers, renderer, ref mesh, transformToActorLocalSpace);
					break;
				}
			}
		}
	}
}
