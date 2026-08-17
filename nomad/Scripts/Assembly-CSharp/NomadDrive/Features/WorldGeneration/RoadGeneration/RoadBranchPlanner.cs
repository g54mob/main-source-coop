using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NomadDrive.Features.EvilRoads;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.WorldGeneration.POISpawning;
using UnityEngine;
using UnityEngine.Splines;

namespace NomadDrive.Features.WorldGeneration.RoadGeneration
{
	public class RoadBranchPlanner
	{
		private const int SplineSampleCount = 64;

		private readonly EvilRoadsManager _evilRoadsManager;

		private readonly RoadGenerationConfig _config;

		private readonly SeedManager _seedManager;

		public RoadBranchPlanner(EvilRoadsManager evilRoadsManager, RoadGenerationConfig config, SeedManager seedManager)
		{
			_evilRoadsManager = evilRoadsManager;
			_config = config;
			_seedManager = seedManager;
		}

		public async UniTask BuildBranchesAsync(TileInfo tileInfo, EvilRoad mainRoad, List<SpawnedPoiRecord> pois, Terrain terrain)
		{
			if (!_config.enableBranches || mainRoad == null || terrain == null || pois == null)
			{
				return;
			}
			Spline spline = mainRoad.GetSpline();
			if (spline == null || spline.Count < 2)
			{
				return;
			}
			foreach (SpawnedPoiRecord poi in pois)
			{
				if (!TryPlanSpur(poi, mainRoad, terrain, out var spurPoints))
				{
					continue;
				}
				EvilRoad evilRoad = await _evilRoadsManager.CreateBranchRoadAsync(spurPoints, terrain);
				if (tileInfo.road == null)
				{
					if (evilRoad != null)
					{
						_evilRoadsManager.DestroyRoad(evilRoad);
					}
					return;
				}
				if (evilRoad != null)
				{
					tileInfo.branchRoads.Add(evilRoad);
				}
			}
		}

		private bool TryPlanSpur(SpawnedPoiRecord poi, EvilRoad mainRoad, Terrain terrain, out Vector3[] spurPoints)
		{
			spurPoints = null;
			if (!poi.GenerateBranchRoad)
			{
				return false;
			}
			Vector3 vector = new Vector3(poi.Position.x, 0f, poi.Position.y);
			int positionBasedSeed = _seedManager.GetPositionBasedSeed(FloatingOriginManager.ToTrueWorld(vector));
			if ((float)((positionBasedSeed & 0x7FFFFFFF) % 10000) / 10000f > _config.branchToPoiChance)
			{
				return false;
			}
			Vector3 tangentWorld;
			Vector3 vector2 = NearestSplinePointWorld(mainRoad, vector, out tangentWorld);
			Vector2 vector3 = new Vector2(vector2.x, vector2.z);
			Vector2 vector4 = new Vector2(vector.x, vector.z);
			float num = Vector2.Distance(vector3, vector4);
			if (num < _config.branchMinPoiDistance || num > _config.branchMaxLength)
			{
				return false;
			}
			float num2 = num - (poi.DeformRadius + _config.branchEndMargin);
			if (num2 < _config.branchMinPoiDistance * 0.5f)
			{
				return false;
			}
			float num3 = mainRoad.GetRoadConfig().roadWidth * 0.5f;
			Vector2 vector5 = new Vector2(tangentWorld.x, tangentWorld.z);
			Vector2 vector6 = new Vector2(vector5.y, 0f - vector5.x);
			Vector2 vector7 = vector4 - vector3;
			float num4 = Mathf.Sign(Vector2.Dot(vector7, vector6));
			if (num4 == 0f)
			{
				num4 = 1f;
			}
			Vector2 vector8 = vector6 * num4;
			float num5 = Mathf.Min(_config.branchConnectBlend, num2 * 0.3f);
			Vector2 vector9 = vector3 + vector8 * (num3 + _config.branchEdgeOffset);
			Vector2 a = vector3 + vector8 * (num3 + _config.branchEdgeOffset + num5);
			Vector2 vector10 = vector7 / num;
			Vector2 vector11 = new Vector2(0f - vector10.y, vector10.x);
			Vector2 vector12 = vector3 + vector10 * num2;
			float num6 = ((((positionBasedSeed >> 8) & 1) == 0) ? 1f : (-1f)) * Mathf.Min(8f, num2 * 0.12f);
			Vector2 xz = Vector2.Lerp(a, vector12, 0.5f) + vector11 * num6;
			Vector3 vector13 = new Vector3(vector9.x, vector2.y, vector9.y);
			Vector3 vector14 = new Vector3(a.x, vector2.y, a.y);
			Vector3 vector15 = GroundPoint(xz, terrain);
			Vector3 vector16 = GroundPoint(vector12, terrain);
			spurPoints = new Vector3[4] { vector13, vector14, vector15, vector16 };
			return true;
		}

		private Vector3 GroundPoint(Vector2 xz, Terrain terrain)
		{
			float y = RoadHeightCalculator.GetTerrainHeight(new Vector3(xz.x, 0f, xz.y), terrain) + _config.branchRoadHeight;
			return new Vector3(xz.x, y, xz.y);
		}

		private static Vector3 NearestSplinePointWorld(EvilRoad road, Vector3 target, out Vector3 tangentWorld)
		{
			Spline spline = road.GetSpline();
			Transform transform = road.transform;
			float num = 3.4028235E+38f;
			Vector3 result = target;
			float num2 = 0f;
			Vector2 vector = new Vector2(target.x, target.z);
			for (int i = 0; i <= 64; i++)
			{
				float num3 = (float)i / 64f;
				Vector3 vector2 = transform.TransformPoint(spline.EvaluatePosition(num3));
				float sqrMagnitude = (new Vector2(vector2.x, vector2.z) - vector).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = vector2;
					num2 = num3;
				}
			}
			Vector3 vector3 = transform.TransformDirection(spline.EvaluateTangent(num2));
			Vector2 vector4 = new Vector2(vector3.x, vector3.z);
			if (vector4.sqrMagnitude < 1E-06f)
			{
				float t = Mathf.Min(1f, num2 + 1f / 64f);
				Vector3 vector5 = transform.TransformPoint(spline.EvaluatePosition(t));
				vector4 = new Vector2(vector5.x - result.x, vector5.z - result.z);
				if (vector4.sqrMagnitude < 1E-06f)
				{
					vector4 = new Vector2(0f, 1f);
				}
			}
			vector4 = vector4.normalized;
			tangentWorld = new Vector3(vector4.x, 0f, vector4.y);
			return result;
		}
	}
}
