using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class EdgeShinyRateMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.edgeShinyRate;
			}
			set
			{
				base.effect.edgeShinyRate = value;
			}
		}
	}
}
