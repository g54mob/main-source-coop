using UnityEngine;

namespace Obi
{
	public struct ParticleRendererData
	{
		public Color color;

		public float radiusScale;

		public ParticleRendererData(Color color, float radiusScale)
		{
			this.color = color;
			this.radiusScale = radiusScale;
		}
	}
}
