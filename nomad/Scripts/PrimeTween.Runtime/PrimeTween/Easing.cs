using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween
{
	[PublicAPI]
	public readonly struct Easing
	{
		internal readonly Ease ease;

		internal readonly AnimationCurve curve;

		internal readonly ParametricEase parametricEase;

		internal readonly float parametricEaseStrength;

		internal readonly float parametricEasePeriod;

		private Easing(Ease ease, [CanBeNull] AnimationCurve curve)
		{
			if (ease == Ease.Custom && (curve == null || !TweenSettings.ValidateCustomCurveKeyframes(curve)))
			{
				Debug.LogError("Ease is Ease.Custom, but AnimationCurve is not configured correctly. Using Ease.Default instead.");
				ease = Ease.Default;
			}
			this.ease = ease;
			this.curve = curve;
			parametricEase = ParametricEase.None;
			parametricEaseStrength = 0f / 0f;
			parametricEasePeriod = 0f / 0f;
		}

		public static implicit operator Easing(Ease ease)
		{
			return Standard(ease);
		}

		public static Easing Standard(Ease ease)
		{
			if (ease == Ease.Default)
			{
				ease = PrimeTweenConfig.defaultEase;
			}
			return new Easing(ease, null);
		}

		public static implicit operator Easing([NotNull] AnimationCurve curve)
		{
			return Curve(curve);
		}

		public static Easing Curve([NotNull] AnimationCurve curve)
		{
			return new Easing(Ease.Custom, curve);
		}

		internal static float Evaluate(float t, ParametricEase parametricEase, float strength, float period, float duration)
		{
			switch (parametricEase)
			{
			case ParametricEase.Overshoot:
				t -= 1f;
				return t * t * ((strength + 1f) * t + strength) + 1f;
			case ParametricEase.Elastic:
			{
				float num;
				if (strength >= 1f)
				{
					num = 1f;
				}
				else
				{
					num = 1f / strength;
					strength = 1f;
				}
				float num2 = Mathf.Pow(2f, -10f * t * num);
				if (duration == 0f)
				{
					return 1f;
				}
				period /= duration;
				float num3 = period / ((float)Math.PI * 2f) * Mathf.Asin(1f / strength);
				if (!(t > 0.9999f))
				{
					return strength * num2 * Mathf.Sin((t - num3) * ((float)Math.PI * 2f) / period) + 1f;
				}
				return 1f;
			}
			case ParametricEase.Bounce:
				return Bounce(t, strength);
			default:
				throw new Exception();
			}
		}

		internal static float EvaluateParametricEase(float t, ref TweenData rt, ref UnmanagedTweenData d)
		{
			ParametricEase parametricEase = rt.cold.parametricEase;
			float num = rt.cold.parametricEaseStrength;
			if (parametricEase == ParametricEase.BounceExact)
			{
				float num2 = ((d.propType == PropType.Quaternion) ? TweenAnimation.ValueWrapper.QuaternionAngle(d.startValue, rt.endValueOrDiff) : rt.endValueOrDiff.Vector4Magnitude());
				float num3 = ((num2 < 0.0001f) ? 1f : (1f / (num2 * 0.25f)));
				return Bounce(t, num * num3);
			}
			return Evaluate(t, parametricEase, num, rt.cold.parametricEasePeriod, d.cycleDuration);
		}

		private static float Bounce(float t, float strength)
		{
			if (t < 0.36363637f)
			{
				return 7.5625f * t * t;
			}
			return 1f - (1f - bounce()) * strength;
			float bounce()
			{
				if (t < 0.72727275f)
				{
					return 7.5625f * (t -= 0.54545456f) * t + 0.75f;
				}
				if ((double)t < 0.9090909090909091)
				{
					return 7.5625f * (t -= 0.8181818f) * t + 0.9375f;
				}
				return 7.5625f * (t -= 21f / 22f) * t + 63f / 64f;
			}
		}
	}
}
