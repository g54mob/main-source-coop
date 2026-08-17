using System;
using UnityEngine;

namespace Coffee.UIEffects.Timeline
{
	[Serializable]
	public class UIEffectFloatBehaviour : UIEffectBehaviour, IGetValue<float>
	{
		public float m_Value = 1f;

		public float m_From = 1f;

		public float Get(float time)
		{
			if (!m_Tween)
			{
				return m_Value;
			}
			return Mathf.Lerp(m_From, m_Value, m_Curve.Evaluate(time));
		}
	}
}
