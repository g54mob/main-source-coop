using System.Collections.Generic;
using UnityEngine;

namespace PrimeTween
{
	internal struct ShakeData
	{
		private float t;

		private Vector3 from;

		private Vector3 to;

		private float symmetryFactor;

		private int falloffEaseInt;

		private AnimationCurve customStrengthOverTime;

		private Ease easeBetweenShakes;

		private float prevInterpolationFactor;

		private int prevCyclesDone;

		private const int disabledFalloff = -42;

		internal Vector3 strengthPerAxis { get; private set; }

		internal float frequency { get; private set; }

		internal bool isAlive => frequency != 0f;

		internal void Setup(ShakeSettings settings, ref TweenData rt, ref UnmanagedTweenData d, object target)
		{
			d.isPunch = settings.isPunch;
			symmetryFactor = Mathf.Clamp01(1f - settings.asymmetry);
			Vector3 strength = settings.strength;
			if (strength == default(Vector3))
			{
				Debug.LogError("Shake's strength is (0, 0, 0).");
			}
			strengthPerAxis = strength;
			float num = settings.frequency;
			if (num <= 0f)
			{
				Debug.LogError($"Shake's frequency should be > 0f, but was {num}.", target as Object);
				num = 10f;
			}
			frequency = num;
			if (settings.enableFalloff)
			{
				Ease ease = settings.falloffEase;
				AnimationCurve strengthOverTime = settings.strengthOverTime;
				if (ease == Ease.Default)
				{
					ease = Ease.Linear;
				}
				if (ease == Ease.Custom && (strengthOverTime == null || !TweenSettings.ValidateCustomCurve(strengthOverTime)))
				{
					Debug.LogError("Shake falloff is Ease.Custom, but strengthOverTime is not configured correctly. Using Ease.Linear instead.", target as Object);
					ease = Ease.Linear;
				}
				falloffEaseInt = (int)ease;
				customStrengthOverTime = strengthOverTime;
			}
			else
			{
				falloffEaseInt = -42;
			}
			Ease ease2 = settings.easeBetweenShakes;
			if (ease2 == Ease.Custom)
			{
				Debug.LogError("easeBetweenShakes doesn't support Ease.Custom.", target as Object);
				ease2 = Ease.OutQuad;
			}
			if (ease2 == Ease.Default)
			{
				ease2 = Ease.OutQuad;
			}
			easeBetweenShakes = ease2;
			onCycleComplete(ref rt, ref d);
		}

		internal void onCycleComplete(ref TweenData rt, ref UnmanagedTweenData d)
		{
			resetAfterCycle();
			d.shakeSign = d.isPunch || PrimeTweenManager.random.NextDouble() < 0.5;
			to = generateShakePoint(ref d);
		}

		private static int getMainAxisIndex(Vector3 strengthByAxis)
		{
			int result = -1;
			float num = -1f / 0f;
			for (int i = 0; i < 3; i++)
			{
				float num2 = Mathf.Abs(strengthByAxis[i]);
				if (num2 > num)
				{
					num = num2;
					result = i;
				}
			}
			return result;
		}

		internal Vector3 getNextVal(ref TweenData rt, ref UnmanagedTweenData d)
		{
			float easedInterpolationFactor = d.easedInterpolationFactor;
			int num = d.getCyclesDone() - prevCyclesDone;
			prevCyclesDone = d.getCyclesDone();
			if (easedInterpolationFactor == 0f || (num > 0 && d.getCyclesDone() != d.cyclesTotal))
			{
				onCycleComplete(ref rt, ref d);
				prevInterpolationFactor = easedInterpolationFactor;
			}
			float animationDuration = d.animationDuration;
			float num2 = (easedInterpolationFactor - prevInterpolationFactor) * animationDuration;
			prevInterpolationFactor = easedInterpolationFactor;
			float num3 = calcStrengthOverTime(easedInterpolationFactor);
			float num4 = Mathf.Clamp01(num3 * 3f);
			float num5 = d.easedInterpolationFactor * animationDuration;
			float num6 = 0.5f / rt.shakeData.frequency;
			float num7 = ((num5 < num6) ? 2f : 1f);
			t += frequency * num2 * num4 * num7;
			if (t < 0f || t >= 1f)
			{
				d.shakeSign = !d.shakeSign;
				if (t < 0f)
				{
					t = 1f;
					to = from;
					from = generateShakePoint(ref d);
				}
				else
				{
					t = 0f;
					from = to;
					to = generateShakePoint(ref d);
				}
			}
			Vector3 result = default(Vector3);
			for (int i = 0; i < 3; i++)
			{
				result[i] = Mathf.Lerp(from[i], to[i], StandardEasing.Evaluate(t, easeBetweenShakes)) * num3;
			}
			return result;
		}

		private Vector3 generateShakePoint(ref UnmanagedTweenData d)
		{
			int mainAxisIndex = getMainAxisIndex(strengthPerAxis);
			Vector3 result = default(Vector3);
			float num = (d.shakeSign ? 1f : (-1f));
			for (int i = 0; i < 3; i++)
			{
				float num2 = strengthPerAxis[i];
				if (d.isPunch)
				{
					result[i] = clampBySymmetryFactor(num2 * num, num2, symmetryFactor);
				}
				else
				{
					result[i] = ((i == mainAxisIndex) ? calcMainAxisEndVal(num, num2, symmetryFactor) : calcNonMainAxisEndVal(num2, symmetryFactor));
				}
			}
			return result;
		}

		private float calcStrengthOverTime(float interpolationFactor)
		{
			if (falloffEaseInt == -42)
			{
				return 1f;
			}
			Ease ease = (Ease)falloffEaseInt;
			if (ease != Ease.Custom)
			{
				return 1f - StandardEasing.Evaluate(interpolationFactor, ease);
			}
			return customStrengthOverTime.Evaluate(interpolationFactor);
		}

		private static float calcMainAxisEndVal(float velocity, float strength, float symmetryFactor)
		{
			return clampBySymmetryFactor(Mathf.Sign(velocity) * strength * RandomRange(0.6f, 1f), strength, symmetryFactor);
		}

		private static float clampBySymmetryFactor(float val, float strength, float symmetryFactor)
		{
			if (strength > 0f)
			{
				return Mathf.Clamp(val, (0f - strength) * symmetryFactor, strength);
			}
			return Mathf.Clamp(val, strength, (0f - strength) * symmetryFactor);
		}

		private static float calcNonMainAxisEndVal(float strength, float symmetryFactor)
		{
			if (strength > 0f)
			{
				return RandomRange((0f - strength) * symmetryFactor, strength);
			}
			return RandomRange(strength, (0f - strength) * symmetryFactor);
		}

		private static float RandomRange(float minInclusive, float max)
		{
			double num = PrimeTweenManager.random.NextDouble();
			return (float)((double)minInclusive + num * (double)(max - minInclusive));
		}

		internal static bool TryTakeStartValueFromOtherShake(ref TweenData newTween, ref UnmanagedTweenData newTweenData)
		{
			if (!newTween.shakeData.isAlive)
			{
				return false;
			}
			Transform transform = newTween.target as Transform;
			if (transform == null)
			{
				return false;
			}
			Dictionary<(Transform, TweenAnimation.TweenType), (TweenAnimation.ValueWrapper, int)> shakes = PrimeTweenManager.Instance.shakes;
			(Transform, TweenAnimation.TweenType) key = (transform, newTweenData.tweenType);
			if (!shakes.TryGetValue(key, out var value))
			{
				TweenAnimation.ValueWrapper animatedValue = Utils.GetAnimatedValue(newTween.target, newTweenData.tweenType, newTween.cold.longParam);
				shakes.Add(key, (animatedValue, 1));
				return false;
			}
			(newTweenData.startValue, _) = value;
			value.Item2++;
			shakes[key] = value;
			return true;
		}

		internal void Reset(object target, TweenAnimation.TweenType tweenType)
		{
			Transform transform = target as Transform;
			if (transform != null)
			{
				(Transform, TweenAnimation.TweenType) key = (transform, tweenType);
				Dictionary<(Transform, TweenAnimation.TweenType), (TweenAnimation.ValueWrapper, int)> shakes = PrimeTweenManager.Instance.shakes;
				if (shakes.TryGetValue(key, out var value))
				{
					value.Item2--;
					if (value.Item2 == 0)
					{
						shakes.Remove(key);
					}
					else
					{
						shakes[key] = value;
					}
				}
			}
			resetAfterCycle();
			customStrengthOverTime = null;
			frequency = 0f;
			prevInterpolationFactor = 0f;
			prevCyclesDone = 0;
		}

		private void resetAfterCycle()
		{
			t = 0f;
			from = default(Vector3);
		}
	}
}
