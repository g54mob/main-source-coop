using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public abstract class ObiLineRopeRenderSystem : RenderSystem<ObiRopeLineRenderer>, IRenderSystem
	{
		protected List<ObiRopeLineRenderer> sortedRenderers = new List<ObiRopeLineRenderer>();

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

		protected ObiNativeList<int> pathSmootherIndices;

		protected ObiNativeList<BurstLineMeshData> rendererData;

		protected ObiNativeList<int> vertexOffsets;

		protected ObiNativeList<int> triangleOffsets;

		protected ObiNativeList<int> vertexCounts;

		protected ObiNativeList<int> triangleCounts;

		protected ObiPathSmootherRenderSystem pathSmootherSystem;

		private Action<ScriptableRenderContext, Camera> renderCallback;

		public Oni.RenderingSystemType typeEnum => Oni.RenderingSystemType.LineRope;

		public RendererSet<ObiRopeLineRenderer> renderers { get; } = new RendererSet<ObiRopeLineRenderer>();

		public ObiLineRopeRenderSystem(ObiSolver solver)
		{
			renderCallback = delegate(ScriptableRenderContext cntxt, Camera cam)
			{
				RenderFromCamera(cam);
			};
			RenderPipelineManager.beginCameraRendering += renderCallback;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(RenderFromCamera));
			m_Solver = solver;
			pathSmootherIndices = new ObiNativeList<int>();
			rendererData = new ObiNativeList<BurstLineMeshData>();
			vertexOffsets = new ObiNativeList<int>();
			triangleOffsets = new ObiNativeList<int>();
			vertexCounts = new ObiNativeList<int>();
			triangleCounts = new ObiNativeList<int>();
		}

		public void Dispose()
		{
			RenderPipelineManager.beginCameraRendering -= renderCallback;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(RenderFromCamera));
			for (int i = 0; i < batchList.Count; i++)
			{
				batchList[i].Dispose();
			}
			batchList.Clear();
			if (pathSmootherIndices != null)
			{
				pathSmootherIndices.Dispose();
			}
			if (rendererData != null)
			{
				rendererData.Dispose();
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
			pathSmootherIndices.Clear();
			rendererData.Clear();
			vertexOffsets.Clear();
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
				if (renderers[i].TryGetComponent<ObiPathSmoother>(out var component) && component.enabled)
				{
					renderers[i].renderParams.layer = renderers[i].gameObject.layer;
					batchList.Add(new ProceduralRenderBatch<ProceduralRopeVertex>(i, renderers[i].material, renderers[i].renderParams));
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
				int num = pathSmootherSystem.chunkOffsets[indexInSystem];
				int num2 = pathSmootherSystem.chunkOffsets[indexInSystem + 1] - num;
				for (int k = num; k < num + num2; k++)
				{
					int num3 = pathSmootherSystem.smoothFrameCounts[k];
					proceduralRenderBatch.vertexCount += num3 * 2;
					proceduralRenderBatch.triangleCount += (num3 - 1) * 2;
				}
				vertexCounts.Add(proceduralRenderBatch.vertexCount);
				triangleCounts.Add(proceduralRenderBatch.triangleCount);
				rendererData.Add(new BurstLineMeshData(sortedRenderers[j]));
			}
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

		public abstract void RenderFromCamera(Camera camera);

		public abstract void Render();

		public void Step()
		{
		}
	}
}
