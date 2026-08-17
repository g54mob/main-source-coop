using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("wave")]
	internal class WaveBehavior : BehaviorSine
	{
		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			amplitude = data.defaults.waveAmplitude;
			frequency = data.defaults.waveFrequency;
			waveSize = data.defaults.waveWaveSize;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.MoveChar(Vector3.up * Mathf.Sin(base.time.timeSinceStart * frequency + (float)charIndex * waveSize) * amplitude * uniformIntensity);
		}
	}
}
