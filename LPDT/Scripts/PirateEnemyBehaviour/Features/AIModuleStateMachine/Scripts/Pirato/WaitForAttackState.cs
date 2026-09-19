using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class WaitForAttackState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		private float _waitTime;

		private Vector3 _lastMoveToPoint;

		private bool _hasLastMoveToPoint;

		private bool _shouldFlee;

		private readonly MultiplayerModel _multiplayerModel;

		public WaitForAttackState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext, MultiplayerModel multiplayerModel)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
			_multiplayerModel = multiplayerModel;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.ApplyChaseSpeed();
			_waitTime = 0f;
			_hasLastMoveToPoint = false;
			_pirateEnemyContext.ResetChaseTargetLostTimer();
			_pirateEnemyContext.RefreshTargetBeachState();
			if ((!_pirateEnemyContext.IsTargetAlive() || _pirateEnemyContext.IsTargetOnBeach) && _pirateEnemyContext.TryFindClosestAlivePlayerInsideGate(_pirateEnemy.transform.position, out var closestPlayer))
			{
				_pirateEnemyContext.TargetPlayer = closestPlayer;
			}
			_pirateEnemy.StartFleeTimer();
			_shouldFlee = _pirateEnemy.TryConsumeFinishedFleeTimer();
		}

		public override void OnExit()
		{
		}

		public override void OnLogic()
		{
			_pirateEnemyContext.TryStealTargetToPlayer();
			if (!_pirateEnemyContext.CanEnemyInteractWithTarget())
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnTargetLost);
				return;
			}
			_waitTime += _multiplayerModel.NetworkRunner.DeltaTime;
			if (TryHandleTargetOnBeach())
			{
				return;
			}
			if (_shouldFlee)
			{
				_shouldFlee = false;
				_pirateEnemy.TriggerEvent(PirateEvent.OnFlee);
			}
			else if (!_pirateEnemyContext.IsTargetAlive())
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
			}
			else
			{
				if (TryHandleTargetLostByVisibility())
				{
					return;
				}
				if (_pirateEnemyContext.TryPrepareSafeZoneAttackPosition() && _waitTime >= _pirateEnemyContext.PirateConfiguration.AttackWaitCooldown)
				{
					_pirateEnemy.TriggerEvent(PirateEvent.OnSafeZoneAttack);
				}
				else
				{
					if (!_pirateEnemyContext.IsTargetVisible())
					{
						return;
					}
					if (!_pirateEnemyContext.TryGetTargetMovePosition(out var targetPosition))
					{
						_pirateEnemy.TriggerEvent(PirateEvent.OnTargetLost);
						return;
					}
					float num = Vector3.Distance(targetPosition, _pirateEnemy.transform.position);
					if (num < _pirateEnemyContext.PirateConfiguration.MeleeAttackRange)
					{
						if (_pirateEnemyContext.CanAttackTargetWithLineOfSight())
						{
							_pirateEnemyContext.EnemyMovableBase.ResetPath();
							_pirateEnemy.TriggerEvent(PirateEvent.OnMeleeAttack);
						}
						return;
					}
					TryMoveToTarget(targetPosition);
					if (num > _pirateEnemyContext.PirateConfiguration.AttackRange)
					{
						_waitTime = 0f;
					}
					else if (_pirateEnemyContext.CanAttackTargetWithLineOfSight() && !(_waitTime < _pirateEnemyContext.PirateConfiguration.AttackWaitCooldown))
					{
						if (Random.value >= _pirateEnemyContext.PirateConfiguration.AttackChance)
						{
							_waitTime = 0f;
							return;
						}
						_pirateEnemyContext.EnemyMovableBase.ResetPath();
						_pirateEnemy.TriggerEvent(PirateEvent.OnRangedAttack);
					}
				}
			}
		}

		private void TryMoveToTarget(Vector3 targetPosition)
		{
			float moveToPointUpdateDistanceThreshold = _pirateEnemyContext.PirateConfiguration.MoveToPointUpdateDistanceThreshold;
			if (moveToPointUpdateDistanceThreshold > 0f && _hasLastMoveToPoint && !_pirateEnemyContext.EnemyMovableBase.HasReachedEnd())
			{
				float sqrMagnitude = (targetPosition - _lastMoveToPoint).sqrMagnitude;
				float num = moveToPointUpdateDistanceThreshold * moveToPointUpdateDistanceThreshold;
				if (sqrMagnitude <= num)
				{
					return;
				}
			}
			_lastMoveToPoint = targetPosition;
			_hasLastMoveToPoint = true;
			_pirateEnemyContext.EnemyMovableBase.MoveToPoint(targetPosition);
		}

		private bool TryHandleTargetOnBeach()
		{
			_pirateEnemyContext.RefreshTargetBeachState();
			if (!_pirateEnemyContext.IsTargetOnBeach)
			{
				return false;
			}
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
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
