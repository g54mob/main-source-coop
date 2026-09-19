using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class PirateInvestigateLastSeenState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		private bool _isMovingToLastKnownPosition;

		private bool _hasSearchDestination;

		private float _lookAroundTime;

		private float _searchMoveTime;

		private Vector3 _searchDestination;

		public PirateInvestigateLastSeenState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
		}

		public override void OnEnter()
		{
			_lookAroundTime = 0f;
			_searchMoveTime = 0f;
			_hasSearchDestination = false;
			_isMovingToLastKnownPosition = _pirateEnemyContext.HasLastKnownTargetPosition;
			if (!_isMovingToLastKnownPosition)
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
			}
			else
			{
				_pirateEnemyContext.EnemyMovableBase.MoveToPoint(_pirateEnemyContext.LastKnownTargetPosition);
			}
		}

		public override void OnLogic()
		{
			if (TryHandleTargetOnBeach())
			{
				return;
			}
			if (!_pirateEnemyContext.IsTargetAlive())
			{
				_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
			}
			else
			{
				if (TryHandleVisibleTarget())
				{
					return;
				}
				if (_isMovingToLastKnownPosition)
				{
					if (_pirateEnemyContext.EnemyMovableBase.HasReachedEnd())
					{
						_pirateEnemyContext.EnemyMovableBase.ResetPath();
						_isMovingToLastKnownPosition = false;
						TryMoveToSearchDestination();
					}
				}
				else
				{
					TickLookAround();
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

		private bool TryHandleVisibleTarget()
		{
			if (_pirateEnemyContext.IsTargetInSafeZone())
			{
				return false;
			}
			if (!_pirateEnemyContext.ObserveTargetIfVisible())
			{
				return false;
			}
			_pirateEnemyContext.ResetChaseTargetLostTimer();
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			_pirateEnemy.TriggerEvent(PirateEvent.OnTargetAcquired);
			return true;
		}

		private void TickLookAround()
		{
			float deltaTime = GetDeltaTime();
			_lookAroundTime += deltaTime;
			_searchMoveTime += deltaTime;
			if (_lookAroundTime >= _pirateEnemyContext.PirateConfiguration.InvestigationLookAroundTime)
			{
				CompleteInvestigationToIdle();
			}
			else if (!_hasSearchDestination || !(_searchMoveTime < _pirateEnemyContext.PirateConfiguration.InvestigationSearchMoveInterval) || _pirateEnemyContext.EnemyMovableBase.HasReachedEnd())
			{
				TryMoveToSearchDestination();
			}
		}

		private void TryMoveToSearchDestination()
		{
			_searchMoveTime = 0f;
			if (!_pirateEnemyContext.TryGetInvestigationSearchPosition(out var position) || !_pirateEnemyContext.EnemyMovableBase.IsCanMoveToPoint(position))
			{
				_hasSearchDestination = false;
				return;
			}
			_searchDestination = position;
			_hasSearchDestination = true;
			_pirateEnemyContext.EnemyMovableBase.MoveToPoint(position);
		}

		private void CompleteInvestigationToIdle()
		{
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			Vector3 position = (_pirateEnemyContext.HasLastKnownTargetPosition ? _pirateEnemyContext.LastKnownTargetPosition : _pirateEnemy.transform.position);
			_pirateEnemy.RaiseChangeAreaTriggered(position);
			_pirateEnemy.TriggerEvent(PirateEvent.OnIdle);
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
