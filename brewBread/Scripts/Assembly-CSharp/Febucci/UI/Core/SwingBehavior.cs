using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("swing")]
	internal class SwingBehavior : BehaviorSine
	{
		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			amplitude = data.defaults.swingAmplitude;
			frequency = data.defaults.swingFrequency;
			waveSize = data.defaults.swingWaveSize;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.RotateChar(Mathf.Cos(base.time.timeSinceStart * frequency + (float)charIndex * waveSize) * amplitude);
		}
	}
}
