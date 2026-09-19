using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.NavigationModule.Scripts
{
	public class NavigationService : INavigationService
	{
		private const int GROUND_PROBE_HIT_CAPACITY = 16;

		private const float GROUND_PROBE_TIE_EPSILON = 0.001f;

		private readonly RaycastHit[] _groundProbeHits = new RaycastHit[16];

		private readonly NavigationConfiguration _navigationConfiguration;

		private readonly PlayerRaycastPointsModel _raycastPointsModel;

		private readonly IPlayerTrackingPositionService _playerTrackingPositionService;

		public NavigationService(NavigationConfiguration navigationConfiguration, PlayerRaycastPointsModel raycastPointsModel, IPlayerTrackingPositionService playerTrackingPositionService)
		{
			_navigationConfiguration = navigationConfiguration;
			_raycastPointsModel = raycastPointsModel;
			_playerTrackingPositionService = playerTrackingPositionService;
		}

		public bool TryGetPlayerTrackingPosition(PlayerRef player, out Vector3 position)
		{
			return _playerTrackingPositionService.TryGetTrackingPosition(player, out position);
		}

		public bool TryGetRandomNavmeshPosition(Vector3 center, float radius, out Vector3 position)
		{
			Vector2 vector = Random.insideUnitCircle * radius;
			if (NavMesh.SamplePosition(center + new Vector3(vector.x, 0f, vector.y), out var hit, radius, -1))
			{
				position = hit.position;
				return true;
			}
			position = Vector3.zero;
			return false;
		}

		public bool TryGetRandomSafeNavmeshPosition(Vector3 center, float radius, int attempts, out Vector3 position)
		{
			int areaMask = -1;
			position = Vector3.zero;
			Vector3 vector = Vector3.zero;
			float num = 0f;
			bool flag = false;
			for (int i = 0; i < attempts; i++)
			{
				Vector2 vector2 = Random.insideUnitCircle * radius;
				if (NavMesh.SamplePosition(center + new Vector3(vector2.x, 0f, vector2.y), out var hit, radius, areaMask))
				{
					Vector3 vector3 = hit.position - center;
					vector3.y = 0f;
					float sqrMagnitude = vector3.sqrMagnitude;
					if (!flag || sqrMagnitude > num)
					{
						vector = hit.position;
						num = sqrMagnitude;
						flag = true;
					}
				}
			}
			if (flag)
			{
				position = vector;
				return true;
			}
			return false;
		}

		public bool TryGetRandomSafeNavmeshPositionWithPathFinding(Vector3 center, float radius, float maxPathLength, int attempts, out Vector3 position)
		{
			int areaMask = -1;
			position = Vector3.zero;
			Vector3 vector = Vector3.zero;
			float num = 0f;
			bool flag = false;
			for (int i = 0; i < attempts; i++)
			{
				Vector2 vector2 = Random.insideUnitCircle * radius;
				if (!NavMesh.SamplePosition(center + new Vector3(vector2.x, 0f, vector2.y), out var hit, radius, areaMask))
				{
					continue;
				}
				Vector3 vector3 = hit.position - center;
				vector3.y = 0f;
				float sqrMagnitude = vector3.sqrMagnitude;
				if (!flag || sqrMagnitude > num)
				{
					NavMeshPath path = new NavMeshPath();
					if (NavMesh.CalculatePath(new Vector3(center.x, hit.position.y, center.z), hit.position, areaMask, path) && path.GetLength() <= maxPathLength)
					{
						vector = hit.position;
						num = sqrMagnitude;
						flag = true;
					}
				}
			}
			if (flag)
			{
				position = vector;
				return true;
			}
			return false;
		}

		public bool TryGetRandomSafeNavmeshPosition(Vector3 center, float searchRadius, int attempts, List<Vector3> avoidPositions, int agentTypeID, int areaMask, out Vector3 bestPosition, float minDistanceFromCenter = 0f, float centerDistanceWeight = 1f, float averageAvoidDistanceWeight = 1f, float tooClosePenaltyRadius = 0f, float tooClosePenaltyWeight = 0f)
		{
			bestPosition = Vector3.zero;
			bool flag = false;
			float num = float.NegativeInfinity;
			Vector3 vector = Vector3.zero;
			float num2 = minDistanceFromCenter * minDistanceFromCenter;
			float num3 = tooClosePenaltyRadius * tooClosePenaltyRadius;
			bool flag2 = IsRegisteredAgentType(agentTypeID);
			NavMeshQueryFilter filter = new NavMeshQueryFilter
			{
				agentTypeID = agentTypeID,
				areaMask = areaMask
			};
			Vector3 vector2 = center;
			vector2.y = 0f;
			bool flag3 = avoidPositions != null && avoidPositions.Count > 0;
			for (int i = 0; i < attempts; i++)
			{
				Vector2 vector3 = Random.insideUnitCircle * searchRadius;
				Vector3 sourcePosition = center + new Vector3(vector3.x, 0f, vector3.y);
				if (!(flag2 ? NavMesh.SamplePosition(sourcePosition, out var hit, searchRadius, filter) : NavMesh.SamplePosition(sourcePosition, out hit, searchRadius, areaMask)))
				{
					continue;
				}
				Vector3 position = hit.position;
				Vector3 vector4 = position;
				vector4.y = 0f;
				float sqrMagnitude = (vector4 - vector2).sqrMagnitude;
				if (minDistanceFromCenter > 0f && sqrMagnitude < num2)
				{
					continue;
				}
				float num4 = Mathf.Sqrt(sqrMagnitude);
				if (!flag3)
				{
					float num5 = centerDistanceWeight * num4;
					if (!flag || num5 > num)
					{
						num = num5;
						vector = position;
						flag = true;
					}
					continue;
				}
				float num6 = 0f;
				float num7 = 0f;
				for (int j = 0; j < avoidPositions.Count; j++)
				{
					Vector3 vector5 = avoidPositions[j];
					vector5.y = 0f;
					float sqrMagnitude2 = (vector4 - vector5).sqrMagnitude;
					float num8 = Mathf.Sqrt(sqrMagnitude2);
					num6 += num8;
					if (tooClosePenaltyRadius > 0f && sqrMagnitude2 < num3)
					{
						float num9 = 1f - num8 / tooClosePenaltyRadius;
						num7 += num9;
					}
				}
				float num10 = num6 / (float)avoidPositions.Count;
				float num11 = centerDistanceWeight * num4 + averageAvoidDistanceWeight * num10 - tooClosePenaltyWeight * num7;
				if (!flag || num11 > num)
				{
					num = num11;
					vector = position;
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			bestPosition = vector;
			return true;
		}

		public Vector3 ValidatePointOnNavmesh(Vector3 point, float radius)
		{
			if (!NavMesh.SamplePosition(point, out var hit, radius, -1))
			{
				return point;
			}
			return hit.position;
		}

		public bool IsPointOnNavMeshProjected(Vector3 point, NavMeshAgent agent, out NavMeshHit hit)
		{
			NavMeshQueryFilter filter = new NavMeshQueryFilter
			{
				agentTypeID = agent.agentTypeID,
				areaMask = agent.areaMask
			};
			return NavMesh.SamplePosition(GetProjectedPoint(point), out hit, _navigationConfiguration.PointEqualityApproximation, filter);
		}

		public bool IsPlayerOnReachablePoint(PlayerRef targetPlayer, NavMeshAgent agent, out PlayerReachableData reachableData)
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			reachableData = new PlayerReachableData();
			if (!IsAgentReadyForPathQuery(agent))
			{
				return false;
			}
			if (!TryGetPlayerTrackingPosition(targetPlayer, out var position))
			{
				return false;
			}
			if (!IsPointOnNavMeshProjected(position, agent, out var hit))
			{
				return false;
			}
			if (!agent.CalculatePath(hit.position, navMeshPath))
			{
				return false;
			}
			reachableData.NavMeshProjectedHit = hit;
			reachableData.pathToPoint = navMeshPath;
			return true;
		}

		public bool IsPlayerOnReachablePoint(PlayerRef targetPlayer, NavMeshAgent agent, float detectionDistance, out PlayerReachableData reachableData)
		{
			if (IsPlayerOnReachablePoint(targetPlayer, agent, out reachableData))
			{
				return reachableData.pathToPoint.GetLength() <= detectionDistance;
			}
			return false;
		}

		public bool HasAvailablePointInRange(NavMeshAgent agent, Vector3 position, float range, out Vector3 availablePosition)
		{
			availablePosition = position;
			if (!IsAgentReadyForPathQuery(agent))
			{
				return false;
			}
			if (IsPointOnNavMeshProjected(position, agent, out var hit))
			{
				position = hit.position;
			}
			availablePosition = position;
			if (NavMesh.SamplePosition(position, out var hit2, range * 0.8f, agent.areaMask))
			{
				NavMeshPath navMeshPath = new NavMeshPath();
				agent.CalculatePath(hit2.position, navMeshPath);
				if (navMeshPath.status == NavMeshPathStatus.PathPartial && Vector3.Distance(position, availablePosition = navMeshPath.corners.Last()) <= range)
				{
					return true;
				}
				if (navMeshPath.status == NavMeshPathStatus.PathComplete)
				{
					availablePosition = hit2.position;
					return true;
				}
				_ = navMeshPath.status;
				_ = 2;
				return false;
			}
			return false;
		}

		public bool IsPointOnNavMeshProjected(Vector3 point, out NavMeshHit hit)
		{
			return NavMesh.SamplePosition(GetProjectedPoint(point), out hit, _navigationConfiguration.PointEqualityApproximation, -1);
		}

		public bool TryGetPointOnNavMeshProjected(Vector3 point, out Vector3 projectedPoint)
		{
			return TryGetPointOnNavMeshProjected(point, _navigationConfiguration.PointProjectionRadius, out projectedPoint);
		}

		public bool TryGetPointOnNavMeshProjected(Vector3 point, float radius, out Vector3 projectedPoint)
		{
			projectedPoint = Vector3.zero;
			if (!NavMesh.SamplePosition(GetProjectedPoint(point), out var hit, radius, -1))
			{
				return false;
			}
			projectedPoint = hit.position;
			return true;
		}

		public bool IsPointOnNavMeshProjected(Vector3 point, int agentType, out NavMeshHit hit)
		{
			NavMeshQueryFilter filter = new NavMeshQueryFilter
			{
				agentTypeID = agentType,
				areaMask = -1
			};
			return NavMesh.SamplePosition(GetProjectedPoint(point), out hit, _navigationConfiguration.PointEqualityApproximation, filter);
		}

		public List<Vector3> FindPath(IReadOnlyList<NavPoint> points, int startIndex, int endIndex, bool projectPoints)
		{
			int count = points.Count;
			Dictionary<NavPoint, int> dictionary = new Dictionary<NavPoint, int>();
			for (int i = 0; i < count; i++)
			{
				dictionary[points[i]] = i;
			}
			Dictionary<int, List<(int, float)>> dictionary2 = new Dictionary<int, List<(int, float)>>();
			for (int j = 0; j < count; j++)
			{
				dictionary2[j] = new List<(int, float)>();
			}
			NavMeshPath path = new NavMeshPath();
			for (int k = 0; k < count; k++)
			{
				foreach (NavPoint item in points[k].Adjacent)
				{
					if (dictionary.TryGetValue(item, out var value) && value > k)
					{
						float navMeshDistance = GetNavMeshDistance(points[k].Position, item.Position, path, projectPoints);
						if (navMeshDistance >= 0f)
						{
							dictionary2[k].Add((value, navMeshDistance));
							dictionary2[value].Add((k, navMeshDistance));
						}
					}
				}
			}
			return Dijkstra(points.Select((NavPoint p) => p.Position).ToList(), dictionary2, count, startIndex, endIndex);
		}

		public bool TryGetPathCorners(Vector3 from, Vector3 to, float radius, out List<Vector3> corners)
		{
			corners = null;
			if (!TryGetPointOnNavMeshProjected(from, radius, out var projectedPoint))
			{
				return false;
			}
			if (!TryGetPointOnNavMeshProjected(to, radius, out var projectedPoint2))
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (!NavMesh.CalculatePath(projectedPoint, projectedPoint2, -1, navMeshPath))
			{
				return false;
			}
			if (navMeshPath.status == NavMeshPathStatus.PathInvalid || navMeshPath.corners.Length == 0)
			{
				return false;
			}
			corners = new List<Vector3>(navMeshPath.corners);
			return true;
		}

		public bool TryGetPathPolyline(Vector3 from, Vector3 to, float radius, float pointSpacing, int maxPointsPerSegment, float groundProbeDistance, out List<Vector3> polyline)
		{
			polyline = null;
			if (!TryGetPathCorners(from, to, radius, out var corners))
			{
				return false;
			}
			polyline = new List<Vector3>(corners.Count);
			polyline.Add(corners[0]);
			for (int i = 1; i < corners.Count; i++)
			{
				Vector3 a = corners[i - 1];
				Vector3 vector = corners[i];
				int extraPointCount = GetExtraPointCount(Vector3.Distance(a, vector), pointSpacing, maxPointsPerSegment);
				for (int j = 1; j <= extraPointCount; j++)
				{
					Vector3 point = Vector3.Lerp(a, vector, (float)j / (float)(extraPointCount + 1));
					if (TryProjectPointOnGround(point, groundProbeDistance, out var groundPoint))
					{
						polyline.Add(groundPoint);
					}
				}
				polyline.Add(vector);
			}
			return true;
		}

		private bool TryProjectPointOnGround(Vector3 point, float probeDistance, out Vector3 groundPoint)
		{
			groundPoint = point;
			int num = Physics.RaycastNonAlloc(point + Vector3.up * probeDistance, Vector3.down, _groundProbeHits, probeDistance * 2f, _navigationConfiguration.FloorLayerMask, QueryTriggerInteraction.Ignore);
			if (num == 0)
			{
				return false;
			}
			bool flag = false;
			float num2 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				Vector3 point2 = _groundProbeHits[i].point;
				float num3 = Mathf.Abs(point2.y - point.y);
				if (flag)
				{
					bool num4 = num3 > num2 + 0.001f;
					bool flag2 = num3 >= num2 - 0.001f && point2.y >= groundPoint.y;
					if (num4 || flag2)
					{
						continue;
					}
				}
				num2 = num3;
				groundPoint = point2;
				flag = true;
			}
			return flag;
		}

		private int GetExtraPointCount(float segmentLength, float pointSpacing, int maxPointsPerSegment)
		{
			if (pointSpacing <= 0f || maxPointsPerSegment <= 0)
			{
				return 0;
			}
			return Mathf.Clamp(Mathf.FloorToInt(segmentLength / pointSpacing), 0, maxPointsPerSegment);
		}

		private Vector3 GetProjectedPoint(Vector3 point)
		{
			if (!Physics.Raycast(point, Vector3.down, out var hitInfo, _navigationConfiguration.PointProjectionDistance, _navigationConfiguration.FloorLayerMask))
			{
				return point;
			}
			return hitInfo.point;
		}

		private float GetNavMeshDistance(Vector3 from, Vector3 to, NavMeshPath path, bool projectPoints)
		{
			path.ClearCorners();
			if (projectPoints)
			{
				if (!TryGetPointOnNavMeshProjected(from, out var projectedPoint))
				{
					return -1f;
				}
				if (!TryGetPointOnNavMeshProjected(to, out var projectedPoint2))
				{
					return -1f;
				}
				from = projectedPoint;
				to = projectedPoint2;
			}
			if (!NavMesh.CalculatePath(from, to, -1, path))
			{
				return -1f;
			}
			if (path.status != NavMeshPathStatus.PathComplete)
			{
				return -1f;
			}
			Vector3[] corners = path.corners;
			float num = 0f;
			for (int i = 0; i < corners.Length - 1; i++)
			{
				num += Vector3.Distance(corners[i], corners[i + 1]);
			}
			return num;
		}

		private List<Vector3> Dijkstra(IReadOnlyList<Vector3> points, Dictionary<int, List<(int neighbor, float cost)>> edges, int n, int start, int end)
		{
			float[] array = new float[n];
			int[] array2 = new int[n];
			for (int i = 0; i < n; i++)
			{
				array[i] = float.MaxValue;
				array2[i] = -1;
			}
			array[start] = 0f;
			SortedSet<(float, int)> sortedSet = new SortedSet<(float, int)>(Comparer<(float, int)>.Create(((float, int) a, (float, int) b) => Mathf.Approximately(a.Item1, b.Item1) ? a.Item2.CompareTo(b.Item2) : a.Item1.CompareTo(b.Item1)));
			sortedSet.Add((0f, start));
			while (sortedSet.Count > 0)
			{
				var (num, num2) = sortedSet.Min;
				sortedSet.Remove(sortedSet.Min);
				if (num2 == end)
				{
					break;
				}
				if (num > array[num2])
				{
					continue;
				}
				foreach (var item3 in edges[num2])
				{
					int item = item3.neighbor;
					float item2 = item3.cost;
					float num3 = array[num2] + item2;
					if (num3 < array[item])
					{
						array[item] = num3;
						array2[item] = num2;
						sortedSet.Add((num3, item));
					}
				}
			}
			if (Mathf.Approximately(array[end], float.MaxValue))
			{
				return null;
			}
			List<Vector3> list = new List<Vector3>();
			for (int num4 = end; num4 != -1; num4 = array2[num4])
			{
				list.Add(points[num4]);
			}
			list.Reverse();
			return list;
		}

		private static bool IsAgentReadyForPathQuery(NavMeshAgent agent)
		{
			if (agent != null && agent.isActiveAndEnabled)
			{
				return agent.isOnNavMesh;
			}
			return false;
		}

		public bool TryWarpAgentOntoNavMesh(NavMeshAgent agent, Vector3 worldPosition, float sampleRadius)
		{
			if (agent == null || !agent.isActiveAndEnabled)
			{
				return false;
			}
			if (agent.isOnNavMesh)
			{
				agent.Warp(worldPosition);
				return agent.isOnNavMesh;
			}
			if (IsPointOnNavMeshProjected(worldPosition, agent, out var hit))
			{
				agent.Warp(hit.position);
				return agent.isOnNavMesh;
			}
			int areaMask = ((agent.areaMask != 0) ? agent.areaMask : (-1));
			if (NavMesh.SamplePosition(worldPosition, out var hit2, sampleRadius, areaMask))
			{
				agent.Warp(hit2.position);
				return agent.isOnNavMesh;
			}
			return false;
		}

		public bool TryGetCompletePath(NavMeshAgent agent, Vector3 goalWorldPos, out NavMeshPath path)
		{
			path = new NavMeshPath();
			if (!IsAgentReadyForPathQuery(agent))
			{
				return false;
			}
			if (!IsPointOnNavMeshProjected(goalWorldPos, agent, out var hit))
			{
				return false;
			}
			if (!agent.CalculatePath(hit.position, path))
			{
				return false;
			}
			return path.status == NavMeshPathStatus.PathComplete;
		}

		public bool TryGetCompletePath(NavMeshAgent agent, Vector3 goalWorldPos, float sampleRadius, out NavMeshPath path)
		{
			path = new NavMeshPath();
			if (!IsAgentReadyForPathQuery(agent))
			{
				return false;
			}
			float maxDistance = Mathf.Max(0.01f, sampleRadius);
			NavMeshQueryFilter filter = new NavMeshQueryFilter
			{
				agentTypeID = agent.agentTypeID,
				areaMask = agent.areaMask
			};
			if (!NavMesh.SamplePosition(goalWorldPos, out var hit, maxDistance, filter))
			{
				return false;
			}
			if (!agent.CalculatePath(hit.position, path))
			{
				return false;
			}
			return path.status == NavMeshPathStatus.PathComplete;
		}

		public bool TryResolveReachablePoint(NavMeshAgent agent, Vector3 desired, out Vector3 reachable, out NavMeshPathStatus status)
		{
			reachable = default(Vector3);
			status = NavMeshPathStatus.PathInvalid;
			if (!IsAgentReadyForPathQuery(agent))
			{
				return false;
			}
			Vector3 vector = desired;
			if (IsPointOnNavMeshProjected(desired, agent, out var hit))
			{
				vector = hit.position;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (!agent.CalculatePath(vector, navMeshPath) || navMeshPath.corners == null || navMeshPath.corners.Length == 0)
			{
				status = navMeshPath.status;
				return false;
			}
			status = navMeshPath.status;
			if (status == NavMeshPathStatus.PathComplete)
			{
				reachable = vector;
				return true;
			}
			if (status == NavMeshPathStatus.PathPartial)
			{
				reachable = navMeshPath.corners[navMeshPath.corners.Length - 1];
				return true;
			}
			return false;
		}

		private static bool IsRegisteredAgentType(int agentTypeID)
		{
			int settingsCount = NavMesh.GetSettingsCount();
			for (int i = 0; i < settingsCount; i++)
			{
				if (NavMesh.GetSettingsByIndex(i).agentTypeID == agentTypeID)
				{
					return true;
				}
			}
			return false;
		}
	}
}
