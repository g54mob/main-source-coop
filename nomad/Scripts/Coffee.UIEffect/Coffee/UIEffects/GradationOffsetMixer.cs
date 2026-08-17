using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class GradationOffsetMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.gradationOffset;
			}
			set
			{
				base.effect.gradationOffset = value;
			}
		}
	}
}
