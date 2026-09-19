using UnityEngine;

namespace Obi
{
	public struct ChainRendererData
	{
		public int modifierOffset;

		public float twistAnchor;

		public float twist;

		public uint usesOrientedParticles;

		public Vector4 scale;

		public ChainRendererData(int modifierOffset, float twistAnchor, float twist, Vector3 scale, bool usesOrientedParticles)
		{
			this.modifierOffset = modifierOffset;
			this.twistAnchor = twistAnchor;
			this.twist = twist;
			this.usesOrientedParticles = (usesOrientedParticles ? 1u : 0u);
			this.scale = scale;
		}
	}
}
