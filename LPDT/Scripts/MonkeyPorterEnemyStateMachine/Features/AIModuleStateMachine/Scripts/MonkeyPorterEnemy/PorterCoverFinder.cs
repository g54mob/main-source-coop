using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class PorterCoverFinder
	{
		private const int CANDIDATE_CAPACITY = 512;

		private const float MAX_COVER_HEIGHT = 2.5f;

		private const float MAX_COVER_WIDTH = 4f;

		private const float CEILING_PROBE_OFFSET = 0.1f;

		private const float PROBE_SAMPLE_DISTANCE = 0.5f;

		private const float PROBE_INSET_SCALE = 0.35f;

		private const float ZONE_PROBE_INSET = 0.5f;

		private const float ZONE_SAMPLE_DISTANCE = 1.5f;

		private const float COVER_FLOOR_TOLERANCE = 0.5f;

		private const float MIN_DISTANCE_FROM_THREAT = 4f;

		private static readonly Vector2[] _probeOffsets = new Vector2[5]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(0f, -1f)
		};

		private static int _coverMask = -1;

		private readonly MonkeyPorterContext _context;

		private readonly MonkeyPorterSettings _settings;

		private readonly Collider[] _overlapBuffer = new Collider[512];

		private readonly List<PlayerSafeZone> _zoneCandidates = new List<PlayerSafeZone>();

		private readonly List<Collider> _geometryCandidates = new List<Collider>();

		private readonly HashSet<int> _rejected = new HashSet<int>();

		private int _lastCoverId;

		private bool _hasLastCover;

		private static int CoverMask
		{
			get
			{
				if (_coverMask < 0)
				{
					_coverMask = LayerMask.GetMask("Default", "NavMeshProp");
				}
				return _coverMask;
			}
		}

		public PorterCoverFinder(MonkeyPorterContext context, MonkeyPorterSettings settings)
		{
			_context = context;
			_settings = settings;
		}

		public void Reset()
		{
			_rejected.Clear();
			_hasLastCover = false;
		}

		public void RejectLastCover()
		{
			if (_hasLastCover)
			{
				_rejected.Add(_lastCoverId);
				_hasLastCover = false;
			}
		}

		public bool TryFindCover(Vector3 threatPosition, out Vector3 coverPosition)
		{
			CollectSafeZones();
			if (TryPickNearestZone(threatPosition, out coverPosition))
			{
				return true;
			}
			CollectGeometry();
			return TryPickNearestGeometry(threatPosition, out coverPosition);
		}

		private void CollectSafeZones()
		{
			Vector3 position = _context.transform.position;
			float num = _settings.CoverSearchRadius * _settings.CoverSearchRadius;
			_zoneCandidates.Clear();
			IReadOnlyList<PlayerSafeZone> activeZones = PlayerSafeZone.ActiveZones;
			for (int i = 0; i < activeZones.Count; i++)
			{
				PlayerSafeZone playerSafeZone = activeZones[i];
				if (!(playerSafeZone == null) && playerSafeZone.SafeZoneType == SafeZoneType.Table && !_rejected.Contains(playerSafeZone.GetInstanceID()) && !((playerSafeZone.transform.position - position).sqrMagnitude > num))
				{
					_zoneCandidates.Add(playerSafeZone);
				}
			}
			_zoneCandidates.Sort(CompareZonesByDistanceToPorter);
		}

		private void CollectGeometry()
		{
			int num = Physics.OverlapSphereNonAlloc(_context.transform.position, _settings.CoverSearchRadius, _overlapBuffer, CoverMask, QueryTriggerInteraction.Ignore);
			_geometryCandidates.Clear();
			for (int i = 0; i < num; i++)
			{
				Collider collider = _overlapBuffer[i];
				if (!(collider == null) && !_rejected.Contains(collider.GetInstanceID()) && HasCoverProfile(collider))
				{
					_geometryCandidates.Add(collider);
				}
			}
			_geometryCandidates.Sort(CompareCollidersByDistanceToPorter);
		}

		private bool TryPickNearestZone(Vector3 threatPosition, out Vector3 coverPosition)
		{
			coverPosition = default(Vector3);
			bool flag = false;
			Vector3 vector = default(Vector3);
			int instanceId = 0;
			for (int i = 0; i < _zoneCandidates.Count; i++)
			{
				PlayerSafeZone playerSafeZone = _zoneCandidates[i];
				if (!TryResolveZonePoint(playerSafeZone, out var point))
				{
					_rejected.Add(playerSafeZone.GetInstanceID());
					continue;
				}
				if (IsFarEnoughFromThreat(point, threatPosition))
				{
					coverPosition = point;
					RememberLastCover(playerSafeZone.GetInstanceID());
					return true;
				}
				if (!flag)
				{
					vector = point;
					instanceId = playerSafeZone.GetInstanceID();
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			coverPosition = vector;
			RememberLastCover(instanceId);
			return true;
		}

		private bool TryPickNearestGeometry(Vector3 threatPosition, out Vector3 coverPosition)
		{
			coverPosition = default(Vector3);
			bool flag = false;
			Vector3 vector = default(Vector3);
			int instanceId = 0;
			for (int i = 0; i < _geometryCandidates.Count; i++)
			{
				Collider collider = _geometryCandidates[i];
				if (!TryResolveGeometryPoint(collider, out var point))
				{
					_rejected.Add(collider.GetInstanceID());
					continue;
				}
				if (IsFarEnoughFromThreat(point, threatPosition))
				{
					coverPosition = point;
					RememberLastCover(collider.GetInstanceID());
					return true;
				}
				if (!flag)
				{
					vector = point;
					instanceId = collider.GetInstanceID();
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			coverPosition = vector;
			RememberLastCover(instanceId);
			return true;
		}

		private void RememberLastCover(int instanceId)
		{
			_lastCoverId = instanceId;
			_hasLastCover = true;
		}

		private static bool IsFarEnoughFromThreat(Vector3 point, Vector3 threatPosition)
		{
			return (point - threatPosition).sqrMagnitude >= 16f;
		}

		private int CompareZonesByDistanceToPorter(PlayerSafeZone left, PlayerSafeZone right)
		{
			Vector3 position = _context.transform.position;
			float sqrMagnitude = (left.transform.position - position).sqrMagnitude;
			float sqrMagnitude2 = (right.transform.position - position).sqrMagnitude;
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		}

		private int CompareCollidersByDistanceToPorter(Collider left, Collider right)
		{
			Vector3 position = _context.transform.position;
			float sqrMagnitude = (left.bounds.center - position).sqrMagnitude;
			float sqrMagnitude2 = (right.bounds.center - position).sqrMagnitude;
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		}

		private bool HasCoverProfile(Collider candidate)
		{
			if (candidate.isTrigger)
			{
				return false;
			}
			Bounds bounds = candidate.bounds;
			float num = Mathf.Max(bounds.size.x, bounds.size.z);
			float num2 = Mathf.Min(bounds.size.x, bounds.size.z);
			if (bounds.size.y <= 2.5f && num >= _settings.CoverMinWidth && num <= 4f)
			{
				return num2 >= _context.AgentRadius * 2f;
			}
			return false;
		}

		private bool TryResolveZonePoint(PlayerSafeZone zone, out Vector3 point)
		{
			point = default(Vector3);
			Vector3 position = zone.transform.position;
			for (int i = 0; i < _probeOffsets.Length; i++)
			{
				Vector3 desired = position + new Vector3(_probeOffsets[i].x, 0f, _probeOffsets[i].y) * 0.5f;
				if (_context.TrySampleAgentPosition(desired, 1.5f, out var position2) && HasCrawlSpace(position2) && _context.IsGoalReachable(position2))
				{
					point = position2;
					return true;
				}
			}
			return false;
		}

		private bool TryResolveGeometryPoint(Collider candidate, out Vector3 point)
		{
			point = default(Vector3);
			Bounds bounds = candidate.bounds;
			float num = bounds.extents.x * 0.35f;
			float num2 = bounds.extents.z * 0.35f;
			for (int i = 0; i < _probeOffsets.Length; i++)
			{
				Vector3 desired = new Vector3(bounds.center.x + _probeOffsets[i].x * num, bounds.min.y, bounds.center.z + _probeOffsets[i].y * num2);
				if (_context.TrySampleAgentPosition(desired, 0.5f, out var position) && IsUnderCandidate(bounds, position) && HasCrawlSpaceUnder(candidate, position) && _context.IsGoalReachable(position))
				{
					point = position;
					return true;
				}
			}
			return false;
		}

		private static bool IsUnderCandidate(Bounds bounds, Vector3 point)
		{
			if (point.y > bounds.min.y + 0.5f)
			{
				return false;
			}
			if (point.x >= bounds.min.x && point.x <= bounds.max.x && point.z >= bounds.min.z)
			{
				return point.z <= bounds.max.z;
			}
			return false;
		}

		private bool HasCrawlSpaceUnder(Collider candidate, Vector3 point)
		{
			Ray ray = new Ray(point + Vector3.up * 0.1f, Vector3.up);
			if (!candidate.Raycast(ray, out var hitInfo, _settings.CoverCeilingHeight))
			{
				return false;
			}
			return IsClearanceEnough(hitInfo.distance);
		}

		private bool HasCrawlSpace(Vector3 point)
		{
			if (!Physics.Raycast(point + Vector3.up * 0.1f, Vector3.up, out var hitInfo, _settings.CoverCeilingHeight, CoverMask, QueryTriggerInteraction.Ignore))
			{
				return false;
			}
			return IsClearanceEnough(hitInfo.distance);
		}

		private bool IsClearanceEnough(float probeDistance)
		{
			return probeDistance + 0.1f >= _settings.CoverMinClearance;
		}
	}
}
