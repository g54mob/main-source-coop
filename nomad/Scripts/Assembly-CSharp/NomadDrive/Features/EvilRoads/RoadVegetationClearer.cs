using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.Extensions;
using NomadDrive.Features.EvilRoads.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace NomadDrive.Features.EvilRoads
{
	public class RoadVegetationClearer
	{
		private const float MaxClearMsPerFrame = 3f;

		private readonly EvilRoadConfig _roadConfig;

		private readonly Terrain _terrain;

		public RoadVegetationClearer(EvilRoadConfig roadConfig, Terrain terrain)
		{
			_roadConfig = roadConfig;
			_terrain = terrain;
		}

		public void ClearVegetationAroundRoad(Spline spline)
		{
			if (!(_terrain == null) && spline != null)
			{
				TerrainData terrainData = _terrain.terrainData;
				Vector3 size = terrainData.size;
				Vector3 position = _terrain.transform.position;
				float clearRadius = _roadConfig.roadWidth + _roadConfig.terrainInfluenceDistance;
				List<Vector3> roadPoints = PreCalculateRoadPoints(spline, clearRadius);
				ClearTerrainTreesOptimized(terrainData, size, position, clearRadius, roadPoints);
				ClearTerrainDetailsOptimized(terrainData, size, position, clearRadius, roadPoints);
			}
		}

		public async UniTask ClearVegetationAroundRoadAsync(Spline spline, bool deferTreeRebuild = true)
		{
			if (!(_terrain == null) && spline != null)
			{
				TerrainData terrainData = _terrain.terrainData;
				Vector3 terrainSize = terrainData.size;
				Vector3 terrainPosition = _terrain.transform.position;
				float clearRadius = _roadConfig.roadWidth + _roadConfig.terrainInfluenceDistance;
				List<Vector3> roadPoints = PreCalculateRoadPoints(spline, clearRadius);
				await ClearTerrainTreesAsync(terrainData, terrainSize, terrainPosition, clearRadius, roadPoints, deferTreeRebuild);
				if (!(_terrain == null))
				{
					await ClearTerrainDetailsAsync(terrainData, terrainSize, terrainPosition, clearRadius, roadPoints);
				}
			}
		}

		private async UniTask ClearTerrainTreesAsync(TerrainData terrainData, Vector3 terrainSize, Vector3 terrainPosition, float clearRadius, List<Vector3> roadPoints, bool deferTreeRebuild)
		{
			TreeInstance[] currentTrees = (deferTreeRebuild ? TerrainColliderRefreshScheduler.GetPendingTrees(_terrain).ToArray() : terrainData.treeInstances);
			if (currentTrees.Length == 0 || roadPoints.Count == 0)
			{
				return;
			}
			float num = 3.4028235E+38f;
			float num2 = -3.4028235E+38f;
			float num3 = 3.4028235E+38f;
			float num4 = -3.4028235E+38f;
			foreach (Vector3 roadPoint in roadPoints)
			{
				if (roadPoint.x < num)
				{
					num = roadPoint.x;
				}
				if (roadPoint.x > num2)
				{
					num2 = roadPoint.x;
				}
				if (roadPoint.z < num3)
				{
					num3 = roadPoint.z;
				}
				if (roadPoint.z > num4)
				{
					num4 = roadPoint.z;
				}
			}
			num -= clearRadius;
			num2 += clearRadius;
			num3 -= clearRadius;
			num4 += clearRadius;
			NativeArray<float2> treePositions = new NativeArray<float2>(currentTrees.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < currentTrees.Length; i++)
			{
				TreeInstance treeInstance = currentTrees[i];
				treePositions[i] = new float2(treeInstance.position.x * terrainSize.x + terrainPosition.x, treeInstance.position.z * terrainSize.z + terrainPosition.z);
			}
			NativeArray<float2> roadPts = new NativeArray<float2>(roadPoints.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < roadPoints.Count; j++)
			{
				roadPts[j] = new float2(roadPoints[j].x, roadPoints[j].z);
			}
			NativeArray<bool> removeFlags = new NativeArray<bool>(currentTrees.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			TreeCullJob jobData = new TreeCullJob
			{
				TreePositions = treePositions,
				RoadPoints = roadPts,
				Remove = removeFlags,
				ClearRadiusSqr = clearRadius * clearRadius,
				Bounds = new float4(num, num2, num3, num4)
			};
			JobHandle handle = jobData.Schedule(currentTrees.Length, 64);
			JobHandle.ScheduleBatchedJobs();
			while (!handle.IsCompleted)
			{
				await UniTask.Yield();
				if (_terrain == null)
				{
					handle.Complete();
					treePositions.Dispose();
					roadPts.Dispose();
					removeFlags.Dispose();
					return;
				}
			}
			handle.Complete();
			List<TreeInstance> list = new List<TreeInstance>(currentTrees.Length);
			bool flag = false;
			for (int k = 0; k < currentTrees.Length; k++)
			{
				if (removeFlags[k])
				{
					flag = true;
				}
				else
				{
					list.Add(currentTrees[k]);
				}
			}
			treePositions.Dispose();
			roadPts.Dispose();
			removeFlags.Dispose();
			if (flag)
			{
				if (deferTreeRebuild)
				{
					TerrainColliderRefreshScheduler.SetPendingTrees(_terrain, list);
				}
				else
				{
					terrainData.treeInstances = list.ToArray();
				}
				TerrainColliderRefreshScheduler.MarkDirty(_terrain);
			}
		}

		private async UniTask ClearTerrainDetailsAsync(TerrainData terrainData, Vector3 terrainSize, Vector3 terrainPosition, float clearRadius, List<Vector3> roadPoints)
		{
			int detailResolution = terrainData.detailResolution;
			if (detailResolution <= 0)
			{
				return;
			}
			float clearRadiusSqr = clearRadius * clearRadius;
			(int minX, int maxX, int minZ, int maxZ) gridBounds = CalculateAffectedDetailGridBounds(roadPoints, terrainSize, terrainPosition, clearRadius, detailResolution);
			int gridW = gridBounds.maxX - gridBounds.minX;
			int gridH = gridBounds.maxZ - gridBounds.minZ;
			if (gridW <= 0 || gridH <= 0)
			{
				return;
			}
			int layerCount = terrainData.detailPrototypes.Length;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int layer = 0; layer < layerCount; layer++)
			{
				int[,] detailLayer = terrainData.GetDetailLayer(gridBounds.minX, gridBounds.minZ, gridW, gridH, layer);
				bool changed = false;
				for (int lz = 0; lz < gridH; lz++)
				{
					float num = (float)(gridBounds.minZ + lz) / (float)detailResolution * terrainSize.z + terrainPosition.z;
					for (int i = 0; i < gridW; i++)
					{
						if (detailLayer[lz, i] <= 0)
						{
							continue;
						}
						float num2 = (float)(gridBounds.minX + i) / (float)detailResolution * terrainSize.x + terrainPosition.x;
						foreach (Vector3 roadPoint in roadPoints)
						{
							float num3 = num2 - roadPoint.x;
							float num4 = num - roadPoint.z;
							if (num3 * num3 + num4 * num4 <= clearRadiusSqr)
							{
								detailLayer[lz, i] = 0;
								changed = true;
								break;
							}
						}
					}
					if ((Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f >= 3f)
					{
						await UniTask.Yield();
						if (_terrain == null)
						{
							return;
						}
						realtimeSinceStartup = Time.realtimeSinceStartup;
					}
				}
				if (changed)
				{
					terrainData.SetDetailLayer(gridBounds.minX, gridBounds.minZ, layer, detailLayer);
				}
			}
		}

		private List<Vector3> PreCalculateRoadPoints(Spline spline, float clearRadius)
		{
			List<Vector3> list = new List<Vector3>();
			int value = Mathf.RoundToInt(spline.GetLength() / (clearRadius * 0.5f));
			value = Mathf.Clamp(value, 5, 100);
			for (int i = 0; i <= value; i++)
			{
				float t = (float)i / (float)value;
				Vector3 item = spline.EvaluatePosition(t);
				list.Add(item);
			}
			return list;
		}

		private void ClearTerrainTreesOptimized(TerrainData terrainData, Vector3 terrainSize, Vector3 terrainPosition, float clearRadius, List<Vector3> roadPoints)
		{
			List<TreeInstance> list = new List<TreeInstance>();
			TreeInstance[] treeInstances = terrainData.treeInstances;
			float num = clearRadius * clearRadius;
			TreeInstance[] array = treeInstances;
			for (int i = 0; i < array.Length; i++)
			{
				TreeInstance item = array[i];
				Vector3 vector = new Vector3(item.position.x * terrainSize.x + terrainPosition.x, item.position.y * terrainSize.y + terrainPosition.y, item.position.z * terrainSize.z + terrainPosition.z);
				bool flag = false;
				foreach (Vector3 roadPoint in roadPoints)
				{
					if ((vector - roadPoint).sqrMagnitude <= num)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(item);
				}
			}
			terrainData.treeInstances = list.ToArray();
			if (list.Count < treeInstances.Length)
			{
				TerrainCollider component = _terrain.GetComponent<TerrainCollider>();
				if (component != null)
				{
					component.enabled = false;
					component.enabled = true;
				}
			}
		}

		private void ClearTerrainDetailsOptimized(TerrainData terrainData, Vector3 terrainSize, Vector3 terrainPosition, float clearRadius, List<Vector3> roadPoints)
		{
			int detailResolution = terrainData.detailResolution;
			float num = clearRadius * clearRadius;
			(int, int, int, int) tuple = CalculateAffectedDetailGridBounds(roadPoints, terrainSize, terrainPosition, clearRadius, detailResolution);
			if (tuple.Item1 >= tuple.Item2 || tuple.Item3 >= tuple.Item4)
			{
				return;
			}
			for (int i = 0; i < terrainData.detailPrototypes.Length; i++)
			{
				int[,] detailLayer = terrainData.GetDetailLayer(0, 0, detailResolution, detailResolution, i);
				int num2 = 0;
				var (j, _, _, _) = tuple;
				for (; j < tuple.Item2; j++)
				{
					for (int k = tuple.Item3; k < tuple.Item4; k++)
					{
						if (detailLayer[k, j] <= 0)
						{
							continue;
						}
						Vector3 vector = new Vector3((float)j / (float)detailResolution * terrainSize.x + terrainPosition.x, terrainPosition.y, (float)k / (float)detailResolution * terrainSize.z + terrainPosition.z);
						bool flag = false;
						foreach (Vector3 roadPoint in roadPoints)
						{
							if ((vector.x - roadPoint.x) * (vector.x - roadPoint.x) + (vector.z - roadPoint.z) * (vector.z - roadPoint.z) <= num)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							num2 += detailLayer[k, j];
							detailLayer[k, j] = 0;
						}
					}
				}
				terrainData.SetDetailLayer(0, 0, i, detailLayer);
			}
		}

		private (int minX, int maxX, int minZ, int maxZ) CalculateAffectedDetailGridBounds(List<Vector3> roadPoints, Vector3 terrainSize, Vector3 terrainPosition, float clearRadius, int detailResolution)
		{
			if (roadPoints.Count == 0)
			{
				return (minX: 0, maxX: 0, minZ: 0, maxZ: 0);
			}
			float num = 3.4028235E+38f;
			float num2 = -3.4028235E+38f;
			float num3 = 3.4028235E+38f;
			float num4 = -3.4028235E+38f;
			foreach (Vector3 roadPoint in roadPoints)
			{
				num = Mathf.Min(num, roadPoint.x - clearRadius);
				num2 = Mathf.Max(num2, roadPoint.x + clearRadius);
				num3 = Mathf.Min(num3, roadPoint.z - clearRadius);
				num4 = Mathf.Max(num4, roadPoint.z + clearRadius);
			}
			int item = Mathf.Max(0, Mathf.FloorToInt((num - terrainPosition.x) / terrainSize.x * (float)detailResolution));
			int item2 = Mathf.Min(detailResolution, Mathf.CeilToInt((num2 - terrainPosition.x) / terrainSize.x * (float)detailResolution));
			int item3 = Mathf.Max(0, Mathf.FloorToInt((num3 - terrainPosition.z) / terrainSize.z * (float)detailResolution));
			int item4 = Mathf.Min(detailResolution, Mathf.CeilToInt((num4 - terrainPosition.z) / terrainSize.z * (float)detailResolution));
			return (minX: item, maxX: item2, minZ: item3, maxZ: item4);
		}
	}
}
