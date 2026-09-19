using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States
{
	public class CoinRobSwarmWanderingState : StateBase<CoinRobSwarmStateId>
	{
		private readonly CoinRobSwarmEnemy _enemy;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmFormationService _formation;

		private readonly CoinRobSwarmEnemySettings _swarmSettings;

		private int _patrolArrivalCheckDelayFrames;

		public CoinRobSwarmWanderingState(CoinRobSwarmEnemy enemy, CoinRobSwarmEnemyContext context, CoinRobSwarmFormationService formation, CoinRobSwarmEnemySettings swarmSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_formation = formation;
			_swarmSettings = swarmSettings;
		}

		public override void OnEnter()
		{
			_context.ChaseTargetItem = null;
			_context.IsPatrolWaiting = false;
			_context.WanderingTimer = 0f;
			_patrolArrivalCheckDelayFrames = 3;
			_enemy.SetVisualState(CoinRobSwarmVisualState.Wandering);
			_context.EnableReactiveAggro();
			if (_context.HasActiveMembers)
			{
				_formation.MoveSwarmToPatrolAroundKing();
			}
		}

		public override void OnExit()
		{
			_context.DisableReactiveAggro();
		}

		public override void OnLogic()
		{
			if (_enemy.TryResolveReactivePlayerAttackRequest())
			{
				return;
			}
			if (_context.IsFearing)
			{
				if (_context.IsDespawnAfterFear)
				{
					_enemy.DespawnSwarmWithDissolve();
					return;
				}
				_context.IsFearing = false;
				_enemy.FearCompleted = true;
			}
			else if (_context.HasActiveMembers && _context.ChaseTargetItem == null)
			{
				_formation.SyncPatrolLocomotionAnimations();
				UpdateLocalPatrol(_enemy.GetTickDelta());
			}
		}

		private void UpdateLocalPatrol(float deltaTime)
		{
			_formation.RepathStuckWanderingUnits(deltaTime);
			if (_context.IsPatrolWaiting)
			{
				_formation.RefreshUnitsPatrolAroundKingOnArrival();
				_context.WanderingTimer += deltaTime;
				if (!(_context.WanderingTimer < _swarmSettings.WanderingDestinationUpdateTime))
				{
					_context.IsPatrolWaiting = false;
					_context.WanderingTimer = 0f;
					_patrolArrivalCheckDelayFrames = 3;
					_formation.MoveSwarmToPatrolAroundKing();
				}
			}
			else
			{
				_formation.KeepUnitsFollowingKingWhileMoving();
				if (_patrolArrivalCheckDelayFrames > 0)
				{
					_patrolArrivalCheckDelayFrames--;
				}
				else if (_formation.IsKingReadyForNextPatrol())
				{
					_context.SwarmKing?.StopMovement();
					_context.IsPatrolWaiting = true;
					_context.WanderingTimer = 0f;
				}
			}
		}
	}
}
