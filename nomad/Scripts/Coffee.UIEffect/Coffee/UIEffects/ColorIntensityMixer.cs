using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class ColorIntensityMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.colorIntensity;
			}
			set
			{
				base.effect.colorIntensity = value;
			}
		}
	}
}
