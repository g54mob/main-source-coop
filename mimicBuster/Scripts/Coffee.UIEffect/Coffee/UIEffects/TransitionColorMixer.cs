using Coffee.UIEffects.Timeline;
using UnityEngine;

namespace Coffee.UIEffects
{
	public class TransitionColorMixer : UIEffectColorMixerBehaviour
	{
		protected override Color currentValue
		{
			get
			{
				return base.effect.transitionColor;
			}
			set
			{
				base.effect.transitionColor = value;
			}
		}
	}
}
