using UnityEngine;

namespace Features.PostProcessingModule.Scripts.Rendering
{
	public static class FlickerMath
	{
		public static float Evaluate(float time, float frequency, float randomization)
		{
			float num = Mathf.Max(frequency, 0.01f);
			float num2 = 1f / num;
			float num3 = Hash(Mathf.FloorToInt(time / num2), 0f) * randomization;
			int num4 = Mathf.FloorToInt((time + num3 * num2) / num2);
			float a = (((num4 & 1) == 0) ? 0f : 1f);
			float b = Hash(num4, 17f);
			return Mathf.Lerp(a, b, randomization);
		}

		public static float Hash(int seed, float offset)
		{
			return Mathf.Repeat(Mathf.Sin(Vector2.Dot(new Vector2((float)seed + offset, (float)seed * 0.37f + offset), new Vector2(12.9898f, 78.233f))) * 43758.547f, 1f);
		}
	}
}
