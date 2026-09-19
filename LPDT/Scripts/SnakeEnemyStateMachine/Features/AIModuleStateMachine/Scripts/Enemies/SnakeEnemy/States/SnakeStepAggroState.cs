using Features.PlayerSpawner.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States
{
	public class SnakeStepAggroState : StateBase<SnakeStateId>
	{
		private const float DestinationRetargetXZ = 0.35f;

		private const float MinNavSampleRadius = 2.5f;

		private const float StuckVelocitySqr = 0.05f;

		private const float StuckRemainingDistance = 0.15f;

		private const float UnreachableHoldSeconds = 0.35f;

		private const float SampleNearRootXZ = 0.4f;

		private readonly SnakeEnemy _enemy;

		private readonly SnakeEnemyContext _context;

		private float _unreachableHoldTime;

		private Vector3 _lastSetDestination;

		public SnakeStepAggroState(SnakeEnemy enemy, SnakeEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SnakeStateId.StepAggro);
			_context.SetVisualState(SnakeVisualState.StepAggro);
			_context.ApplyStepAggroMoveSpeed();
			_context.ApplyChaseSlitherFrequency();
			_context.ApplyChaseVisualRotationSpeed();
			_unreachableHoldTime = 0f;
			_lastSetDestination = new Vector3(float.NaN, 0f, 0f);
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (navMeshAgent != null)
			{
				navMeshAgent.isStopped = false;
				navMeshAgent.speed = _context.MoveSpeed;
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
			StopAgent();
			_context.ApplyBaseMoveSpeed();
			_context.ClearChaseSlitherFrequency();
			_context.ClearChaseVisualRotationSpeed();
			_unreachableHoldTime = 0f;
		}

		public override void OnLogic()
		{
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				Disengage();
				return;
			}
			int playerId = priorityPlayer.NetworkObject.InputAuthority.PlayerId;
			if (_context.IsPlayerOutsideGate(playerId))
			{
				Disengage();
				return;
			}
			if (!_context.CanTargetPlayerForWrap(playerId))
			{
				Disengage();
				return;
			}
			if (_context.IsPriorityPlayerInSafeZone())
			{
				if (_context.TryPrepareSafeZoneApproachPosition())
				{
					_enemy.TriggerEvent(SnakeEvent.OnSafeZoneApproach);
				}
				else
				{
					Disengage();
				}
				return;
			}
			Vector3 position = _enemy.transform.position;
			Vector3 position2 = priorityPlayer.NetworkObject.transform.position;
			float num = Vector3.Distance(position, position2);
			float xzDistance = HorizontalDistance(position, position2);
			float deltaY = Mathf.Abs(position.y - position2.y);
			if (!_context.IsPriorityPinned && num > _context.StepAggroLoseDistance)
			{
				Disengage();
				return;
			}
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
			{
				return;
			}
			navMeshAgent.speed = _context.MoveSpeed;
			_context.SetTargetPosition(position2);
			_context.SetTargetPositionCompleted(isCompleted: false);
			if (IsWithinStandDistance(num, xzDistance))
			{
				_unreachableHoldTime = 0f;
				if (_context.IsWrapEscapeCooldownActive)
				{
					navMeshAgent.isStopped = true;
					if (navMeshAgent.hasPath)
					{
						navMeshAgent.ResetPath();
					}
					navMeshAgent.velocity = Vector3.zero;
				}
				else
				{
					TriggerWrap();
				}
			}
			else
			{
				navMeshAgent.isStopped = false;
				if (!TryBeginWrapWhenUnreachable(navMeshAgent, xzDistance))
				{
					TryUpdateDestination(navMeshAgent, position, position2, deltaY, xzDistance);
				}
			}
		}

		private bool IsWithinStandDistance(float distance3D, float xzDistance)
		{
			float stepAggroStandDistance = _context.StepAggroStandDistance;
			if (!(distance3D <= stepAggroStandDistance))
			{
				return xzDistance <= stepAggroStandDistance;
			}
			return true;
		}

		private void TriggerWrap()
		{
			_enemy.TriggerEvent(SnakeEvent.OnReachedWrapTarget);
		}

		private bool TryBeginWrapWhenUnreachable(NavMeshAgent agent, float xzDistance)
		{
			if (_context.IsWrapEscapeCooldownActive)
			{
				return false;
			}
			float num = ((_enemy.Runner != null) ? _enemy.Runner.DeltaTime : Time.deltaTime);
			bool pathPending = agent.pathPending;
			bool flag = agent.velocity.sqrMagnitude <= 0.05f;
			bool flag2 = !agent.hasPath || agent.remainingDistance <= 0.15f;
			bool flag3 = xzDistance > _context.StepAggroStandDistance;
			if (pathPending || !flag || !flag2 || !flag3)
			{
				_unreachableHoldTime = 0f;
				return false;
			}
			_unreachableHoldTime += num;
			if (_unreachableHoldTime < 0.35f)
			{
				return false;
			}
			_unreachableHoldTime = 0f;
			TriggerWrap();
			return true;
		}

		private void TryUpdateDestination(NavMeshAgent agent, Vector3 rootPosition, Vector3 playerPosition, float deltaY, float xzDistance)
		{
			float maxDistance = Mathf.Max(2.5f, deltaY + 0.5f);
			NavMeshQueryFilter filter = new NavMeshQueryFilter
			{
				agentTypeID = agent.agentTypeID,
				areaMask = agent.areaMask
			};
			if (!NavMesh.SamplePosition(playerPosition, out var hit, maxDistance, filter))
			{
				if (ShouldRetarget(_lastSetDestination, playerPosition))
				{
					SetDestination(agent, playerPosition);
				}
			}
			else if (HorizontalDistance(hit.position, rootPosition) <= 0.4f && xzDistance > _context.StepAggroStandDistance)
			{
				if (!_context.IsWrapEscapeCooldownActive)
				{
					TriggerWrap();
				}
			}
			else if (ShouldRetarget(_lastSetDestination, hit.position))
			{
				SetDestination(agent, hit.position);
			}
		}

		private void SetDestination(NavMeshAgent agent, Vector3 destination)
		{
			agent.SetDestination(destination);
			_lastSetDestination = destination;
		}

		private static bool ShouldRetarget(Vector3 lastDestination, Vector3 nextDestination)
		{
			if (float.IsNaN(lastDestination.x))
			{
				return true;
			}
			return HorizontalDistance(lastDestination, nextDestination) >= 0.35f;
		}

		private static float HorizontalDistance(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		private void Disengage()
		{
			_context.ClearPinnedPriorityPlayer();
			_context.SetPriorityPlayer(null);
			_enemy.TriggerEvent(SnakeEvent.OnStepAggroDisengaged);
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
	}
}
