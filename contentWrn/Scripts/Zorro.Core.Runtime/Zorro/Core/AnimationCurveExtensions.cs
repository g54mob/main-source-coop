using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Zorro.Core
{
	public static class AnimationCurveExtensions
	{
		public static float GetStartTime(this AnimationCurve animationCurve)
		{
			if (animationCurve.keys.Length == 0)
			{
				return 0f;
			}
			return animationCurve[0].time;
		}

		public static float GetEndTime(this AnimationCurve animationCurve)
		{
			if (animationCurve.keys.Length == 0)
			{
				return 0f;
			}
			return animationCurve[animationCurve.length - 1].time;
		}

		public static Keyframe First(this AnimationCurve animationCurve)
		{
			if (animationCurve.keys.Length != 0)
			{
				return animationCurve.keys[0];
			}
			throw new IndexOutOfRangeException("Animation curves has no keys. Can't fetch first");
		}

		public static IEnumerator YieldForCurve(this AnimationCurve animationCurve, Action<float> onSampleCurve, bool timeScale = true, float speed = 1f)
		{
			float timer = 0f;
			float length = animationCurve.GetEndTime();
			while (timer < length)
			{
				float num = (timeScale ? Time.deltaTime : Time.unscaledDeltaTime);
				if (num > 1f / 60f)
				{
					num = 1f / 60f;
				}
				timer += num * speed;
				onSampleCurve?.Invoke(animationCurve.Evaluate(timer));
				yield return null;
			}
			onSampleCurve?.Invoke(animationCurve.Evaluate(animationCurve.keys.Last().time));
		}

		public static IEnumerator YieldForCurveFixedUpdate(this AnimationCurve animationCurve, Action<float> onSampleCurve, bool timeScale = true, float speed = 1f)
		{
			float timer = 0f;
			float length = animationCurve.GetEndTime();
			WaitForFixedUpdate yield = new WaitForFixedUpdate();
			while (timer < length)
			{
				timer += (timeScale ? Time.deltaTime : Time.unscaledDeltaTime) * speed;
				onSampleCurve?.Invoke(animationCurve.Evaluate(timer));
				yield return yield;
			}
			onSampleCurve?.Invoke(animationCurve.Evaluate(animationCurve.keys.Last().time));
		}
	}
}
