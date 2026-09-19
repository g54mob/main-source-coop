using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States
{
	public class SnakeFearState : StateBase<SnakeStateId>
	{
		private readonly SnakeEnemy _enemy;

		private readonly SnakeEnemyContext _context;

		private bool _hasFleeDestination;

		public SnakeFearState(SnakeEnemy enemy, SnakeEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SnakeStateId.Fear);
			_context.SetVisualState(SnakeVisualState.Fear);
			_context.ApplyStepAggroMoveSpeed();
			_context.NeedToFindTargetPosition = false;
			_context.FearDestinationTimer = _context.FearDestinationUpdateInterval;
			_context.SetPriorityPlayer(null);
			_hasFleeDestination = _enemy.TryPrepareFearFleeDestination();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SnakeDamageReactionSystem.Disable();
		}

		public override void OnExit()
		{
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_hasFleeDestination = false;
		}

		public override void OnLogic()
		{
			if (!_context.IsEnemyVisibleByPlayers())
			{
				CompleteFear();
				return;
			}
			float tickDelta = _enemy.GetTickDelta();
			_context.FearDestinationTimer += tickDelta;
			if (!(_context.FearDestinationTimer < _context.FearDestinationUpdateInterval) && (!_hasFleeDestination || HasReachedFearDestination()))
			{
				_context.FearDestinationTimer = 0f;
				_hasFleeDestination = _enemy.TryPrepareFearFleeDestination();
			}
		}

		private bool HasReachedFearDestination()
		{
			if (_context.TargetPositionCompleted)
			{
				return true;
			}
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (navMeshAgent == null || !navMeshAgent.hasPath)
			{
				return true;
			}
			return Vector3.Distance(navMeshAgent.pathEndPosition, _context.transform.position) <= _context.FearDestinationReachedDistance;
		}

		private void CompleteFear()
		{
			if (_enemy.IsDespawnAfterFear)
			{
				_enemy.RequestDespawn();
				return;
			}
			_enemy.MarkFearCompleted();
			_enemy.TriggerEvent(SnakeEvent.OnFearEscapeCompleted);
		}
	}
}
