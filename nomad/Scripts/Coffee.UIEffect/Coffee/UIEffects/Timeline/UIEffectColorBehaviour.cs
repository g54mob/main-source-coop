using System;
using UnityEngine;

namespace Coffee.UIEffects.Timeline
{
	[Serializable]
	public class UIEffectColorBehaviour : UIEffectBehaviour, IGetValue<Color>
	{
		[ColorUsage(true, true)]
		public Color m_Value = Color.white;

		[ColorUsage(true, true)]
		public Color m_From = Color.white;

		public Color Get(float time)
		{
			if (!m_Tween)
			{
				return m_Value;
			}
			return Color.Lerp(m_From, m_Value, m_Curve.Evaluate(time));
		}
	}
}
