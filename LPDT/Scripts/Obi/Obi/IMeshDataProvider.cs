using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public interface IMeshDataProvider
	{
		Mesh sourceMesh { get; }

		uint meshInstances { get; }

		int vertexCount { get; }

		int triangleCount { get; }

		void GetVertices(List<Vector3> vertices);

		void GetNormals(List<Vector3> normals);

		void GetTangents(List<Vector4> tangents);

		void GetColors(List<Color> colors);

		void GetUVs(int channel, List<Vector2> uvs);

		void GetTriangles(List<int> triangles);
	}
}
