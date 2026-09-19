using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class ProceduralRenderBatch<T> : IRenderBatch, IComparable<IRenderBatch> where T : struct
	{
		private RenderBatchParams renderBatchParams;

		public Material material;

		public Mesh mesh;

		public int firstRenderer;

		public int rendererCount;

		public int firstParticle;

		public NativeArray<T> vertices;

		public NativeArray<int> triangles;

		public GraphicsBuffer gpuVertexBuffer;

		public GraphicsBuffer gpuIndexBuffer;

		public int vertexCount;

		public int triangleCount;

		public RenderParams renderParams { get; private set; }

		public ProceduralRenderBatch(int rendererIndex, Material material, RenderBatchParams param)
		{
			renderBatchParams = param;
			this.material = material;
			firstRenderer = rendererIndex;
			firstParticle = 0;
			rendererCount = 1;
			vertexCount = 0;
			triangleCount = 0;
		}

		public void Initialize(VertexAttributeDescriptor[] layout, bool gpu = false)
		{
			RenderParams renderParams = renderBatchParams.ToRenderParams();
			renderParams.material = material;
			this.renderParams = renderParams;
			mesh = new Mesh();
			mesh.SetVertexBufferParams(vertexCount, layout);
			mesh.SetIndexBufferParams(triangleCount * 3, IndexFormat.UInt32);
			vertices = new NativeArray<T>(vertexCount, Allocator.Persistent);
			mesh.SetVertexBufferData(vertices, 0, 0, vertices.Length, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			triangles = new NativeArray<int>(triangleCount * 3, Allocator.Persistent);
			mesh.SetIndexBufferData(triangles, 0, 0, triangles.Length, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			mesh.subMeshCount = 1;
			SubMeshDescriptor desc = new SubMeshDescriptor
			{
				indexCount = triangleCount * 3
			};
			mesh.SetSubMesh(0, desc, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			if (!gpu)
			{
				return;
			}
			vertices.Dispose();
			triangles.Dispose();
			mesh.vertexBufferTarget |= GraphicsBuffer.Target.Raw;
			mesh.indexBufferTarget |= GraphicsBuffer.Target.Raw;
			if (mesh.vertexCount > 0)
			{
				if (gpuVertexBuffer == null)
				{
					gpuVertexBuffer = mesh.GetVertexBuffer(0);
				}
				if (gpuIndexBuffer == null)
				{
					gpuIndexBuffer = mesh.GetIndexBuffer();
				}
			}
		}

		public void Dispose()
		{
			gpuVertexBuffer?.Dispose();
			gpuIndexBuffer?.Dispose();
			gpuVertexBuffer = null;
			gpuIndexBuffer = null;
			if (vertices.IsCreated)
			{
				vertices.Dispose();
			}
			if (triangles.IsCreated)
			{
				triangles.Dispose();
			}
			UnityEngine.Object.DestroyImmediate(mesh);
		}

		public bool TryMergeWith(IRenderBatch other)
		{
			if (other is ProceduralRenderBatch<T> proceduralRenderBatch && CompareTo(proceduralRenderBatch) == 0 && vertexCount + proceduralRenderBatch.vertexCount < 65000)
			{
				rendererCount += proceduralRenderBatch.rendererCount;
				triangleCount += proceduralRenderBatch.triangleCount;
				vertexCount += proceduralRenderBatch.vertexCount;
				return true;
			}
			return false;
		}

		public int CompareTo(IRenderBatch other)
		{
			ProceduralRenderBatch<T> proceduralRenderBatch = other as ProceduralRenderBatch<T>;
			int num = ((material != null) ? material.GetInstanceID() : 0);
			int value = ((proceduralRenderBatch != null && proceduralRenderBatch.material != null) ? proceduralRenderBatch.material.GetInstanceID() : 0);
			int num2 = num.CompareTo(value);
			if (num2 == 0)
			{
				return renderBatchParams.CompareTo(proceduralRenderBatch.renderBatchParams);
			}
			return num2;
		}

		public void BakeMesh(int vertexOffset, int vertexCount, int triangleOffset, int triangleCount, Matrix4x4 transform, ref Mesh bakedMesh, bool transformVertices = false)
		{
			bool flag = !vertices.IsCreated;
			if (flag)
			{
				vertices = new NativeArray<T>(this.vertexCount, Allocator.Persistent);
				triangles = new NativeArray<int>(this.triangleCount * 3, Allocator.Persistent);
				AsyncGPUReadback.RequestIntoNativeArray(ref vertices, gpuVertexBuffer, this.vertexCount * UnsafeUtility.SizeOf<T>(), 0).WaitForCompletion();
				AsyncGPUReadback.RequestIntoNativeArray(ref triangles, gpuIndexBuffer, this.triangleCount * 3 * 4, 0).WaitForCompletion();
			}
			bakedMesh.Clear();
			bakedMesh.SetVertexBufferParams(vertexCount, mesh.GetVertexAttributes());
			bakedMesh.SetVertexBufferData(vertices, vertexOffset, 0, vertexCount, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			if (transformVertices)
			{
				Matrix4x4 matrix4x = transform;
				Vector3[] array = bakedMesh.vertices;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = matrix4x.MultiplyPoint3x4(array[i]);
				}
				bakedMesh.vertices = array;
			}
			ObiNativeList<int> obiNativeList = new ObiNativeList<int>(triangleCount * 3);
			for (int j = 0; j < triangleCount * 3; j++)
			{
				int b = triangles[triangleOffset * 3 + j] - vertexOffset;
				obiNativeList.Add(Mathf.Max(0, b));
			}
			bakedMesh.SetIndexBufferParams(triangleCount * 3, IndexFormat.UInt32);
			bakedMesh.SetIndexBufferData(obiNativeList.AsNativeArray<int>(), 0, 0, triangleCount * 3, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			bakedMesh.subMeshCount = 1;
			SubMeshDescriptor desc = new SubMeshDescriptor
			{
				indexCount = triangleCount * 3
			};
			bakedMesh.SetSubMesh(0, desc, MeshUpdateFlags.DontValidateIndices);
			if (flag)
			{
				if (vertices.IsCreated)
				{
					vertices.Dispose();
				}
				if (triangles.IsCreated)
				{
					triangles.Dispose();
				}
			}
			bakedMesh.RecalculateBounds();
		}
	}
}
