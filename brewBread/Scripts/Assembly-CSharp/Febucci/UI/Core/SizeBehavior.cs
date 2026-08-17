using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("incr")]
	internal sealed class SizeBehavior : BehaviorSine
	{
		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			amplitude = data.defaults.sizeAmplitude * -1f + 1f;
			frequency = data.defaults.sizeFrequency;
			waveSize = data.defaults.sizeWaveSize;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.LerpUnclamped(data.vertices.GetMiddlePos(), (Mathf.Cos(base.time.timeSinceStart * frequency + (float)charIndex * waveSize) + 1f) / 2f * amplitude);
		}
	}
}
