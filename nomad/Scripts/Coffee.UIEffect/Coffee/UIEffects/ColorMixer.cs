using Coffee.UIEffects.Timeline;
using UnityEngine;

namespace Coffee.UIEffects
{
	public class ColorMixer : UIEffectColorMixerBehaviour
	{
		protected override Color currentValue
		{
			get
			{
				return base.effect.color;
			}
			set
			{
				base.effect.color = value;
			}
		}
	}
}
