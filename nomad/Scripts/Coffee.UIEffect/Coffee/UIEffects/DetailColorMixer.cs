using Coffee.UIEffects.Timeline;
using UnityEngine;

namespace Coffee.UIEffects
{
	public class DetailColorMixer : UIEffectColorMixerBehaviour
	{
		protected override Color currentValue
		{
			get
			{
				return base.effect.detailColor;
			}
			set
			{
				base.effect.detailColor = value;
			}
		}
	}
}
