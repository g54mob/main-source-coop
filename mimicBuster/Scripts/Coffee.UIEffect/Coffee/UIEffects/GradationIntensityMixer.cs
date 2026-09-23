using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class GradationIntensityMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.gradationIntensity;
			}
			set
			{
				base.effect.gradationIntensity = value;
			}
		}
	}
}
