using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

namespace NomadDrive.Features.EvilRoads
{
	public class RoadMeshGenerator
	{
		private struct SplineCache
		{
			public float3 position;

			public float3 tangent;

			public bool isValid;
		}

		private readonly EvilRoadConfig _roadConfig;

		private readonly Terrain _associatedTerrain;

		public RoadMeshGenerator(EvilRoadConfig roadConfig, Terrain associatedTerrain = null)
		{
			_roadConfig = roadConfig;
			_associatedTerrain = associatedTerrain;
		}

		public Mesh GenerateRoadMesh(Spline spline, int gameObjectId)
		{
			if (spline == null)
			{
				EvilLogger.LogError("Spline is null!", "GenerateRoadMesh", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\RoadMeshGenerator.cs", 33);
				return null;
			}
			if (spline.Count < 2)
			{
				EvilLogger.LogError($"Spline has insufficient points: {spline.Count}", "GenerateRoadMesh", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\RoadMeshGenerator.cs", 39);
				return null;
			}
			float splineLength = CalculateSplineLength(spline);
			int num = CalculateOptimalSegmentCount(spline, splineLength);
			int capacity = (num + 1) * 2;
			int capacity2 = num * 6;
			List<Vector3> list = new List<Vector3>(capacity);
			List<int> triangles = new List<int>(capacity2);
			List<Vector2> uvs = new List<Vector2>(capacity);
			GenerateOptimizedVerticesWithCache(spline, num, list, uvs, splineLength);
			GenerateTrianglesOptimized(triangles, num, list.Count);
			if (list.Count < 4)
			{
				EvilLogger.LogError("Insufficient valid vertices for mesh generation", "GenerateRoadMesh", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\RoadMeshGenerator.cs", 63);
				return null;
			}
			return CreateOptimizedMesh(list, triangles, uvs, gameObjectId);
		}

		private float CalculateSplineLength(Spline spline)
		{
			float num = 0f;
			float3 x = spline.EvaluatePosition(0f);
			for (int i = 1; i <= 50; i++)
			{
				float t = (float)i / 50f;
				float3 float5 = spline.EvaluatePosition(t);
				num += math.distance(x, float5);
				x = float5;
			}
			return num;
		}

		private int CalculateOptimalSegmentCount(Spline spline, float splineLength)
		{
			float num = Mathf.Max(0.05f, _roadConfig.meshIntensity / 8f);
			float num2 = splineLength * num;
			float num3 = 1f;
			if (_roadConfig.useAdaptiveMeshDensity)
			{
				float t = CalculateAverageCurvature(spline);
				num3 = Mathf.Lerp(1f, 2f, t);
			}
			return Mathf.Clamp(Mathf.RoundToInt(num2 * num3), 10, _roadConfig.maxMeshSegments);
		}

		private float CalculateAverageCurvature(Spline spline)
		{
			float num = 0f;
			for (int i = 0; i < 19; i++)
			{
				float t = (float)i / 19f;
				float t2 = (float)(i + 1) / 19f;
				float3 x = math.normalize(spline.EvaluateTangent(t));
				float3 y = math.normalize(spline.EvaluateTangent(t2));
				float num2 = math.abs(math.dot(x, y));
				num += 1f - num2;
			}
			return num / 19f;
		}

		private SplineCache[] CacheSplineData(Spline spline, int segmentCount)
		{
			SplineCache[] array = new SplineCache[segmentCount + 1];
			for (int i = 0; i <= segmentCount; i++)
			{
				float t = (float)i / (float)segmentCount;
				array[i] = new SplineCache
				{
					position = spline.EvaluatePosition(t),
					tangent = spline.EvaluateTangent(t),
					isValid = true
				};
				if (RoadHeightCalculator.HasNaN(array[i].position) || RoadHeightCalculator.HasNaN(array[i].tangent))
				{
					array[i].isValid = false;
				}
			}
			return array;
		}

		private void GenerateOptimizedVerticesWithCache(Spline spline, int meshSegments, List<Vector3> vertices, List<Vector2> uvs, float splineLength)
		{
			SplineCache[] array = CacheSplineData(spline, meshSegments);
			float3 previousRight = new float3(1f, 0f, 0f);
			float3 x = new float3(0f, 0f, 1f);
			float num = 0f;
			float3 y = ((array.Length != 0) ? array[0].position : float3.zero);
			float3 up = new float3(0f, 1f, 0f);
			float num2 = _roadConfig.roadWidth * 0.5f;
			for (int i = 0; i <= meshSegments; i++)
			{
				if (i < array.Length && array[i].isValid)
				{
					SplineCache splineCache = array[i];
					float3 position = splineCache.position;
					float3 tangent = splineCache.tangent;
					if (i > 0)
					{
						num += math.distance(position, y);
					}
					y = position;
					tangent = OptimizeTangent(spline, (float)i / (float)meshSegments, i, meshSegments, tangent);
					float3 float5 = CalculateRightVectorOptimized(up, tangent, previousRight, i);
					if (RoadHeightCalculator.HasNaN(float5))
					{
						float5 = new float3(1f, 0f, 0f);
					}
					float num3 = num2;
					if (_roadConfig.enableWidthVariation)
					{
						float num4 = Mathf.PerlinNoise(position.z * _roadConfig.widthVariationFrequency, position.x * _roadConfig.widthVariationFrequency) - 0.5f;
						num3 *= 1f + num4 * 2f * _roadConfig.widthVariationAmount;
					}
					float3 float6 = position + float5 * num3;
					float3 float7 = position - float5 * num3;
					if (_roadConfig.enableBanking && i > 0)
					{
						float num5 = Mathf.Clamp(math.cross(x, tangent).y * 4f, -1f, 1f) * _roadConfig.bankingStrength;
						float6.y += num5;
						float7.y -= num5;
					}
					if (!RoadHeightCalculator.HasNaN(float6) && !RoadHeightCalculator.HasNaN(float7))
					{
						vertices.Add(float6);
						vertices.Add(float7);
						float y2 = num / _roadConfig.textureTileLength;
						uvs.Add(new Vector2(0f, y2));
						uvs.Add(new Vector2(1f, y2));
						previousRight = float5;
						x = tangent;
					}
				}
			}
		}

		private float3 OptimizeTangent(Spline spline, float t, int index, int maxIndex, float3 tangent)
		{
			if (math.lengthsq(tangent) < 0.001f && spline.Count >= 2)
			{
				if (index == 0)
				{
					float3 obj = spline.EvaluatePosition(0.05f);
					float3 float5 = spline.EvaluatePosition(t);
					tangent = obj - float5;
				}
				else if (index == maxIndex)
				{
					float3 float6 = spline.EvaluatePosition(0.95f);
					tangent = spline.EvaluatePosition(t) - float6;
				}
			}
			tangent = ((!(math.lengthsq(tangent) < 0.001f)) ? math.normalize(tangent) : new float3(0f, 0f, 1f));
			return tangent;
		}

		private float3 CalculateRightVectorOptimized(float3 up, float3 tangent, float3 previousRight, int iteration)
		{
			float3 x = math.cross(up, tangent);
			if (math.lengthsq(x) > 0.001f)
			{
				float3 float5 = math.normalize(x);
				if (iteration > 0 && math.dot(float5, previousRight) < 0f)
				{
					float5 = -float5;
				}
				return float5;
			}
			return previousRight;
		}

		private void AdjustPointsToTerrain(ref float3 leftPoint, ref float3 rightPoint)
		{
		}

		private void GenerateTrianglesOptimized(List<int> triangles, int meshSegments, int vertexCount)
		{
			for (int i = 0; i < meshSegments; i++)
			{
				int num = i * 2;
				if (num + 3 < vertexCount)
				{
					triangles.Add(num);
					triangles.Add(num + 1);
					triangles.Add(num + 2);
					triangles.Add(num + 1);
					triangles.Add(num + 3);
					triangles.Add(num + 2);
				}
			}
		}

		private void GenerateTriangles(List<int> triangles, int meshSegments, int vertexCount)
		{
			GenerateTrianglesOptimized(triangles, meshSegments, vertexCount);
		}

		private Mesh CreateOptimizedMesh(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, int gameObjectId)
		{
			Mesh mesh = new Mesh();
			mesh.name = $"Road_Mesh_{gameObjectId}";
			mesh.indexFormat = ((vertices.Count > 65535) ? IndexFormat.UInt32 : IndexFormat.UInt16);
			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, 0);
			mesh.SetUVs(0, uvs);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.Optimize();
			return mesh;
		}

		private Mesh CreateMesh(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, int gameObjectId)
		{
			return CreateOptimizedMesh(vertices, triangles, uvs, gameObjectId);
		}

		public void UpdateMaterialTiling(MeshRenderer meshRenderer)
		{
			if (!(meshRenderer != null))
			{
				return;
			}
			Material material = (Application.isPlaying ? meshRenderer.material : meshRenderer.sharedMaterial);
			if (material != null)
			{
				if (material.HasProperty("_TilingX"))
				{
					material.SetFloat("_TilingX", _roadConfig.roadWidth / _roadConfig.textureTileLength);
				}
				if (material.HasProperty("_TilingY"))
				{
					material.SetFloat("_TilingY", 1f);
				}
				if (material.HasProperty("_RoadWidth"))
				{
					material.SetFloat("_RoadWidth", _roadConfig.roadWidth);
				}
			}
		}
	}
}
