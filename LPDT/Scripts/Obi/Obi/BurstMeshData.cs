using UnityEngine;

namespace Obi
{
	public struct BurstMeshData
	{
		public uint axis;

		public float volumeScaling;

		public uint stretchWithRope;

		public uint spanEntireLength;

		public uint instances;

		public float instanceSpacing;

		public float offset;

		public float meshSizeAlongAxis;

		public Vector4 scale;

		public BurstMeshData(ObiRopeMeshRenderer renderer)
		{
			axis = (uint)renderer.axis;
			volumeScaling = renderer.volumeScaling;
			stretchWithRope = (renderer.stretchWithRope ? 1u : 0u);
			spanEntireLength = (renderer.spanEntireLength ? 1u : 0u);
			instances = renderer.instances;
			instanceSpacing = renderer.instanceSpacing;
			offset = renderer.offset;
			meshSizeAlongAxis = ((renderer.sourceMesh != null) ? renderer.sourceMesh.bounds.size[(int)renderer.axis] : 0f);
			scale = renderer.scale;
		}
	}
}
