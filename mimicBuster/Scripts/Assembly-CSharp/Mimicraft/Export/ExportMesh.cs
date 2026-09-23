using System.Collections.Generic;

namespace Mimicraft.Export
{
	public sealed class ExportMesh
	{
		public string Name = "";

		public readonly List<float> Positions = new List<float>();

		public readonly List<int> Corners = new List<int>();

		public readonly List<float> Normals = new List<float>();

		public readonly List<float> Uvs = new List<float>();

		public int QuadCount => Corners.Count / 4;

		public int PointCount => Positions.Count / 3;
	}
}
