using Coffee.UIEffects.Timeline;
using UnityEngine;

namespace Coffee.UIEffects
{
	public class EdgeColorMixer : UIEffectColorMixerBehaviour
	{
		protected override Color currentValue
		{
			get
			{
				return base.effect.edgeColor;
			}
			set
			{
				base.effect.edgeColor = value;
			}
		}
	}
}
