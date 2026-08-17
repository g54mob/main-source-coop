using UnityEngine;

namespace PenguinPackage.Utilities
{
	public static class Mathp
	{
		public static float Remap(float currentValue, float minValue, float maxValue)
		{
			return Mathf.Clamp((currentValue - minValue) / (maxValue - minValue), 0f, 1f);
		}

		public static float AngleBetween2Points(Vector2 pointA, Vector2 pointB)
		{
			return Mathf.Atan2(pointA.y - pointB.y, pointA.x - pointB.x) * 57.29578f;
		}

		public static float AngleBetween2Points(Vector2 pointA, Vector2 pointB, float offset)
		{
			float num = Mathf.Atan2(pointA.y - pointB.y, pointA.x - pointB.x) * 57.29578f;
			num += offset;
			if (num > 180f)
			{
				num -= 360f;
			}
			return num;
		}

		public static float Angle360Between2Points(Vector2 pointA, Vector2 pointB)
		{
			float num = Mathf.Atan2(pointA.y - pointB.y, pointA.x - pointB.x) * 57.29578f;
			if (num < 0f)
			{
				num = 180f + (180f + num);
			}
			return num;
		}
	}
}
