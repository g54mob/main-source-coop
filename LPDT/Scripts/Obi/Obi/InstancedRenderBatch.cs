using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class InstancedRenderBatch : IRenderBatch, IComparable<IRenderBatch>
	{
		private RenderBatchParams renderBatchParams;

		public Mesh mesh;

		public Material material;

		public int firstRenderer;

		public int rendererCount;

		public int firstInstance;

		public int instanceCount;

		public GraphicsBuffer argsBuffer;

		public RenderParams renderParams { get; private set; }

		public InstancedRenderBatch(int rendererIndex, Mesh mesh, Material material, RenderBatchParams renderBatchParams)
		{
			this.renderBatchParams = renderBatchParams;
			firstRenderer = rendererIndex;
			rendererCount = 1;
			this.mesh = mesh;
			this.material = material;
			firstInstance = 0;
			instanceCount = 0;
		}

		public void Initialize(bool gpu = false)
		{
			renderParams = renderBatchParams.ToRenderParams();
			if (gpu)
			{
				argsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 1, 20);
			}
		}

		public void Dispose()
		{
			argsBuffer?.Dispose();
			argsBuffer = null;
		}

		public bool TryMergeWith(IRenderBatch other)
		{
			if (other is InstancedRenderBatch instancedRenderBatch && CompareTo(instancedRenderBatch) == 0 && instanceCount + instancedRenderBatch.instanceCount < 1023)
			{
				rendererCount += instancedRenderBatch.rendererCount;
				instanceCount += instancedRenderBatch.instanceCount;
				return true;
			}
			return false;
		}

		public int CompareTo(IRenderBatch other)
		{
			InstancedRenderBatch instancedRenderBatch = other as InstancedRenderBatch;
			int num = ((material != null) ? material.GetInstanceID() : 0);
			int value = ((instancedRenderBatch != null && instancedRenderBatch.material != null) ? instancedRenderBatch.material.GetInstanceID() : 0);
			int num2 = num.CompareTo(value);
			if (num2 == 0)
			{
				num = ((mesh != null) ? mesh.GetInstanceID() : 0);
				value = ((instancedRenderBatch != null && instancedRenderBatch.mesh != null) ? instancedRenderBatch.mesh.GetInstanceID() : 0);
				num2 = num.CompareTo(value);
				if (num2 == 0)
				{
					return renderBatchParams.CompareTo(instancedRenderBatch.renderBatchParams);
				}
			}
			return num2;
		}

		public void BakeMesh<T>(RendererSet<T> renderers, T renderer, ObiNativeList<ChunkData> chunkData, ObiNativeList<Matrix4x4> instanceTransforms, Matrix4x4 transform, ref Mesh bakedMesh, bool transformVertices = false) where T : ObiRenderer<T>
		{
			if (argsBuffer != null && argsBuffer.IsValid())
			{
				instanceTransforms.Readback(async: false);
			}
			List<CombineInstance> list = new List<CombineInstance>();
			bakedMesh.Clear();
			for (int i = 0; i < chunkData.count; i++)
			{
				if (renderers[chunkData[i].rendererIndex].Equals(renderer))
				{
					int num = ((i > 0) ? chunkData[i - 1].offset : 0);
					int num2 = chunkData[i].offset - num;
					for (int j = 0; j < num2; j++)
					{
						list.Add(new CombineInstance
						{
							mesh = mesh,
							transform = (transformVertices ? (transform * instanceTransforms[num + j]) : instanceTransforms[num + j])
						});
					}
				}
			}
			bakedMesh.CombineMeshes(list.ToArray(), mergeSubMeshes: true, useMatrices: true, hasLightmapData: false);
			bakedMesh.RecalculateBounds();
		}
	}
}
