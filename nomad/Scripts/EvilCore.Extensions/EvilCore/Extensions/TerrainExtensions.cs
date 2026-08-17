using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class TerrainExtensions
	{
		private readonly struct HeightmapRegion
		{
			public readonly float[,] Heights;

			public readonly int StartX;

			public readonly int StartZ;

			public readonly int Width;

			public readonly int Height;

			public readonly int CenterX;

			public readonly int CenterZ;

			public readonly float RadiusInUnits;

			public readonly float FalloffRadius;

			public readonly float NormalizedTargetHeight;

			public readonly TerrainData TerrainData;

			private HeightmapRegion(float[,] heights, int startX, int startZ, int width, int height, int centerX, int centerZ, float radiusInUnits, float falloffRadius, float normalizedTargetHeight, TerrainData terrainData)
			{
				Heights = heights;
				StartX = startX;
				StartZ = startZ;
				Width = width;
				Height = height;
				CenterX = centerX;
				CenterZ = centerZ;
				RadiusInUnits = radiusInUnits;
				FalloffRadius = falloffRadius;
				NormalizedTargetHeight = normalizedTargetHeight;
				TerrainData = terrainData;
			}

			public static bool TryCreate(Terrain terrain, Vector3 worldPosition, float radius, float targetHeight, float falloffPercent, out HeightmapRegion region)
			{
				region = default(HeightmapRegion);
				TerrainData terrainData = terrain.terrainData;
				Vector3 position = terrain.transform.position;
				Vector3 vector = worldPosition - position;
				int heightmapResolution = terrainData.heightmapResolution;
				float num = radius / terrainData.size.x * (float)heightmapResolution;
				float falloffRadius = num * (1f - falloffPercent);
				int num2 = Mathf.RoundToInt(vector.x / terrainData.size.x * (float)heightmapResolution);
				int num3 = Mathf.RoundToInt(vector.z / terrainData.size.z * (float)heightmapResolution);
				int num4 = Mathf.Max(0, num2 - Mathf.CeilToInt(num));
				int num5 = Mathf.Max(0, num3 - Mathf.CeilToInt(num));
				int num6 = Mathf.Min(heightmapResolution, num2 + Mathf.CeilToInt(num));
				int num7 = Mathf.Min(heightmapResolution, num3 + Mathf.CeilToInt(num));
				int num8 = num6 - num4;
				int num9 = num7 - num5;
				if (num8 <= 0 || num9 <= 0)
				{
					return false;
				}
				float[,] heights = terrainData.GetHeights(num4, num5, num8, num9);
				float normalizedTargetHeight = targetHeight / terrainData.size.y;
				region = new HeightmapRegion(heights, num4, num5, num8, num9, num2, num3, num, falloffRadius, normalizedTargetHeight, terrainData);
				return true;
			}

			public void Apply()
			{
				TerrainData.SetHeights(StartX, StartZ, Heights);
			}
		}

		private const float MAX_TERRAIN_MS_PER_FRAME = 2f;

		private static readonly AnimationCurve DefaultFalloffCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		public static async UniTask FlattenAtPositionAsync(this Terrain terrain, Vector3 worldPosition, float radius, float falloffPercent = 0.3f, AnimationCurve falloffCurve = null)
		{
			if (terrain == null)
			{
				EvilLogger.LogError("[TerrainExtensions] Terrain is null!", "FlattenAtPositionAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\TerrainExtensions.cs", 103);
				return;
			}
			if (falloffCurve == null)
			{
				falloffCurve = DefaultFalloffCurve;
			}
			float targetHeight = terrain.SampleHeight(worldPosition);
			await UniTask.Yield();
			if (!HeightmapRegion.TryCreate(terrain, worldPosition, radius, targetHeight, falloffPercent, out var region))
			{
				return;
			}
			float radiusSq = region.RadiusInUnits * region.RadiusInUnits;
			float falloffRadiusSq = region.FalloffRadius * region.FalloffRadius;
			float falloffRange = region.RadiusInUnits - region.FalloffRadius;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int z = 0; z < region.Height; z++)
			{
				for (int i = 0; i < region.Width; i++)
				{
					float num = region.StartX + i - region.CenterX;
					float num2 = region.StartZ + z - region.CenterZ;
					float num3 = num * num + num2 * num2;
					if (num3 <= radiusSq)
					{
						float t = 1f;
						if (num3 > falloffRadiusSq)
						{
							float num4 = (Mathf.Sqrt(num3) - region.FalloffRadius) / falloffRange;
							t = falloffCurve.Evaluate(1f - num4);
						}
						region.Heights[z, i] = Mathf.Lerp(region.Heights[z, i], region.NormalizedTargetHeight, t);
					}
				}
				if ((Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f >= 2f)
				{
					await UniTask.Yield();
					realtimeSinceStartup = Time.realtimeSinceStartup;
				}
			}
			region.Apply();
			await UniTask.Yield();
		}

		public static float SampleMaxHeightInBounds(this Terrain terrain, Vector3 centerPosition, float boundsWidth, float boundsDepth)
		{
			if (terrain == null)
			{
				EvilLogger.LogError("[TerrainExtensions] Terrain is null!", "SampleMaxHeightInBounds", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\TerrainExtensions.cs", 162);
				return 0f;
			}
			TerrainData terrainData = terrain.terrainData;
			Vector3 position = terrain.transform.position;
			Vector3 size = terrainData.size;
			int heightmapResolution = terrainData.heightmapResolution;
			Vector3 vector = centerPosition - position;
			float num = boundsWidth / 2f;
			float num2 = boundsDepth / 2f;
			int num3 = Mathf.Max(0, Mathf.FloorToInt((vector.x - num) / size.x * (float)heightmapResolution));
			int num4 = Mathf.Max(0, Mathf.FloorToInt((vector.z - num2) / size.z * (float)heightmapResolution));
			int num5 = Mathf.Min(heightmapResolution, Mathf.CeilToInt((vector.x + num) / size.x * (float)heightmapResolution));
			int num6 = Mathf.Min(heightmapResolution, Mathf.CeilToInt((vector.z + num2) / size.z * (float)heightmapResolution));
			int num7 = num5 - num3;
			int num8 = num6 - num4;
			if (num7 <= 0 || num8 <= 0)
			{
				return terrain.SampleHeight(centerPosition);
			}
			float[,] heights = terrainData.GetHeights(num3, num4, num7, num8);
			float num9 = -3.4028235E+38f;
			for (int i = 0; i < num8; i++)
			{
				for (int j = 0; j < num7; j++)
				{
					if (heights[i, j] > num9)
					{
						num9 = heights[i, j];
					}
				}
			}
			return num9 * size.y;
		}

		public static async UniTask RaiseToHeightAsync(this Terrain terrain, Vector3 worldPosition, float targetHeight, float radius, float falloffPercent = 0.3f, AnimationCurve falloffCurve = null)
		{
			if (terrain == null)
			{
				EvilLogger.LogError("[TerrainExtensions] Terrain is null!", "RaiseToHeightAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\TerrainExtensions.cs", 206);
				return;
			}
			if (falloffCurve == null)
			{
				falloffCurve = DefaultFalloffCurve;
			}
			await UniTask.Yield();
			if (!HeightmapRegion.TryCreate(terrain, worldPosition, radius, targetHeight, falloffPercent, out var region))
			{
				return;
			}
			float radiusSq = region.RadiusInUnits * region.RadiusInUnits;
			float falloffRadiusSq = region.FalloffRadius * region.FalloffRadius;
			float falloffRange = region.RadiusInUnits - region.FalloffRadius;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int z = 0; z < region.Height; z++)
			{
				for (int i = 0; i < region.Width; i++)
				{
					float num = region.StartX + i - region.CenterX;
					float num2 = region.StartZ + z - region.CenterZ;
					float num3 = num * num + num2 * num2;
					if (num3 <= radiusSq && region.Heights[z, i] < region.NormalizedTargetHeight)
					{
						float t = 1f;
						if (num3 > falloffRadiusSq)
						{
							float num4 = (Mathf.Sqrt(num3) - region.FalloffRadius) / falloffRange;
							t = falloffCurve.Evaluate(1f - num4);
						}
						region.Heights[z, i] = Mathf.Lerp(region.Heights[z, i], region.NormalizedTargetHeight, t);
					}
				}
				if ((Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f >= 2f)
				{
					await UniTask.Yield();
					realtimeSinceStartup = Time.realtimeSinceStartup;
				}
			}
			region.Apply();
			await UniTask.Yield();
		}

		public static async UniTask CreateFoundationAsync(this Terrain terrain, Vector3 worldPosition, float platformRadius, float embankmentRadius, float targetHeight, float embankmentSteepness = 0.7f)
		{
			if (terrain == null)
			{
				EvilLogger.LogError("[TerrainExtensions] Terrain is null!", "CreateFoundationAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\TerrainExtensions.cs", 261);
				return;
			}
			if (platformRadius >= embankmentRadius)
			{
				EvilLogger.LogError($"[TerrainExtensions] Platform radius ({platformRadius}) must be smaller than embankment radius ({embankmentRadius})!", "CreateFoundationAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\TerrainExtensions.cs", 267);
				return;
			}
			AnimationCurve embankmentCurve = new AnimationCurve();
			embankmentCurve.AddKey(0f, 1f);
			embankmentCurve.AddKey(embankmentSteepness, 0.5f);
			embankmentCurve.AddKey(1f, 0f);
			TerrainData terrainData = terrain.terrainData;
			Vector3 position = terrain.transform.position;
			Vector3 vector = worldPosition - position;
			int heightmapResolution = terrainData.heightmapResolution;
			float platformRadiusInUnits = platformRadius / terrainData.size.x * (float)heightmapResolution;
			float embankmentRadiusInUnits = embankmentRadius / terrainData.size.x * (float)heightmapResolution;
			int centerX = Mathf.RoundToInt(vector.x / terrainData.size.x * (float)heightmapResolution);
			int centerZ = Mathf.RoundToInt(vector.z / terrainData.size.z * (float)heightmapResolution);
			int startX = Mathf.Max(0, centerX - Mathf.CeilToInt(embankmentRadiusInUnits));
			int startZ = Mathf.Max(0, centerZ - Mathf.CeilToInt(embankmentRadiusInUnits));
			int num = Mathf.Min(heightmapResolution, centerX + Mathf.CeilToInt(embankmentRadiusInUnits));
			int num2 = Mathf.Min(heightmapResolution, centerZ + Mathf.CeilToInt(embankmentRadiusInUnits));
			int width = num - startX;
			int height = num2 - startZ;
			if (width <= 0 || height <= 0)
			{
				return;
			}
			await UniTask.Yield();
			float[,] heights = terrainData.GetHeights(startX, startZ, width, height);
			float normalizedTargetHeight = targetHeight / terrainData.size.y;
			float platformRadiusSq = platformRadiusInUnits * platformRadiusInUnits;
			float embankmentRadiusSq = embankmentRadiusInUnits * embankmentRadiusInUnits;
			float embankmentRange = embankmentRadiusInUnits - platformRadiusInUnits;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int z = 0; z < height; z++)
			{
				for (int i = 0; i < width; i++)
				{
					float num3 = startX + i - centerX;
					float num4 = startZ + z - centerZ;
					float num5 = num3 * num3 + num4 * num4;
					if (num5 <= platformRadiusSq)
					{
						heights[z, i] = normalizedTargetHeight;
					}
					else if (num5 <= embankmentRadiusSq)
					{
						float time = (Mathf.Sqrt(num5) - platformRadiusInUnits) / embankmentRange;
						float num6 = embankmentCurve.Evaluate(time);
						float num7 = heights[z, i];
						if (num7 < normalizedTargetHeight)
						{
							heights[z, i] = Mathf.Lerp(num7, normalizedTargetHeight, num6);
						}
						else
						{
							heights[z, i] = Mathf.Lerp(num7, normalizedTargetHeight, num6 * 0.5f);
						}
					}
				}
				if ((Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f >= 2f)
				{
					await UniTask.Yield();
					realtimeSinceStartup = Time.realtimeSinceStartup;
				}
			}
			terrainData.SetHeights(startX, startZ, heights);
			await UniTask.Yield();
		}

		public static async UniTask ClearVegetationAsync(this Terrain terrain, Vector3 worldPosition, float radius)
		{
			if (terrain == null)
			{
				EvilLogger.LogError("[TerrainExtensions] Terrain is null!", "ClearVegetationAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\TerrainExtensions.cs", 355);
				return;
			}
			TerrainData terrainData = terrain.terrainData;
			if (terrainData == null)
			{
				EvilLogger.LogError("[TerrainExtensions] TerrainData is null!", "ClearVegetationAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\TerrainExtensions.cs", 362);
				return;
			}
			await UniTask.Yield();
			await ClearTreesInRadiusAsync(terrain, terrainData, worldPosition, radius);
			await ClearDetailsInRadiusAsync(terrain, terrainData, worldPosition, radius);
		}

		private static async UniTask ClearTreesInRadiusAsync(Terrain terrain, TerrainData terrainData, Vector3 worldPosition, float radius)
		{
			List<TreeInstance> trees = TerrainColliderRefreshScheduler.GetPendingTrees(terrain);
			if (trees.Count == 0)
			{
				return;
			}
			Vector3 position = terrain.transform.position;
			Vector3 size = terrainData.size;
			Vector3 vector = worldPosition - position;
			float normalizedX = vector.x / size.x;
			float normalizedZ = vector.z / size.z;
			float num = radius / size.x;
			float normalizedRadiusSq = num * num;
			List<TreeInstance> treesToKeep = new List<TreeInstance>(trees.Count);
			int removedCount = 0;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int i = 0; i < trees.Count; i++)
			{
				TreeInstance item = trees[i];
				float num2 = item.position.x - normalizedX;
				float num3 = item.position.z - normalizedZ;
				if (num2 * num2 + num3 * num3 > normalizedRadiusSq)
				{
					treesToKeep.Add(item);
				}
				else
				{
					removedCount++;
				}
				if (i % 50 == 0 && (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f >= 2f)
				{
					await UniTask.Yield();
					realtimeSinceStartup = Time.realtimeSinceStartup;
				}
			}
			if (removedCount > 0)
			{
				TerrainColliderRefreshScheduler.SetPendingTrees(terrain, treesToKeep);
				TerrainColliderRefreshScheduler.MarkDirty(terrain);
			}
		}

		private static async UniTask ClearDetailsInRadiusAsync(Terrain terrain, TerrainData terrainData, Vector3 worldPosition, float radius)
		{
			int detailLayers = terrainData.detailPrototypes.Length;
			if (detailLayers == 0)
			{
				return;
			}
			Vector3 position = terrain.transform.position;
			Vector3 size = terrainData.size;
			int detailResolution = terrainData.detailResolution;
			Vector3 vector = worldPosition - position;
			int centerX = Mathf.RoundToInt(vector.x / size.x * (float)detailResolution);
			int centerZ = Mathf.RoundToInt(vector.z / size.z * (float)detailResolution);
			int num = Mathf.CeilToInt(radius / size.x * (float)detailResolution);
			float radiusInDetailsSq = (float)num * (float)num;
			int startX = Mathf.Max(0, centerX - num);
			int startZ = Mathf.Max(0, centerZ - num);
			int num2 = Mathf.Min(detailResolution, centerX + num);
			int num3 = Mathf.Min(detailResolution, centerZ + num);
			int width = num2 - startX;
			int height = num3 - startZ;
			if (width <= 0 || height <= 0)
			{
				return;
			}
			float frameStartTime = Time.realtimeSinceStartup;
			for (int layer = 0; layer < detailLayers; layer++)
			{
				await UniTask.Yield();
				int[,] detailMap = terrainData.GetDetailLayer(startX, startZ, width, height, layer);
				for (int z = 0; z < height; z++)
				{
					for (int i = 0; i < width; i++)
					{
						float num4 = startX + i - centerX;
						float num5 = startZ + z - centerZ;
						if (num4 * num4 + num5 * num5 <= radiusInDetailsSq)
						{
							detailMap[z, i] = 0;
						}
					}
					if ((Time.realtimeSinceStartup - frameStartTime) * 1000f >= 2f)
					{
						await UniTask.Yield();
						frameStartTime = Time.realtimeSinceStartup;
					}
				}
				terrainData.SetDetailLayer(startX, startZ, layer, detailMap);
			}
		}
	}
}
