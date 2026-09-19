using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.GrabModule.Scripts;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States
{
	public class CoinRobSwarmAttackingState : StateBase<CoinRobSwarmStateId>
	{
		private readonly CoinRobSwarmEnemy _enemy;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobTargetSensor _sensor;

		private readonly CoinRobSwarmFormationService _formation;

		private readonly CoinRobChaseSettings _chaseSettings;

		private float _postKingArriveElapsed;

		public CoinRobSwarmAttackingState(CoinRobSwarmEnemy enemy, CoinRobSwarmEnemyContext context, CoinRobTargetSensor sensor, CoinRobSwarmFormationService formation, CoinRobChaseSettings chaseSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_sensor = sensor;
			_formation = formation;
			_chaseSettings = chaseSettings;
		}

		public override void OnEnter()
		{
			_postKingArriveElapsed = 0f;
			_enemy.SetVisualState(CoinRobSwarmVisualState.Attacking);
			_context.EnableReactiveAggro();
			_context.SwarmKing?.StopMovement();
			_formation.BeginEmptyUnitsPatrolAroundKing();
		}

		public override void OnExit()
		{
			_context.DisableReactiveAggro();
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_postKingArriveElapsed += tickDelta;
			_formation.RefreshEmptyUnitsAroundKing();
			if (_enemy.TryResolveReactivePlayerAttackRequest())
			{
				return;
			}
			if (_context.ActiveStealTasks.Count > 0)
			{
				_enemy.Trigger(CoinRobSwarmEvent.OnItemsMarkedForSteal);
			}
			else if (IsChaseItemInCart())
			{
				GiveUpChase();
			}
			else if (_sensor.TryMarkStealItems())
			{
				if (HasActiveCoinChase() && !HasStealTaskForChaseItem())
				{
					if (!(_postKingArriveElapsed < _chaseSettings.ChaseTimeout))
					{
						GiveUpChase();
					}
				}
				else
				{
					_enemy.Trigger(CoinRobSwarmEvent.OnItemsMarkedForSteal);
				}
			}
			else if (HasActiveCoinChase())
			{
				if (!(_postKingArriveElapsed < _chaseSettings.ChaseTimeout))
				{
					GiveUpChase();
				}
			}
			else
			{
				_enemy.BeginRunAway();
				_enemy.Trigger(CoinRobSwarmEvent.OnRunAway);
			}
		}

		private bool HasStealTaskForChaseItem()
		{
			if (_context.ChaseTargetItem == null)
			{
				return false;
			}
			foreach (CoinRobSwarmEnemyContext.ItemStealTask value in _context.ActiveStealTasks.Values)
			{
				if (value.TargetItem == _context.ChaseTargetItem)
				{
					return true;
				}
			}
			return false;
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

		private bool HasActiveCoinChase()
		{
			if (_context.ChaseTargetItem != null)
			{
				return _context.ChaseTargetItem.NetworkObject != null;
			}
			return false;
		}

		private void GiveUpChase()
		{
			_enemy.CancelCoinChase();
			_enemy.BeginRunAway();
			_enemy.Trigger(CoinRobSwarmEvent.OnRunAway);
		}
	}
}
