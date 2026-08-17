using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("pend")]
	internal class PendulumBehavior : BehaviorSine
	{
		private int targetVertex1;

		private int targetVertex2;

		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			frequency = data.defaults.pendFrequency;
			amplitude = data.defaults.pendAmplitude;
			waveSize = data.defaults.pendWaveSize;
			if (data.defaults.pendInverted)
			{
				targetVertex1 = 0;
				targetVertex2 = 3;
			}
			else
			{
				targetVertex1 = 1;
				targetVertex2 = 2;
			}
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.RotateChar(Mathf.Sin((0f - base.time.timeSinceStart) * frequency + waveSize * (float)charIndex) * amplitude, (data.vertices[targetVertex1] + data.vertices[targetVertex2]) / 2f);
		}
	}
}
