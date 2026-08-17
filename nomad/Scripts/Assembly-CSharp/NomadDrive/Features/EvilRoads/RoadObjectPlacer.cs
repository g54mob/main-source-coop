using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using EvilCore.Extensions;
using NomadDrive.Features.EvilRoads.Cable;
using UnityEngine;
using UnityEngine.Splines;

namespace NomadDrive.Features.EvilRoads
{
	public class RoadObjectPlacer
	{
		private struct PlacementData
		{
			public Vector3 position;

			public Vector3 tangent;

			public float splineT;

			public GameObject prefab;

			public bool flipFacing;

			public PlacementData(Vector3 position, Vector3 tangent, float splineT = 0f, GameObject prefab = null, bool flipFacing = false)
			{
				this.position = position;
				this.tangent = tangent;
				this.splineT = splineT;
				this.prefab = prefab;
				this.flipFacing = flipFacing;
			}
		}

		private readonly EvilRoadConfig _roadConfig;

		private readonly Terrain _associatedTerrain;

		private readonly ICableManager _cableManager;

		public RoadObjectPlacer(EvilRoadConfig roadConfig, Terrain associatedTerrain = null, ICableManager cableManager = null)
		{
			_roadConfig = roadConfig;
			_associatedTerrain = associatedTerrain;
			_cableManager = cableManager;
		}

		public void PlaceRoadObjects(Spline spline, Transform transform, GameObject roadGameObject)
		{
		}

		public List<GameObject> PlaceRoadObjectsForConfig(Spline spline, RoadObjectConfig roadObjectConfig, Transform transform, GameObject roadGameObject)
		{
			List<GameObject> list = new List<GameObject>();
			if (spline == null || roadObjectConfig == null)
			{
				return list;
			}
			bool flag = roadObjectConfig.distributionMode == ObjectDistributionMode.RandomScattered;
			if (flag)
			{
				List<RoadObjectVariant> randomVariants = roadObjectConfig.randomVariants;
				if (randomVariants != null && randomVariants.Count > 0)
				{
					goto IL_004c;
				}
			}
			if (roadObjectConfig.roadObject == null)
			{
				return list;
			}
			goto IL_004c;
			IL_004c:
			foreach (PlacementData item in CalculateObjectPlacementPositions(spline, roadObjectConfig, transform))
			{
				GameObject gameObject = PlaceRoadObjectAndReturn(roadObjectConfig, item, transform, roadGameObject, item.prefab);
				if (gameObject != null)
				{
					list.Add(gameObject);
				}
			}
			if (!flag && roadObjectConfig.enableCableConnections && roadObjectConfig.cableConfig != null && list.Count >= 2)
			{
				GameObject gameObject2 = CreateCableConnections(roadObjectConfig, list, roadGameObject);
				if (gameObject2 != null)
				{
					list.Add(gameObject2);
				}
			}
			return list;
		}

		public async UniTask<List<GameObject>> PlaceRoadObjectsForConfigAsync(Spline spline, RoadObjectConfig roadObjectConfig, Transform transform, GameObject roadGameObject)
		{
			List<GameObject> placedObjects = new List<GameObject>();
			if (spline == null || roadObjectConfig == null)
			{
				return placedObjects;
			}
			bool isScatter = roadObjectConfig.distributionMode == ObjectDistributionMode.RandomScattered;
			int num;
			if (isScatter)
			{
				List<RoadObjectVariant> randomVariants = roadObjectConfig.randomVariants;
				num = ((randomVariants != null && randomVariants.Count > 0) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num == 0 && roadObjectConfig.roadObject == null)
			{
				return placedObjects;
			}
			List<PlacementData> list = CalculateObjectPlacementPositions(spline, roadObjectConfig, transform);
			foreach (PlacementData item in list)
			{
				if (!(transform == null) && !(roadGameObject == null))
				{
					PlacementData data = item;
					GameObject gameObject = await MainThreadWorkBudget.RunInstantiate(() => PlaceRoadObjectAndReturn(roadObjectConfig, data, transform, roadGameObject, data.prefab), WorkPriority.High);
					if (gameObject != null)
					{
						placedObjects.Add(gameObject);
					}
					continue;
				}
				break;
			}
			if (!isScatter && roadObjectConfig.enableCableConnections && roadObjectConfig.cableConfig != null && placedObjects.Count >= 2)
			{
				GameObject gameObject2 = await CreateCableConnectionsAsync(roadObjectConfig, placedObjects, roadGameObject);
				if (gameObject2 != null)
				{
					placedObjects.Add(gameObject2);
				}
			}
			return placedObjects;
		}

		private GameObject CreateCableConnections(RoadObjectConfig roadObjectConfig, List<GameObject> placedObjects, GameObject roadGameObject)
		{
			GameObject gameObject = new GameObject(roadObjectConfig.name + "_Cables");
			if (roadGameObject != null)
			{
				gameObject.transform.SetParent(roadGameObject.transform);
			}
			EvilRoad ownerRoad = roadGameObject?.GetComponentInParent<EvilRoad>();
			if (roadObjectConfig.placementMode == RoadObjectPlacementMode.Symmetric)
			{
				List<GameObject> list = placedObjects.Where((GameObject obj, int i) => i % 2 == 0).ToList();
				List<GameObject> list2 = placedObjects.Where((GameObject obj, int i) => i % 2 == 1).ToList();
				if (list.Count >= 2)
				{
					CableConnectionHandler.CreateCableConnections(list, roadObjectConfig.cableConfig, _cableManager, gameObject.transform);
					RegisterEdgeObjects(list, roadObjectConfig, ownerRoad, isLeftSide: true);
				}
				if (list2.Count >= 2)
				{
					CableConnectionHandler.CreateCableConnections(list2, roadObjectConfig.cableConfig, _cableManager, gameObject.transform);
					RegisterEdgeObjects(list2, roadObjectConfig, ownerRoad, isLeftSide: false);
				}
			}
			else
			{
				CableConnectionHandler.CreateCableConnections(placedObjects, roadObjectConfig.cableConfig, _cableManager, gameObject.transform);
				RegisterEdgeObjects(placedObjects, roadObjectConfig, ownerRoad, isLeftSide: true);
			}
			return gameObject;
		}

		private async UniTask<GameObject> CreateCableConnectionsAsync(RoadObjectConfig roadObjectConfig, List<GameObject> placedObjects, GameObject roadGameObject)
		{
			GameObject cableContainer = new GameObject(roadObjectConfig.name + "_Cables");
			if (roadGameObject != null)
			{
				cableContainer.transform.SetParent(roadGameObject.transform);
			}
			EvilRoad ownerRoad = roadGameObject?.GetComponentInParent<EvilRoad>();
			if (roadObjectConfig.placementMode == RoadObjectPlacementMode.Symmetric)
			{
				List<GameObject> leftSideObjects = placedObjects.Where((GameObject obj, int i) => i % 2 == 0).ToList();
				List<GameObject> rightSideObjects = placedObjects.Where((GameObject obj, int i) => i % 2 == 1).ToList();
				if (leftSideObjects.Count >= 2)
				{
					await CableConnectionHandler.CreateCableConnectionsAsync(leftSideObjects, roadObjectConfig.cableConfig, _cableManager, cableContainer.transform);
					RegisterEdgeObjects(leftSideObjects, roadObjectConfig, ownerRoad, isLeftSide: true);
				}
				if (rightSideObjects.Count >= 2)
				{
					await CableConnectionHandler.CreateCableConnectionsAsync(rightSideObjects, roadObjectConfig.cableConfig, _cableManager, cableContainer.transform);
					RegisterEdgeObjects(rightSideObjects, roadObjectConfig, ownerRoad, isLeftSide: false);
				}
			}
			else
			{
				await CableConnectionHandler.CreateCableConnectionsAsync(placedObjects, roadObjectConfig.cableConfig, _cableManager, cableContainer.transform);
				RegisterEdgeObjects(placedObjects, roadObjectConfig, ownerRoad, isLeftSide: true);
			}
			return cableContainer;
		}

		private void RegisterEdgeObjects(List<GameObject> objects, RoadObjectConfig config, EvilRoad ownerRoad, bool isLeftSide)
		{
			if (_cableManager == null || objects == null || objects.Count < 2 || config?.cableConfig == null)
			{
				return;
			}
			GameObject gameObject = objects[0];
			if (gameObject != null)
			{
				CableConnectionPoints component = gameObject.GetComponent<CableConnectionPoints>();
				if (component != null)
				{
					_cableManager.RegisterEdgeObject(new EdgeObjectInfo
					{
						Object = gameObject,
						Points = component,
						Config = config.cableConfig,
						OwnerRoad = ownerRoad,
						IsExit = false,
						IsLeftSide = isLeftSide
					});
				}
			}
			GameObject gameObject2 = objects[objects.Count - 1];
			if (gameObject2 != null)
			{
				CableConnectionPoints component2 = gameObject2.GetComponent<CableConnectionPoints>();
				if (component2 != null)
				{
					_cableManager.RegisterEdgeObject(new EdgeObjectInfo
					{
						Object = gameObject2,
						Points = component2,
						Config = config.cableConfig,
						OwnerRoad = ownerRoad,
						IsExit = true,
						IsLeftSide = isLeftSide
					});
				}
			}
		}

		private List<PlacementData> CalculateObjectPlacementPositions(Spline spline, RoadObjectConfig roadObjectConfig, Transform transform)
		{
			List<PlacementData> list = new List<PlacementData>();
			switch (roadObjectConfig.distributionMode)
			{
			case ObjectDistributionMode.AtSplinePoint:
				CalculateSinglePointPlacement(spline, roadObjectConfig, list);
				break;
			case ObjectDistributionMode.RandomScattered:
				CalculateScatteredPlacement(spline, roadObjectConfig, transform, list);
				break;
			default:
				CalculateRepetitivePlacement(spline, roadObjectConfig, list);
				break;
			}
			return list;
		}

		private void CalculateSinglePointPlacement(Spline spline, RoadObjectConfig roadObjectConfig, List<PlacementData> placementPositions)
		{
			float singleObjectPosition = roadObjectConfig.singleObjectPosition;
			Vector3 vector = spline.EvaluatePosition(singleObjectPosition);
			Vector3 vector2 = spline.EvaluateTangent(singleObjectPosition);
			if (!RoadHeightCalculator.HasNaN(vector) && !RoadHeightCalculator.HasNaN(vector2))
			{
				Vector3 right = CalculateRightVector(vector2);
				CalculatePlacementWithRightVector(vector, right, roadObjectConfig, placementPositions, vector2, singleObjectPosition);
			}
		}

		private void CalculateRepetitivePlacement(Spline spline, RoadObjectConfig roadObjectConfig, List<PlacementData> placementPositions)
		{
			if (roadObjectConfig.offsetBetweenOtherObjects <= 0f)
			{
				return;
			}
			float length = spline.GetLength();
			float num = 0f;
			int num2 = Mathf.FloorToInt((length - num) / roadObjectConfig.offsetBetweenOtherObjects);
			for (int i = 0; i < num2; i++)
			{
				float num3 = (num + (float)i * roadObjectConfig.offsetBetweenOtherObjects) / length;
				if (!(num3 > 1f))
				{
					Vector3 vector = spline.EvaluatePosition(num3);
					Vector3 vector2 = spline.EvaluateTangent(num3);
					if (!RoadHeightCalculator.HasNaN(vector) && !RoadHeightCalculator.HasNaN(vector2))
					{
						Vector3 right = CalculateRightVector(vector2);
						CalculatePlacementWithRightVector(vector, right, roadObjectConfig, placementPositions, vector2, num3);
					}
					continue;
				}
				break;
			}
		}

		private void CalculateScatteredPlacement(Spline spline, RoadObjectConfig roadObjectConfig, Transform transform, List<PlacementData> placementPositions)
		{
			if (roadObjectConfig.randomVariants == null)
			{
				return;
			}
			List<RoadObjectVariant> list = roadObjectConfig.randomVariants.Where((RoadObjectVariant v) => v != null && v.prefab != null).ToList();
			if (list.Count == 0)
			{
				return;
			}
			float length = spline.GetLength();
			if (length <= 0f)
			{
				return;
			}
			float num = Mathf.Max(0.1f, roadObjectConfig.minSpacing);
			float num2 = Mathf.Max(num, roadObjectConfig.maxSpacing);
			float num3 = Mathf.Clamp(roadObjectConfig.startMargin, 0f, length * 0.49f);
			float num4 = length - num3;
			if (num4 <= num3)
			{
				return;
			}
			System.Random random = CreateScatterRandom(spline, transform, roadObjectConfig.placementSeedSalt);
			int num5 = Mathf.Clamp(roadObjectConfig.noRepeatWindow, 0, Mathf.Max(0, list.Count - 1));
			Queue<int> queue = new Queue<int>();
			for (float num6 = num3 + (float)random.NextDouble() * num; num6 <= num4; num6 += num + (float)random.NextDouble() * (num2 - num))
			{
				float num7 = num6 / length;
				Vector3 vector = spline.EvaluatePosition(num7);
				Vector3 vector2 = spline.EvaluateTangent(num7);
				if (!RoadHeightCalculator.HasNaN(vector) && !RoadHeightCalculator.HasNaN(vector2))
				{
					int num8 = PickVariantIndex(list, queue, num5, random);
					GameObject prefab = list[num8].prefab;
					Vector3 vector3 = CalculateRightVector(vector2);
					if (roadObjectConfig.placementMode == RoadObjectPlacementMode.Symmetric)
					{
						Vector3 alongRoadOffset = GetAlongRoadOffset(vector2, roadObjectConfig.symmetricAlongRoadOffset);
						Vector3 position = vector + vector3 * roadObjectConfig.distanceToRoadOrigin;
						Vector3 position2 = vector - vector3 * roadObjectConfig.distanceToRoadOrigin + alongRoadOffset;
						placementPositions.Add(new PlacementData(position, vector2, num7, prefab, roadObjectConfig.flipRotationOnOppositeSide));
						placementPositions.Add(new PlacementData(position2, vector2, num7, prefab));
					}
					else
					{
						Vector3 position3 = vector + vector3 * roadObjectConfig.distanceToRoadOrigin;
						placementPositions.Add(new PlacementData(position3, vector2, num7, prefab));
					}
					queue.Enqueue(num8);
					while (queue.Count > num5)
					{
						queue.Dequeue();
					}
				}
			}
		}

		private int PickVariantIndex(List<RoadObjectVariant> variants, Queue<int> recent, int window, System.Random rng)
		{
			List<int> list = new List<int>(variants.Count);
			for (int i = 0; i < variants.Count; i++)
			{
				if (!recent.Contains(i))
				{
					list.Add(i);
				}
			}
			if (list.Count == 0)
			{
				for (int j = 0; j < variants.Count; j++)
				{
					list.Add(j);
				}
			}
			double num = 0.0;
			foreach (int item in list)
			{
				num += (double)Mathf.Max(0.0001f, variants[item].weight);
			}
			double num2 = rng.NextDouble() * num;
			foreach (int item2 in list)
			{
				num2 -= (double)Mathf.Max(0.0001f, variants[item2].weight);
				if (num2 < 0.0)
				{
					return item2;
				}
			}
			return list[list.Count - 1];
		}

		private System.Random CreateScatterRandom(Spline spline, Transform transform, int salt)
		{
			Vector3 vector = spline.EvaluatePosition(0f);
			Vector3 obj = ((transform != null) ? transform.TransformPoint(vector) : vector);
			int num = Mathf.RoundToInt(obj.x * 10f);
			int num2 = Mathf.RoundToInt(obj.y * 10f);
			int num3 = Mathf.RoundToInt(obj.z * 10f);
			return new System.Random((((17 * 31 + num) * 31 + num2) * 31 + num3) * 31 + salt);
		}

		private void CalculatePlacementWithRightVector(Vector3 splinePosition, Vector3 right, RoadObjectConfig roadObjectConfig, List<PlacementData> placementPositions, Vector3 tangent, float splineT)
		{
			if (roadObjectConfig.placementMode == RoadObjectPlacementMode.OneSide)
			{
				Vector3 position = splinePosition + right * roadObjectConfig.distanceToRoadOrigin;
				placementPositions.Add(new PlacementData(position, tangent, splineT));
				return;
			}
			Vector3 alongRoadOffset = GetAlongRoadOffset(tangent, roadObjectConfig.symmetricAlongRoadOffset);
			Vector3 position2 = splinePosition + right * roadObjectConfig.distanceToRoadOrigin;
			Vector3 position3 = splinePosition - right * roadObjectConfig.distanceToRoadOrigin + alongRoadOffset;
			placementPositions.Add(new PlacementData(position2, tangent, splineT, null, roadObjectConfig.flipRotationOnOppositeSide));
			placementPositions.Add(new PlacementData(position3, tangent, splineT));
		}

		private void PlaceRoadObject(RoadObjectConfig roadObjectConfig, PlacementData placementData, Transform transform, GameObject roadGameObject)
		{
			PlaceRoadObjectAndReturn(roadObjectConfig, placementData, transform, roadGameObject);
		}

		private GameObject PlaceRoadObjectAndReturn(RoadObjectConfig roadObjectConfig, PlacementData placementData, Transform transform, GameObject roadGameObject, GameObject prefabOverride = null)
		{
			GameObject gameObject = ((prefabOverride != null) ? prefabOverride : roadObjectConfig.roadObject);
			if (gameObject == null)
			{
				return null;
			}
			if (transform == null)
			{
				return null;
			}
			Vector3 worldPosition = transform.TransformPoint(placementData.position);
			Vector3 vector = CalculateFinalPosition(worldPosition, roadObjectConfig);
			Quaternion rotation = CalculateFinalRotation(placementData.tangent, roadObjectConfig, vector, placementData.flipFacing);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, vector, rotation);
			if (roadGameObject != null)
			{
				gameObject2.transform.SetParent(roadGameObject.transform);
			}
			RoadObjectIdentifier roadObjectIdentifier = gameObject2.AddComponent<RoadObjectIdentifier>();
			roadObjectIdentifier.sourceConfig = roadObjectConfig;
			roadObjectIdentifier.splineT = placementData.splineT;
			return gameObject2;
		}

		private Vector3 CalculateFinalPosition(Vector3 worldPosition, RoadObjectConfig roadObjectConfig)
		{
			switch (roadObjectConfig.snappingMode)
			{
			case RoadObjectSnappingMode.SnapToTerrain:
				if (_associatedTerrain != null)
				{
					float terrainHeight2 = RoadHeightCalculator.GetTerrainHeight(worldPosition, _associatedTerrain);
					return new Vector3(worldPosition.x, terrainHeight2, worldPosition.z);
				}
				return worldPosition;
			case RoadObjectSnappingMode.SnapToRoad:
				return worldPosition;
			case RoadObjectSnappingMode.Custom:
				if (_associatedTerrain != null)
				{
					float terrainHeight = RoadHeightCalculator.GetTerrainHeight(worldPosition, _associatedTerrain);
					return new Vector3(worldPosition.x, terrainHeight + roadObjectConfig.customHeight, worldPosition.z);
				}
				return new Vector3(worldPosition.x, worldPosition.y + roadObjectConfig.customHeight, worldPosition.z);
			default:
				return worldPosition;
			}
		}

		private Quaternion CalculateFinalRotation(Vector3 tangent, RoadObjectConfig roadObjectConfig, Vector3 finalPosition, bool flipFacing = false)
		{
			Vector3 vector = CalculateSurfaceNormal(finalPosition, roadObjectConfig.snappingMode, tangent, roadObjectConfig.alignToSurfaceNormal);
			Quaternion quaternion2 = Quaternion.identity;
			switch (roadObjectConfig.rotationMode)
			{
			case RoadObjectRotationMode.TowardsRoad:
				if (tangent != Vector3.zero)
				{
					quaternion2 = Quaternion.LookRotation(tangent, vector);
				}
				break;
			case RoadObjectRotationMode.CustomRotation:
				if (roadObjectConfig.alignToSurfaceNormal && vector != Vector3.up)
				{
					Vector3 normalized = Vector3.ProjectOnPlane(Vector3.forward, vector).normalized;
					if (normalized.sqrMagnitude < 0.001f)
					{
						normalized = Vector3.ProjectOnPlane(Vector3.right, vector).normalized;
					}
					quaternion2 = Quaternion.LookRotation(Quaternion.AngleAxis(roadObjectConfig.customRotationAngle, vector) * normalized, vector);
				}
				else
				{
					quaternion2 = Quaternion.Euler(0f, roadObjectConfig.customRotationAngle, 0f);
				}
				break;
			}
			if (flipFacing)
			{
				Vector3 axis = ((vector.sqrMagnitude > 0.001f) ? vector : Vector3.up);
				quaternion2 = Quaternion.AngleAxis(180f, axis) * quaternion2;
			}
			return quaternion2;
		}

		private Vector3 GetAlongRoadOffset(Vector3 tangent, float metres)
		{
			if (Mathf.Approximately(metres, 0f) || tangent.sqrMagnitude < 0.0001f)
			{
				return Vector3.zero;
			}
			return tangent.normalized * metres;
		}

		private Vector3 CalculateRightVector(Vector3 tangent)
		{
			Vector3 vector = Vector3.Cross(Vector3.up, tangent);
			if (vector.sqrMagnitude > 0.001f)
			{
				return vector.normalized;
			}
			return Vector3.right;
		}

		private Vector3 CalculateSurfaceNormal(Vector3 worldPosition, RoadObjectSnappingMode snappingMode, Vector3 roadTangent, bool alignToSurface)
		{
			if (!alignToSurface)
			{
				return Vector3.up;
			}
			return snappingMode switch
			{
				RoadObjectSnappingMode.SnapToTerrain => CalculateTerrainNormal(worldPosition), 
				RoadObjectSnappingMode.SnapToRoad => CalculateRoadNormal(roadTangent), 
				_ => Vector3.up, 
			};
		}

		private Vector3 CalculateTerrainNormal(Vector3 worldPosition)
		{
			if (_associatedTerrain == null || _associatedTerrain.terrainData == null)
			{
				return Vector3.up;
			}
			TerrainData terrainData = _associatedTerrain.terrainData;
			Vector3 position = _associatedTerrain.transform.position;
			Vector3 size = terrainData.size;
			Vector3 vector = worldPosition - position;
			float num = Mathf.Clamp01(vector.x / size.x);
			float num2 = Mathf.Clamp01(vector.z / size.z);
			float num3 = 0.01f;
			float interpolatedHeight = terrainData.GetInterpolatedHeight(Mathf.Clamp01(num - num3), num2);
			float interpolatedHeight2 = terrainData.GetInterpolatedHeight(Mathf.Clamp01(num + num3), num2);
			float interpolatedHeight3 = terrainData.GetInterpolatedHeight(num, Mathf.Clamp01(num2 - num3));
			float interpolatedHeight4 = terrainData.GetInterpolatedHeight(num, Mathf.Clamp01(num2 + num3));
			Vector3 vector2 = Vector3.Cross(rhs: new Vector3(num3 * size.x * 2f, interpolatedHeight2 - interpolatedHeight, 0f), lhs: new Vector3(0f, interpolatedHeight4 - interpolatedHeight3, num3 * size.z * 2f)).normalized;
			if (vector2.y < 0f)
			{
				vector2 = -vector2;
			}
			return vector2;
		}

		private Vector3 CalculateRoadNormal(Vector3 roadTangent)
		{
			if (roadTangent == Vector3.zero)
			{
				return Vector3.up;
			}
			Vector3 rhs = CalculateRightVector(roadTangent);
			Vector3 vector = Vector3.Cross(roadTangent, rhs).normalized;
			if (vector.y < 0f)
			{
				vector = -vector;
			}
			return vector;
		}
	}
}
