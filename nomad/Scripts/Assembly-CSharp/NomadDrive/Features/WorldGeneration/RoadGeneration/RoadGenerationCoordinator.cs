using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Den.Tools;
using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.EvilRoads;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.WorldGeneration.POISpawning;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.RoadGeneration
{
	public class RoadGenerationCoordinator
	{
		private readonly EvilRoadsManager _evilRoadsManager;

		private readonly RoadGenerationConfig _roadGenerationConfig;

		private readonly SeedManager _seedManager;

		private readonly Dictionary<Coord, TileInfo> _activeTileInfosDict;

		private readonly ChunkPoiManager _chunkPoiManager;

		private readonly RoadBranchPlanner _branchPlanner;

		private Transform _lazyLoadingReferenceObject;

		private float _lastReferenceObjectSearchTime;

		public RoadGenerationCoordinator(EvilRoadsManager evilRoadsManager, RoadGenerationConfig roadGenerationConfig, SeedManager seedManager, Dictionary<Coord, TileInfo> activeTileInfosDict, ChunkPoiManager chunkPoiManager)
		{
			_evilRoadsManager = evilRoadsManager;
			_roadGenerationConfig = roadGenerationConfig;
			_seedManager = seedManager;
			_activeTileInfosDict = activeTileInfosDict;
			_chunkPoiManager = chunkPoiManager;
			_branchPlanner = new RoadBranchPlanner(evilRoadsManager, roadGenerationConfig, seedManager);
		}

		public async UniTask CreateRoadAsync(TileInfo tileInfo, bool isRegenerating = false)
		{
			Vector2Int vector2Int = new Vector2Int(tileInfo.tile.coord.x, tileInfo.tile.coord.z);
			Terrain terrain = tileInfo.tile.GetTerrain(isDraft: false);
			if (terrain == null)
			{
				EvilLogger.LogError($"<color=red>[RoadCoordinator]</color> Terrain not found for tile at {vector2Int}", "CreateRoadAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\RoadGeneration\\RoadGenerationCoordinator.cs", 66);
				return;
			}
			bool useSimplifiedCreation = false;
			if (_evilRoadsManager != null && _evilRoadsManager.IsLazyLoadingEnabled())
			{
				float lazyLoadingDistance = _evilRoadsManager.GetLazyLoadingDistance();
				_evilRoadsManager.GetLazyLoadingReferenceTag();
				float num = CalculateDistanceToReference(terrain);
				if (num >= 0f)
				{
					tileInfo.distanceToCamera = num;
					if (num > lazyLoadingDistance)
					{
						useSimplifiedCreation = true;
					}
				}
				else
				{
					useSimplifiedCreation = false;
				}
			}
			Coord key = new Coord(tileInfo.tile.coord.x, tileInfo.tile.coord.z - 1);
			Coord key2 = new Coord(tileInfo.tile.coord.x, tileInfo.tile.coord.z + 1);
			float? startX = null;
			float? endX = null;
			if (_activeTileInfosDict.TryGetValue(key, out var backwardTile) && backwardTile.hasRoadEndPoint)
			{
				startX = backwardTile.roadEndPointX;
			}
			if (_activeTileInfosDict.TryGetValue(key2, out var forwardTile) && forwardTile.hasRoadStartPoint)
			{
				endX = forwardTile.roadStartPointX;
			}
			List<SpawnedPoiRecord> spawnedPoiRecords = _chunkPoiManager.GetAllSpawnedPoiRecorsByTileCoord(vector2Int);
			PoiRoadInfo[] array = new PoiRoadInfo[spawnedPoiRecords.Count];
			for (int i = 0; i < spawnedPoiRecords.Count; i++)
			{
				array[i] = new PoiRoadInfo
				{
					pos = spawnedPoiRecords[i].Position,
					radiusOffset = 0f - spawnedPoiRecords[i].DeformRadius
				};
			}
			Vector3 renderPos = terrain.transform.position + new Vector3(terrain.terrainData.size.x * 0.5f, 0f, terrain.terrainData.size.z * 0.5f);
			int positionBasedSeed = _seedManager.GetPositionBasedSeed(FloatingOriginManager.ToTrueWorld(renderPos));
			RoadGenerator roadGenerator = new RoadGenerator(_roadGenerationConfig, _evilRoadsManager, positionBasedSeed, _seedManager.GetSubSeed("RoadMeander"));
			EvilRoad evilRoad;
			if (useSimplifiedCreation)
			{
				evilRoad = await roadGenerator.CreateRoadWithBidirectionalContinuityAsync(array, terrain, startX, endX, skipTerrainDeformation: true);
				tileInfo.isRoadSimplified = true;
			}
			else
			{
				evilRoad = await roadGenerator.CreateRoadWithBidirectionalContinuityAsync(array, terrain, startX, endX);
				tileInfo.isRoadSimplified = false;
			}
			if (!(evilRoad != null))
			{
				return;
			}
			tileInfo.isRoadGenerated = true;
			tileInfo.road = evilRoad;
			Vector3 startPoint = evilRoad.GetStartPoint();
			Vector3 endPoint = evilRoad.GetEndPoint();
			tileInfo.roadStartPointX = startPoint.x;
			tileInfo.roadEndPointX = endPoint.x;
			tileInfo.hasRoadStartPoint = true;
			tileInfo.hasRoadEndPoint = true;
			if (!useSimplifiedCreation && !isRegenerating && tileInfo.branchRoads.Count == 0)
			{
				_branchPlanner.BuildBranchesAsync(tileInfo, evilRoad, spawnedPoiRecords, terrain).Forget();
			}
			if (isRegenerating)
			{
				return;
			}
			if (forwardTile != null && forwardTile.isRoadGenerated)
			{
				float roadEndPointX = tileInfo.roadEndPointX;
				float roadStartPointX = forwardTile.roadStartPointX;
				if (Mathf.Abs(roadEndPointX - roadStartPointX) > 0.1f)
				{
					DestroyRoad(forwardTile);
					await CreateRoadAsync(forwardTile, isRegenerating: true);
				}
			}
			if (backwardTile != null && backwardTile.isRoadGenerated)
			{
				float roadStartPointX2 = tileInfo.roadStartPointX;
				float roadEndPointX2 = backwardTile.roadEndPointX;
				if (Mathf.Abs(roadStartPointX2 - roadEndPointX2) > 0.1f)
				{
					DestroyRoad(backwardTile);
					await CreateRoadAsync(backwardTile, isRegenerating: true);
				}
			}
		}

		public void DestroyRoad(TileInfo tileInfo)
		{
			if (tileInfo.road != null)
			{
				_evilRoadsManager.DestroyRoad(tileInfo.road);
				tileInfo.road = null;
			}
			if (tileInfo.branchRoads.Count > 0)
			{
				foreach (EvilRoad branchRoad in tileInfo.branchRoads)
				{
					if (branchRoad != null)
					{
						_evilRoadsManager.DestroyRoad(branchRoad);
					}
				}
				tileInfo.branchRoads.Clear();
			}
			tileInfo.isRoadGenerated = false;
			tileInfo.isRoadSimplified = false;
			tileInfo.hasRoadStartPoint = false;
			tileInfo.hasRoadEndPoint = false;
			tileInfo.roadStartPointX = 0f;
			tileInfo.roadEndPointX = 0f;
		}

		public IEnumerator DynamicRoadUpgradeCoroutine()
		{
			float upgradeCheckInterval = _evilRoadsManager.GetUpgradeCheckInterval();
			WaitForSeconds waitInterval = new WaitForSeconds(upgradeCheckInterval);
			while (true)
			{
				yield return waitInterval;
				if (_evilRoadsManager == null || !_evilRoadsManager.IsDynamicUpgradeEnabled() || !_evilRoadsManager.IsLazyLoadingEnabled() || GetLazyLoadingReferenceObject() == null)
				{
					continue;
				}
				float lazyLoadingDistance = _evilRoadsManager.GetLazyLoadingDistance();
				float upgradeDistanceThreshold = _evilRoadsManager.GetUpgradeDistanceThreshold();
				float num = lazyLoadingDistance * upgradeDistanceThreshold;
				int num2 = 0;
				foreach (KeyValuePair<Coord, TileInfo> item in _activeTileInfosDict)
				{
					TileInfo value = item.Value;
					if (!value.isRoadSimplified || value.road == null || !value.isRoadGenerated)
					{
						continue;
					}
					Terrain terrain = value.tile.GetTerrain(isDraft: false);
					if (!(terrain == null))
					{
						float num3 = CalculateDistanceToReference(terrain);
						if (!(num3 < 0f) && num3 < num)
						{
							UpgradeRoadToFull(value).Forget();
							num2++;
						}
					}
				}
				if (num2 <= 0)
				{
				}
			}
		}

		public async UniTask UpgradeRoadToFull(TileInfo tileInfo)
		{
			if (!tileInfo.isRoadSimplified || tileInfo.road == null)
			{
				return;
			}
			Vector2Int coord2D = new Vector2Int(tileInfo.tile.coord.x, tileInfo.tile.coord.z);
			Terrain terrain = tileInfo.tile.GetTerrain(isDraft: false);
			if (terrain == null)
			{
				EvilLogger.LogError($"<color=red>[RoadCoordinator.Upgrade]</color> Terrain not found for tile at {coord2D}", "UpgradeRoadToFull", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\RoadGeneration\\RoadGenerationCoordinator.cs", 371);
				return;
			}
			if (tileInfo.road != null)
			{
				_evilRoadsManager.DestroyRoad(tileInfo.road);
				tileInfo.road = null;
				tileInfo.isRoadGenerated = false;
				tileInfo.isRoadSimplified = false;
			}
			Coord key = new Coord(tileInfo.tile.coord.x, tileInfo.tile.coord.z - 1);
			Coord key2 = new Coord(tileInfo.tile.coord.x, tileInfo.tile.coord.z + 1);
			float? startX = null;
			float? endX = null;
			if (_activeTileInfosDict.TryGetValue(key, out var value) && value.hasRoadEndPoint)
			{
				startX = value.roadEndPointX;
			}
			if (_activeTileInfosDict.TryGetValue(key2, out var value2) && value2.hasRoadStartPoint)
			{
				endX = value2.roadStartPointX;
			}
			List<SpawnedPoiRecord> spawnedPoiRecords = _chunkPoiManager.GetAllSpawnedPoiRecorsByTileCoord(coord2D);
			PoiRoadInfo[] array = new PoiRoadInfo[spawnedPoiRecords.Count];
			for (int i = 0; i < spawnedPoiRecords.Count; i++)
			{
				array[i] = new PoiRoadInfo
				{
					pos = spawnedPoiRecords[i].Position,
					radiusOffset = 0f - spawnedPoiRecords[i].DeformRadius
				};
			}
			Vector3 renderPos = terrain.transform.position + new Vector3(terrain.terrainData.size.x * 0.5f, 0f, terrain.terrainData.size.z * 0.5f);
			int positionBasedSeed = _seedManager.GetPositionBasedSeed(FloatingOriginManager.ToTrueWorld(renderPos));
			EvilRoad evilRoad = await new RoadGenerator(_roadGenerationConfig, _evilRoadsManager, positionBasedSeed, _seedManager.GetSubSeed("RoadMeander")).CreateRoadWithBidirectionalContinuityAsync(array, terrain, startX, endX);
			if (evilRoad != null)
			{
				tileInfo.isRoadGenerated = true;
				tileInfo.road = evilRoad;
				tileInfo.isRoadSimplified = false;
				Vector3 startPoint = evilRoad.GetStartPoint();
				Vector3 endPoint = evilRoad.GetEndPoint();
				tileInfo.roadStartPointX = startPoint.x;
				tileInfo.roadEndPointX = endPoint.x;
				tileInfo.hasRoadStartPoint = true;
				tileInfo.hasRoadEndPoint = true;
				if (tileInfo.branchRoads.Count == 0)
				{
					_branchPlanner.BuildBranchesAsync(tileInfo, evilRoad, spawnedPoiRecords, terrain).Forget();
				}
			}
			else
			{
				EvilLogger.LogError($"<color=red>[RoadCoordinator.Upgrade]</color> Failed to upgrade road at tile ({coord2D.x}, {coord2D.y})", "UpgradeRoadToFull", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\RoadGeneration\\RoadGenerationCoordinator.cs", 463);
			}
		}

		private Transform GetLazyLoadingReferenceObject()
		{
			if (_lazyLoadingReferenceObject != null)
			{
				return _lazyLoadingReferenceObject;
			}
			float time = Time.time;
			if (time - _lastReferenceObjectSearchTime < 2f)
			{
				return null;
			}
			_lastReferenceObjectSearchTime = time;
			string text = ((_evilRoadsManager != null) ? _evilRoadsManager.GetLazyLoadingReferenceTag() : "Player");
			if (!string.IsNullOrEmpty(text))
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag(text);
				if (gameObject != null)
				{
					_lazyLoadingReferenceObject = gameObject.transform;
					return _lazyLoadingReferenceObject;
				}
			}
			return null;
		}

		private float CalculateDistanceToReference(Terrain terrain)
		{
			Transform lazyLoadingReferenceObject = GetLazyLoadingReferenceObject();
			if (lazyLoadingReferenceObject == null)
			{
				return -1f;
			}
			Vector3 vector = terrain.transform.position + new Vector3(terrain.terrainData.size.x * 0.5f, 0f, terrain.terrainData.size.z * 0.5f);
			Vector3 a = new Vector3(lazyLoadingReferenceObject.position.x, 0f, lazyLoadingReferenceObject.position.z);
			Vector3 b = new Vector3(vector.x, 0f, vector.z);
			return Vector3.Distance(a, b);
		}
	}
}
