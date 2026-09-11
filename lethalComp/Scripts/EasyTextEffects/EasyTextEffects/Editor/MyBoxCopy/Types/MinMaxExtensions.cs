using UnityEngine;

namespace EasyTextEffects.Editor.MyBoxCopy.Types
{
	public static class MinMaxExtensions
	{
		public static bool IsInRange(this MinMaxInt minMax, int value)
		{
			if (value >= minMax.Min)
			{
				return value <= minMax.Max;
			}
			return false;
		}

		public static bool IsInRange(this MinMaxFloat minMax, float value)
		{
			if (value >= minMax.Min)
			{
				return value <= minMax.Max;
			}
			return false;
		}

		public static int Clamp(this MinMaxInt minMax, int value)
		{
			return Mathf.Clamp(value, minMax.Min, minMax.Max);
		}

		public static float Clamp(this MinMaxFloat minMax, float value)
		{
			return Mathf.Clamp(value, minMax.Min, minMax.Max);
		}

		public static int Length(this MinMaxInt minMax)
		{
			return minMax.Max - minMax.Min;
		}

		public static float Length(this MinMaxFloat minMax)
		{
			return minMax.Max - minMax.Min;
		}

		public static int MidPoint(this MinMaxInt minMax)
		{
			return minMax.Min + minMax.Length() / 2;
		}

		public static float MidPoint(this MinMaxFloat minMax)
		{
			return minMax.Min + minMax.Length() / 2f;
		}

		public static float Lerp(this MinMaxInt minMax, float value)
		{
			return Mathf.Lerp(minMax.Min, minMax.Max, value);
		}

		public static float Lerp(this MinMaxFloat minMax, float value)
		{
			return Mathf.Lerp(minMax.Min, minMax.Max, value);
		}

		public static float InverseLerp(this MinMaxInt minMax, float value)
		{
			return Mathf.InverseLerp(minMax.Min, minMax.Max, value);
		}

		public static float InverseLerp(this MinMaxFloat minMax, float value)
		{
			return Mathf.InverseLerp(minMax.Min, minMax.Max, value);
		}

		public static float LerpUnclamped(this MinMaxInt minMax, float value)
		{
			return Mathf.LerpUnclamped(minMax.Min, minMax.Max, value);
		}

		public static float LerpUnclamped(this MinMaxFloat minMax, float value)
		{
			return Mathf.LerpUnclamped(minMax.Min, minMax.Max, value);
		}

		public static int RandomInRange(this MinMaxInt minMax)
		{
			return Random.Range(minMax.Min, minMax.Max);
		}

		public static float RandomInRange(this MinMaxFloat minMax)
		{
			return Random.Range(minMax.Min, minMax.Max);
		}

		public static int RandomInRangeInclusive(this MinMaxInt minMax)
		{
			return Random.Range(minMax.Min, minMax.Max + 1);
		}
	}
}
