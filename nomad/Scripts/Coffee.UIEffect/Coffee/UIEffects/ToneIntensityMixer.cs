using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class ToneIntensityMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.toneIntensity;
			}
			set
			{
				base.effect.toneIntensity = value;
			}
		}
	}
}
