using UnityEngine;

namespace EvilCore.Extensions
{
	public static class FloatExtensions
	{
		public static float Remap(this float value, float min1, float max1, float min2, float max2)
		{
			return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
		}

		public static bool Approximately(this float value, float other)
		{
			return Mathf.Approximately(value, other);
		}

		public static string ToShortenedString(this float value)
		{
			if (value % 1f != 0f)
			{
				return value.ToString("F1");
			}
			return value.ToString("F0");
		}
	}
}
