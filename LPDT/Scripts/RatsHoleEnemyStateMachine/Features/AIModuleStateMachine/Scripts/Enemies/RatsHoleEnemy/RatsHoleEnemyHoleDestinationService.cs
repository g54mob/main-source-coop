using Features.AIModuleStateMachine.Scripts.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	public class RatsHoleEnemyHoleDestinationService
	{
		private readonly IReadyHoleLocator _readyHoleLocator;

		public RatsHoleEnemyHoleDestinationService(IReadyHoleLocator readyHoleLocator)
		{
			_readyHoleLocator = readyHoleLocator;
		}

		public bool TryPrepareNearestHoleDestination(RatsHoleEnemy enemy, RatsHoleEnemyContext context)
		{
			NavMeshAgent navMeshAgent = context.NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
			{
				return false;
			}
			if (!TryGetNearestReadyHolePosition(enemy.transform.position, out var holePosition))
			{
				return false;
			}
			return TryPrepareDestination(navMeshAgent, context, holePosition, holePosition);
		}

		public bool TryPrepareHomeThenNearestHoleDestination(RatsHoleEnemy enemy, RatsHoleEnemyContext context)
		{
			NavMeshAgent navMeshAgent = context.NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
			{
				return false;
			}
			if ((bool)context.HasHomePosition && TryPrepareDestination(navMeshAgent, context, context.HomePosition, context.HasHomeAbsorbWorldPosition ? context.HomeAbsorbWorldPosition : context.HomePosition))
			{
				return true;
			}
			if (!TryGetNearestReadyHolePosition(enemy.transform.position, out var holePosition))
			{
				return false;
			}
			return TryPrepareDestination(navMeshAgent, context, holePosition, holePosition);
		}

		private bool TryPrepareDestination(NavMeshAgent agent, RatsHoleEnemyContext context, Vector3 holePosition, Vector3 absorbWorldPosition)
		{
			Vector3 vector = holePosition;
			float maxDistance = Mathf.Max(1f, context.AvailablePointRange);
			if (NavMesh.SamplePosition(holePosition, out var hit, maxDistance, -1))
			{
				vector = hit.position;
			}
			if (!HasCompletePath(agent, vector))
			{
				return false;
			}
			context.SetTargetPosition(vector);
			context.SetHoleAbsorbWorldPosition(absorbWorldPosition);
			context.SetTargetPositionCompleted(isCompleted: false);
			context.NeedToFindTargetPosition = false;
			agent.stoppingDistance = 0f;
			agent.isStopped = false;
			agent.ResetPath();
			agent.SetDestination(vector);
			return true;
		}

		private bool TryGetNearestReadyHolePosition(Vector3 worldPosition, out Vector3 holePosition)
		{
			holePosition = default(Vector3);
			if (_readyHoleLocator != null)
			{
				return _readyHoleLocator.TryGetNearestReadyHolePosition(worldPosition, out holePosition);
			}
			return false;
		}

		private static bool HasCompletePath(NavMeshAgent agent, Vector3 destination)
		{
			if (agent == null || !agent.isOnNavMesh)
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (!agent.CalculatePath(destination, navMeshPath))
			{
				return false;
			}
			return navMeshPath.status == NavMeshPathStatus.PathComplete;
		}
	}
}
