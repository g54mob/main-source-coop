using Ami.Extension;

namespace Ami.BroAudio
{
	public struct Fading
	{
		public float FadeIn;

		public float FadeOut;

		public Ease FadeInEase;

		public Ease FadeOutEase;

		public Fading(float fadeIn, float fadeOut, EffectType effectType)
			: this(effectType)
		{
			FadeIn = fadeIn;
			FadeOut = fadeOut;
		}

		public Fading(float fadeTime, EffectType effectType)
			: this(effectType)
		{
			FadeIn = fadeTime;
			FadeOut = fadeTime;
		}

		public Fading(EffectType effectType)
		{
			switch (effectType)
			{
			case EffectType.LowPass:
				FadeInEase = Ease.OutCubic;
				FadeOutEase = Ease.InCubic;
				break;
			case EffectType.HighPass:
				FadeInEase = Ease.InCubic;
				FadeOutEase = Ease.OutCubic;
				break;
			default:
				FadeInEase = Ease.Linear;
				FadeOutEase = Ease.Linear;
				break;
			}
			FadeIn = 0f;
			FadeOut = 0f;
		}

		public Fading(float fadeIn, float fadeOut, Ease fadeInEase, Ease fadeOutEase)
		{
			FadeOut = fadeOut;
			FadeIn = fadeIn;
			FadeInEase = fadeInEase;
			FadeOutEase = fadeOutEase;
		}
	}
}
