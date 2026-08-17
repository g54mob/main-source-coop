using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("slide")]
	internal class SlideBehavior : BehaviorSine
	{
		private float sin;

		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			amplitude = data.defaults.slideAmplitude;
			frequency = data.defaults.slideFrequency;
			waveSize = data.defaults.slideWaveSize;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			sin = Mathf.Sin(frequency * base.time.timeSinceStart + (float)charIndex * waveSize) * amplitude * uniformIntensity;
			data.vertices[0] += Vector3.right * sin;
			data.vertices[3] += Vector3.right * sin;
			data.vertices[1] += Vector3.right * (0f - sin);
			data.vertices[2] += Vector3.right * (0f - sin);
		}
	}
}
