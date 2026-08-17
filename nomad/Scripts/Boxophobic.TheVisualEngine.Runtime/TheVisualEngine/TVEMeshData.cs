using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEMeshData
	{
		public Mesh mesh;

		public List<Vector3> vertices;

		public List<Color> colors;

		public List<Vector3> normals;

		public List<Vector4> tangents;

		public List<Vector4> UV0;

		public List<Vector4> UV2;

		public List<Vector4> UV4;
	}
}
