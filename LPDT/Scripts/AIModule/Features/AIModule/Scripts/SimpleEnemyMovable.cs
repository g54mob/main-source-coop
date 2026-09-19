using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class SimpleEnemyMovable : EnemyMovableBase
	{
		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		[SerializeField]
		private float _maxDistanceToCanMove = 0.5f;

		[SerializeField]
		private float _destinationUpdateThreshold = 0.3f;

		public override void MoveToPoint(Vector3 position)
		{
			_navMeshAgent.SetDestination(position);
		}

		public override bool IsCanMoveToPoint(Vector3 position)
		{
			NavMeshHit hit;
			return NavMesh.SamplePosition(position, out hit, _maxDistanceToCanMove, -1);
		}

		public override void Warp(Vector3 position)
		{
			_navMeshAgent.Warp(position);
		}

		public override Vector3 GetVelocity()
		{
			return _navMeshAgent.velocity;
		}

		public override bool HasReachedEnd()
		{
			if (_navMeshAgent.pathPending)
			{
				return false;
			}
			if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
			{
				return false;
			}
			if (_navMeshAgent.hasPath && _navMeshAgent.velocity.sqrMagnitude > 0.01f)
			{
				return false;
			}
			return true;
		}

		public override bool HavePath()
		{
			return _navMeshAgent.hasPath;
		}

		public override float GetStopingDistance()
		{
			return _navMeshAgent.stoppingDistance;
		}

		public override void ResetPath()
		{
			_navMeshAgent.ResetPath();
		}

		public override void EnableNavMeshAgent(bool enable)
		{
			_navMeshAgent.enabled = enable;
		}

		public override void SetMovementSpeed(float speed)
		{
			_navMeshAgent.speed = speed;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
