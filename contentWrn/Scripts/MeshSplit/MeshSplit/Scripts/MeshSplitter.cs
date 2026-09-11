using System.Collections.Generic;
using System.Linq;
using MeshSplit.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Rendering;

namespace MeshSplit.Scripts
{
	public class MeshSplitter
	{
		private static readonly MeshUpdateFlags MeshUpdateFlags = MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds;

		private readonly MeshSplitParameters _parameters;

		private Mesh _sourceMesh;

		private readonly bool _verbose;

		private Dictionary<Vector3Int, List<int>> _pointIndicesMap;

		private byte[] _vertexData;

		private VertexAttributeDescriptor[] _sourceMeshVertexAttributes;

		public MeshSplitter(MeshSplitParameters parameters, bool verbose)
		{
			_parameters = parameters;
			_verbose = verbose;
		}

		public List<(Vector3Int gridPoint, Mesh mesh)> Split(Mesh mesh)
		{
			SetMesh(mesh);
			if (_verbose)
			{
				PerformanceMonitor.Start("CreatePointIndicesMap");
			}
			CreatePointIndicesMap();
			if (_verbose)
			{
				PerformanceMonitor.Stop("CreatePointIndicesMap");
			}
			if (_verbose)
			{
				PerformanceMonitor.Start("CreateChildMeshes");
			}
			List<(Vector3Int gridPoint, Mesh mesh)> result = CreateChildMeshes();
			if (_verbose)
			{
				PerformanceMonitor.Stop("CreateChildMeshes");
			}
			return result;
		}

		private void SetMesh(Mesh mesh)
		{
			_sourceMesh = mesh;
			GraphicsBuffer vertexBuffer = _sourceMesh.GetVertexBuffer(0);
			_vertexData = new byte[_sourceMesh.GetVertexBufferStride(0) * _sourceMesh.vertexCount];
			vertexBuffer.GetData(_vertexData);
			vertexBuffer.Dispose();
			_sourceMeshVertexAttributes = _sourceMesh.GetVertexAttributes();
		}

		private void CreatePointIndicesMap()
		{
			_pointIndicesMap = new Dictionary<Vector3Int, List<int>>();
			int[] triangles = _sourceMesh.triangles;
			Vector3[] vertices = _sourceMesh.vertices;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 vector = (vertices[triangles[i]] + vertices[triangles[i + 1]] + vertices[triangles[i + 2]]) / 3f;
				Vector3Int key = new Vector3Int(_parameters.SplitAxes.x ? Mathf.FloorToInt(Mathf.Floor(vector.x / _parameters.GridSize) * _parameters.GridSize * (float)MeshSplitController.GridSizeMultiplier) : 0, _parameters.SplitAxes.y ? Mathf.FloorToInt(Mathf.Floor(vector.y / _parameters.GridSize) * _parameters.GridSize * (float)MeshSplitController.GridSizeMultiplier) : 0, _parameters.SplitAxes.z ? Mathf.FloorToInt(Mathf.Floor(vector.z / _parameters.GridSize) * _parameters.GridSize * (float)MeshSplitController.GridSizeMultiplier) : 0);
				if (!_pointIndicesMap.TryGetValue(key, out var value))
				{
					value = new List<int>();
					_pointIndicesMap.TryAdd(key, value);
				}
				value.Add(triangles[i]);
				value.Add(triangles[i + 1]);
				value.Add(triangles[i + 2]);
			}
		}

		private List<(Vector3Int gridPoint, Mesh mesh)> CreateChildMeshes()
		{
			Mesh.MeshDataArray data = new SubMeshBuilder(_pointIndicesMap, _vertexData, _sourceMesh.GetVertexBufferStride(0), _sourceMeshVertexAttributes).Build(_sourceMesh);
			List<Mesh> list = new List<Mesh>(data.Length);
			Vector3Int[] array = _pointIndicesMap.Keys.ToArray();
			Vector3Int[] array2 = array;
			foreach (Vector3Int vector3Int in array2)
			{
				list.Add(new Mesh
				{
					name = $"SubMesh {vector3Int}"
				});
			}
			Mesh.ApplyAndDisposeWritableMeshData(data, list, MeshUpdateFlags);
			foreach (Mesh item in list)
			{
				item.RecalculateBounds(MeshUpdateFlags);
			}
			return new List<(Vector3Int, Mesh)>(array.Zip(list, (Vector3Int point, Mesh mesh) => (point: point, mesh: mesh)));
		}
	}
}
