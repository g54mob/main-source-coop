using UnityEngine;

namespace EvilCore.DynamicCasting
{
	public class MeshDataCache
	{
		public Mesh Mesh { get; private set; }

		public Vector3[] Vertices { get; private set; }

		public int[] Triangles { get; private set; }

		public Vector3[] Normals { get; private set; }

		public Vector2[] UVs { get; private set; }

		public Bounds LocalBounds { get; private set; }

		public int LastFrameAccessed { get; set; }

		public int TriangleCount
		{
			get
			{
				if (Triangles == null)
				{
					return 0;
				}
				return Triangles.Length / 3;
			}
		}

		public void Refresh(Mesh mesh)
		{
			if (!(mesh == null))
			{
				if (Mesh == mesh && Vertices != null)
				{
					LastFrameAccessed = Time.frameCount;
					return;
				}
				Mesh = mesh;
				Vertices = mesh.vertices;
				Triangles = mesh.triangles;
				Normals = mesh.normals;
				UVs = mesh.uv;
				LocalBounds = mesh.bounds;
				LastFrameAccessed = Time.frameCount;
			}
		}

		public void Clear()
		{
			Mesh = null;
			Vertices = null;
			Triangles = null;
			Normals = null;
			UVs = null;
		}

		public void GetTriangleVertices(int triangleIndex, out Vector3 v0, out Vector3 v1, out Vector3 v2)
		{
			int num = triangleIndex * 3;
			v0 = Vertices[Triangles[num]];
			v1 = Vertices[Triangles[num + 1]];
			v2 = Vertices[Triangles[num + 2]];
		}

		public void GetTriangleNormals(int triangleIndex, out Vector3 n0, out Vector3 n1, out Vector3 n2)
		{
			if (Normals == null || Normals.Length == 0)
			{
				n0 = (n1 = (n2 = Vector3.up));
				return;
			}
			int num = triangleIndex * 3;
			n0 = Normals[Triangles[num]];
			n1 = Normals[Triangles[num + 1]];
			n2 = Normals[Triangles[num + 2]];
		}

		public void GetTriangleUVs(int triangleIndex, out Vector2 uv0, out Vector2 uv1, out Vector2 uv2)
		{
			if (UVs == null || UVs.Length == 0)
			{
				uv0 = (uv1 = (uv2 = Vector2.zero));
				return;
			}
			int num = triangleIndex * 3;
			uv0 = UVs[Triangles[num]];
			uv1 = UVs[Triangles[num + 1]];
			uv2 = UVs[Triangles[num + 2]];
		}
	}
}
