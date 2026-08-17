using System;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public static class RoadCurveEstimator
	{
		public static float EstimateRoadXAtZ(float worldZ, float terrainPositionX, float terrainPositionZ, float terrainSizeX, float terrainSizeZ, float roadCurveAmplitude, float roadCurveFrequency, bool usePerlinNoise, float perlinSeed)
		{
			float num = terrainPositionX + terrainSizeX / 2f;
			float num2 = Mathf.Clamp01((worldZ - terrainPositionZ) / terrainSizeZ);
			float num3 = Mathf.Sin(num2 * (float)Math.PI);
			float distanceAlongRoad = terrainSizeZ * num2;
			float amplitude = roadCurveAmplitude * 0.5f * num3;
			float num4 = CalculateCurve(distanceAlongRoad, amplitude, roadCurveFrequency, usePerlinNoise, perlinSeed);
			return num + num4;
		}

		public static float DerivePerlinSeed(int tileSeed)
		{
			return (float)new System.Random(tileSeed).NextDouble() * 10000f;
		}

		private static float CalculateCurve(float distanceAlongRoad, float amplitude, float frequency, bool usePerlinNoise, float perlinSeed)
		{
			if (usePerlinNoise)
			{
				return (Mathf.PerlinNoise(distanceAlongRoad * frequency + perlinSeed, perlinSeed) - 0.5f) * 2f * amplitude;
			}
			return Mathf.Sin(distanceAlongRoad * frequency * 2f * (float)Math.PI) * amplitude;
		}
	}
}
