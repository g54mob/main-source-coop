using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class PirateChaseState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		public PirateChaseState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.ApplyChaseSpeed();
			_pirateEnemyContext.ResetChaseTargetLostTimer();
		}

		public override void OnLogic()
		{
			_pirateEnemyContext.TryStealTargetToPlayer();
			if (!_pirateEnemyContext.CanEnemyInteractWithTarget())
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnTargetLost);
			}
			else
			{
				if (TryHandleTargetOnBeach())
				{
					return;
				}
				if (!_pirateEnemyContext.IsTargetAlive())
				{
					_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
				}
				else if (!TryHandleTargetLostByVisibility())
				{
					if (!_pirateEnemyContext.TryGetTargetMovePosition(out var targetPosition))
					{
						_pirateEnemy.TriggerEvent(PirateEvent.OnTargetLost);
					}
					else if (Vector3.Distance(targetPosition, _pirateEnemy.transform.position) < _pirateEnemyContext.PirateConfiguration.AttackRange)
					{
						_pirateEnemy.TriggerEvent(PirateEvent.OnTargetInAttackRange);
						_pirateEnemyContext.EnemyMovableBase.ResetPath();
					}
					else
					{
						_pirateEnemyContext.EnemyMovableBase.MoveToPoint(targetPosition);
					}
				}
			}
		}

		private bool TryHandleTargetOnBeach()
		{
			_pirateEnemyContext.RefreshTargetBeachState();
			if (!_pirateEnemyContext.IsTargetOnBeach)
			{
				return false;
			}
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			_pirateEnemy.TriggerEvent(_pirateEnemyContext.HasOtherAlivePlayersInsideGate() ? PirateEvent.OnIdle : PirateEvent.OnFlee);
			return true;
		}

		private bool TryHandleTargetLostByVisibility()
		{
			if (_pirateEnemyContext.ObserveTargetIfVisible())
			{
				_pirateEnemyContext.ResetChaseTargetLostTimer();
				if (_pirateEnemyContext.IsTargetInSafeZone() && !_pirateEnemyContext.HasObservedTargetEnterSafeZone)
				{
					_pirateEnemyContext.EnemyMovableBase.ResetPath();
					_pirateEnemy.TriggerEvent(PirateEvent.OnTargetLost);
					return true;
				}
				return false;
			}
			if (_pirateEnemyContext.TryMarkTargetSafeZoneEntryFromRecentObservation() || _pirateEnemyContext.ShouldPreserveCombatForObservedSafeZoneEntry())
			{
				_pirateEnemyContext.ResetChaseTargetLostTimer();
				return false;
			}
			if (!_pirateEnemyContext.IncreaseChaseTargetLostTimer(GetDeltaTime(), _pirateEnemyContext.PirateConfiguration.LostTargetTime))
			{
				return false;
			}
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			_pirateEnemy.TriggerEvent(PirateEvent.OnTargetLost);
			return true;
		}

		private float GetDeltaTime()
		{
			if (!(_pirateEnemy.Runner != null))
			{
				return Time.deltaTime;
			}
			return _pirateEnemy.Runner.DeltaTime;
		}
	}
}
