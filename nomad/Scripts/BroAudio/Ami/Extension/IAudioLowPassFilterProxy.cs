using UnityEngine;

namespace Ami.Extension
{
	public interface IAudioLowPassFilterProxy
	{
		AnimationCurve customCutoffCurve { get; set; }

		float cutoffFrequency { get; set; }

		float lowpassResonanceQ { get; set; }

		bool enabled { get; set; }
	}
}
