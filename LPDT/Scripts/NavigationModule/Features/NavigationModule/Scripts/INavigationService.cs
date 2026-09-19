using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.NavigationModule.Scripts
{
	public interface INavigationService
	{
		bool TryGetRandomNavmeshPosition(Vector3 center, float radius, out Vector3 position);

		bool TryGetRandomSafeNavmeshPosition(Vector3 center, float radius, int attempts, out Vector3 position);

		bool TryGetRandomSafeNavmeshPositionWithPathFinding(Vector3 center, float radius, float maxPathLength, int attempts, out Vector3 position);

		bool TryGetRandomSafeNavmeshPosition(Vector3 center, float searchRadius, int attempts, List<Vector3> avoidPositions, int agentTypeID, int areaMask, out Vector3 bestPosition, float minDistanceFromCenter = 0f, float centerDistanceWeight = 1f, float averageAvoidDistanceWeight = 1f, float tooClosePenaltyRadius = 0f, float tooClosePenaltyWeight = 0f);

		bool TryGetPlayerTrackingPosition(PlayerRef player, out Vector3 position);

		Vector3 ValidatePointOnNavmesh(Vector3 point, float radius);

		bool IsPointOnNavMeshProjected(Vector3 point, int agentType, out NavMeshHit hit);

		bool IsPointOnNavMeshProjected(Vector3 point, NavMeshAgent agent, out NavMeshHit hit);

		bool IsPlayerOnReachablePoint(PlayerRef targetPlayer, NavMeshAgent agent, out PlayerReachableData reachableData);

		bool IsPlayerOnReachablePoint(PlayerRef targetPlayer, NavMeshAgent agent, float detectionDistance, out PlayerReachableData reachableData);

		bool HasAvailablePointInRange(NavMeshAgent agent, Vector3 position, float range, out Vector3 availablePosition);

		bool IsPointOnNavMeshProjected(Vector3 point, out NavMeshHit hit);

		bool TryGetPointOnNavMeshProjected(Vector3 point, out Vector3 projectedPoint);

		bool TryGetPointOnNavMeshProjected(Vector3 point, float radius, out Vector3 projectedPoint);

		List<Vector3> FindPath(IReadOnlyList<NavPoint> points, int startIndex, int endIndex, bool projectPoints);

		bool TryGetPathCorners(Vector3 from, Vector3 to, float radius, out List<Vector3> corners);

		bool TryGetPathPolyline(Vector3 from, Vector3 to, float radius, float pointSpacing, int maxPointsPerSegment, float groundProbeDistance, out List<Vector3> polyline);

		bool TryWarpAgentOntoNavMesh(NavMeshAgent agent, Vector3 worldPosition, float sampleRadius);

		bool TryGetCompletePath(NavMeshAgent agent, Vector3 goalWorldPos, out NavMeshPath path);

		bool TryGetCompletePath(NavMeshAgent agent, Vector3 goalWorldPos, float sampleRadius, out NavMeshPath path);

		bool TryResolveReachablePoint(NavMeshAgent agent, Vector3 desired, out Vector3 reachable, out NavMeshPathStatus status);
	}
}
