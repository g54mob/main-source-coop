using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.States
{
	public class CoinRobSwarmStealingState : StateBase<CoinRobSwarmStateId>
	{
		private readonly CoinRobSwarmEnemy _enemy;

		private readonly CoinRobSwarmEnemyContext _context;

		private readonly CoinRobTargetSensor _sensor;

		private readonly CoinRobStealSettings _stealSettings;

		private readonly CoinRobChaseSettings _chaseSettings;

		private readonly CoinRobSwarmAnimatorPresenter _presenter;

		private readonly CoinRobSwarmFormationService _formation;

		public CoinRobSwarmStealingState(CoinRobSwarmEnemy enemy, CoinRobSwarmEnemyContext context, CoinRobTargetSensor sensor, CoinRobStealSettings stealSettings, CoinRobChaseSettings chaseSettings, CoinRobSwarmAnimatorPresenter presenter, CoinRobSwarmFormationService formation)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_sensor = sensor;
			_stealSettings = stealSettings;
			_chaseSettings = chaseSettings;
			_presenter = presenter;
			_formation = formation;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(CoinRobSwarmVisualState.Stealing);
			_context.EnableReactiveAggro();
			_formation.BeginEmptyUnitsPatrolAroundKing();
		}

		public override void OnExit()
		{
			foreach (var (coinRobBehaviour2, task) in _context.ActiveStealTasks)
			{
				if (!(coinRobBehaviour2 == null))
				{
					if (_context.IsFearing)
					{
						TryAttachReachableStealItemForFear(coinRobBehaviour2, task);
					}
					_presenter.EndUnitSteal(coinRobBehaviour2);
				}
			}
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			if (_enemy.TryResolveReactivePlayerAttackRequest())
			{
				return;
			}
			if (_context.ActiveStealTasks.Count == 0)
			{
				ResolvePostStealTransition();
				return;
			}
			List<CoinRobBehaviour> list = new List<CoinRobBehaviour>();
			foreach (var (coinRobBehaviour2, itemStealTask2) in _context.ActiveStealTasks)
			{
				if (itemStealTask2.TargetItem == null || itemStealTask2.TargetItem.NetworkObject == null)
				{
					list.Add(coinRobBehaviour2);
					continue;
				}
				if (IsItemInCart(itemStealTask2.TargetItem) || !itemStealTask2.TargetItem.AvailableForEnemy)
				{
					FinishFailedSteal(coinRobBehaviour2, itemStealTask2, list);
					continue;
				}
				if (_sensor.IsItemTooHighToSteal(itemStealTask2.TargetItem, coinRobBehaviour2.transform.position))
				{
					FinishFailedSteal(coinRobBehaviour2, itemStealTask2, list);
					continue;
				}
				if (itemStealTask2.TotalTimer >= _stealSettings.StealTaskTimeout)
				{
					FinishFailedSteal(coinRobBehaviour2, itemStealTask2, list);
					continue;
				}
				itemStealTask2.TargetPosition = itemStealTask2.TargetItem.NetworkObject.transform.position;
				if (!itemStealTask2.ReachedTarget)
				{
					float num = Vector3.Distance(new Vector3(coinRobBehaviour2.transform.position.x, itemStealTask2.TargetPosition.y, coinRobBehaviour2.transform.position.z), itemStealTask2.TargetPosition);
					if (itemStealTask2.IsOnKingDetour)
					{
						UpdateStealKingDetour(coinRobBehaviour2, itemStealTask2, num, tickDelta, list);
						if (list.Contains(coinRobBehaviour2))
						{
							continue;
						}
					}
					else if (IsUnitStealReachStuck(coinRobBehaviour2, num))
					{
						itemStealTask2.UnreachableTimer += tickDelta;
						itemStealTask2.StuckRepathTimer += tickDelta;
						if (itemStealTask2.UnreachableTimer >= _stealSettings.StealUnreachableGiveUpTime)
						{
							FinishFailedSteal(coinRobBehaviour2, itemStealTask2, list);
							continue;
						}
						if (itemStealTask2.StuckRepathTimer >= _formation.UnitStuckRepathTime)
						{
							BeginStealKingDetour(coinRobBehaviour2, itemStealTask2);
						}
					}
					else
					{
						itemStealTask2.StuckRepathTimer = 0f;
						if (num + 0.15f < itemStealTask2.ClosestApproachDistance)
						{
							itemStealTask2.ClosestApproachDistance = num;
							itemStealTask2.UnreachableTimer = 0f;
						}
					}
					if (!itemStealTask2.IsOnKingDetour && num <= _stealSettings.StealApproachDistance)
					{
						BeginStealPickup(coinRobBehaviour2, itemStealTask2);
					}
				}
				if (itemStealTask2.ReachedTarget)
				{
					itemStealTask2.AnimationTimer += tickDelta;
					TryStartGrabDuringStealAnim(coinRobBehaviour2, itemStealTask2);
					if (TryCompleteStealPickup(coinRobBehaviour2, itemStealTask2, out var rejectItem))
					{
						if (rejectItem)
						{
							_context.RejectStealItem(itemStealTask2.TargetItem);
						}
						list.Add(coinRobBehaviour2);
					}
				}
				itemStealTask2.TotalTimer += tickDelta;
			}
			foreach (CoinRobBehaviour item in list)
			{
				_presenter.EndUnitSteal(item);
				_context.ActiveStealTasks.Remove(item);
			}
			_formation.RefreshStealingIdleUnitsAroundKing();
			if (_context.ActiveStealTasks.Count == 0)
			{
				ResolvePostStealTransition();
			}
		}

		private void BeginStealKingDetour(CoinRobBehaviour unit, CoinRobSwarmEnemyContext.ItemStealTask task)
		{
			task.StuckRepathTimer = 0f;
			task.UnreachableTimer = 0f;
			task.IsOnKingDetour = true;
			_formation.RepathUnitAroundKingToward(unit, task.TargetPosition);
			_presenter.SetMemberLocomotion(unit, isRunning: true);
		}

		private void UpdateStealKingDetour(CoinRobBehaviour unit, CoinRobSwarmEnemyContext.ItemStealTask task, float approachDistance, float deltaTime, List<CoinRobBehaviour> finishedTasks)
		{
			if (HasReachedStealDetour(unit))
			{
				task.IsOnKingDetour = false;
				task.StuckRepathTimer = 0f;
				_formation.SetUnitDestinationNear(unit, task.TargetPosition);
				_presenter.SetMemberLocomotion(unit, isRunning: true);
				return;
			}
			if (!IsUnitStealReachStuck(unit, approachDistance))
			{
				task.StuckRepathTimer = 0f;
				return;
			}
			task.StuckRepathTimer += deltaTime;
			task.UnreachableTimer += deltaTime;
			if (task.UnreachableTimer >= _stealSettings.StealUnreachableGiveUpTime)
			{
				FinishFailedSteal(unit, task, finishedTasks);
			}
			else if (!(task.StuckRepathTimer < _formation.UnitStuckRepathTime))
			{
				BeginStealKingDetour(unit, task);
			}
		}

		private bool HasReachedStealDetour(CoinRobBehaviour unit)
		{
			if (unit.PathPending)
			{
				return false;
			}
			if (!unit.HasPath)
			{
				return false;
			}
			float num = _chaseSettings.TargetReachError + unit.StoppingDistance;
			return unit.RemainingDistance <= num;
		}

		private void FinishFailedSteal(CoinRobBehaviour unit, CoinRobSwarmEnemyContext.ItemStealTask task, List<CoinRobBehaviour> finishedTasks)
		{
			_context.RejectStealItem(task.TargetItem);
			finishedTasks.Add(unit);
		}

		private void BeginStealPickup(CoinRobBehaviour unit, CoinRobSwarmEnemyContext.ItemStealTask task)
		{
			unit.StopMovement();
			_presenter.SetUnitSteal(unit);
			task.AnimationTimer = 0f;
			task.ReachedTarget = true;
			task.IsOnKingDetour = false;
			task.GrabStarted = false;
			task.GrabAttempted = false;
		}

		private void TryStartGrabDuringStealAnim(CoinRobBehaviour unit, CoinRobSwarmEnemyContext.ItemStealTask task)
		{
			if (!task.GrabAttempted && !(task.AnimationTimer < _stealSettings.GrabStartDelay))
			{
				task.GrabAttempted = true;
				if (_sensor.IsTargetItemInRange(unit.transform.position, task.TargetItem) && !_sensor.IsItemTooHighToSteal(task.TargetItem, unit.transform.position))
				{
					unit.AttachItem(task.TargetItem);
					task.GrabStarted = unit.IsCarryingLoot;
				}
			}
		}

		private void TryAttachReachableStealItemForFear(CoinRobBehaviour unit, CoinRobSwarmEnemyContext.ItemStealTask task)
		{
			if (!unit.HasAttachedItem && !task.GrabAttempted && task.TargetItem != null && !(task.TargetItem.NetworkObject == null) && !IsItemInCart(task.TargetItem) && task.TargetItem.AvailableForEnemy && _sensor.IsTargetItemInRange(unit.transform.position, task.TargetItem) && !_sensor.IsItemTooHighToSteal(task.TargetItem, unit.transform.position))
			{
				unit.AttachItem(task.TargetItem);
				task.GrabStarted = unit.IsCarryingLoot;
			}
		}

		private bool TryCompleteStealPickup(CoinRobBehaviour unit, CoinRobSwarmEnemyContext.ItemStealTask task, out bool rejectItem)
		{
			rejectItem = false;
			if (task.AnimationTimer < _stealSettings.AttackTime)
			{
				return false;
			}
			if (unit.IsCarryingLoot)
			{
				_presenter.EndUnitSteal(unit);
				_formation.BeginUnitPatrolAroundKing(unit);
				return true;
			}
			if (task.GrabAttempted)
			{
				rejectItem = true;
				_presenter.EndUnitSteal(unit);
				return true;
			}
			return false;
		}

		private bool IsUnitStealReachStuck(CoinRobBehaviour unit, float approachDistance)
		{
			if (approachDistance <= _stealSettings.StealApproachDistance)
			{
				return false;
			}
			if (unit.PathPending)
			{
				return false;
			}
			if (unit.PathStatus == NavMeshPathStatus.PathInvalid || unit.PathStatus == NavMeshPathStatus.PathPartial)
			{
				return true;
			}
			if (!unit.HasPath)
			{
				return true;
			}
			if (unit.RemainingDistance <= _chaseSettings.TargetReachError)
			{
				return true;
			}
			return unit.Velocity.sqrMagnitude <= 0.01f;
		}

		private bool IsItemInCart(IItem item)
		{
			if (item == null || item.NetworkObject == null)
			{
				return false;
			}
			if (!item.NetworkObject.TryGetComponent<IPointGrabable>(out var component))
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

		private void ResolvePostStealTransition()
		{
			_context.ClearRejectedStealItems();
			_enemy.CancelCoinChase();
			_enemy.BeginRunAway();
			_enemy.Trigger(CoinRobSwarmEvent.OnRunAway);
		}
	}
}
