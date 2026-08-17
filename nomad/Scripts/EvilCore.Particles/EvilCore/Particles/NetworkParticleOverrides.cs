using UnityEngine;

namespace EvilCore.Particles
{
	public struct NetworkParticleOverrides
	{
		public float ScaleMultiplier;

		public Color StartColor;

		public float SimulationSpeed;

		public ParticleOverrides ToLocal()
		{
			return new ParticleOverrides
			{
				ScaleMultiplier = ((ScaleMultiplier > 0f) ? new float?(ScaleMultiplier) : ((float?)null)),
				StartColor = ((StartColor != Color.clear) ? new Color?(StartColor) : ((Color?)null)),
				SimulationSpeed = ((SimulationSpeed > 0f) ? new float?(SimulationSpeed) : ((float?)null))
			};
		}
	}
}
