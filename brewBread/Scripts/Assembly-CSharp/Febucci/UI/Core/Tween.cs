using UnityEngine;

namespace Febucci.UI.Core
{
	public static class Tween
	{
		public static float EaseIn(float t)
		{
			return t * t;
		}

		public static float Flip(float x)
		{
			return 1f - x;
		}

		public static float Square(float t)
		{
			return t * t;
		}

		public static float EaseOut(float t)
		{
			return Flip(Square(Flip(t)));
		}

		public static float EaseInOut(float t)
		{
			return Mathf.Lerp(EaseIn(t), EaseOut(t), t);
		}

		public static float BounceOut(float t)
		{
			if (t < 0.36363637f)
			{
				return 7.5625f * t * t;
			}
			if (t < 0.72727275f)
			{
				return 7.5625f * (t -= 0.54545456f) * t + 0.75f;
			}
			if (t < 0.90909094f)
			{
				return 7.5625f * (t -= 0.8181818f) * t + 0.9375f;
			}
			return 7.5625f * (t -= 21f / 22f) * t + 63f / 64f;
		}
	}
}
