using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.GrabModule.Scripts;
using Features.NavigationModule.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States
{
	public class CoinRobSwarmChasingState : StateBase<CoinRobSwarmStateId>
	{
		private readonly CoinRobSwarmEnemy _enemy;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobSwarmFormationService _formation;

		private readonly CoinRobChaseSettings _chaseSettings;

		private readonly INavigationService _navigationService;

		private float _repathElapsed;

		private float _unreachableElapsed;

		private float _closestDistanceToChaseTarget = float.MaxValue;

		public CoinRobSwarmChasingState(CoinRobSwarmEnemy enemy, CoinRobSwarmEnemyContext context, CoinRobSwarmFormationService formation, CoinRobChaseSettings chaseSettings, INavigationService navigationService)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_formation = formation;
			_chaseSettings = chaseSettings;
			_navigationService = navigationService;
		}

		public override void OnEnter()
		{
			_repathElapsed = 0f;
			_unreachableElapsed = 0f;
			_closestDistanceToChaseTarget = float.MaxValue;
			_enemy.SetVisualState(CoinRobSwarmVisualState.Chasing);
			_formation.SetSwarmDestination(_context.ChasePosition);
			_context.EnableReactiveAggro();
		}

		public override void OnExit()
		{
			_context.DisableReactiveAggro();
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_repathElapsed += tickDelta;
			if (_enemy.TryResolveReactivePlayerAttackRequest())
			{
				return;
			}
			if (!_context.HasActiveMembers)
			{
				GiveUpChase();
				return;
			}
			if (IsChaseItemInCart())
			{
				GiveUpChase();
				return;
			}
			if (!TryGetChaseItemPosition(out var position))
			{
				GiveUpChase();
				return;
			}
			_context.ChasePosition = position;
			if (!_navigationService.IsPointOnNavMeshProjected(position, out var _))
			{
				if (UpdateUnreachableTimer(tickDelta))
				{
					GiveUpChase();
				}
				return;
			}
			if (_repathElapsed >= _chaseSettings.ChaseRepathInterval)
			{
				_repathElapsed = 0f;
				_formation.SetSwarmDestination(_context.ChasePosition);
			}
			_formation.KeepUnitsActiveAroundChaseTarget(_context.ChasePosition, tickDelta);
			if (IsKingNearChaseTarget())
			{
				_enemy.Trigger(CoinRobSwarmEvent.OnReachChasePoint);
			}
			else if (IsKingUnableToReachChaseTarget(tickDelta))
			{
				GiveUpChase();
			}
		}

		private bool IsKingNearChaseTarget()
		{
			if (_context.SwarmKing == null)
			{
				return false;
			}
			float num = Mathf.Max(_chaseSettings.AttackRange, _context.SwarmKing.StoppingDistance + 0.15f);
			return Vector3.Distance(_context.SwarmKing.transform.position, _context.ChasePosition) <= num;
		}

		private bool IsKingUnableToReachChaseTarget(float deltaTime)
		{
			if (_context.SwarmKing == null)
			{
				return false;
			}
			CoinRobBehaviour swarmKing = _context.SwarmKing;
			if (swarmKing.PathPending)
			{
				return false;
			}
			if (swarmKing.PathStatus == NavMeshPathStatus.PathInvalid || swarmKing.PathStatus == NavMeshPathStatus.PathPartial || !swarmKing.HasPath)
			{
				return UpdateUnreachableTimer(deltaTime);
			}
			float num = Vector3.Distance(swarmKing.transform.position, _context.ChasePosition);
			if (num + 0.15f < _closestDistanceToChaseTarget)
			{
				_closestDistanceToChaseTarget = num;
				_unreachableElapsed = 0f;
				return false;
			}
			if (swarmKing.Velocity.sqrMagnitude > 0.01f)
			{
				return false;
			}
			return UpdateUnreachableTimer(deltaTime);
		}

		private bool UpdateUnreachableTimer(float deltaTime)
		{
			_unreachableElapsed += deltaTime;
			return _unreachableElapsed >= _chaseSettings.ChaseTimeout;
		}

		private bool TryGetChaseItemPosition(out Vector3 position)
		{
			position = default(Vector3);
			if (_context.ChaseTargetItem == null || _context.ChaseTargetItem.NetworkObject == null)
			{
				return false;
			}
			position = _context.ChaseTargetItem.NetworkObject.transform.position;
			return true;
		}

		private bool IsChaseItemInCart()
		{
			if (_context.ChaseTargetItem == null || _context.ChaseTargetItem.NetworkObject == null)
			{
				return false;
			}
			if (!_context.ChaseTargetItem.NetworkObject.TryGetComponent<IPointGrabable>(out var component))
			{
				return false;
			}
			if (!component.InCart)
			{
				if (component.Carts != null)
				{
					return component.Carts.Count > 0;
				}
				return false;
			}
			return true;
		}

		private void GiveUpChase()
		{
			_enemy.CancelCoinChase();
			_enemy.BeginRunAway();
			_enemy.Trigger(CoinRobSwarmEvent.OnRunAway);
		}
	}
}
