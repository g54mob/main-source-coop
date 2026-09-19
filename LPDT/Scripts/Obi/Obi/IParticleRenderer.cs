using UnityEngine;

namespace Obi
{
	public interface IParticleRenderer
	{
		ObiActor actor { get; }

		Color particleColor { get; }

		float radiusScale { get; }
	}
}
