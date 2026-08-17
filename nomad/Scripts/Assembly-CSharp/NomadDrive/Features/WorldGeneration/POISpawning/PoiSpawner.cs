using System;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public class PoiSpawner : MonoBehaviour
	{
		public async UniTask<Poi> SpawnPoiOnTerrain(AssetReferenceGameObject poiReference, TileInfo tileInfo, Vector2 spawnXZ, Vector3 rotationEuler, TerrainDeformMode? deformModeOverride = null, float? radiusOverride = null, float? falloffOverride = null, float? terrainHeightOffsetOverride = null, float? foundationYOffsetOverride = null, bool? vegetationCleaningOverride = null)
		{
			if (poiReference == null || string.IsNullOrEmpty(poiReference.AssetGUID))
			{
				EvilLogger.LogError("[PoiSpawner] Invalid POI reference - null or empty GUID", "SpawnPoiOnTerrain", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\POISpawning\\PoiSpawner.cs", 27);
				return null;
			}
			return await SpawnPoiOnTerrainInternal(poiReference.AssetGUID, tileInfo, spawnXZ, rotationEuler, deformModeOverride, radiusOverride, falloffOverride, terrainHeightOffsetOverride, foundationYOffsetOverride, vegetationCleaningOverride);
		}

		public async UniTask<Poi> SpawnPoiFromPlan(PoiSpawnPlan plan, TileInfo tileInfo)
		{
			TerrainDeformMode value = (plan.PlaceNearRoad ? TerrainDeformMode.None : plan.DeformMode);
			return await SpawnPoiOnTerrainInternal(plan.PoiAssetGuid, tileInfo, plan.SpawnXZ, plan.RotationEuler, value, plan.DeformRadius, plan.DeformFalloff, plan.TerrainHeightOffset, plan.FoundationYOffset, plan.VegetationCleaning);
		}

		public async UniTask<Poi> SpawnPoiOnTerrainByGuid(string poiGuid, TileInfo tileInfo, Vector2 spawnXZ, Vector3 rotationEuler, TerrainDeformMode? deformModeOverride = null, float? radiusOverride = null, float? falloffOverride = null, float? terrainHeightOffsetOverride = null, float? foundationYOffsetOverride = null, bool? vegetationCleaningOverride = null)
		{
			if (string.IsNullOrEmpty(poiGuid))
			{
				EvilLogger.LogError("[PoiSpawner] Invalid POI GUID - null or empty", "SpawnPoiOnTerrainByGuid", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\POISpawning\\PoiSpawner.cs", 77);
				return null;
			}
			return await SpawnPoiOnTerrainInternal(poiGuid, tileInfo, spawnXZ, rotationEuler, deformModeOverride, radiusOverride, falloffOverride, terrainHeightOffsetOverride, foundationYOffsetOverride, vegetationCleaningOverride);
		}

		private async UniTask<Poi> SpawnPoiOnTerrainInternal(string poiGuid, TileInfo tileInfo, Vector2 spawnXZ, Vector3 rotationEuler, TerrainDeformMode? deformModeOverride, float? radiusOverride, float? falloffOverride, float? terrainHeightOffsetOverride, float? foundationYOffsetOverride, bool? vegetationCleaningOverride)
		{
			GameObject gameObject = await LoadPrefabByGuid(poiGuid);
			if (gameObject == null)
			{
				return null;
			}
			Poi poiPrefab = gameObject.GetComponent<Poi>();
			if (poiPrefab == null)
			{
				EvilLogger.LogError("[PoiSpawner] Loaded prefab does not contain a Poi component.", "SpawnPoiOnTerrainInternal", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\POISpawning\\PoiSpawner.cs", 113);
				return null;
			}
			PoiDefinition component = gameObject.GetComponent<PoiDefinition>();
			TerrainDeformMode deformMode = deformModeOverride ?? component?.DefaultDeformMode ?? TerrainDeformMode.RaiseToMax;
			float radius = radiusOverride ?? component?.DefaultDeformRadius ?? 10f;
			float falloff = falloffOverride ?? component?.DefaultDeformFalloff ?? 0.3f;
			float terrainHeightOffset = terrainHeightOffsetOverride ?? component?.DefaultTerrainHeightOffset ?? 0.05f;
			float foundationYOffset = foundationYOffsetOverride ?? component?.DefaultFoundationYOffset ?? 0.1f;
			bool flag = vegetationCleaningOverride ?? component?.VegetationCleaning ?? true;
			Terrain terrain = tileInfo.tile.GetTerrain(isDraft: false);
			if (terrain == null)
			{
				return null;
			}
			Vector3 terrainPosition = terrain.transform.position;
			Vector3 spawnPosition = terrainPosition + new Vector3(spawnXZ.x, 0f, spawnXZ.y);
			Bounds poiBounds = GetBoundsFromPrefab(gameObject);
			if (flag)
			{
				await terrain.ClearVegetationAsync(spawnPosition, radius);
			}
			switch (deformMode)
			{
			case TerrainDeformMode.FlattenToCenter:
			{
				float num4 = terrain.SampleHeight(spawnPosition);
				spawnPosition.y = terrainPosition.y + num4;
				await terrain.FlattenAtPositionAsync(spawnPosition, radius, falloff);
				break;
			}
			case TerrainDeformMode.RaiseToMax:
			{
				float num3 = terrain.SampleMaxHeightInBounds(spawnPosition, poiBounds.size.x, poiBounds.size.z);
				spawnPosition.y = terrainPosition.y + num3;
				await terrain.RaiseToHeightAsync(spawnPosition, num3, radius, falloff);
				break;
			}
			case TerrainDeformMode.Foundation:
			{
				float num2 = terrain.SampleMaxHeightInBounds(spawnPosition, poiBounds.size.x, poiBounds.size.z);
				spawnPosition.y = terrainPosition.y + num2;
				float platformRadius = radius * 0.6f;
				await terrain.CreateFoundationAsync(spawnPosition, platformRadius, radius, num2);
				spawnPosition.y += foundationYOffset;
				break;
			}
			case TerrainDeformMode.None:
			{
				float num = terrain.SampleHeight(spawnPosition);
				spawnPosition.y = terrainPosition.y + num;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("deformMode", deformMode, null);
			}
			spawnPosition.y -= terrainHeightOffset;
			Quaternion rotation = Quaternion.Euler(rotationEuler);
			AsyncInstantiateOperation.SetIntegrationTimeMS(1f);
			AsyncInstantiateOperation<Poi> asyncOp = UnityEngine.Object.InstantiateAsync(poiPrefab, spawnPosition, rotation);
			await asyncOp;
			Poi poi = asyncOp.Result[0];
			FloatingOriginManager instance = FloatingOriginManager.Instance;
			if (instance != null && instance.WorldContentRoot != null)
			{
				poi.transform.SetParent(instance.WorldContentRoot, worldPositionStays: true);
			}
			return poi;
		}

		private async UniTask<GameObject> LoadPrefabByGuid(string guid)
		{
			return await WorldPrefabCache.GetOrLoadAsync(guid);
		}

		private static Bounds GetBoundsFromPrefab(GameObject prefab)
		{
			if (prefab == null)
			{
				return new Bounds(Vector3.zero, Vector3.one * 10f);
			}
			Renderer[] componentsInChildren = prefab.GetComponentsInChildren<Renderer>();
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				Bounds localBounds = componentsInChildren[0].localBounds;
				for (int i = 1; i < componentsInChildren.Length; i++)
				{
					localBounds.Encapsulate(componentsInChildren[i].localBounds);
				}
				return localBounds;
			}
			Collider[] componentsInChildren2 = prefab.GetComponentsInChildren<Collider>();
			if (componentsInChildren2 != null && componentsInChildren2.Length != 0)
			{
				Bounds bounds = componentsInChildren2[0].bounds;
				for (int j = 1; j < componentsInChildren2.Length; j++)
				{
					bounds.Encapsulate(componentsInChildren2[j].bounds);
				}
				return bounds;
			}
			return new Bounds(Vector3.zero, Vector3.one * 10f);
		}
	}
}
