using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class DynamicRenderBatch<T> : IRenderBatch, IComparable<IRenderBatch> where T : IMeshDataProvider, IActorRenderer
	{
		private VertexAttributeDescriptor[] vertexLayout;

		private RenderBatchParams renderBatchParams;

		public Material[] materials;

		public Mesh mesh;

		public int firstRenderer;

		public int rendererCount;

		public ObiNativeList<int> vertexToRenderer;

		public ObiNativeList<int> particleToRenderer;

		public ObiNativeList<int> particleIndices;

		public ObiNativeList<DynamicBatchVertex> dynamicVertexData;

		public ObiNativeList<StaticBatchVertex> staticVertexData;

		public ObiNativeList<int> triangles;

		public GraphicsBuffer gpuVertexBuffer;

		public int vertexCount;

		public RenderParams renderParams { get; private set; }

		public int triangleCount => triangles.count / 3;

		public int particleCount => particleIndices.count;

		public DynamicRenderBatch(int rendererIndex, int vertexCount, Material[] materials, RenderBatchParams param)
		{
			renderBatchParams = param;
			this.materials = materials;
			this.vertexCount = vertexCount;
			firstRenderer = rendererIndex;
			rendererCount = 1;
		}

		public void Initialize(List<T> renderers, MeshDataBatch meshData, ObiNativeList<int> meshIndices, VertexAttributeDescriptor[] layout, bool gpu = false)
		{
			renderParams = renderBatchParams.ToRenderParams();
			vertexLayout = layout;
			mesh = new Mesh();
			vertexToRenderer = new ObiNativeList<int>();
			particleToRenderer = new ObiNativeList<int>();
			particleIndices = new ObiNativeList<int>();
			dynamicVertexData = new ObiNativeList<DynamicBatchVertex>();
			staticVertexData = new ObiNativeList<StaticBatchVertex>();
			triangles = new ObiNativeList<int>();
			SubMeshDescriptor[] array = new SubMeshDescriptor[materials.Length];
			for (int i = 0; i < materials.Length; i++)
			{
				int num = 0;
				SubMeshDescriptor subMeshDescriptor = new SubMeshDescriptor
				{
					indexStart = triangles.count
				};
				for (int j = firstRenderer; j < firstRenderer + rendererCount; j++)
				{
					T val = renderers[j];
					int meshIndex = meshIndices[j];
					int index = Mathf.Min(i, val.sourceMesh.subMeshCount - 1);
					SubMeshDescriptor subMesh = val.sourceMesh.GetSubMesh(index);
					NativeSlice<int> nativeSlice = meshData.GetTriangles(meshIndex);
					for (int k = 0; k < val.meshInstances; k++)
					{
						for (int l = subMesh.indexStart; l < subMesh.indexStart + subMesh.indexCount; l++)
						{
							triangles.Add(num + nativeSlice[l]);
						}
						num += meshData.GetVertexCount(meshIndex);
					}
				}
				subMeshDescriptor.indexCount = triangles.count - subMeshDescriptor.indexStart;
				array[i] = subMeshDescriptor;
			}
			for (int m = firstRenderer; m < firstRenderer + rendererCount; m++)
			{
				T val2 = renderers[m];
				int meshIndex2 = meshIndices[m];
				int num2 = meshData.GetVertexCount(meshIndex2);
				for (int n = 0; n < val2.meshInstances; n++)
				{
					vertexToRenderer.AddReplicate(m, num2);
					particleToRenderer.AddReplicate(m, val2.actor.solverIndices.count);
					particleIndices.AddRange(val2.actor.solverIndices);
					NativeSlice<Vector3> vertices = meshData.GetVertices(meshIndex2);
					NativeSlice<Vector3> normals = meshData.GetNormals(meshIndex2);
					NativeSlice<Vector4> tangents = meshData.GetTangents(meshIndex2);
					NativeSlice<Color> colors = meshData.GetColors(meshIndex2);
					NativeSlice<Vector2> uV = meshData.GetUV(meshIndex2);
					NativeSlice<Vector2> uV2 = meshData.GetUV2(meshIndex2);
					NativeSlice<Vector2> uV3 = meshData.GetUV3(meshIndex2);
					NativeSlice<Vector2> uV4 = meshData.GetUV4(meshIndex2);
					for (int num3 = 0; num3 < num2; num3++)
					{
						dynamicVertexData.Add(new DynamicBatchVertex
						{
							pos = vertices[num3],
							normal = normals[num3],
							tangent = tangents[num3],
							color = ((num3 < colors.Length) ? ((Vector4)colors[num3]) : Vector4.one)
						});
						staticVertexData.Add(new StaticBatchVertex
						{
							uv = ((num3 < uV.Length) ? uV[num3] : Vector2.zero),
							uv2 = ((num3 < uV2.Length) ? uV2[num3] : Vector2.zero),
							uv3 = ((num3 < uV3.Length) ? uV3[num3] : Vector2.zero),
							uv4 = ((num3 < uV4.Length) ? uV4[num3] : Vector2.zero)
						});
					}
				}
			}
			mesh.SetVertexBufferParams(vertexCount, layout);
			mesh.SetIndexBufferParams(triangles.count, IndexFormat.UInt32);
			mesh.SetVertexBufferData(dynamicVertexData.AsNativeArray<DynamicBatchVertex>(), 0, 0, dynamicVertexData.count, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			mesh.SetVertexBufferData(staticVertexData.AsNativeArray<StaticBatchVertex>(), 0, 0, staticVertexData.count, 1, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			mesh.SetIndexBufferData(triangles.AsNativeArray<int>(), 0, 0, triangles.count, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			mesh.subMeshCount = materials.Length;
			for (int num4 = 0; num4 < materials.Length; num4++)
			{
				mesh.SetSubMesh(num4, array[num4], MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
			}
			if (!gpu)
			{
				return;
			}
			dynamicVertexData.Dispose();
			mesh.vertexBufferTarget |= GraphicsBuffer.Target.Raw;
			try
			{
				if (mesh.vertexCount > 0 && gpuVertexBuffer == null)
				{
					gpuVertexBuffer = mesh.GetVertexBuffer(0);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			particleIndices.AsComputeBuffer<int>();
			vertexToRenderer.AsComputeBuffer<int>();
			particleToRenderer.AsComputeBuffer<int>();
		}

		public void Dispose()
		{
			if (vertexToRenderer != null)
			{
				vertexToRenderer.Dispose();
			}
			if (particleToRenderer != null)
			{
				particleToRenderer.Dispose();
			}
			if (particleIndices != null)
			{
				particleIndices.Dispose();
			}
			if (dynamicVertexData != null)
			{
				dynamicVertexData.Dispose();
			}
			if (staticVertexData != null)
			{
				staticVertexData.Dispose();
			}
			if (triangles != null)
			{
				triangles.Dispose();
			}
			gpuVertexBuffer?.Dispose();
			gpuVertexBuffer = null;
			UnityEngine.Object.DestroyImmediate(mesh);
		}

		public bool TryMergeWith(IRenderBatch other)
		{
			if (other is DynamicRenderBatch<T> dynamicRenderBatch && CompareTo(dynamicRenderBatch) == 0 && vertexCount + dynamicRenderBatch.vertexCount < 65000)
			{
				rendererCount += dynamicRenderBatch.rendererCount;
				vertexCount += dynamicRenderBatch.vertexCount;
				return true;
			}
			return false;
		}

		private static int CompareMaterialLists(Material[] a, Material[] b)
		{
			int num = Mathf.Min(a.Length, b.Length);
			for (int i = 0; i < num; i++)
			{
				if (a[i] == null && b[i] == null)
				{
					return 0;
				}
				if (a[i] == null)
				{
					return -1;
				}
				if (b[i] == null)
				{
					return 1;
				}
				int num2 = a[i].GetInstanceID().CompareTo(b[i].GetInstanceID());
				if (num2 != 0)
				{
					return num2;
				}
			}
			return a.Length.CompareTo(b.Length);
		}

		public int CompareTo(IRenderBatch other)
		{
			DynamicRenderBatch<T> dynamicRenderBatch = other as DynamicRenderBatch<T>;
			int num = CompareMaterialLists(materials, dynamicRenderBatch.materials);
			if (num == 0)
			{
				return renderBatchParams.CompareTo(dynamicRenderBatch.renderBatchParams);
			}
			return num;
		}

		public void BakeMesh(List<T> renderers, T renderer, ref Mesh bakedMesh, bool transformToActorLocalSpace = false)
		{
			bool flag = !dynamicVertexData.isCreated || dynamicVertexData == null;
			if (flag)
			{
				dynamicVertexData = new ObiNativeList<DynamicBatchVertex>();
				dynamicVertexData.ResizeUninitialized(vertexCount);
				NativeArray<DynamicBatchVertex> output = dynamicVertexData.AsNativeArray<DynamicBatchVertex>();
				AsyncGPUReadback.RequestIntoNativeArray(ref output, gpuVertexBuffer, vertexCount * dynamicVertexData.stride, 0).WaitForCompletion();
			}
			bakedMesh.Clear();
			int num = 0;
			int num2 = 0;
			for (int i = firstRenderer; i < firstRenderer + rendererCount; i++)
			{
				int num3 = 0;
				for (int j = 0; j < renderers[i].meshInstances; j++)
				{
					num3 += renderers[i].sourceMesh.vertexCount;
				}
				int num4 = 0;
				for (int k = 0; k < materials.Length; k++)
				{
					int index = Mathf.Min(k, renderers[i].sourceMesh.subMeshCount - 1);
					num4 += renderers[i].sourceMesh.GetSubMesh(index).indexCount * (int)renderers[i].meshInstances;
				}
				if (renderers[i].Equals(renderer))
				{
					bakedMesh.SetVertexBufferParams(num3, vertexLayout);
					bakedMesh.SetVertexBufferData(dynamicVertexData.AsNativeArray<DynamicBatchVertex>(), num, 0, num3, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
					bakedMesh.SetVertexBufferData(staticVertexData.AsNativeArray<StaticBatchVertex>(), num, 0, num3, 1, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
					if (transformToActorLocalSpace)
					{
						Matrix4x4 actorSolverToLocalMatrix = renderer.actor.actorSolverToLocalMatrix;
						Vector3[] vertices = bakedMesh.vertices;
						for (int l = 0; l < vertices.Length; l++)
						{
							vertices[l] = actorSolverToLocalMatrix.MultiplyPoint3x4(vertices[l]);
						}
						bakedMesh.vertices = vertices;
					}
					ObiNativeList<int> obiNativeList = new ObiNativeList<int>(num4);
					SubMeshDescriptor[] array = new SubMeshDescriptor[materials.Length];
					for (int m = 0; m < materials.Length; m++)
					{
						int num5 = 0;
						SubMeshDescriptor subMeshDescriptor = new SubMeshDescriptor
						{
							indexStart = obiNativeList.count
						};
						int index2 = Mathf.Min(m, renderer.sourceMesh.subMeshCount - 1);
						SubMeshDescriptor subMesh = renderer.sourceMesh.GetSubMesh(index2);
						for (int n = 0; n < renderer.meshInstances; n++)
						{
							int[] array2 = renderer.sourceMesh.triangles;
							for (int num6 = subMesh.indexStart; num6 < subMesh.indexStart + subMesh.indexCount; num6++)
							{
								obiNativeList.Add(num5 + array2[num6]);
							}
							num5 += renderer.sourceMesh.vertexCount;
						}
						subMeshDescriptor.indexCount = obiNativeList.count - subMeshDescriptor.indexStart;
						array[m] = subMeshDescriptor;
					}
					bakedMesh.SetIndexBufferParams(num4, IndexFormat.UInt32);
					bakedMesh.SetIndexBufferData(obiNativeList.AsNativeArray<int>(), 0, 0, num4, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
					bakedMesh.subMeshCount = materials.Length;
					for (int num7 = 0; num7 < materials.Length; num7++)
					{
						bakedMesh.SetSubMesh(num7, array[num7], MeshUpdateFlags.DontValidateIndices);
					}
					bakedMesh.RecalculateBounds();
					return;
				}
				num += num3;
				num2 += num4;
			}
			if (flag)
			{
				dynamicVertexData.Dispose();
			}
		}
	}
}
