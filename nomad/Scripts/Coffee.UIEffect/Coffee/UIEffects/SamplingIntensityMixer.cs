using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class SamplingIntensityMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.samplingIntensity;
			}
			set
			{
				base.effect.samplingIntensity = value;
			}
		}
	}
}
