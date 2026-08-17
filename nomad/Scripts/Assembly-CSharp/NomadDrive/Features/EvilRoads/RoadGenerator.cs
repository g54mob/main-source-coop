using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NomadDrive.Features.FloatingOrigin;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	public class RoadGenerator
	{
		private readonly float _naturalRoadHeight;

		private readonly float _segmentDistance;

		private readonly float _roadCurveAmplitude;

		private readonly float _roadCurveFrequency;

		private readonly bool _usePerlinNoise;

		private readonly float _poiCurveAmplitude;

		private readonly bool _enablePoiCurve;

		private readonly string _roadName;

		private readonly EvilRoadsManager _evilRoadsManager;

		private readonly bool _useMeander;

		private readonly int _meanderOctaves;

		private readonly float _meanderBaseAmplitude;

		private readonly float _meanderBaseFrequency;

		private readonly float _meanderPersistence;

		private readonly float _meanderLacunarity;

		private readonly float _maxMeanderAmplitude;

		private readonly int _meanderSeed;

		private readonly System.Random _random;

		private readonly float _perlinSeed;

		public RoadGenerator(RoadGenerationConfig roadGenerationConfig, EvilRoadsManager evilRoadsManager, int seed, int meanderSeed)
		{
			_roadName = roadGenerationConfig.roadName;
			_naturalRoadHeight = roadGenerationConfig.naturalRoadHeight;
			_segmentDistance = roadGenerationConfig.segmentDistance;
			_roadCurveAmplitude = roadGenerationConfig.roadCurveAmplitude;
			_roadCurveFrequency = roadGenerationConfig.roadCurveFrequency;
			_usePerlinNoise = roadGenerationConfig.usePerlinNoise;
			_poiCurveAmplitude = roadGenerationConfig.poiCurveAmplitude;
			_enablePoiCurve = roadGenerationConfig.enablePoiCurve;
			_useMeander = roadGenerationConfig.useMeander;
			_meanderOctaves = roadGenerationConfig.meanderOctaves;
			_meanderBaseAmplitude = roadGenerationConfig.meanderBaseAmplitude;
			_meanderBaseFrequency = roadGenerationConfig.meanderBaseFrequency;
			_meanderPersistence = roadGenerationConfig.meanderPersistence;
			_meanderLacunarity = roadGenerationConfig.meanderLacunarity;
			_maxMeanderAmplitude = roadGenerationConfig.maxMeanderAmplitude;
			_meanderSeed = meanderSeed;
			_evilRoadsManager = evilRoadsManager;
			_random = new System.Random(seed);
			_perlinSeed = (float)_random.NextDouble() * 10000f;
		}

		public EvilRoad CreateRoad(PoiRoadInfo[] poiRoadInfos, Terrain terrain)
		{
			Vector3 startPoint = CalculateTileBasedStartPoint(terrain);
			float tileLength = GetTileLength(terrain);
			return CreateRoadWithParams(poiRoadInfos, terrain, startPoint, tileLength);
		}

		public EvilRoad CreateRoadWithContinuity(PoiRoadInfo[] poiRoadInfos, Terrain terrain, float? previousRoadEndX)
		{
			Vector3 startPoint = CalculateTileBasedStartPoint(terrain);
			float tileLength = GetTileLength(terrain);
			if (previousRoadEndX.HasValue)
			{
				startPoint.x = previousRoadEndX.Value;
			}
			return CreateRoadWithParams(poiRoadInfos, terrain, startPoint, tileLength);
		}

		public EvilRoad CreateRoadWithBidirectionalContinuity(PoiRoadInfo[] poiRoadInfos, Terrain terrain, float? startX, float? endX)
		{
			Vector3 vector = CalculateTileBasedStartPoint(terrain);
			Vector3 endPoint = vector + Vector3.forward * GetTileLength(terrain);
			if (startX.HasValue)
			{
				vector.x = startX.Value;
			}
			if (endX.HasValue)
			{
				endPoint.x = endX.Value;
			}
			return CreateRoadWithBidirectionalParams(poiRoadInfos, terrain, vector, endPoint);
		}

		public async UniTask<EvilRoad> CreateRoadWithBidirectionalContinuityAsync(PoiRoadInfo[] poiRoadInfos, Terrain terrain, float? startX, float? endX, bool skipTerrainDeformation = false)
		{
			Vector3 vector = CalculateTileBasedStartPoint(terrain);
			Vector3 endPoint = vector + Vector3.forward * GetTileLength(terrain);
			if (startX.HasValue)
			{
				vector.x = startX.Value;
			}
			if (endX.HasValue)
			{
				endPoint.x = endX.Value;
			}
			_ = skipTerrainDeformation;
			return await CreateRoadWithBidirectionalParamsAsync(poiRoadInfos, terrain, vector, endPoint, skipTerrainDeformation);
		}

		private EvilRoad CreateRoadWithBidirectionalParams(PoiRoadInfo[] poiRoadInfos, Terrain terrain, Vector3 startPoint, Vector3 endPoint)
		{
			Vector3 startPoint2 = AdjustStartPointToTerrain(startPoint, terrain);
			Vector3 endPoint2 = AdjustEndPointToTerrain(endPoint, terrain);
			Vector3[] roadPoints = GenerateBidirectionalRoadPath(startPoint2, endPoint2, poiRoadInfos);
			Vector3[] roadPoints2 = ApplyPoiAvoidanceToAllPoints(roadPoints, poiRoadInfos);
			Vector3[] groundedRoadPoints = GetGroundedRoadPoints(roadPoints2, terrain);
			return _evilRoadsManager.CreateRoad(groundedRoadPoints, terrain);
		}

		private async UniTask<EvilRoad> CreateRoadWithBidirectionalParamsAsync(PoiRoadInfo[] poiRoadInfos, Terrain terrain, Vector3 startPoint, Vector3 endPoint, bool skipTerrainDeformation = false)
		{
			Vector3 startPoint2 = AdjustStartPointToTerrain(startPoint, terrain);
			Vector3 endPoint2 = AdjustEndPointToTerrain(endPoint, terrain);
			Vector3[] array = (_useMeander ? GenerateMeanderRoadPath(terrain) : GenerateBidirectionalRoadPath(startPoint2, endPoint2, poiRoadInfos));
			Vector3[] roadPoints = (_useMeander ? array : ApplyPoiAvoidanceToAllPoints(array, poiRoadInfos));
			Vector3[] groundedRoadPoints = GetGroundedRoadPoints(roadPoints, terrain);
			RoadBoundaryTangents boundary = (_useMeander ? ComputeMeanderBoundaryTangents(terrain) : default(RoadBoundaryTangents));
			if (skipTerrainDeformation)
			{
				return await _evilRoadsManager.CreateRoadAsync(groundedRoadPoints, null, null, overrideRoadObjects: false, boundary);
			}
			return await _evilRoadsManager.CreateRoadAsync(groundedRoadPoints, terrain, null, null, overrideRoadObjects: false, boundary);
		}

		private RoadBoundaryTangents ComputeMeanderBoundaryTangents(Terrain terrain)
		{
			Vector3 position = terrain.transform.position;
			Vector3 size = terrain.terrainData.size;
			float z = position.z;
			float renderZ = z + size.z;
			float maxAmplitude = RoadMeander.MaxAmplitude(size.x, _maxMeanderAmplitude);
			Vector3 startDir = RoadMeander.Direction(FloatingOriginManager.ToTrueWorldZ(z), _meanderOctaves, _meanderBaseAmplitude, _meanderBaseFrequency, _meanderPersistence, _meanderLacunarity, maxAmplitude, _meanderSeed);
			Vector3 endDir = RoadMeander.Direction(FloatingOriginManager.ToTrueWorldZ(renderZ), _meanderOctaves, _meanderBaseAmplitude, _meanderBaseFrequency, _meanderPersistence, _meanderLacunarity, maxAmplitude, _meanderSeed);
			return new RoadBoundaryTangents(startDir, endDir);
		}

		private Vector3[] GenerateBidirectionalRoadPath(Vector3 startPoint, Vector3 endPoint, PoiRoadInfo[] poiRoadInfos)
		{
			List<Vector3> list = new List<Vector3>();
			float num = Vector3.Distance(new Vector3(startPoint.x, 0f, startPoint.z), new Vector3(endPoint.x, 0f, endPoint.z));
			int num2 = Mathf.Max(2, Mathf.CeilToInt(num / _segmentDistance) + 1);
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)(num2 - 1);
				float distanceAlongRoad = num * num3;
				float num4 = Mathf.Lerp(startPoint.x, endPoint.x, num3);
				float z = Mathf.Lerp(startPoint.z, endPoint.z, num3);
				bool flag = i == 0;
				bool flag2 = i == num2 - 1;
				if (!flag && !flag2)
				{
					float num5 = Mathf.Sin(num3 * (float)Math.PI);
					float num6 = CalculateRoadCurve(distanceAlongRoad, _roadCurveAmplitude * 0.5f * num5);
					num4 += num6;
				}
				Vector3 vector = new Vector3(num4, 0f, z);
				float terrainHeightAtPosition = GetTerrainHeightAtPosition(vector);
				vector.y = terrainHeightAtPosition + _naturalRoadHeight;
				list.Add(vector);
			}
			return list.ToArray();
		}

		private Vector3[] GenerateMeanderRoadPath(Terrain terrain)
		{
			Vector3 position = terrain.transform.position;
			Vector3 size = terrain.terrainData.size;
			float num = position.x + size.x * 0.5f;
			float z = position.z;
			float b = z + size.z;
			int num2 = Mathf.Max(2, Mathf.CeilToInt(size.z / _segmentDistance) + 1);
			float maxAmplitude = RoadMeander.MaxAmplitude(size.x, _maxMeanderAmplitude);
			List<Vector3> list = new List<Vector3>(num2);
			for (int i = 0; i < num2; i++)
			{
				float t = (float)i / (float)(num2 - 1);
				float num3 = Mathf.Lerp(z, b, t);
				float x = num + MeanderX(FloatingOriginManager.ToTrueWorldZ(num3), maxAmplitude);
				float terrainHeightAtPosition = GetTerrainHeightAtPosition(new Vector3(x, 0f, num3));
				list.Add(new Vector3(x, terrainHeightAtPosition + _naturalRoadHeight, num3));
			}
			return list.ToArray();
		}

		private float MeanderX(float worldZ, float maxAmplitude)
		{
			return RoadMeander.Offset(worldZ, _meanderOctaves, _meanderBaseAmplitude, _meanderBaseFrequency, _meanderPersistence, _meanderLacunarity, maxAmplitude, _meanderSeed);
		}

		private Vector3 AdjustEndPointToTerrain(Vector3 endPoint, Terrain terrain)
		{
			float terrainHeightAtPosition = GetTerrainHeightAtPosition(endPoint);
			endPoint.y = terrainHeightAtPosition + _naturalRoadHeight;
			return endPoint;
		}

		private EvilRoad CreateRoadWithParams(PoiRoadInfo[] poiRoadInfos, Terrain terrain, Vector3 startPoint, float roadLength)
		{
			Vector3 startPoint2 = AdjustStartPointToTerrain(startPoint, terrain);
			Vector3[] roadPoints = GenerateRoadPath(startPoint2, roadLength, poiRoadInfos);
			Vector3[] roadPoints2 = ApplyPoiAvoidanceToAllPoints(roadPoints, poiRoadInfos);
			Vector3[] groundedRoadPoints = GetGroundedRoadPoints(roadPoints2, terrain);
			return _evilRoadsManager.CreateRoad(groundedRoadPoints, terrain);
		}

		private Vector3[] ApplyPoiAvoidanceToAllPoints(Vector3[] roadPoints, PoiRoadInfo[] poiRoadInfos)
		{
			if (poiRoadInfos == null || poiRoadInfos.Length == 0)
			{
				return roadPoints;
			}
			Vector3[] array = new Vector3[roadPoints.Length];
			for (int i = 0; i < roadPoints.Length; i++)
			{
				Vector3 vector = roadPoints[i];
				bool num = i == 0;
				bool flag = i == roadPoints.Length - 1;
				if (num || flag)
				{
					array[i] = vector;
					continue;
				}
				Vector3.Distance(vector, array[i] = ApplyPoiInfluence(vector, poiRoadInfos));
				_ = 5f;
			}
			return array;
		}

		private Vector3 ApplyPoiInfluence(Vector3 roadPoint, PoiRoadInfo[] allPois)
		{
			if (allPois == null || allPois.Length == 0)
			{
				return roadPoint;
			}
			Vector3 vector = roadPoint;
			Vector3 vector2 = new Vector3(roadPoint.x, 0f, roadPoint.z);
			for (int i = 0; i < allPois.Length; i++)
			{
				PoiRoadInfo poiRoadInfo = allPois[i];
				Vector3 vector3 = new Vector3(poiRoadInfo.pos.x, 0f, poiRoadInfo.pos.z);
				float radiusOffset = poiRoadInfo.radiusOffset;
				float num = Vector3.Distance(vector2, vector3);
				if (radiusOffset < 0f)
				{
					float num2 = Mathf.Abs(radiusOffset);
					float num3 = num2 * 3f;
					if (num < num3)
					{
						Vector3 vector4 = vector3 - vector2;
						if (!(vector4.sqrMagnitude < 0.001f))
						{
							vector4.Normalize();
							float f = 1f - num / num3;
							f = Mathf.Pow(f, 2f);
							float num4 = num2;
							float num5 = Mathf.Clamp((0f - (num - num4)) * f * 0.3f, -10f, 10f);
							Vector3 vector5 = vector4 * num5;
							vector += vector5;
						}
					}
				}
				else
				{
					if (!(radiusOffset > 0f))
					{
						continue;
					}
					float num6 = radiusOffset;
					if (num < num6)
					{
						Vector3 vector6 = vector2 - vector3;
						if (vector6.sqrMagnitude < 0.001f)
						{
							vector6 = new Vector3((float)(_random.NextDouble() * 2.0 - 1.0), 0f, (float)(_random.NextDouble() * 2.0 - 1.0));
						}
						vector6.Normalize();
						float value = num6 - num + 2f;
						value = Mathf.Clamp(value, 0f, 20f);
						Vector3 vector7 = vector6 * value;
						vector += vector7;
					}
				}
			}
			vector.y = roadPoint.y;
			float num7 = 50f;
			Vector3 vector8 = vector - roadPoint;
			if (vector8.magnitude > num7)
			{
				vector8 = vector8.normalized * num7;
				vector = roadPoint + vector8;
			}
			return vector;
		}

		private Vector3 EnsureAllPoiDistances(Vector3 roadPoint, PoiRoadInfo[] allPois)
		{
			Vector3 result = roadPoint;
			int num = 10;
			for (int i = 0; i < num; i++)
			{
				bool flag = false;
				Vector3 zero = Vector3.zero;
				for (int j = 0; j < allPois.Length; j++)
				{
					PoiRoadInfo poiRoadInfo = allPois[j];
					Vector3 pos = poiRoadInfo.pos;
					float radiusOffset = poiRoadInfo.radiusOffset;
					Vector3 vector = new Vector3(result.x, 0f, result.z);
					Vector3 vector2 = new Vector3(pos.x, 0f, pos.z);
					float num2 = Vector3.Distance(vector, vector2);
					if (num2 < radiusOffset)
					{
						flag = true;
						Vector3 vector3 = (vector - vector2).normalized;
						if (vector3.magnitude < 0.01f)
						{
							vector3 = ((_random.NextDouble() > 0.5) ? Vector3.right : Vector3.left);
						}
						float num3 = radiusOffset - num2 + 2f;
						zero += vector3 * num3;
					}
				}
				if (!flag)
				{
					break;
				}
				result += zero;
				result.y = roadPoint.y;
			}
			return result;
		}

		private Vector3 CalculateTileBasedStartPoint(Terrain terrain)
		{
			if (terrain == null)
			{
				return new Vector3(0f, _naturalRoadHeight, 0f);
			}
			Vector3 position = terrain.transform.position;
			TerrainData terrainData = terrain.terrainData;
			Vector3 vector = new Vector3(position.x + terrainData.size.x * 0.5f, 0f, position.z);
			float terrainHeightAtPosition = GetTerrainHeightAtPosition(vector);
			vector.y = terrainHeightAtPosition + _naturalRoadHeight;
			return vector;
		}

		private float GetTileLength(Terrain terrain)
		{
			if (terrain == null)
			{
				return 1000f;
			}
			return terrain.terrainData.size.z;
		}

		private Vector3 CalculateOptimalStartPoint(PoiRoadInfo[] poiRoadInfos)
		{
			Vector3 result = new Vector3(0f, 0f, 200f);
			if (poiRoadInfos == null || poiRoadInfos.Length == 0)
			{
				return result;
			}
			PoiRoadInfo poiRoadInfo = default(PoiRoadInfo);
			float num = 3.4028235E+38f;
			for (int i = 0; i < poiRoadInfos.Length; i++)
			{
				PoiRoadInfo poiRoadInfo2 = poiRoadInfos[i];
				if (poiRoadInfo2.pos.z < num)
				{
					num = poiRoadInfo2.pos.z;
					poiRoadInfo = poiRoadInfo2;
				}
			}
			Vector3 result2 = poiRoadInfo.pos - Vector3.forward * (poiRoadInfo.radiusOffset + 100f);
			result2.y = _naturalRoadHeight;
			return result2;
		}

		private Vector3[] GetGroundedRoadPoints(Vector3[] roadPoints, Terrain terrain)
		{
			Vector3[] array = new Vector3[roadPoints.Length];
			for (int i = 0; i < roadPoints.Length; i++)
			{
				array[i] = GetTerrainPosition(roadPoints[i], terrain);
			}
			return array;
		}

		private Vector3 GetTerrainPosition(Vector3 position, Terrain terrain)
		{
			if (terrain == null)
			{
				return Vector3.zero;
			}
			TerrainData terrainData = terrain.terrainData;
			Vector3 position2 = terrain.transform.position;
			Vector3 vector = position - position2;
			float x = vector.x / terrainData.size.x;
			float y = vector.z / terrainData.size.z;
			float interpolatedHeight = terrainData.GetInterpolatedHeight(x, y);
			return new Vector3(position.x, position2.y + interpolatedHeight, position.z);
		}

		private Vector3[] GenerateRoadPath(Vector3 startPoint, float totalLengthInMeters, PoiRoadInfo[] poiRoadInfos)
		{
			return GenerateStraightRoadPath(startPoint, totalLengthInMeters);
		}

		private Vector3[] GenerateStraightRoadPath(Vector3 startPoint, float totalLengthInMeters)
		{
			List<Vector3> list = new List<Vector3>();
			int num = Mathf.Max(2, Mathf.CeilToInt(totalLengthInMeters / _segmentDistance) + 1);
			Vector3 vector = new Vector3(startPoint.x, 0f, startPoint.z + totalLengthInMeters);
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / (float)(num - 1);
				float distanceAlongRoad = totalLengthInMeters * num2;
				float z = Mathf.Lerp(startPoint.z, vector.z, num2);
				float x = startPoint.x;
				float num3 = CalculateRoadCurve(distanceAlongRoad, _roadCurveAmplitude);
				x += num3;
				Vector3 vector2 = new Vector3(x, 0f, z);
				float terrainHeightAtPosition = GetTerrainHeightAtPosition(vector2);
				vector2.y = terrainHeightAtPosition + _naturalRoadHeight;
				list.Add(vector2);
			}
			return list.ToArray();
		}

		private Vector3[] GetNaturalRoadPointsWithPoi(Vector3 startPoint, float totalLengthInMeters, PoiRoadInfo[] poiRoadInfos)
		{
			List<PathSegment> list = CreatePathSegmentsWithPoi(startPoint, totalLengthInMeters, poiRoadInfos);
			List<Vector3> list2 = new List<Vector3>();
			foreach (PathSegment item in list)
			{
				Vector3[] array = GenerateSegmentPoints(item);
				for (int i = ((list2.Count > 0) ? 1 : 0); i < array.Length; i++)
				{
					list2.Add(array[i]);
				}
			}
			return list2.ToArray();
		}

		private List<PathSegment> CreatePathSegmentsWithPoi(Vector3 startPoint, float totalLength, PoiRoadInfo[] poiData)
		{
			List<PathSegment> list = new List<PathSegment>();
			List<PoiRoadInfo> list2 = new List<PoiRoadInfo>();
			if (poiData != null)
			{
				for (int i = 0; i < poiData.Length; i++)
				{
					PoiRoadInfo item = poiData[i];
					float num = item.pos.z - startPoint.z;
					if (num >= 0f && num <= totalLength)
					{
						list2.Add(item);
					}
				}
				list2.Sort((PoiRoadInfo a, PoiRoadInfo b) => a.pos.z.CompareTo(b.pos.z));
			}
			Vector3 startPoint2 = startPoint;
			float num2 = 0f;
			foreach (PoiRoadInfo item4 in list2)
			{
				float num3 = item4.pos.z - startPoint2.z;
				if (num3 > 0f)
				{
					PathSegment item2 = new PathSegment
					{
						StartPoint = startPoint2,
						EndPoint = GetPoiTargetPoint(item4.pos, item4.radiusOffset),
						Length = num3,
						StartDistance = num2,
						HasPoiTarget = true,
						TargetPoi = item4.pos,
						PoiRadius = item4.radiusOffset
					};
					list.Add(item2);
					startPoint2 = item2.EndPoint;
					num2 += num3;
				}
			}
			float num4 = totalLength - num2;
			if (num4 > 0f)
			{
				Vector3 vector = new Vector3(startPoint.x, 0f, startPoint.z + totalLength);
				float terrainHeightAtPosition = GetTerrainHeightAtPosition(vector);
				vector.y = terrainHeightAtPosition + _naturalRoadHeight;
				PathSegment item3 = new PathSegment
				{
					StartPoint = startPoint2,
					EndPoint = vector,
					Length = num4,
					StartDistance = num2,
					HasPoiTarget = false,
					TargetPoi = Vector3.zero,
					PoiRadius = 0f
				};
				list.Add(item3);
			}
			return list;
		}

		private Vector3 GetPoiTargetPoint(Vector3 poi, float poiRadius)
		{
			float num = Mathf.Abs(poiRadius);
			Vector3 vector = ((!(poi.x >= 0f)) ? new Vector3(poi.x + num, 0f, poi.z) : new Vector3(poi.x - num, 0f, poi.z));
			if (Mathf.Abs(Vector3.Distance(new Vector3(poi.x, 0f, poi.z), new Vector3(vector.x, 0f, vector.z)) - num) > 0.1f)
			{
				Vector3 normalized = (vector - poi).normalized;
				vector = poi + normalized * num;
			}
			float terrainHeightAtPosition = GetTerrainHeightAtPosition(vector);
			vector.y = terrainHeightAtPosition + _naturalRoadHeight;
			return vector;
		}

		private Vector3[] GenerateSegmentPoints(PathSegment segment)
		{
			int num = Mathf.Max(2, Mathf.CeilToInt(segment.Length / _segmentDistance) + 1);
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < num; i++)
			{
				float t = (float)i / (float)(num - 1);
				Vector3 item = CalculateSegmentPoint(segment, t);
				list.Add(item);
			}
			return list.ToArray();
		}

		private Vector3 CalculateSegmentPoint(PathSegment segment, float t)
		{
			float num = Mathf.Lerp(segment.StartPoint.x, segment.EndPoint.x, t);
			float z = Mathf.Lerp(segment.StartPoint.z, segment.EndPoint.z, t);
			float distanceAlongRoad = segment.StartDistance + segment.Length * t;
			float num2 = 0f;
			if (segment.HasPoiTarget && _enablePoiCurve)
			{
				num2 = CalculateRoadCurve(distanceAlongRoad, _poiCurveAmplitude);
			}
			else if (!segment.HasPoiTarget)
			{
				num2 = CalculateRoadCurve(distanceAlongRoad, _roadCurveAmplitude);
			}
			num += num2;
			Vector3 vector = new Vector3(num, 0f, z);
			float terrainHeightAtPosition = GetTerrainHeightAtPosition(vector);
			vector.y = terrainHeightAtPosition + _naturalRoadHeight;
			return vector;
		}

		private float CalculateRoadCurve(float distanceAlongRoad, float amplitude)
		{
			if (_usePerlinNoise)
			{
				return (Mathf.PerlinNoise(distanceAlongRoad * _roadCurveFrequency + _perlinSeed, _perlinSeed) - 0.5f) * 2f * amplitude;
			}
			return Mathf.Sin(distanceAlongRoad * _roadCurveFrequency * 2f * (float)Math.PI) * amplitude;
		}

		private static float GetTerrainHeightAtPosition(Vector3 worldPos)
		{
			Terrain activeTerrain = Terrain.activeTerrain;
			if (activeTerrain != null)
			{
				TerrainData terrainData = activeTerrain.terrainData;
				Vector3 position = activeTerrain.transform.position;
				Vector3 vector = worldPos - position;
				if (vector.x < 0f || vector.x > terrainData.size.x || vector.z < 0f || vector.z > terrainData.size.z)
				{
					return 0f;
				}
				float x = vector.x / terrainData.size.x;
				float y = vector.z / terrainData.size.z;
				float interpolatedHeight = terrainData.GetInterpolatedHeight(x, y);
				return position.y + interpolatedHeight;
			}
			return 0f;
		}

		private Vector3 AdjustStartPointToTerrain(Vector3 startPoint, Terrain terrain)
		{
			float terrainHeightAtPosition = GetTerrainHeightAtPosition(startPoint);
			startPoint.y = terrainHeightAtPosition + _naturalRoadHeight;
			return startPoint;
		}
	}
}
