using System;
using UnityEngine;

[Serializable]
public class PerlinSampler
{
	public float scale = 1f;

	public int iterations = 2;

	public float scaleIncrease = 3f;

	public float roughness = 0.3f;

	public float pow = 1f;

	public Vector2 minMax = new Vector2(0f, 1f);

	public bool Sample(Vector2 pos, int seed = 0)
	{
		float num = SampleValue(pos, seed);
		if (num > minMax.x)
		{
			return num < minMax.y;
		}
		return false;
	}

	public float SampleValue(Vector2 pos, int seed = 0)
	{
		float num = 0f;
		for (int i = 0; i < iterations; i++)
		{
			float num2 = scale;
			num2 *= Mathf.Pow(roughness, i);
			float num3 = Mathf.PerlinNoise((float)(12345 + seed) + pos.x * num2 * 0.1f, (float)(12345 + seed) + pos.y * num2 * 0.1f);
			if (i == 0)
			{
				num = num3;
				continue;
			}
			float t = Mathf.Pow(roughness, i);
			num = Mathf.Lerp(num, num3, t);
		}
		if (!Mathf.Approximately(pow, 1f))
		{
			num = Mathf.Pow(num, pow);
		}
		return num;
	}
}
