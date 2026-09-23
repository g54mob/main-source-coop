using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class DetailIntensityMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.detailIntensity;
			}
			set
			{
				base.effect.detailIntensity = value;
			}
		}
	}
}
