using UnityEngine;

namespace Obi
{
	public struct BurstLineMeshData
	{
		public Vector2 uvScale;

		public float thicknessScale;

		public float uvAnchor;

		public uint normalizeV;

		public BurstLineMeshData(ObiRopeLineRenderer renderer)
		{
			uvAnchor = renderer.uvAnchor;
			thicknessScale = renderer.thicknessScale;
			uvScale = renderer.uvScale;
			normalizeV = (renderer.normalizeV ? 1u : 0u);
		}
	}
}
