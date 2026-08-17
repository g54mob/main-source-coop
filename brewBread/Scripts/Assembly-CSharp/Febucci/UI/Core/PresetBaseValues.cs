using System;
using UnityEngine;

namespace Febucci.UI.Core
{
	[Serializable]
	internal class PresetBaseValues
	{
		public string effectTag;

		[SerializeField]
		public FloatCurve movementX;

		[SerializeField]
		public FloatCurve movementY;

		[SerializeField]
		public FloatCurve movementZ;

		[SerializeField]
		public FloatCurve scaleX;

		[SerializeField]
		public FloatCurve scaleY;

		[SerializeField]
		public FloatCurve rotX;

		[SerializeField]
		public FloatCurve rotY;

		[SerializeField]
		public FloatCurve rotZ;

		[SerializeField]
		public ColorCurve color;

		public float GetMaxDuration()
		{
			return Mathf.Max(GetEffectEvaluatorDuration(movementX), GetEffectEvaluatorDuration(movementY), GetEffectEvaluatorDuration(movementZ), GetEffectEvaluatorDuration(scaleX), GetEffectEvaluatorDuration(scaleY), color.enabled ? color.GetDuration() : 0f);
			static float GetEffectEvaluatorDuration(EffectEvaluator effect)
			{
				if (effect.isEnabled)
				{
					return effect.GetDuration();
				}
				return 0f;
			}
		}

		public virtual void Initialize(bool isAppearance)
		{
			int num = (isAppearance ? 3 : 0);
			movementX.Initialize(num);
			movementY.Initialize(num);
			movementZ.Initialize(num);
			scaleX.Initialize(num + 1);
			scaleY.Initialize(num + 1);
			rotX.Initialize(num + 2);
			rotY.Initialize(num + 2);
			rotZ.Initialize(num + 2);
			color.Initialize(isAppearance);
		}
	}
}
