using System;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy
{
	public class EarSoundDestinationResolver
	{
		private readonly struct ApproachCandidate
		{
			public static readonly ApproachCandidate None = new ApproachCandidate(hasValue: false, default(Vector3), 0f, 0f);

			private readonly float _distanceToDesiredSqr;

			private readonly float _pathLength;

			public bool HasValue { get; }

			public Vector3 Position { get; }

			private ApproachCandidate(bool hasValue, Vector3 position, float distanceToDesiredSqr, float pathLength)
			{
				HasValue = hasValue;
				Position = position;
				_distanceToDesiredSqr = distanceToDesiredSqr;
				_pathLength = pathLength;
			}

			public static ApproachCandidate Create(Vector3 position, float distanceToDesiredSqr, float pathLength)
			{
				return new ApproachCandidate(hasValue: true, position, distanceToDesiredSqr, pathLength);
			}

			public bool IsBetterThan(ApproachCandidate other)
			{
				if (!other.HasValue)
				{
					return true;
				}
				if (Mathf.Approximately(_distanceToDesiredSqr, other._distanceToDesiredSqr))
				{
					return _pathLength < other._pathLength;
				}
				return _distanceToDesiredSqr < other._distanceToDesiredSqr;
			}
		}

		private const float DIRECT_SAMPLE_RADIUS = 2f;

		private const float APPROACH_SAMPLE_RADIUS = 4f;

		private const float APPROACH_MIN_RADIUS = 0.75f;

		private const float APPROACH_MAX_RADIUS = 5f;

		private const int APPROACH_RING_COUNT = 4;

		private const int APPROACH_POINTS_PER_RING = 10;

		private readonly NavMeshPath _path = new NavMeshPath();

		public bool TryResolve(NavMeshAgent agent, Vector3 desired, out Vector3 resolved)
		{
			resolved = default(Vector3);
			if (!IsAgentReady(agent))
			{
				return false;
			}
			NavMeshQueryFilter queryFilter = CreateQueryFilter(agent);
			if (!TryResolveDirectPath(agent, desired, queryFilter, out resolved) && !TryResolveClosestApproach(agent, desired, queryFilter, out resolved))
			{
				return TryResolvePartialPath(agent, desired, queryFilter, out resolved);
			}
			return true;
		}

		private static bool IsAgentReady(NavMeshAgent agent)
		{
			if (agent != null && agent.isActiveAndEnabled)
			{
				return agent.isOnNavMesh;
			}
			return false;
		}

		private static NavMeshQueryFilter CreateQueryFilter(NavMeshAgent agent)
		{
			return new NavMeshQueryFilter
			{
				agentTypeID = agent.agentTypeID,
				areaMask = ((agent.areaMask != 0) ? agent.areaMask : (-1))
			};
		}

		private bool TryResolveDirectPath(NavMeshAgent agent, Vector3 desired, NavMeshQueryFilter queryFilter, out Vector3 resolved)
		{
			float pathLength;
			return TryGetCompletePath(agent, desired, 2f, queryFilter, out resolved, out pathLength);
		}

		private bool TryResolveClosestApproach(NavMeshAgent agent, Vector3 desired, NavMeshQueryFilter queryFilter, out Vector3 resolved)
		{
			ApproachCandidate approachCandidate = FindBestApproachCandidate(agent, desired, queryFilter);
			resolved = approachCandidate.Position;
			return approachCandidate.HasValue;
		}

		private ApproachCandidate FindBestApproachCandidate(NavMeshAgent agent, Vector3 desired, NavMeshQueryFilter queryFilter)
		{
			ApproachCandidate approachCandidate = ApproachCandidate.None;
			float approachRadiusStep = GetApproachRadiusStep();
			for (int i = 0; i < 4; i++)
			{
				float radius = 0.75f + approachRadiusStep * (float)i;
				approachCandidate = FindBestCandidateInRing(agent, desired, queryFilter, radius, approachCandidate);
			}
			return approachCandidate;
		}

		private ApproachCandidate FindBestCandidateInRing(NavMeshAgent agent, Vector3 desired, NavMeshQueryFilter queryFilter, float radius, ApproachCandidate bestCandidate)
		{
			float num = MathF.PI / 5f;
			for (int i = 0; i < 10; i++)
			{
				Vector3 candidatePosition = GetCandidatePosition(desired, radius, num * (float)i);
				if (TryBuildApproachCandidate(agent, desired, candidatePosition, queryFilter, out var candidate) && candidate.IsBetterThan(bestCandidate))
				{
					bestCandidate = candidate;
				}
			}
			return bestCandidate;
		}

		private bool TryBuildApproachCandidate(NavMeshAgent agent, Vector3 desired, Vector3 candidatePosition, NavMeshQueryFilter queryFilter, out ApproachCandidate candidate)
		{
			candidate = ApproachCandidate.None;
			if (!TryGetCompletePath(agent, candidatePosition, 4f, queryFilter, out var resolved, out var pathLength))
			{
				return false;
			}
			candidate = ApproachCandidate.Create(resolved, GetFlatDistanceSqr(resolved, desired), pathLength);
			return true;
		}

		private bool TryResolvePartialPath(NavMeshAgent agent, Vector3 desired, NavMeshQueryFilter queryFilter, out Vector3 resolved)
		{
			resolved = default(Vector3);
			if (!TrySamplePosition(desired, 2f, queryFilter, out var pointOnNavMesh))
			{
				return false;
			}
			if (!TryCalculatePath(agent, pointOnNavMesh))
			{
				return false;
			}
			if (_path.status != NavMeshPathStatus.PathPartial)
			{
				return false;
			}
			resolved = _path.corners[_path.corners.Length - 1];
			return true;
		}

		private bool TryGetCompletePath(NavMeshAgent agent, Vector3 desired, float sampleRadius, NavMeshQueryFilter queryFilter, out Vector3 resolved, out float pathLength)
		{
			resolved = default(Vector3);
			pathLength = 0f;
			if (!TrySamplePosition(desired, sampleRadius, queryFilter, out var pointOnNavMesh))
			{
				return false;
			}
			if (!TryCalculatePath(agent, pointOnNavMesh))
			{
				return false;
			}
			if (_path.status != NavMeshPathStatus.PathComplete)
			{
				return false;
			}
			resolved = pointOnNavMesh;
			pathLength = GetPathLength(_path);
			return true;
		}

		private bool TrySamplePosition(Vector3 position, float radius, NavMeshQueryFilter queryFilter, out Vector3 pointOnNavMesh)
		{
			pointOnNavMesh = default(Vector3);
			if (!NavMesh.SamplePosition(position, out var hit, radius, queryFilter))
			{
				return false;
			}
			pointOnNavMesh = hit.position;
			return true;
		}

		private bool TryCalculatePath(NavMeshAgent agent, Vector3 destination)
		{
			if (agent.CalculatePath(destination, _path) && _path.corners != null)
			{
				return _path.corners.Length != 0;
			}
			return false;
		}

		private static float GetApproachRadiusStep()
		{
			return 1.4166666f;
		}

		private static Vector3 GetCandidatePosition(Vector3 center, float radius, float angle)
		{
			return center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
		}

		private static float GetFlatDistanceSqr(Vector3 a, Vector3 b)
		{
			a.y = 0f;
			b.y = 0f;
			return (a - b).sqrMagnitude;
		}

		private static float GetPathLength(NavMeshPath path)
		{
			if (path?.corners == null || path.corners.Length < 2)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 1; i < path.corners.Length; i++)
			{
				num += Vector3.Distance(path.corners[i - 1], path.corners[i]);
			}
			return num;
		}
	}
}
