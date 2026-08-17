using System;
using UnityEngine;

namespace Febucci.UI.Core
{
	[Serializable]
	internal class FloatCurve : EffectEvaluator
	{
		public bool enabled;

		[SerializeField]
		protected float amplitude;

		[SerializeField]
		protected AnimationCurve curve;

		[SerializeField]
		[HideInInspector]
		protected float defaultReturn;

		[SerializeField]
		[Range(0f, 100f)]
		protected float charsTimeOffset;

		[NonSerialized]
		private float calculatedDuration;

		private bool isAppearance;

		public bool isEnabled => enabled;

		public float GetDuration()
		{
			return calculatedDuration;
		}

		public void Initialize(int type)
		{
			calculatedDuration = curve.CalculateCurveDuration();
			isAppearance = type >= 3;
			switch (type)
			{
			default:
				defaultReturn = 0f;
				break;
			case 1:
				defaultReturn = 1f;
				break;
			case 2:
				defaultReturn = 0f;
				break;
			case 3:
				defaultReturn = 0f;
				break;
			case 4:
				defaultReturn = 1f;
				break;
			case 5:
				defaultReturn = 0f;
				break;
			}
		}

		public float Evaluate(float time, int characterIndex)
		{
			if (!enabled)
			{
				return defaultReturn;
			}
			if (isAppearance)
			{
				return Mathf.LerpUnclamped(amplitude, defaultReturn, curve.Evaluate(time) * Mathf.Cos((float)Math.PI / 180f * ((float)characterIndex * charsTimeOffset / 2f)));
			}
			return curve.Evaluate(time + (float)characterIndex * (charsTimeOffset / 100f)) * amplitude;
		}
	}
}
