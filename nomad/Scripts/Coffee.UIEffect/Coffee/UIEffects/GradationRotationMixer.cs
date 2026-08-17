using Coffee.UIEffects.Timeline;

namespace Coffee.UIEffects
{
	public class GradationRotationMixer : UIEffectFloatMixerBehaviour
	{
		protected override float currentValue
		{
			get
			{
				return base.effect.gradationRotation;
			}
			set
			{
				base.effect.gradationRotation = value;
			}
		}
	}
}
