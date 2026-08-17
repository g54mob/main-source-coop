using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using NomadDrive.Features.EvilRoads;
using NomadDrive.Features.FloatingOrigin;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public class ChunkPoiManager : MonoBehaviour
	{
		[Header("Configuration")]
		[SerializeField]
		private PoiJourneyConfig journeyConfig;

		[Header("References")]
		[SerializeField]
		private PoiSpawner poiSpawner;

		private SeedManager _seedManager;

		private readonly Dictionary<Vector2Int, SpawnedPoiRecord> _spawnedPoIs = new Dictionary<Vector2Int, SpawnedPoiRecord>();

		private PoiArm _forwardArm;

		private PoiArm _backwardArm;

		private float _roadCurveAmplitude;

		private float _roadCurveFrequency;

		private bool _usePerlinNoise;

		private float _roadHalfWidth;

		private bool _useMeander;

		private int _meanderOctaves;

		private float _meanderBaseAmplitude;

		private float _meanderBaseFrequency;

		private float _meanderPersistence;

		private float _meanderLacunarity;

		private float _maxMeanderAmplitude;

		private int _meanderSeed;

		public void Initialize(SeedManager seedManager, float roadCurveAmplitude, float roadCurveFrequency, bool usePerlinNoise, float roadWidth, bool useMeander, int meanderOctaves, float meanderBaseAmplitude, float meanderBaseFrequency, float meanderPersistence, float meanderLacunarity, float maxMeanderAmplitude, int meanderSeed)
		{
			_seedManager = seedManager;
			_roadCurveAmplitude = roadCurveAmplitude;
			_roadCurveFrequency = roadCurveFrequency;
			_usePerlinNoise = usePerlinNoise;
			_roadHalfWidth = roadWidth / 2f;
			_useMeander = useMeander;
			_meanderOctaves = meanderOctaves;
			_meanderBaseAmplitude = meanderBaseAmplitude;
			_meanderBaseFrequency = meanderBaseFrequency;
			_meanderPersistence = meanderPersistence;
			_meanderLacunarity = meanderLacunarity;
			_maxMeanderAmplitude = maxMeanderAmplitude;
			_meanderSeed = meanderSeed;
			if (journeyConfig == null)
			{
				EvilLogger.LogError("[ChunkPOISpawner] POIJourneyConfig is not assigned!", "Initialize", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\POISpawning\\ChunkPoiManager.cs", 74);
			}
			else
			{
				BuildArms();
			}
		}

		private void BuildArms()
		{
			PoiSpawnTuning tuning = new PoiSpawnTuning
			{
				MinMinorsBetweenMajors = journeyConfig.minMinorsBetweenMajors,
				MaxMinorsBetweenMajors = journeyConfig.maxMinorsBetweenMajors,
				MinChunksBetweenPois = journeyConfig.minChunksBetweenPois,
				MaxChunksBetweenPois = journeyConfig.maxChunksBetweenPois,
				ForcedEmptyChunksAfterMajor = journeyConfig.forcedEmptyChunksAfterMajor,
				MinFirstOffsetChunks = journeyConfig.minFirstOffsetChunks,
				MaxFirstOffsetChunks = journeyConfig.maxFirstOffsetChunks
			};
			int seed = _seedManager.Seed;
			int subSeed = _seedManager.GetSubSeed("POIArm_Forward");
			int subSeed2 = _seedManager.GetSubSeed("POIArm_Backward");
			PoiWeightEntry[] entries = ((journeyConfig.minorPoiRules != null) ? journeyConfig.minorPoiRules.poiEntries : null);
			PoiWeightEntry[] entries2 = ((journeyConfig.majorPoiRules != null) ? journeyConfig.majorPoiRules.poiEntries : null);
			_forwardArm = new PoiArm(seed, subSeed, 1, tuning, new DeterministicWeightedBag(entries), new DeterministicWeightedBag(entries2));
			_backwardArm = new PoiArm(seed, subSeed2, -1, tuning, new DeterministicWeightedBag(entries), new DeterministicWeightedBag(entries2));
		}

		public List<SpawnedPoiRecord> GetAllSpawnedPoiRecorsByTileCoord(Vector2Int chunkCoord)
		{
			List<SpawnedPoiRecord> list = new List<SpawnedPoiRecord>();
			if (_spawnedPoIs.TryGetValue(chunkCoord, out var value))
			{
				list.Add(value);
			}
			return list;
		}

		public IEnumerable<string> GetAllPoiGuids()
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (journeyConfig == null)
			{
				return hashSet;
			}
			CollectGuids(hashSet, (journeyConfig.minorPoiRules != null) ? journeyConfig.minorPoiRules.poiEntries : null);
			CollectGuids(hashSet, (journeyConfig.majorPoiRules != null) ? journeyConfig.majorPoiRules.poiEntries : null);
			CollectGuids(hashSet, (journeyConfig.majorPoiRules != null) ? journeyConfig.majorPoiRules.originOnlyPois : null);
			return hashSet;
		}

		private static void CollectGuids(HashSet<string> guids, PoiWeightEntry[] entries)
		{
			if (entries == null)
			{
				return;
			}
			foreach (PoiWeightEntry poiWeightEntry in entries)
			{
				if (poiWeightEntry != null && poiWeightEntry.HasValidReference && !string.IsNullOrEmpty(poiWeightEntry.AssetGuid))
				{
					guids.Add(poiWeightEntry.AssetGuid);
				}
			}
		}

		public void ApplyOriginShift(Vector3 delta)
		{
			Vector2 vector = new Vector2(delta.x, delta.z);
			foreach (SpawnedPoiRecord value in _spawnedPoIs.Values)
			{
				value.Position += vector;
			}
		}

		public async UniTask ProcessChunkForLoading(TileInfo tileInfo)
		{
			int x = tileInfo.tile.coord.x;
			int z = tileInfo.tile.coord.z;
			Vector2Int chunkCoord = new Vector2Int(x, z);
			Terrain terrain = tileInfo.tile.GetTerrain(isDraft: false);
			Vector3 terrainSize = terrain.terrainData.size;
			Vector3 terrainPosition = terrain.transform.position;
			float capturedTime = Time.time;
			Vector3 renderPos = terrainPosition + terrainSize / 2f;
			int tileSeed = _seedManager.GetPositionBasedSeed(FloatingOriginManager.ToTrueWorld(renderPos));
			await Awaitable.BackgroundThreadAsync();
			List<PoiSpawnPlan> plans = ComputeSpawnPlans(chunkCoord, terrainSize, terrainPosition, capturedTime, tileSeed);
			await Awaitable.MainThreadAsync();
			for (int i = 0; i < plans.Count; i++)
			{
				PoiSpawnPlan plan = plans[i];
				Poi poi = await poiSpawner.SpawnPoiFromPlan(plan, tileInfo);
				if (poi != null)
				{
					tileInfo.spawnedPois.Add(poi);
					poi.SetRoadsidePlacement(plan.PlaceNearRoad, plan.TerrainHeightOffset);
					poi.SetStableKey(_seedManager.GetStableKey(chunkCoord, i));
					if (poi.HasPlayerSpawnPoint)
					{
						NetworkSingleton<WorldGenerator>.Instance.RegisterPlayerSpawnPosition(poi.PlayerSpawnPoint.position);
					}
				}
			}
		}

		public static async UniTask ProcessChunkForUnloading(TileInfo tileInfo)
		{
			if (tileInfo.spawnedPois.Count > 0)
			{
				foreach (Poi item in tileInfo.spawnedPois.ToList())
				{
					if (item != null && item.gameObject != null)
					{
						UnityEngine.Object.Destroy(item.gameObject);
					}
				}
				tileInfo.spawnedPois.Clear();
			}
			await UniTask.CompletedTask;
		}

		private List<PoiSpawnPlan> ComputeSpawnPlans(Vector2Int chunkCoord, Vector3 terrainSize, Vector3 terrainPosition, float capturedTime, int tileSeed)
		{
			List<PoiSpawnPlan> list = new List<PoiSpawnPlan>();
			if (_spawnedPoIs.TryGetValue(chunkCoord, out var value))
			{
				list.Add(new PoiSpawnPlan
				{
					ChunkCoord = chunkCoord,
					PoiAssetGuid = value.PoiAssetGuid,
					Category = value.Category,
					SpawnXZ = value.Position,
					RotationEuler = value.RotationEuler,
					DeformMode = value.DeformMode,
					DeformRadius = value.DeformRadius,
					DeformFalloff = value.DeformFalloff,
					TerrainHeightOffset = value.TerrainHeightOffset,
					FoundationYOffset = value.FoundationYOffset,
					VegetationCleaning = value.VegetationClearing,
					GenerateBranchRoad = value.GenerateBranchRoad,
					PlaceNearRoad = value.PlaceNearRoad
				});
				return list;
			}
			if (chunkCoord == Vector2Int.zero)
			{
				ComputeOriginChunkPlans(list, chunkCoord, terrainSize, terrainPosition, capturedTime, tileSeed);
				return list;
			}
			int y = chunkCoord.y;
			PoiArm poiArm = ((y > 0) ? _forwardArm : _backwardArm);
			if (poiArm == null)
			{
				return list;
			}
			poiArm.EnsureBuiltThrough(Mathf.Abs(y));
			if (!poiArm.TryGetPlacement(y, out var placement) || !placement.HasEntry)
			{
				return list;
			}
			PoiWeightEntry entry = placement.Entry;
			bool num = placement.Category == POICategory.Major;
			System.Random random = _seedManager.CreateRandom($"POI_Chunk_{chunkCoord.x}_{chunkCoord.y}");
			Vector2 vector;
			Vector3 rotationEuler;
			if (num)
			{
				vector = CalculateDeterministicPosition(random, terrainSize, terrainPosition, journeyConfig.majorPoiRules.minOffsetX, journeyConfig.majorPoiRules.maxOffsetX, journeyConfig.majorPoiRules.minOffsetZ, journeyConfig.majorPoiRules.maxOffsetZ, RoadSide.Both, journeyConfig.majorPoiRules.minRoadClearance, tileSeed, entry.terrainDeformRadius, entry.placeNearRoad, entry.roadEdgeDistance);
				rotationEuler = CalculateDeterministicRotation(random, journeyConfig.majorPoiRules.minRotationY, journeyConfig.majorPoiRules.maxRotationY, entry.alignToRoad, entry.roadAlignmentYawOffset, vector, terrainPosition, terrainSize, tileSeed);
			}
			else
			{
				vector = CalculateDeterministicPosition(random, terrainSize, terrainPosition, journeyConfig.minorPoiRules.minOffsetX, journeyConfig.minorPoiRules.maxOffsetX, journeyConfig.minorPoiRules.minOffsetZ, journeyConfig.minorPoiRules.maxOffsetZ, journeyConfig.minorPoiRules.allowedRoadSides, journeyConfig.minorPoiRules.minRoadClearance, tileSeed, entry.terrainDeformRadius, entry.placeNearRoad, entry.roadEdgeDistance);
				rotationEuler = CalculateDeterministicRotation(random, journeyConfig.minorPoiRules.minRotationY, journeyConfig.minorPoiRules.maxRotationY, entry.alignToRoad, entry.roadAlignmentYawOffset, vector, terrainPosition, terrainSize, tileSeed);
			}
			list.Add(new PoiSpawnPlan
			{
				ChunkCoord = chunkCoord,
				PoiAssetGuid = entry.AssetGuid,
				Category = placement.Category,
				SpawnXZ = vector,
				RotationEuler = rotationEuler,
				DeformMode = entry.terrainDeformMode,
				DeformRadius = entry.terrainDeformRadius,
				DeformFalloff = entry.terrainDeformFalloff,
				TerrainHeightOffset = entry.terrainHeightOffset,
				FoundationYOffset = entry.foundationYOffset,
				VegetationCleaning = entry.vegetationCleaning,
				GenerateBranchRoad = entry.generateBranchRoad,
				PlaceNearRoad = entry.placeNearRoad
			});
			RecordPoiSpawn(chunkCoord, entry.AssetGuid, placement.Category, vector, rotationEuler, entry.vegetationCleaning, entry.terrainDeformMode, entry.terrainDeformRadius, entry.terrainDeformFalloff, entry.terrainHeightOffset, entry.foundationYOffset, capturedTime, entry.generateBranchRoad, entry.placeNearRoad);
			return list;
		}

		private void ComputeOriginChunkPlans(List<PoiSpawnPlan> plans, Vector2Int chunkCoord, Vector3 terrainSize, Vector3 terrainPosition, float capturedTime, int tileSeed)
		{
			if (journeyConfig.majorPoiRules?.originOnlyPois == null)
			{
				return;
			}
			PoiWeightEntry[] originOnlyPois = journeyConfig.majorPoiRules.originOnlyPois;
			foreach (PoiWeightEntry poiWeightEntry in originOnlyPois)
			{
				if (poiWeightEntry != null && poiWeightEntry.HasValidReference)
				{
					System.Random random = _seedManager.CreateRandom("SpecialOriginPOI_" + poiWeightEntry.AssetGuid);
					Vector2 vector = CalculateDeterministicPosition(random, terrainSize, terrainPosition, journeyConfig.majorPoiRules.originMinOffsetX, journeyConfig.majorPoiRules.originMaxOffsetX, journeyConfig.majorPoiRules.originMinOffsetZ, journeyConfig.majorPoiRules.originMaxOffsetZ, journeyConfig.majorPoiRules.originAllowedRoadSides, journeyConfig.majorPoiRules.minRoadClearance, tileSeed, poiWeightEntry.terrainDeformRadius, poiWeightEntry.placeNearRoad, poiWeightEntry.roadEdgeDistance);
					Vector3 rotationEuler = CalculateDeterministicRotation(random, journeyConfig.majorPoiRules.originMinRotationY, journeyConfig.majorPoiRules.originMaxRotationY, poiWeightEntry.alignToRoad, poiWeightEntry.roadAlignmentYawOffset, vector, terrainPosition, terrainSize, tileSeed);
					plans.Add(new PoiSpawnPlan
					{
						ChunkCoord = chunkCoord,
						PoiAssetGuid = poiWeightEntry.AssetGuid,
						Category = POICategory.Major,
						SpawnXZ = vector,
						RotationEuler = rotationEuler,
						DeformMode = poiWeightEntry.terrainDeformMode,
						DeformRadius = poiWeightEntry.terrainDeformRadius,
						DeformFalloff = poiWeightEntry.terrainDeformFalloff,
						TerrainHeightOffset = poiWeightEntry.terrainHeightOffset,
						FoundationYOffset = poiWeightEntry.foundationYOffset,
						VegetationCleaning = poiWeightEntry.vegetationCleaning,
						GenerateBranchRoad = poiWeightEntry.generateBranchRoad,
						PlaceNearRoad = poiWeightEntry.placeNearRoad
					});
					RecordPoiSpawn(chunkCoord, poiWeightEntry.AssetGuid, POICategory.Major, vector, rotationEuler, poiWeightEntry.vegetationCleaning, poiWeightEntry.terrainDeformMode, poiWeightEntry.terrainDeformRadius, poiWeightEntry.terrainDeformFalloff, poiWeightEntry.terrainHeightOffset, poiWeightEntry.foundationYOffset, capturedTime, poiWeightEntry.generateBranchRoad, poiWeightEntry.placeNearRoad);
				}
			}
		}

		private Vector2 CalculateDeterministicPosition(System.Random random, Vector3 terrainSize, Vector3 terrainPosition, float minOffsetX, float maxOffsetX, float minOffsetZ, float maxOffsetZ, RoadSide allowedSides, float minRoadClearance, int tileSeed, float poiDeformRadius = 15f, bool placeNearRoad = false, float roadEdgeDistance = 5f)
		{
			float num = terrainPosition.x + terrainSize.x / 2f;
			float num2 = terrainPosition.z + terrainSize.z / 2f;
			float num3;
			float num4;
			switch (allowedSides)
			{
			case RoadSide.Both:
			{
				bool num5 = random.NextDouble() < 0.5;
				float num6 = (float)(random.NextDouble() * (double)(maxOffsetX - minOffsetX) + (double)minOffsetX);
				num3 = (num5 ? (0f - num6) : num6);
				num4 = (num5 ? (-1f) : 1f);
				break;
			}
			case RoadSide.Left:
				num3 = 0f - (float)(random.NextDouble() * (double)(maxOffsetX - minOffsetX) + (double)minOffsetX);
				num4 = -1f;
				break;
			default:
				num3 = (float)(random.NextDouble() * (double)(maxOffsetX - minOffsetX) + (double)minOffsetX);
				num4 = 1f;
				break;
			}
			float num7 = (float)(random.NextDouble() * (double)(maxOffsetZ - minOffsetZ) + (double)minOffsetZ);
			float num8 = num2 + num7;
			float num9 = num + num3;
			if (placeNearRoad)
			{
				num9 = CalculateRoadX(num8, terrainPosition, terrainSize, tileSeed) + num4 * (_roadHalfWidth + roadEdgeDistance);
			}
			else if (minRoadClearance > 0f)
			{
				float num10 = CalculateRoadX(num8, terrainPosition, terrainSize, tileSeed);
				float num11 = _roadHalfWidth + poiDeformRadius + minRoadClearance;
				float x = terrainPosition.x;
				float max = terrainPosition.x + terrainSize.x;
				float num12 = Mathf.Clamp(num9, x, max);
				if (Mathf.Abs(num12 - num10) >= num11)
				{
					num9 = num12;
				}
				else
				{
					float num13 = Mathf.Clamp(num10 - num11, x, max);
					float num14 = Mathf.Clamp(num10 + num11, x, max);
					float num15 = Mathf.Abs(num13 - num10);
					float num16 = Mathf.Abs(num14 - num10);
					bool flag = num9 < num10;
					if ((flag ? num15 : num16) >= num11)
					{
						num9 = (flag ? num13 : num14);
					}
					else if (Mathf.Max(num15, num16) >= num11)
					{
						num9 = ((num15 >= num16) ? num13 : num14);
					}
					else
					{
						num9 = ((num15 >= num16) ? num13 : num14);
						if (journeyConfig != null)
						{
							_ = journeyConfig.enableDebugLogs;
						}
					}
				}
			}
			float value = num8 - terrainPosition.z;
			value = Mathf.Clamp(value, 0f, terrainSize.z);
			return new Vector2(Mathf.Clamp(num9 - terrainPosition.x, 0f, terrainSize.x), value);
		}

		private float CalculateRoadX(float worldZ, Vector3 terrainPosition, Vector3 terrainSize, int tileSeed)
		{
			if (_useMeander)
			{
				return RoadMeander.X(FloatingOriginManager.ToTrueWorldZ(worldZ), terrainPosition.x, terrainSize.x, _meanderOctaves, _meanderBaseAmplitude, _meanderBaseFrequency, _meanderPersistence, _meanderLacunarity, _maxMeanderAmplitude, _meanderSeed);
			}
			float perlinSeed = RoadCurveEstimator.DerivePerlinSeed(tileSeed);
			return RoadCurveEstimator.EstimateRoadXAtZ(worldZ, terrainPosition.x, terrainPosition.z, terrainSize.x, terrainSize.z, _roadCurveAmplitude, _roadCurveFrequency, _usePerlinNoise, perlinSeed);
		}

		private Vector3 CalculateDeterministicRotation(System.Random random, float minRotationY, float maxRotationY, bool alignToRoad, float yawOffset, Vector2 spawnXZ, Vector3 terrainPosition, Vector3 terrainSize, int tileSeed)
		{
			if (!alignToRoad)
			{
				float y = (float)(random.NextDouble() * (double)(maxRotationY - minRotationY) + (double)minRotationY);
				return new Vector3(0f, y, 0f);
			}
			float num = terrainPosition.z + spawnXZ.y;
			float num2 = terrainPosition.x + spawnXZ.x;
			float num3 = CalculateRoadX(num, terrainPosition, terrainSize, tileSeed);
			float num4 = CalculateRoadX(num + 2f, terrainPosition, terrainSize, tileSeed) - CalculateRoadX(num - 2f, terrainPosition, terrainSize, tileSeed);
			float x = 4f;
			Vector2 vector = new Vector2(x, 0f - num4);
			if (vector.sqrMagnitude < 1E-06f)
			{
				vector = new Vector2(1f, 0f);
			}
			vector.Normalize();
			float num5 = Mathf.Sign(num2 - num3);
			if (num5 == 0f)
			{
				num5 = 1f;
			}
			Vector2 vector2 = (0f - num5) * vector;
			float y2 = Mathf.Atan2(vector2.x, vector2.y) * 57.29578f + yawOffset;
			return new Vector3(0f, y2, 0f);
		}

		private void RecordPoiSpawn(Vector2Int chunkCoord, string poiAssetGuid, POICategory category, Vector2 position, Vector3 rotationEuler, bool vegetationClearing, TerrainDeformMode deformMode, float deformRadius, float deformFalloff, float terrainHeightOffset, float foundationYOffset, float spawnTime, bool generateBranchRoad, bool placeNearRoad)
		{
			_spawnedPoIs[chunkCoord] = new SpawnedPoiRecord
			{
				ChunkCoord = chunkCoord,
				PoiAssetGuid = poiAssetGuid,
				Category = category,
				Position = position,
				RotationEuler = rotationEuler,
				VegetationClearing = vegetationClearing,
				DeformMode = deformMode,
				DeformRadius = deformRadius,
				DeformFalloff = deformFalloff,
				TerrainHeightOffset = terrainHeightOffset,
				FoundationYOffset = foundationYOffset,
				SpawnTime = spawnTime,
				GenerateBranchRoad = generateBranchRoad,
				PlaceNearRoad = placeNearRoad
			};
		}
	}
}
