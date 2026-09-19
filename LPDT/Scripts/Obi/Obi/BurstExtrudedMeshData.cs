using UnityEngine;

namespace Obi
{
	public struct BurstExtrudedMeshData
	{
		public int sectionVertexCount;

		public float thicknessScale;

		public float uvAnchor;

		public uint normalizeV;

		public Vector2 uvScale;

		public BurstExtrudedMeshData(ObiRopeExtrudedRenderer renderer)
		{
			sectionVertexCount = renderer.section.vertices.Count;
			uvAnchor = renderer.uvAnchor;
			thicknessScale = renderer.thicknessScale;
			uvScale = renderer.uvScale;
			normalizeV = (renderer.normalizeV ? 1u : 0u);
		}
	}
}
