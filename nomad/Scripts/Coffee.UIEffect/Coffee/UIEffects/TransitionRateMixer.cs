using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class TransitionRateMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.transitionRate;
			}
			set
			{
				base.effect.transitionRate = value;
			}
		}
	}
}
