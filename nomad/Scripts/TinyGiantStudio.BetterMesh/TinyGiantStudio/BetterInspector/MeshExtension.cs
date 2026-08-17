using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

namespace TinyGiantStudio.BetterInspector
{
	public static class MeshExtension
	{
		private readonly struct Edge : IEquatable<Edge>
		{
			private readonly int _vertexIndexA;

			private readonly int _vertexIndexB;

			public Edge(int vertexIndexA, int vertexIndexB)
			{
				_vertexIndexA = Mathf.Min(vertexIndexA, vertexIndexB);
				_vertexIndexB = Mathf.Max(vertexIndexA, vertexIndexB);
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(_vertexIndexA, _vertexIndexB);
			}

			public override bool Equals(object obj)
			{
				if (!(obj is Edge edge))
				{
					return false;
				}
				if (_vertexIndexA == edge._vertexIndexA)
				{
					return _vertexIndexB == edge._vertexIndexB;
				}
				return false;
			}

			public bool Equals(Edge other)
			{
				if (_vertexIndexA == other._vertexIndexA)
				{
					return _vertexIndexB == other._vertexIndexB;
				}
				return false;
			}
		}

		public static void GetAllTriangles(this Mesh mesh, List<int> outTriangleList)
		{
			outTriangleList.Clear();
			if (mesh == null || mesh.vertexCount == 0)
			{
				return;
			}
			List<int> value;
			using (CollectionPool<List<int>, int>.Get(out value))
			{
				for (int i = 0; i < mesh.subMeshCount; i++)
				{
					if (mesh.GetSubMesh(i).topology == MeshTopology.Triangles)
					{
						mesh.GetTriangles(value, i);
						outTriangleList.AddRange(value);
					}
				}
			}
		}

		public static void SubMeshVertexCount(this Mesh mesh, List<int> outVertexCountList)
		{
			outVertexCountList.Clear();
			if (!(mesh == null) && mesh.vertexCount != 0)
			{
				for (int i = 0; i < mesh.subMeshCount; i++)
				{
					outVertexCountList.Add(mesh.GetSubMesh(i).vertexCount);
				}
			}
		}

		public static int TrianglesCount(this Mesh mesh)
		{
			if (mesh == null || mesh.vertexCount == 0)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				if (mesh.GetSubMesh(i).topology == MeshTopology.Triangles)
				{
					num += (int)mesh.GetIndexCount(i);
				}
			}
			return num / 3;
		}

		public static int GetTangentCount(this Mesh mesh)
		{
			if (mesh == null)
			{
				return 0;
			}
			if (!mesh.HasVertexAttribute(VertexAttribute.Tangent))
			{
				return 0;
			}
			return mesh.vertexCount;
		}

		public static int EdgeCount(this Mesh mesh)
		{
			CollectionPool<HashSet<Edge>, Edge>.Get(out var value);
			List<int> value2;
			using (CollectionPool<List<int>, int>.Get(out value2))
			{
				mesh.GetAllTriangles(value2);
				int count = value2.Count;
				for (int i = 0; i < count; i += 3)
				{
					Edge item = new Edge(value2[i], value2[i + 1]);
					Edge item2 = new Edge(value2[i + 1], value2[i + 2]);
					Edge item3 = new Edge(value2[i + 2], value2[i]);
					value.Add(item);
					value.Add(item2);
					value.Add(item3);
				}
				return value.Count;
			}
		}

		public static int FaceCount(this Mesh mesh)
		{
			return mesh.TrianglesCount();
		}

		public static Mesh FlipNormals(this Mesh mesh)
		{
			if (mesh == null || mesh.vertexCount == 0)
			{
				return mesh;
			}
			List<Vector3> value;
			using (CollectionPool<List<Vector3>, Vector3>.Get(out value))
			{
				int count = value.Count;
				for (int i = 0; i < count; i++)
				{
					value[i] = -value[i];
				}
				mesh.SetNormals(value);
				List<int> value2;
				using (CollectionPool<List<int>, int>.Get(out value2))
				{
					int subMeshCount = mesh.subMeshCount;
					for (int j = 0; j < subMeshCount; j++)
					{
						value2.Clear();
						mesh.GetTriangles(value2, j);
						int count2 = value2.Count;
						for (int k = 0; k < count2; k += 3)
						{
							List<int> list = value2;
							int index = k;
							List<int> list2 = value2;
							int index2 = k + 2;
							int num = value2[k + 2];
							int num2 = value2[k];
							int num3 = (list[index] = num);
							num3 = (list2[index2] = num2);
						}
						mesh.SetTriangles(value2, j);
					}
					return mesh;
				}
			}
		}
	}
}
