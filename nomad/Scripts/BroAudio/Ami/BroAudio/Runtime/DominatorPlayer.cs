using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio.Runtime
{
	public class DominatorPlayer : AudioPlayerDecorator, IPlayerEffect, IVolumeSettable, IMusicDecoratable, IAudioStoppable
	{
		public DominatorPlayer(AudioPlayer instance)
			: base(instance)
		{
		}

		IPlayerEffect IPlayerEffect.QuietOthers(float othersVol, float fadeTime)
		{
			return this.QuietOthers(othersVol, new Fading(fadeTime, EffectType.Volume));
		}

		IPlayerEffect IPlayerEffect.QuietOthers(float othersVol, Fading fading)
		{
			if (othersVol <= 0f || othersVol > 1f)
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>othersVol should be less than 1 and greater than 0.");
				return this;
			}
			SetAllEffectExceptDominator(new Effect(EffectType.Volume, othersVol, fading, isDominator: true));
			return this;
		}

		IPlayerEffect IPlayerEffect.LowPassOthers(float freq, float fadeTime)
		{
			return this.LowPassOthers(freq, new Fading(fadeTime, EffectType.LowPass));
		}

		IPlayerEffect IPlayerEffect.LowPassOthers(float freq, Fading fading)
		{
			if (!AudioExtension.IsValidFrequency(freq))
			{
				return this;
			}
			SetAllEffectExceptDominator(new Effect(EffectType.LowPass, freq, fading, isDominator: true));
			return this;
		}

		IPlayerEffect IPlayerEffect.HighPassOthers(float freq, float fadeTime)
		{
			return this.HighPassOthers(freq, new Fading(fadeTime, EffectType.HighPass));
		}

		IPlayerEffect IPlayerEffect.HighPassOthers(float freq, Fading fading)
		{
			if (!AudioExtension.IsValidFrequency(freq))
			{
				return this;
			}
			SetAllEffectExceptDominator(new Effect(EffectType.HighPass, freq, fading, isDominator: true));
			return this;
		}

		private void SetAllEffectExceptDominator(Effect effect)
		{
			if (base.Instance != null)
			{
				SoundManager.Instance.SetEffect(effect).While(PlayerIsPlaying);
				base.Instance?.SetTrackEffect(EffectType.None, SetEffectMode.Override);
			}
		}

		private bool PlayerIsPlaying()
		{
			return base.IsActive;
		}
	}
}
