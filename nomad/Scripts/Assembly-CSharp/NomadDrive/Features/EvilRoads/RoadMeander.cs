using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	public static class RoadMeander
	{
		public const float TileEdgeMargin = 40f;

		public static float MaxAmplitude(float terrainSizeX, float maxMeanderAmplitude)
		{
			float b = Mathf.Min(maxMeanderAmplitude, terrainSizeX * 0.5f - 40f);
			return Mathf.Max(0f, b);
		}

		public static float Offset(float worldZ, int octaves, float baseAmplitude, float baseFrequency, float persistence, float lacunarity, float maxAmplitude, int meanderSeed)
		{
			float num = 0f;
			float num2 = baseAmplitude;
			float num3 = baseFrequency;
			for (int i = 0; i < octaves; i++)
			{
				float num4 = (float)meanderSeed * 0.0001f + (float)i * 137.13f;
				float num5 = Mathf.PerlinNoise(worldZ * num3 + num4, num4 * 0.37f) - 0.5f;
				num += num5 * 2f * num2;
				num2 *= persistence;
				num3 *= lacunarity;
			}
			return Mathf.Clamp(num, 0f - maxAmplitude, maxAmplitude);
		}

		public static Vector3 Direction(float worldZ, int octaves, float baseAmplitude, float baseFrequency, float persistence, float lacunarity, float maxAmplitude, int meanderSeed, float delta = 1f)
		{
			float num = Offset(worldZ + delta, octaves, baseAmplitude, baseFrequency, persistence, lacunarity, maxAmplitude, meanderSeed);
			float num2 = Offset(worldZ - delta, octaves, baseAmplitude, baseFrequency, persistence, lacunarity, maxAmplitude, meanderSeed);
			Vector3 vector = new Vector3(num - num2, 0f, 2f * delta);
			if (!(vector.sqrMagnitude > 1E-08f))
			{
				return new Vector3(0f, 0f, 1f);
			}
			return vector.normalized;
		}

		public static float X(float worldZ, float terrainPositionX, float terrainSizeX, int octaves, float baseAmplitude, float baseFrequency, float persistence, float lacunarity, float maxMeanderAmplitude, int meanderSeed)
		{
			float num = terrainPositionX + terrainSizeX * 0.5f;
			float maxAmplitude = MaxAmplitude(terrainSizeX, maxMeanderAmplitude);
			return num + Offset(worldZ, octaves, baseAmplitude, baseFrequency, persistence, lacunarity, maxAmplitude, meanderSeed);
		}
	}
}
