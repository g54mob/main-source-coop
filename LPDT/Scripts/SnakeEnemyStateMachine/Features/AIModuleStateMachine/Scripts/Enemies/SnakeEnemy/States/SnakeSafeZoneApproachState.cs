using Features.SnakeModule.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States
{
	public class SnakeSafeZoneApproachState : StateBase<SnakeStateId>
	{
		private readonly SnakeEnemy _enemy;

		private readonly SnakeEnemyContext _context;

		private float _waitElapsed;

		private int _completedVisits;

		private bool _hasReachedPoint;

		private bool _isFinishing;

		public SnakeSafeZoneApproachState(SnakeEnemy enemy, SnakeEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SnakeStateId.SafeZoneApproach);
			_context.SetVisualState(SnakeVisualState.SafeZoneApproach);
			_context.ApplyStepAggroMoveSpeed();
			_waitElapsed = 0f;
			_completedVisits = 0;
			_hasReachedPoint = false;
			_isFinishing = false;
			if (!_context.HasSafeZoneApproachPosition && !_context.TryPrepareSafeZoneApproachPosition())
			{
				FinishApproach();
				return;
			}
			if (!BeginMoveToCurrentApproachPoint())
			{
				FinishApproach();
				return;
			}
			_context.AreaTypeTrackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SnakeDamageReactionSystem.Enable();
		}

		public override void OnExit()
		{
			_context.AreaTypeTrackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.SnakeDamageReactionSystem.Disable();
			ClearForcedLook();
			StopAgent();
			_context.ApplyBaseMoveSpeed();
			_context.ReleaseSafeZoneApproachPoint();
			_waitElapsed = 0f;
			_completedVisits = 0;
			_hasReachedPoint = false;
			_isFinishing = false;
		}

		public override void OnLogic()
		{
			if (!_context.HasSafeZoneApproachPosition)
			{
				FinishApproach();
				return;
			}
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
			{
				FinishApproach();
				return;
			}
			navMeshAgent.speed = _context.MoveSpeed;
			float num = ((_enemy.Runner != null) ? _enemy.Runner.DeltaTime : Time.deltaTime);
			if (!_hasReachedPoint)
			{
				UpdateLookWhileMoving(navMeshAgent);
				if (!HasReachedApproachPoint(navMeshAgent))
				{
					return;
				}
				_hasReachedPoint = true;
				navMeshAgent.isStopped = true;
				if (navMeshAgent.hasPath)
				{
					navMeshAgent.ResetPath();
				}
				navMeshAgent.velocity = Vector3.zero;
				FacePriorityPlayer();
				_context.TriggerSafeZoneWaitAnimation();
				_enemy.PlayUnderTableAttackSound();
			}
			else
			{
				FacePriorityPlayer();
			}
			_waitElapsed += num;
			if (!(_waitElapsed < _context.SafeZoneWaitDuration))
			{
				_completedVisits++;
				if (_completedVisits >= _context.SafeZoneApproachVisitCount)
				{
					FinishApproach();
				}
				else if (!_context.TryPrepareNextSafeZoneApproachPosition() || !BeginMoveToCurrentApproachPoint())
				{
					FinishApproach();
				}
			}
		}

		private bool BeginMoveToCurrentApproachPoint()
		{
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
			{
				return false;
			}
			navMeshAgent.isStopped = false;
			navMeshAgent.speed = _context.MoveSpeed;
			navMeshAgent.SetDestination(_context.SafeZoneApproachPosition);
			ClearForcedLook();
			_waitElapsed = 0f;
			_hasReachedPoint = false;
			return true;
		}

		private void UpdateLookWhileMoving(NavMeshAgent agent)
		{
			if (IsNearApproachLookStart(agent))
			{
				FacePriorityPlayer();
			}
			else
			{
				ClearForcedLook();
			}
		}

		private bool IsNearApproachLookStart(NavMeshAgent agent)
		{
			float safeZoneApproachLookStartDistance = _context.SafeZoneApproachLookStartDistance;
			Vector3 safeZoneApproachPosition = _context.SafeZoneApproachPosition;
			if (HorizontalDistance(_enemy.transform.position, safeZoneApproachPosition) <= safeZoneApproachLookStartDistance)
			{
				return true;
			}
			if (agent.pathPending || !agent.hasPath)
			{
				return false;
			}
			return agent.remainingDistance <= safeZoneApproachLookStartDistance;
		}

		private void FacePriorityPlayer()
		{
			SnakeController snakeController = _context.SnakeController;
			if (!(snakeController == null))
			{
				if (!_context.TryGetPriorityPlayerLookPosition(out var playerPosition))
				{
					snakeController.ClearForcedLookTarget();
				}
				else
				{
					snakeController.SetForcedLookTarget(playerPosition);
				}
			}
		}

		private void ClearForcedLook()
		{
			SnakeController snakeController = _context.SnakeController;
			if (!(snakeController == null))
			{
				snakeController.ClearForcedLookTarget();
			}
		}

		private bool HasReachedApproachPoint(NavMeshAgent agent)
		{
			float safeZoneApproachReachedDistance = _context.SafeZoneApproachReachedDistance;
			Vector3 safeZoneApproachPosition = _context.SafeZoneApproachPosition;
			if (HorizontalDistance(_enemy.transform.position, safeZoneApproachPosition) <= safeZoneApproachReachedDistance)
			{
				return true;
			}
			if (agent.pathPending)
			{
				return false;
			}
			if (!agent.hasPath)
			{
				return HorizontalDistance(_enemy.transform.position, safeZoneApproachPosition) <= safeZoneApproachReachedDistance;
			}
			return agent.remainingDistance <= safeZoneApproachReachedDistance;
		}

		private void FinishApproach()
		{
			if (!_isFinishing)
			{
				_isFinishing = true;
				_context.SetPriorityPlayer(null);
				_context.ReleaseSafeZoneApproachPoint();
				_enemy.TriggerEvent(SnakeEvent.OnSafeZoneApproachFinished);
			}
		}

		private void StopAgent()
		{
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (!(navMeshAgent == null) && navMeshAgent.enabled)
			{
				navMeshAgent.isStopped = false;
				if (navMeshAgent.isOnNavMesh)
				{
					navMeshAgent.ResetPath();
					navMeshAgent.velocity = Vector3.zero;
				}
			}
		}

		private static float HorizontalDistance(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}
	}
}
