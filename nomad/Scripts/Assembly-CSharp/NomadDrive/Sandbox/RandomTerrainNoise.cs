using System;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace NomadDrive.Sandbox
{
	public class RandomTerrainNoise : MonoBehaviour
	{
		[Header("Terrain Reference")]
		private Terrain _terrain;

		[Header("Generation Settings")]
		[SerializeField]
		private bool generateOnStart;

		[SerializeField]
		private bool generateOnValidate;

		[SerializeField]
		private int seed = 12345;

		[Header("Base Terrain")]
		[Range(0f, 1f)]
		public float baseHeight = 0.1f;

		[Header("Noise Layers")]
		public NoiseLayer[] noiseLayers = new NoiseLayer[3]
		{
			new NoiseLayer
			{
				layerName = "Base Mountains",
				amplitude = 0.3f,
				frequency = 0.005f,
				octaves = 6,
				persistence = 0.6f,
				lacunarity = 2f
			},
			new NoiseLayer
			{
				layerName = "Rolling Hills",
				amplitude = 0.15f,
				frequency = 0.02f,
				octaves = 4,
				persistence = 0.5f,
				lacunarity = 2f
			},
			new NoiseLayer
			{
				layerName = "Fine Detail",
				amplitude = 0.05f,
				frequency = 0.08f,
				octaves = 3,
				persistence = 0.4f,
				lacunarity = 2f
			}
		};

		[Header("Post Processing")]
		[Range(0.1f, 5f)]
		public float heightMultiplier = 1f;

		[Range(0f, 1f)]
		public float smoothness;

		public AnimationCurve heightCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[Header("Debug")]
		public bool showNoiseLayers = true;

		private void Start()
		{
			if (generateOnStart)
			{
				GenerateTerrainNoise();
			}
		}

		private void OnValidate()
		{
			if (_terrain == null)
			{
				_terrain = GetComponent<Terrain>();
			}
			if (generateOnValidate && _terrain != null)
			{
				GenerateTerrainNoise();
			}
		}

		public void GenerateTerrainNoise()
		{
			if (_terrain == null)
			{
				EvilLogger.LogError("No Terrain component found!", "GenerateTerrainNoise", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Sandbox\\RandomTerrainNoise.cs", 131);
				return;
			}
			TerrainData terrainData = _terrain.terrainData;
			int heightmapResolution = terrainData.heightmapResolution;
			int heightmapResolution2 = terrainData.heightmapResolution;
			float[,] array = new float[heightmapResolution, heightmapResolution2];
			UnityEngine.Random.InitState(seed);
			for (int i = 0; i < heightmapResolution; i++)
			{
				for (int j = 0; j < heightmapResolution2; j++)
				{
					array[i, j] = baseHeight;
				}
			}
			for (int k = 0; k < noiseLayers.Length; k++)
			{
				if (noiseLayers[k].enabled)
				{
					ApplyNoiseLayer(array, noiseLayers[k], heightmapResolution, heightmapResolution2, k);
				}
			}
			ApplyPostProcessing(array, heightmapResolution, heightmapResolution2);
			terrainData.SetHeights(0, 0, array);
		}

		private void ApplyNoiseLayer(float[,] heights, NoiseLayer layer, int width, int height, int layerIndex)
		{
			Vector2 vector = layer.offset;
			if (vector == Vector2.zero)
			{
				UnityEngine.Random.InitState(seed + layerIndex * 1000);
				vector = new Vector2(UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f));
			}
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					float num = GenerateOctaveNoise((float)i + vector.x, (float)j + vector.y, layer.frequency, layer.octaves, layer.persistence, layer.lacunarity);
					num *= layer.amplitude;
					heights[i, j] = ApplyBlendMode(heights[i, j], num, layer.blendMode);
					heights[i, j] = Mathf.Clamp01(heights[i, j]);
				}
			}
		}

		private float GenerateOctaveNoise(float x, float y, float frequency, int octaves, float persistence, float lacunarity)
		{
			float num = 0f;
			float num2 = 1f;
			float num3 = 0f;
			for (int i = 0; i < octaves; i++)
			{
				num += Mathf.PerlinNoise(x * frequency, y * frequency) * num2;
				num3 += num2;
				num2 *= persistence;
				frequency *= lacunarity;
			}
			return num / num3;
		}

		private float ApplyBlendMode(float baseValue, float layerValue, BlendMode mode)
		{
			switch (mode)
			{
			case BlendMode.Add:
				return baseValue + layerValue;
			case BlendMode.Multiply:
				return baseValue * (1f + layerValue);
			case BlendMode.Overlay:
				if (!(baseValue < 0.5f))
				{
					return 1f - 2f * (1f - baseValue) * (1f - layerValue);
				}
				return 2f * baseValue * layerValue;
			case BlendMode.Screen:
				return 1f - (1f - baseValue) * (1f - layerValue);
			case BlendMode.Max:
				return Mathf.Max(baseValue, layerValue);
			case BlendMode.Min:
				return Mathf.Min(baseValue, layerValue);
			default:
				return baseValue + layerValue;
			}
		}

		private void ApplyPostProcessing(float[,] heights, int width, int height)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					float time = heights[i, j];
					time = heightCurve.Evaluate(time);
					time *= heightMultiplier;
					heights[i, j] = Mathf.Clamp01(time);
				}
			}
			if (smoothness > 0f)
			{
				ApplySmoothing(heights, width, height, smoothness);
			}
		}

		private void ApplySmoothing(float[,] heights, int width, int height, float intensity)
		{
			float[,] array = (float[,])heights.Clone();
			int num = Mathf.RoundToInt(intensity * 3f);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					float num2 = 0f;
					int num3 = 0;
					for (int k = -num; k <= num; k++)
					{
						for (int l = -num; l <= num; l++)
						{
							int num4 = i + k;
							int num5 = j + l;
							if (num4 >= 0 && num4 < width && num5 >= 0 && num5 < height)
							{
								num2 += heights[num4, num5];
								num3++;
							}
						}
					}
					if (num3 > 0)
					{
						float b = num2 / (float)num3;
						array[i, j] = Mathf.Lerp(heights[i, j], b, intensity);
					}
				}
			}
			for (int m = 0; m < width; m++)
			{
				for (int n = 0; n < height; n++)
				{
					heights[m, n] = array[m, n];
				}
			}
		}

		[ContextMenu("Randomize Seed")]
		public void RandomizeSeed()
		{
			seed = UnityEngine.Random.Range(1, 999999);
		}

		[ContextMenu("Reset Terrain")]
		public void ResetTerrain()
		{
			if (!(_terrain == null))
			{
				TerrainData terrainData = _terrain.terrainData;
				int heightmapResolution = terrainData.heightmapResolution;
				int heightmapResolution2 = terrainData.heightmapResolution;
				float[,] heights = new float[heightmapResolution, heightmapResolution2];
				terrainData.SetHeights(0, 0, heights);
			}
		}

		[ContextMenu("Add Random Layer")]
		public void AddRandomLayer()
		{
			NoiseLayer[] array = new NoiseLayer[noiseLayers.Length + 1];
			Array.Copy(noiseLayers, array, noiseLayers.Length);
			array[noiseLayers.Length] = new NoiseLayer
			{
				layerName = $"Random Layer {noiseLayers.Length + 1}",
				amplitude = UnityEngine.Random.Range(0.05f, 0.3f),
				frequency = UnityEngine.Random.Range(0.005f, 0.05f),
				octaves = UnityEngine.Random.Range(2, 6),
				persistence = UnityEngine.Random.Range(0.3f, 0.7f),
				lacunarity = UnityEngine.Random.Range(1.5f, 3f)
			};
			noiseLayers = array;
		}
	}
}
