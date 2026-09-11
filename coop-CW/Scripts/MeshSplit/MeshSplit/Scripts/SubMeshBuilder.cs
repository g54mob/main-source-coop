using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace MeshSplit.Scripts
{
	public class SubMeshBuilder
	{
		[BurstCompile]
		private struct BuildSubMeshJob : IJobParallelFor
		{
			private static readonly MeshUpdateFlags MeshUpdateFlags = MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers;

			public int SourceSubMeshIndex;

			[NativeDisableParallelForRestriction]
			public Mesh.MeshDataArray TargetMeshDataArray;

			[NativeDisableContainerSafetyRestriction]
			public NativeList<int> AllIndices;

			[NativeDisableContainerSafetyRestriction]
			public NativeList<int2> IndexRanges;

			[NativeDisableContainerSafetyRestriction]
			public NativeArray<byte> VertexData;

			[NativeDisableContainerSafetyRestriction]
			public NativeArray<VertexAttributeDescriptor> VertexAttributeDescriptors;

			[ReadOnly]
			public int VertexStride;

			public void Execute(int index)
			{
				Mesh.MeshData meshData = TargetMeshDataArray[index];
				int x = IndexRanges[index].x;
				int y = IndexRanges[index].y;
				NativeArray<uint> array = new NativeArray<uint>(y * 3, Allocator.Temp);
				NativeArray<byte> nativeArray = new NativeArray<byte>(VertexStride * y, Allocator.Temp);
				int num = 0;
				for (int i = 0; i < y; i += 3)
				{
					int sourceVertexIndex = x + i;
					int sourceVertexIndex2 = x + i + 1;
					int sourceVertexIndex3 = x + i + 2;
					AddVertex(nativeArray, sourceVertexIndex, num++);
					AddVertex(nativeArray, sourceVertexIndex2, num++);
					AddVertex(nativeArray, sourceVertexIndex3, num++);
					array[i] = (uint)i;
					array[i + 1] = (uint)(i + 1);
					array[i + 2] = (uint)(i + 2);
				}
				meshData.SetVertexBufferParams(y, VertexAttributeDescriptors);
				meshData.GetVertexData<byte>().CopyFrom(nativeArray);
				IndexFormat indexFormat = ((array.Length >= 65535) ? IndexFormat.UInt32 : IndexFormat.UInt16);
				meshData.SetIndexBufferParams(array.Length, indexFormat);
				switch (indexFormat)
				{
				case IndexFormat.UInt16:
				{
					NativeArray<ushort> indexData = meshData.GetIndexData<ushort>();
					NativeArray<ushort> array2 = new NativeArray<ushort>(array.Length, Allocator.Temp);
					for (int j = 0; j < array.Length; j++)
					{
						array2[j] = (ushort)array[j];
					}
					indexData.CopyFrom(array2);
					array2.Dispose();
					break;
				}
				case IndexFormat.UInt32:
					meshData.GetIndexData<uint>().CopyFrom(array);
					break;
				}
				meshData.subMeshCount = 1;
				meshData.SetSubMesh(0, new SubMeshDescriptor(0, array.Length), MeshUpdateFlags);
				array.Dispose();
				nativeArray.Dispose();
			}

			private unsafe void AddVertex(NativeArray<byte> targetVertexData, int sourceVertexIndex, int targetVertexIndex)
			{
				int num = AllIndices[sourceVertexIndex];
				void* source = (void*)IntPtr.Add((IntPtr)VertexData.GetUnsafePtr(), num * VertexStride);
				UnsafeUtility.MemCpy((void*)IntPtr.Add((IntPtr)targetVertexData.GetUnsafePtr(), targetVertexIndex * VertexStride), source, VertexStride);
			}
		}

		private readonly Dictionary<Vector3Int, List<int>> _pointIndices;

		private readonly byte[] _vertexData;

		private readonly int _vertexBufferStride;

		private readonly VertexAttributeDescriptor[] _vertexAttributeDescriptors;

		public SubMeshBuilder(Dictionary<Vector3Int, List<int>> pointIndices, byte[] vertexData, int vertexBufferStride, VertexAttributeDescriptor[] vertexAttributeDescriptors)
		{
			_pointIndices = pointIndices;
			_vertexData = vertexData;
			_vertexBufferStride = vertexBufferStride;
			_vertexAttributeDescriptors = vertexAttributeDescriptors;
		}

		private (NativeList<int> allIndices, NativeList<int2> indexRangesArray) FlattenPointIndices()
		{
			NativeList<int> item = new NativeList<int>(100, Allocator.Persistent);
			NativeList<int2> item2 = new NativeList<int2>(100, Allocator.Persistent);
			foreach (KeyValuePair<Vector3Int, List<int>> pointIndex in _pointIndices)
			{
				NativeArray<int> array = new NativeArray<int>(pointIndex.Value.ToArray(), Allocator.Temp);
				item2.Add(new int2(item.Length, array.Length));
				item.AddRange(array);
				array.Dispose();
			}
			return (allIndices: item, indexRangesArray: item2);
		}

		public Mesh.MeshDataArray Build(Mesh mesh)
		{
			NativeArray<Vector3Int> nativeArray = new NativeArray<Vector3Int>(_pointIndices.Keys.ToArray(), Allocator.TempJob);
			(NativeList<int> allIndices, NativeList<int2> indexRangesArray) tuple = FlattenPointIndices();
			NativeList<int> item = tuple.allIndices;
			NativeList<int2> item2 = tuple.indexRangesArray;
			Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(_pointIndices.Count);
			Mesh.MeshDataArray meshDataArray2 = Mesh.AcquireReadOnlyMeshData(mesh);
			NativeArray<byte> vertexData = new NativeArray<byte>(_vertexData, Allocator.TempJob);
			NativeArray<VertexAttributeDescriptor> vertexAttributeDescriptors = new NativeArray<VertexAttributeDescriptor>(_vertexAttributeDescriptors, Allocator.TempJob);
			JobHandle? jobHandle = null;
			int innerloopBatchCount = meshDataArray2.Length / Mathf.Clamp(Environment.ProcessorCount, 1, 8);
			for (int i = 0; i < meshDataArray2.Length; i++)
			{
				BuildSubMeshJob jobData = new BuildSubMeshJob
				{
					AllIndices = item,
					IndexRanges = item2,
					VertexData = vertexData,
					VertexStride = _vertexBufferStride,
					VertexAttributeDescriptors = vertexAttributeDescriptors,
					SourceSubMeshIndex = i,
					TargetMeshDataArray = meshDataArray
				};
				jobHandle = (jobHandle.HasValue ? IJobParallelForExtensions.Schedule(jobData, nativeArray.Length, innerloopBatchCount, jobHandle.Value) : IJobParallelForExtensions.Schedule(jobData, nativeArray.Length, innerloopBatchCount));
			}
			jobHandle?.Complete();
			vertexData.Dispose();
			vertexAttributeDescriptors.Dispose();
			item.Dispose();
			item2.Dispose();
			nativeArray.Dispose();
			meshDataArray2.Dispose();
			return meshDataArray;
		}
	}
}
