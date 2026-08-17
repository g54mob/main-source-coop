using UnityEngine;
using UnityEngine.Scripting;

namespace Febucci.UI.Core
{
	[Preserve]
	[EffectInfo("bounce")]
	internal class BounceBehavior : BehaviorSine
	{
		public override void SetDefaultValues(BehaviorDefaultValues data)
		{
			amplitude = data.defaults.bounceAmplitude;
			frequency = data.defaults.bounceFrequency;
			waveSize = data.defaults.bounceWaveSize;
		}

		public override void ApplyEffect(ref CharacterData data, int charIndex)
		{
			data.vertices.MoveChar(Vector3.up * uniformIntensity * BounceTween(Mathf.Repeat(base.time.timeSinceStart * frequency - waveSize * (float)charIndex, 1f)) * amplitude);
			static float BounceTween(float t)
			{
				if (t <= 0.2f)
				{
					return Tween.EaseInOut(t / 0.2f);
				}
				t -= 0.2f;
				if (t <= 0.6f)
				{
					return 1f - Tween.BounceOut(t / 0.6f);
				}
				return 0f;
			}
		}
	}
}
