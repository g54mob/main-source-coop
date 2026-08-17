using Cysharp.Threading.Tasks;
using NomadDrive.Features.EvilRoads.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	public class RoadTerrainDeformer
	{
		private readonly EvilRoadConfig _roadConfig;

		private readonly Terrain _terrain;

		public RoadTerrainDeformer(EvilRoadConfig roadConfig, Terrain terrain)
		{
			_roadConfig = roadConfig;
			_terrain = terrain;
		}

		public void DeformTerrainToRoadMesh(Mesh roadMesh, Transform roadTransform)
		{
			if (!(_terrain == null) && !(roadMesh == null) && !(roadTransform == null))
			{
				TerrainData terrainData = _terrain.terrainData;
				int heightmapResolution = terrainData.heightmapResolution;
				int heightmapResolution2 = terrainData.heightmapResolution;
				float[,] heights = terrainData.GetHeights(0, 0, heightmapResolution, heightmapResolution2);
				Bounds roadBounds = CalculateRoadBounds(roadMesh, roadTransform);
				if (_roadConfig.enableSmartFillCut)
				{
					ApplyMultiPassSmartFillCut(heights, roadMesh, roadTransform, roadBounds, heightmapResolution, heightmapResolution2, terrainData);
				}
				if (_roadConfig.enableEdgeCleanup)
				{
					ApplyRoadEdgeCleanup(heights, roadMesh, roadTransform, roadBounds, heightmapResolution, heightmapResolution2, terrainData);
				}
				ApplyOptimizedDeformation(heights, roadMesh, roadTransform, roadBounds, heightmapResolution, heightmapResolution2, terrainData);
				if (_roadConfig.enableUltraPrecision)
				{
					ApplyFinalPrecisionPass(heights, roadMesh, roadTransform, roadBounds, heightmapResolution, heightmapResolution2, terrainData);
				}
				ApplyEdgeSmoothing(heights, roadMesh, roadTransform, roadBounds, heightmapResolution, heightmapResolution2, terrainData);
				terrainData.SetHeights(0, 0, heights);
			}
		}

		public async UniTask DeformTerrainToRoadMeshAsync(Mesh roadMesh, Transform roadTransform)
		{
			if (_terrain == null || roadMesh == null || roadTransform == null)
			{
				return;
			}
			TerrainData terrainData = _terrain.terrainData;
			int heightmapResolution = terrainData.heightmapResolution;
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			Bounds roadBounds = CalculateRoadBounds(roadMesh, roadTransform);
			(int minX, int maxX, int minZ, int maxZ) bounds = CalculateHeightmapBounds(roadBounds, position, size, heightmapResolution, heightmapResolution);
			int subWidth = bounds.maxX - bounds.minX + 1;
			int subHeight = bounds.maxZ - bounds.minZ + 1;
			if (subWidth <= 0 || subHeight <= 0)
			{
				return;
			}
			int num = subWidth * subHeight;
			float[,] managedHeights = terrainData.GetHeights(bounds.minX, bounds.minZ, subWidth, subHeight);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			NativeArray<float> inHeights = new NativeArray<float>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<float> deformedHeights = new NativeArray<float>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<float> finalHeights = new NativeArray<float>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<byte> zone = new NativeArray<byte>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<float3> worldVerts = new NativeArray<float3>(vertices.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> triangles2 = new NativeArray<int>(triangles.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < subHeight; i++)
			{
				int num2 = i * subWidth;
				for (int j = 0; j < subWidth; j++)
				{
					inHeights[num2 + j] = managedHeights[i, j];
				}
			}
			for (int k = 0; k < vertices.Length; k++)
			{
				worldVerts[k] = roadTransform.TransformPoint(vertices[k]);
			}
			triangles2.CopyFrom(triangles);
			RoadTerrainDeformJob jobData = new RoadTerrainDeformJob
			{
				WorldVerts = worldVerts,
				Triangles = triangles2,
				InHeights = inHeights,
				OutHeights = deformedHeights,
				Zone = zone,
				SubWidth = subWidth,
				MinX = bounds.minX,
				MinZ = bounds.minZ,
				HeightmapResolution = heightmapResolution,
				TerrainPosition = position,
				TerrainSize = size,
				RoadSurfaceAdjustment = _roadConfig.roadSurfaceAdjustment,
				InfluenceDistance = _roadConfig.terrainInfluenceDistance,
				BlendingSmoothness = _roadConfig.blendingSmoothness
			};
			RoadTerrainSmoothJob jobData2 = new RoadTerrainSmoothJob
			{
				Source = deformedHeights,
				Zone = zone,
				Output = finalHeights,
				SubWidth = subWidth,
				SubHeight = subHeight
			};
			JobHandle dependsOn = jobData.Schedule(num, 64);
			JobHandle smoothHandle = jobData2.Schedule(num, 64, dependsOn);
			JobHandle.ScheduleBatchedJobs();
			while (!smoothHandle.IsCompleted)
			{
				await UniTask.Yield();
				if (roadTransform == null || _terrain == null)
				{
					smoothHandle.Complete();
					DisposeDeformBuffers(inHeights, deformedHeights, finalHeights, zone, worldVerts, triangles2);
					return;
				}
			}
			smoothHandle.Complete();
			for (int l = 0; l < subHeight; l++)
			{
				int num3 = l * subWidth;
				for (int m = 0; m < subWidth; m++)
				{
					managedHeights[l, m] = finalHeights[num3 + m];
				}
			}
			terrainData.SetHeights(bounds.minX, bounds.minZ, managedHeights);
			DisposeDeformBuffers(inHeights, deformedHeights, finalHeights, zone, worldVerts, triangles2);
			await UniTask.Yield();
		}

		private static void DisposeDeformBuffers(NativeArray<float> inHeights, NativeArray<float> deformedHeights, NativeArray<float> finalHeights, NativeArray<byte> zone, NativeArray<float3> worldVerts, NativeArray<int> triangles)
		{
			if (inHeights.IsCreated)
			{
				inHeights.Dispose();
			}
			if (deformedHeights.IsCreated)
			{
				deformedHeights.Dispose();
			}
			if (finalHeights.IsCreated)
			{
				finalHeights.Dispose();
			}
			if (zone.IsCreated)
			{
				zone.Dispose();
			}
			if (worldVerts.IsCreated)
			{
				worldVerts.Dispose();
			}
			if (triangles.IsCreated)
			{
				triangles.Dispose();
			}
		}

		private Bounds CalculateRoadBounds(Mesh roadMesh, Transform roadTransform)
		{
			Bounds bounds = roadMesh.bounds;
			Bounds result = default(Bounds);
			Vector3[] array = new Vector3[8];
			Vector3 center = bounds.center;
			Vector3 extents = bounds.extents;
			array[0] = roadTransform.TransformPoint(center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z));
			array[1] = roadTransform.TransformPoint(center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z));
			array[2] = roadTransform.TransformPoint(center + new Vector3(0f - extents.x, extents.y, 0f - extents.z));
			array[3] = roadTransform.TransformPoint(center + new Vector3(extents.x, extents.y, 0f - extents.z));
			array[4] = roadTransform.TransformPoint(center + new Vector3(0f - extents.x, 0f - extents.y, extents.z));
			array[5] = roadTransform.TransformPoint(center + new Vector3(extents.x, 0f - extents.y, extents.z));
			array[6] = roadTransform.TransformPoint(center + new Vector3(0f - extents.x, extents.y, extents.z));
			array[7] = roadTransform.TransformPoint(center + new Vector3(extents.x, extents.y, extents.z));
			result.SetMinMax(array[0], array[0]);
			for (int i = 1; i < array.Length; i++)
			{
				result.Encapsulate(array[i]);
			}
			float amount = _roadConfig.terrainInfluenceDistance * 2f;
			result.Expand(amount);
			return result;
		}

		private void ApplyOptimizedDeformation(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			float num = size.x / (float)heightmapWidth;
			int num2 = Mathf.Max(1, Mathf.RoundToInt(0.3f / num));
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i += num2)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j += num2)
				{
					Vector3 worldPos = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					ProcessOptimizedTerrainPoint(heights, i, j, worldPos, vertices, triangles, roadTransform, position, size, num2, heightmapWidth, heightmapHeight);
				}
			}
		}

		private void ApplyFinalPrecisionPass(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			float num = _roadConfig.roadWidth * 0.6f;
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					Vector3 vector = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					if (CalculateFastDistanceToRoadMesh(vector, vertices, triangles, roadTransform) <= num)
					{
						float? roadMeshHeight = RoadHeightCalculator.GetRoadMeshHeight(vector, roadMesh, roadTransform);
						if (roadMeshHeight.HasValue)
						{
							float value = (roadMeshHeight.Value + _roadConfig.roadSurfaceAdjustment - position.y) / size.y;
							heights[j, i] = Mathf.Clamp01(value);
						}
					}
				}
			}
		}

		private void ApplyMeshBasedDeformation(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					Vector3 worldPos = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					ProcessTerrainPoint(heights, i, j, worldPos, roadMesh, roadTransform, position, size);
				}
			}
		}

		private void ApplyHardRoadDeformation(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					Vector3 position2 = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					Vector3 point = roadTransform.InverseTransformPoint(position2);
					float roadHeight = 3.4028235E+38f;
					bool flag = false;
					float num = 3.4028235E+38f;
					for (int k = 0; k < triangles.Length; k += 3)
					{
						Vector3 vector = vertices[triangles[k]];
						Vector3 vector2 = vertices[triangles[k + 1]];
						Vector3 vector3 = vertices[triangles[k + 2]];
						if (IsPointInTriangle2D(point, vector, vector2, vector3))
						{
							float y = InterpolateTriangleHeight(point, vector, vector2, vector3);
							float value = (roadTransform.TransformPoint(new Vector3(point.x, y, point.z)).y - position.y) / size.y;
							heights[j, i] = Mathf.Clamp01(value);
							flag = true;
							break;
						}
						float num2 = DistanceToTriangle2D(point, vector, vector2, vector3);
						if (num2 < num)
						{
							num = num2;
							float y2 = InterpolateTriangleHeight(point, vector, vector2, vector3);
							roadHeight = roadTransform.TransformPoint(new Vector3(point.x, y2, point.z)).y;
						}
					}
					if (!flag && num <= _roadConfig.terrainInfluenceDistance)
					{
						float currentHeight = heights[j, i] * size.y + position.y;
						ApplyInfluenceZoneTransition(heights, j, i, currentHeight, roadHeight, num, position, size);
					}
				}
			}
		}

		private void ApplyRoadSurfacePerfection(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					Vector3 vector = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					float[] array = new float[3] { -0.25f, 0f, 0.25f };
					bool flag = false;
					float num = 0f;
					float num2 = 3.4028235E+38f;
					float[] array2 = array;
					foreach (float x in array2)
					{
						float[] array3 = array;
						foreach (float z in array3)
						{
							Vector3 position2 = vector + new Vector3(x, 0f, z);
							Vector3 point = roadTransform.InverseTransformPoint(position2);
							for (int m = 0; m < triangles.Length; m += 3)
							{
								Vector3 vector2 = vertices[triangles[m]];
								Vector3 vector3 = vertices[triangles[m + 1]];
								Vector3 vector4 = vertices[triangles[m + 2]];
								if (IsPointInTriangle2D(point, vector2, vector3, vector4))
								{
									float y = InterpolateTriangleHeight(point, vector2, vector3, vector4);
									num = roadTransform.TransformPoint(new Vector3(point.x, y, point.z)).y;
									flag = true;
									num2 = 0f;
									break;
								}
								float num3 = DistanceToTriangle2D(point, vector2, vector3, vector4);
								if (num3 < num2)
								{
									num2 = num3;
									float y2 = InterpolateTriangleHeight(point, vector2, vector3, vector4);
									num = roadTransform.TransformPoint(new Vector3(point.x, y2, point.z)).y;
								}
							}
							if (flag)
							{
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						float value = (num + _roadConfig.roadSurfaceAdjustment - position.y) / size.y;
						heights[j, i] = Mathf.Clamp01(value);
					}
					else if (num2 <= _roadConfig.terrainInfluenceDistance)
					{
						float currentHeight = heights[j, i] * size.y + position.y;
						ApplyInfluenceZoneTransition(heights, j, i, currentHeight, num, num2, position, size);
					}
				}
			}
		}

		private float CalculateFastDistanceToRoadMesh(Vector3 worldPos, Vector3[] vertices, int[] triangles, Transform roadTransform)
		{
			Vector3 point = roadTransform.InverseTransformPoint(worldPos);
			float num = 3.4028235E+38f;
			for (int i = 0; i < triangles.Length && i + 2 < triangles.Length; i += 9)
			{
				Vector3 v = vertices[triangles[i]];
				Vector3 v2 = vertices[triangles[i + 1]];
				Vector3 v3 = vertices[triangles[i + 2]];
				float num2 = DistanceToTriangle2D(point, v, v2, v3);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		private void ProcessOptimizedTerrainPoint(float[,] heights, int x, int z, Vector3 worldPos, Vector3[] vertices, int[] triangles, Transform roadTransform, Vector3 terrainPosition, Vector3 terrainSize, int sampleStep, int heightmapWidth, int heightmapHeight)
		{
			Vector3 point = roadTransform.InverseTransformPoint(worldPos);
			float currentHeight = heights[z, x] * terrainSize.y + terrainPosition.y;
			bool flag = false;
			float num = 0f;
			float num2 = 3.4028235E+38f;
			for (int i = 0; i < triangles.Length && i + 2 < triangles.Length; i += 3)
			{
				Vector3 vector = vertices[triangles[i]];
				Vector3 vector2 = vertices[triangles[i + 1]];
				Vector3 vector3 = vertices[triangles[i + 2]];
				if (IsPointInTriangle2D(point, vector, vector2, vector3))
				{
					num = InterpolateTriangleHeight(point, vector, vector2, vector3);
					num = roadTransform.TransformPoint(new Vector3(point.x, num, point.z)).y;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < triangles.Length && j + 2 < triangles.Length; j += 6)
				{
					Vector3 v = vertices[triangles[j]];
					Vector3 v2 = vertices[triangles[j + 1]];
					Vector3 v3 = vertices[triangles[j + 2]];
					float num3 = DistanceToTriangle2D(point, v, v2, v3);
					if (num3 < num2)
					{
						num2 = num3;
						float y = InterpolateTriangleHeight(point, v, v2, v3);
						num = roadTransform.TransformPoint(new Vector3(point.x, y, point.z)).y;
					}
				}
			}
			if (flag)
			{
				float num4 = num + _roadConfig.roadSurfaceAdjustment;
				float num5 = (num4 - terrainPosition.y) / terrainSize.y;
				heights[z, x] = Mathf.Clamp01(num5);
				if (sampleStep > 1)
				{
					FillSurroundingPixelsSmooth(heights, x, z, num5, sampleStep, heightmapWidth, heightmapHeight, num4, terrainPosition, terrainSize);
				}
			}
			else if (num2 <= _roadConfig.terrainInfluenceDistance)
			{
				float roadHeight = num + _roadConfig.roadSurfaceAdjustment;
				ApplyFastInfluenceTransition(heights, z, x, currentHeight, roadHeight, num2, terrainPosition, terrainSize, sampleStep, heightmapWidth, heightmapHeight);
			}
		}

		private void FillSurroundingPixels(float[,] heights, int centerX, int centerZ, float height, int sampleStep, int heightmapWidth, int heightmapHeight)
		{
			for (int i = 0; i < sampleStep && centerX + i < heightmapWidth; i++)
			{
				for (int j = 0; j < sampleStep && centerZ + j < heightmapHeight; j++)
				{
					heights[centerZ + j, centerX + i] = height;
				}
			}
		}

		private void FillSurroundingPixelsSmooth(float[,] heights, int centerX, int centerZ, float centerHeight, int sampleStep, int heightmapWidth, int heightmapHeight, float roadHeight, Vector3 terrainPosition, Vector3 terrainSize)
		{
			float b = (roadHeight + _roadConfig.roadSurfaceAdjustment - terrainPosition.y) / terrainSize.y;
			for (int i = 0; i < sampleStep && centerX + i < heightmapWidth; i++)
			{
				for (int j = 0; j < sampleStep && centerZ + j < heightmapHeight; j++)
				{
					float num = Mathf.Sqrt(i * i + j * j);
					float num2 = (float)sampleStep * 0.7f;
					if (num <= num2)
					{
						float t = 1f - num / num2 * 0.2f;
						float value = Mathf.Lerp(heights[centerZ + j, centerX + i], b, t);
						heights[centerZ + j, centerX + i] = Mathf.Clamp01(value);
					}
				}
			}
		}

		private void ApplyFastInfluenceTransition(float[,] heights, int z, int x, float currentHeight, float roadHeight, float distanceToRoad, Vector3 terrainPosition, Vector3 terrainSize, int sampleStep, int heightmapWidth, int heightmapHeight)
		{
			float num = _roadConfig.roadWidth * 0.5f;
			if (distanceToRoad <= _roadConfig.terrainInfluenceDistance)
			{
				float t;
				if (distanceToRoad <= num)
				{
					t = (1f - distanceToRoad / num) * _roadConfig.deformationStrength;
				}
				else
				{
					float num2 = (distanceToRoad - num) / (_roadConfig.terrainInfluenceDistance - num);
					float num3 = Mathf.Pow(1f - num2, _roadConfig.blendingSmoothness);
					t = _roadConfig.deformationStrength * num3 * 0.5f;
				}
				float num4 = Mathf.Lerp(currentHeight, roadHeight, t);
				float num5 = (num4 - terrainPosition.y) / terrainSize.y;
				heights[z, x] = Mathf.Clamp01(num5);
				if (sampleStep > 1)
				{
					FillSurroundingPixelsSmooth(heights, x, z, num5, sampleStep, heightmapWidth, heightmapHeight, num4, terrainPosition, terrainSize);
				}
			}
		}

		private void ApplyInfluenceZoneTransition(float[,] heights, int z, int x, float currentHeight, float roadHeight, float distanceToRoad, Vector3 terrainPosition, Vector3 terrainSize)
		{
			float num = _roadConfig.roadWidth * 0.5f;
			if (distanceToRoad <= num)
			{
				float f = 1f - distanceToRoad / num;
				f = Mathf.Pow(f, 1f / _roadConfig.blendingSmoothness);
				float value = (Mathf.Lerp(currentHeight, roadHeight, f * _roadConfig.deformationStrength) - terrainPosition.y) / terrainSize.y;
				heights[z, x] = Mathf.Clamp01(value);
			}
			else
			{
				if (!(distanceToRoad <= _roadConfig.terrainInfluenceDistance))
				{
					return;
				}
				float num2 = num;
				float num3 = _roadConfig.terrainInfluenceDistance - num2;
				if (num3 > 0.1f)
				{
					float value2 = (distanceToRoad - num2) / num3;
					value2 = Mathf.Clamp01(value2);
					float num4 = Mathf.Pow(1f - value2, _roadConfig.blendingSmoothness);
					float t = _roadConfig.deformationStrength * num4;
					float num5;
					if (currentHeight > roadHeight)
					{
						float b = Mathf.Lerp(roadHeight, currentHeight, value2);
						num5 = Mathf.Lerp(currentHeight, b, t);
					}
					else
					{
						float b2 = Mathf.Lerp(roadHeight, currentHeight, value2);
						num5 = Mathf.Lerp(currentHeight, b2, t);
					}
					float value3 = (num5 - terrainPosition.y) / terrainSize.y;
					heights[z, x] = Mathf.Clamp01(value3);
				}
			}
		}

		private void ApplySmoothingPass(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			float[,] array = (float[,])heights.Clone();
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					if (RoadHeightCalculator.GetRoadMeshHeight(HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight), roadMesh, roadTransform).HasValue)
					{
						ApplyLocalSmoothing(array, i, j, heightmapWidth, heightmapHeight);
					}
				}
			}
			var (k, _, _, _) = tuple;
			for (; k <= tuple.Item2; k++)
			{
				for (int l = tuple.Item3; l <= tuple.Item4; l++)
				{
					heights[l, k] = array[l, k];
				}
			}
		}

		private void ProcessTerrainPoint(float[,] heights, int heightmapX, int heightmapZ, Vector3 worldPos, Mesh roadMesh, Transform roadTransform, Vector3 terrainPosition, Vector3 terrainSize)
		{
			float currentHeight = heights[heightmapZ, heightmapX] * terrainSize.y + terrainPosition.y;
			float? roadMeshHeight = RoadHeightCalculator.GetRoadMeshHeight(worldPos, roadMesh, roadTransform);
			if (roadMeshHeight.HasValue)
			{
				float value = (roadMeshHeight.Value + _roadConfig.roadSurfaceAdjustment - terrainPosition.y) / terrainSize.y;
				heights[heightmapZ, heightmapX] = Mathf.Clamp01(value);
				return;
			}
			float num = CalculateDistanceToRoadMesh(worldPos, roadMesh, roadTransform);
			if (num <= _roadConfig.terrainInfluenceDistance)
			{
				float? roadMeshHeight2 = RoadHeightCalculator.GetRoadMeshHeight(FindClosestPointOnRoadMesh(worldPos, roadMesh, roadTransform), roadMesh, roadTransform);
				if (roadMeshHeight2.HasValue)
				{
					ApplyInfluenceZoneTransition(heights, heightmapZ, heightmapX, currentHeight, roadMeshHeight2.Value, num, terrainPosition, terrainSize);
				}
			}
		}

		private (int minX, int maxX, int minZ, int maxZ) CalculateHeightmapBounds(Bounds roadBounds, Vector3 terrainPosition, Vector3 terrainSize, int heightmapWidth, int heightmapHeight)
		{
			Vector3 vector = roadBounds.min - terrainPosition;
			Vector3 vector2 = roadBounds.max - terrainPosition;
			int item = Mathf.Max(0, Mathf.FloorToInt(vector.x / terrainSize.x * (float)heightmapWidth));
			int item2 = Mathf.Min(heightmapWidth - 1, Mathf.CeilToInt(vector2.x / terrainSize.x * (float)heightmapWidth));
			int item3 = Mathf.Max(0, Mathf.FloorToInt(vector.z / terrainSize.z * (float)heightmapHeight));
			int item4 = Mathf.Min(heightmapHeight - 1, Mathf.CeilToInt(vector2.z / terrainSize.z * (float)heightmapHeight));
			return (minX: item, maxX: item2, minZ: item3, maxZ: item4);
		}

		private Vector3 HeightmapToWorldPosition(int heightmapX, int heightmapZ, Vector3 terrainPosition, Vector3 terrainSize, int heightmapWidth, int heightmapHeight)
		{
			float x = terrainPosition.x + (float)heightmapX / (float)(heightmapWidth - 1) * terrainSize.x;
			float z = terrainPosition.z + (float)heightmapZ / (float)(heightmapHeight - 1) * terrainSize.z;
			float y = terrainPosition.y;
			return new Vector3(x, y, z);
		}

		private float CalculateDistanceToRoadMesh(Vector3 worldPos, Mesh roadMesh, Transform roadTransform)
		{
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			float num = 3.4028235E+38f;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 a = roadTransform.TransformPoint(vertices[triangles[i]]);
				Vector3 b = roadTransform.TransformPoint(vertices[triangles[i + 1]]);
				Vector3 c = roadTransform.TransformPoint(vertices[triangles[i + 2]]);
				float b2 = DistanceToTriangle(worldPos, a, b, c);
				num = Mathf.Min(num, b2);
			}
			return num;
		}

		private Vector3 FindClosestPointOnRoadMesh(Vector3 worldPos, Mesh roadMesh, Transform roadTransform)
		{
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			Vector3 result = worldPos;
			float num = 3.4028235E+38f;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 a = roadTransform.TransformPoint(vertices[triangles[i]]);
				Vector3 b = roadTransform.TransformPoint(vertices[triangles[i + 1]]);
				Vector3 c = roadTransform.TransformPoint(vertices[triangles[i + 2]]);
				Vector3 vector = ClosestPointOnTriangle(worldPos, a, b, c);
				float num2 = Vector3.Distance(worldPos, vector);
				if (num2 < num)
				{
					num = num2;
					result = vector;
				}
			}
			return result;
		}

		private float DistanceToTriangle(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 b2 = ClosestPointOnTriangle(point, a, b, c);
			return Vector3.Distance(point, b2);
		}

		private Vector3 ClosestPointOnTriangle(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = c - a;
			Vector3 rhs = point - a;
			float num = Vector3.Dot(vector, rhs);
			float num2 = Vector3.Dot(vector2, rhs);
			if (num <= 0f && num2 <= 0f)
			{
				return a;
			}
			Vector3 rhs2 = point - b;
			float num3 = Vector3.Dot(vector, rhs2);
			float num4 = Vector3.Dot(vector2, rhs2);
			if (num3 >= 0f && num4 <= num3)
			{
				return b;
			}
			Vector3 rhs3 = point - c;
			float num5 = Vector3.Dot(vector, rhs3);
			float num6 = Vector3.Dot(vector2, rhs3);
			if (num6 >= 0f && num5 <= num6)
			{
				return c;
			}
			float num7 = num * num4 - num3 * num2;
			if (num7 <= 0f && num >= 0f && num3 <= 0f)
			{
				float num8 = num / (num - num3);
				return a + num8 * vector;
			}
			float num9 = num5 * num2 - num * num6;
			if (num9 <= 0f && num2 >= 0f && num6 <= 0f)
			{
				float num10 = num2 / (num2 - num6);
				return a + num10 * vector2;
			}
			float num11 = num3 * num6 - num5 * num4;
			if (num11 <= 0f && num4 - num3 >= 0f && num5 - num6 >= 0f)
			{
				float num12 = (num4 - num3) / (num4 - num3 + (num5 - num6));
				return b + num12 * (c - b);
			}
			float num13 = 1f / (num11 + num9 + num7);
			float num14 = num9 * num13;
			float num15 = num7 * num13;
			return a + vector * num14 + vector2 * num15;
		}

		private bool IsPointInTriangle2D(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
		{
			Vector2 vector = new Vector2(point.x, point.z);
			Vector2 vector2 = new Vector2(a.x, a.z);
			Vector2 vector3 = new Vector2(b.x, b.z);
			Vector2 vector4 = new Vector2(c.x, c.z);
			float num = (vector3.y - vector4.y) * (vector2.x - vector4.x) + (vector4.x - vector3.x) * (vector2.y - vector4.y);
			if (Mathf.Abs(num) < 1E-06f)
			{
				return false;
			}
			float num2 = ((vector3.y - vector4.y) * (vector.x - vector4.x) + (vector4.x - vector3.x) * (vector.y - vector4.y)) / num;
			float num3 = ((vector4.y - vector2.y) * (vector.x - vector4.x) + (vector2.x - vector4.x) * (vector.y - vector4.y)) / num;
			float num4 = 1f - num2 - num3;
			if (num2 >= 0f && num3 >= 0f)
			{
				return num4 >= 0f;
			}
			return false;
		}

		private float InterpolateTriangleHeight(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
		{
			Vector2 vector = new Vector2(point.x, point.z);
			Vector2 vector2 = new Vector2(v0.x, v0.z);
			Vector2 vector3 = new Vector2(v1.x, v1.z);
			Vector2 vector4 = new Vector2(v2.x, v2.z);
			float num = (vector3.y - vector4.y) * (vector2.x - vector4.x) + (vector4.x - vector3.x) * (vector2.y - vector4.y);
			if (Mathf.Abs(num) < 1E-06f)
			{
				return v0.y;
			}
			float num2 = ((vector3.y - vector4.y) * (vector.x - vector4.x) + (vector4.x - vector3.x) * (vector.y - vector4.y)) / num;
			float num3 = ((vector4.y - vector2.y) * (vector.x - vector4.x) + (vector2.x - vector4.x) * (vector.y - vector4.y)) / num;
			float num4 = 1f - num2 - num3;
			return num2 * v0.y + num3 * v1.y + num4 * v2.y;
		}

		private float DistanceToTriangle2D(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
		{
			Vector2 point2 = new Vector2(point.x, point.z);
			Vector2 vector = new Vector2(v0.x, v0.z);
			Vector2 vector2 = new Vector2(v1.x, v1.z);
			Vector2 vector3 = new Vector2(v2.x, v2.z);
			float a = DistanceToLineSegment(point2, vector, vector2);
			float a2 = DistanceToLineSegment(point2, vector2, vector3);
			float b = DistanceToLineSegment(point2, vector3, vector);
			return Mathf.Min(a, Mathf.Min(a2, b));
		}

		private float DistanceToLineSegment(Vector2 point, Vector2 a, Vector2 b)
		{
			Vector2 vector = b - a;
			Vector2 lhs = point - a;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude == 0f)
			{
				return Vector2.Distance(point, a);
			}
			float num = Mathf.Clamp01(Vector2.Dot(lhs, vector) / sqrMagnitude);
			Vector2 b2 = a + num * vector;
			return Vector2.Distance(point, b2);
		}

		private void ApplyLocalSmoothing(float[,] heights, int centerX, int centerZ, int width, int height)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = centerX - 1; i <= centerX + 1; i++)
			{
				for (int j = centerZ - 1; j <= centerZ + 1; j++)
				{
					if (i >= 0 && i < width && j >= 0 && j < height)
					{
						num += heights[j, i];
						num2++;
					}
				}
			}
			if (num2 > 0)
			{
				float b = num / (float)num2;
				heights[centerZ, centerX] = Mathf.Lerp(heights[centerZ, centerX], b, 0.3f);
			}
		}

		private void ApplySmartFillCut(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					Vector3 worldPos = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					ApplySmartFillCutAtPoint(heights, i, j, worldPos, vertices, triangles, roadTransform, position, size);
				}
			}
		}

		private void ApplySmartFillCutAtPoint(float[,] heights, int x, int z, Vector3 worldPos, Vector3[] vertices, int[] triangles, Transform roadTransform, Vector3 terrainPosition, Vector3 terrainSize)
		{
			Vector3 point = roadTransform.InverseTransformPoint(worldPos);
			float num = heights[z, x] * terrainSize.y + terrainPosition.y;
			float num2 = 3.4028235E+38f;
			float roadHeight = num;
			bool flag = false;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 vector = vertices[triangles[i]];
				Vector3 vector2 = vertices[triangles[i + 1]];
				Vector3 vector3 = vertices[triangles[i + 2]];
				if (IsPointInTriangle2D(point, vector, vector2, vector3))
				{
					float y = InterpolateTriangleHeight(point, vector, vector2, vector3);
					roadHeight = roadTransform.TransformPoint(new Vector3(point.x, y, point.z)).y;
					num2 = 0f;
					flag = true;
					break;
				}
				float num3 = DistanceToTriangle2D(point, vector, vector2, vector3);
				if (num3 < num2)
				{
					num2 = num3;
					float y2 = InterpolateTriangleHeight(point, vector, vector2, vector3);
					roadHeight = roadTransform.TransformPoint(new Vector3(point.x, y2, point.z)).y;
					flag = num3 <= _roadConfig.roadWidth * 0.5f;
				}
			}
			if (flag)
			{
				ApplyRoadWidthAdjustment(heights, x, z, num, roadHeight, num2, terrainPosition, terrainSize);
			}
			else if (num2 <= _roadConfig.terrainInfluenceDistance)
			{
				ApplyInfluenceZoneTransition(heights, z, x, num, roadHeight, num2, terrainPosition, terrainSize);
			}
		}

		private void ApplyRoadWidthAdjustment(float[,] heights, int x, int z, float currentHeight, float roadHeight, float distanceFromRoadCenter, Vector3 terrainPosition, Vector3 terrainSize)
		{
			float num = _roadConfig.roadWidth * 0.5f;
			float num2;
			if (distanceFromRoadCenter == 0f)
			{
				num2 = roadHeight + _roadConfig.roadSurfaceAdjustment;
			}
			else
			{
				float f = 1f - distanceFromRoadCenter / num;
				f = Mathf.Pow(f, 1f / _roadConfig.blendingSmoothness);
				float num3 = roadHeight + _roadConfig.roadSurfaceAdjustment;
				if (currentHeight < num3)
				{
					float t = _roadConfig.cuttingStrength * f;
					num2 = Mathf.Lerp(currentHeight, num3, t);
				}
				else
				{
					float t2 = _roadConfig.cuttingStrength * f;
					num2 = Mathf.Lerp(currentHeight, num3, t2);
				}
			}
			float value = (num2 - terrainPosition.y) / terrainSize.y;
			heights[z, x] = Mathf.Clamp01(value);
		}

		private void ApplyRoadEdgeCleanup(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					Vector3 worldPos = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					ProcessRoadEdgePoint(heights, i, j, worldPos, vertices, triangles, roadTransform, position, size);
				}
			}
		}

		private void ProcessRoadEdgePoint(float[,] heights, int x, int z, Vector3 worldPos, Vector3[] vertices, int[] triangles, Transform roadTransform, Vector3 terrainPosition, Vector3 terrainSize)
		{
			Vector3 point = roadTransform.InverseTransformPoint(worldPos);
			float num = heights[z, x] * terrainSize.y + terrainPosition.y;
			bool flag = false;
			bool flag2 = false;
			float num2 = num;
			float num3 = 3.4028235E+38f;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 vector = vertices[triangles[i]];
				Vector3 vector2 = vertices[triangles[i + 1]];
				Vector3 vector3 = vertices[triangles[i + 2]];
				if (IsPointInTriangleExpanded2D(point, vector, vector2, vector3, _roadConfig.edgeDetectionSensitivity))
				{
					float y = InterpolateTriangleHeight(point, vector, vector2, vector3);
					num2 = roadTransform.TransformPoint(new Vector3(point.x, y, point.z)).y;
					flag = true;
					break;
				}
				float num4 = DistanceToTriangleEdge2D(point, vector, vector2, vector3);
				if (num4 < num3)
				{
					num3 = num4;
					if (num4 <= _roadConfig.roadWidth * _roadConfig.edgeDetectionSensitivity)
					{
						flag2 = true;
						float y2 = InterpolateTriangleHeight(point, vector, vector2, vector3);
						num2 = roadTransform.TransformPoint(new Vector3(point.x, y2, point.z)).y;
					}
				}
			}
			if (flag)
			{
				float value = (num2 + _roadConfig.roadSurfaceAdjustment - terrainPosition.y) / terrainSize.y;
				heights[z, x] = Mathf.Clamp01(value);
			}
			else if (flag2 && Mathf.Abs(num - num2) > _roadConfig.roadSurfaceAdjustment * 5f)
			{
				float t = _roadConfig.cuttingStrength * 0.3f;
				float value2 = (Mathf.Lerp(num, num2, t) - terrainPosition.y) / terrainSize.y;
				heights[z, x] = Mathf.Clamp01(value2);
			}
		}

		private bool IsPointInTriangleExpanded2D(Vector3 point, Vector3 a, Vector3 b, Vector3 c, float expansion)
		{
			Vector2 point2 = new Vector2(point.x, point.z);
			Vector2 vector = new Vector2(a.x, a.z);
			Vector2 vector2 = new Vector2(b.x, b.z);
			Vector2 vector3 = new Vector2(c.x, c.z);
			if (expansion > 0f)
			{
				Vector2 vector4 = (vector + vector2 + vector3) / 3f;
				vector += (vector - vector4).normalized * expansion;
				vector2 += (vector2 - vector4).normalized * expansion;
				vector3 += (vector3 - vector4).normalized * expansion;
			}
			return IsPointInTriangle(point2, vector, vector2, vector3);
		}

		private bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
		{
			float num = (b.y - c.y) * (a.x - c.x) + (c.x - b.x) * (a.y - c.y);
			if (Mathf.Abs(num) < 1E-06f)
			{
				return false;
			}
			float num2 = ((b.y - c.y) * (point.x - c.x) + (c.x - b.x) * (point.y - c.y)) / num;
			float num3 = ((c.y - a.y) * (point.x - c.x) + (a.x - c.x) * (point.y - c.y)) / num;
			float num4 = 1f - num2 - num3;
			if (num2 >= 0f && num3 >= 0f)
			{
				return num4 >= 0f;
			}
			return false;
		}

		private float DistanceToTriangleEdge2D(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
		{
			Vector2 point2 = new Vector2(point.x, point.z);
			Vector2 vector = new Vector2(v0.x, v0.z);
			Vector2 vector2 = new Vector2(v1.x, v1.z);
			Vector2 vector3 = new Vector2(v2.x, v2.z);
			if (IsPointInTriangle(point2, vector, vector2, vector3))
			{
				return 0f;
			}
			float a = DistanceToLineSegment(point2, vector, vector2);
			float a2 = DistanceToLineSegment(point2, vector2, vector3);
			float b = DistanceToLineSegment(point2, vector3, vector);
			return Mathf.Min(a, Mathf.Min(a2, b));
		}

		private void ApplyEdgeSmoothing(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			float[,] array = (float[,])heights.Clone();
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					Vector3 worldPos = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					if (CalculateDistanceToRoadMesh(worldPos, roadMesh, roadTransform) <= _roadConfig.terrainInfluenceDistance)
					{
						ApplyDirectionalSmoothing(array, i, j, heightmapWidth, heightmapHeight, worldPos, roadMesh, roadTransform);
					}
				}
			}
			var (k, _, _, _) = tuple;
			for (; k <= tuple.Item2; k++)
			{
				for (int l = tuple.Item3; l <= tuple.Item4; l++)
				{
					heights[l, k] = array[l, k];
				}
			}
		}

		private void ApplyDirectionalSmoothing(float[,] heights, int centerX, int centerZ, int heightmapWidth, int heightmapHeight, Vector3 worldPos, Mesh roadMesh, Transform roadTransform)
		{
			float a = heights[centerZ, centerX];
			if (RoadHeightCalculator.GetRoadMeshHeight(worldPos, roadMesh, roadTransform).HasValue)
			{
				return;
			}
			float num = 0f;
			int num2 = 0;
			float num3 = 0f;
			for (int i = centerX - 2; i <= centerX + 2; i++)
			{
				for (int j = centerZ - 2; j <= centerZ + 2; j++)
				{
					if (i >= 0 && i < heightmapWidth && j >= 0 && j < heightmapHeight && (i != centerX || j != centerZ))
					{
						float num4 = Mathf.Sqrt((i - centerX) * (i - centerX) + (j - centerZ) * (j - centerZ));
						float num5 = 1f / (1f + num4);
						num += heights[j, i] * num5;
						num3 += num5;
						num2++;
					}
				}
			}
			if (num3 > 0f)
			{
				float b = num / num3;
				heights[centerZ, centerX] = Mathf.Lerp(a, b, 0.4f);
			}
		}

		private void ApplyMultiPassSmartFillCut(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData)
		{
			int num = Mathf.Max(1, _roadConfig.terrainCleanupIterations);
			for (int i = 0; i < num; i++)
			{
				ApplySmartFillCut(heights, roadMesh, roadTransform, roadBounds, heightmapWidth, heightmapHeight, terrainData);
				if (i > 0)
				{
					ApplyAdaptiveCleanup(heights, roadMesh, roadTransform, roadBounds, heightmapWidth, heightmapHeight, terrainData, i);
				}
			}
		}

		private void ApplyAdaptiveCleanup(float[,] heights, Mesh roadMesh, Transform roadTransform, Bounds roadBounds, int heightmapWidth, int heightmapHeight, TerrainData terrainData, int iteration)
		{
			Vector3 position = _terrain.transform.position;
			Vector3 size = terrainData.size;
			(int, int, int, int) tuple = CalculateHeightmapBounds(roadBounds, position, size, heightmapWidth, heightmapHeight);
			Vector3[] vertices = roadMesh.vertices;
			int[] triangles = roadMesh.triangles;
			float adaptiveStrength = _roadConfig.cuttingStrength * (1f + (float)iteration * 0.3f);
			var (i, _, _, _) = tuple;
			for (; i <= tuple.Item2; i++)
			{
				for (int j = tuple.Item3; j <= tuple.Item4; j++)
				{
					Vector3 worldPos = HeightmapToWorldPosition(i, j, position, size, heightmapWidth, heightmapHeight);
					ApplyAdaptiveCleanupAtPoint(heights, i, j, worldPos, vertices, triangles, roadTransform, position, size, adaptiveStrength);
				}
			}
		}

		private void ApplyAdaptiveCleanupAtPoint(float[,] heights, int x, int z, Vector3 worldPos, Vector3[] vertices, int[] triangles, Transform roadTransform, Vector3 terrainPosition, Vector3 terrainSize, float adaptiveStrength)
		{
			Vector3 point = roadTransform.InverseTransformPoint(worldPos);
			float num = heights[z, x] * terrainSize.y + terrainPosition.y;
			float num2 = _roadConfig.roadWidth * 0.5f;
			float num3 = num;
			float num4 = 3.4028235E+38f;
			bool flag = false;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 v = vertices[triangles[i]];
				Vector3 v2 = vertices[triangles[i + 1]];
				Vector3 v3 = vertices[triangles[i + 2]];
				float num5 = DistanceToTriangle2D(point, v, v2, v3);
				if (num5 < num4)
				{
					num4 = num5;
					float y = InterpolateTriangleHeight(point, v, v2, v3);
					num3 = roadTransform.TransformPoint(new Vector3(point.x, y, point.z)).y;
					flag = num5 <= num2;
				}
			}
			if (flag && num > num3 + _roadConfig.roadSurfaceAdjustment * 2f)
			{
				float b = num3 + _roadConfig.roadSurfaceAdjustment;
				float value = (Mathf.Lerp(num, b, adaptiveStrength) - terrainPosition.y) / terrainSize.y;
				heights[z, x] = Mathf.Clamp01(value);
			}
		}
	}
}
