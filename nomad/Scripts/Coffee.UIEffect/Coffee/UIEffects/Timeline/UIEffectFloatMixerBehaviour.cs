using UnityEngine;

namespace Coffee.UIEffects.Timeline
{
	public abstract class UIEffectFloatMixerBehaviour : UIEffectMixerBehaviour<float, UIEffectFloatBehaviour>
	{
		protected override float Add(float baseValue, float value, float weight)
		{
			return baseValue + value * weight;
		}

		protected override float Lerp(float defaultValue, float value, float weight)
		{
			return Mathf.Lerp(defaultValue, value, weight);
		}
	}
}
