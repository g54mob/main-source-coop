using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class GradationScaleMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.gradationScale;
			}
			set
			{
				base.effect.gradationScale = value;
			}
		}
	}
}
