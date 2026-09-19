using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States
{
	public class CrabGrabWanderingState : StateBase<CrabStateId>
	{
		private readonly CrabEnemy _enemy;

		private readonly CrabEnemyContext _context;

		private bool _isPlayerMode;

		public CrabGrabWanderingState(CrabEnemy enemy, CrabEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_isPlayerMode = !_context.CrabItemCarrySystem.IsCarryingItem;
			_enemy.SetCurrentStateId(CrabStateId.GrabWandering);
			_context.SetVisualState(_isPlayerMode ? CrabVisualState.HoldingPlayer : CrabVisualState.CarryingItem);
			_context.CrabHomeRoamSystem.SetHome(_enemy.transform.position, _context.GrabWanderRadius);
			_context.NeedToFindTargetPosition = true;
			_context.CrabDamageAggrSystem.Enable();
			_context.CrabMoveSpeedSetupSystem.Enable();
			_context.CrabBurstMovementSystem.Enable();
			_context.CrabTurnSpeedSystem.Enable();
			_context.CrabHomeRoamSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			if (_isPlayerMode)
			{
				_context.PlayerGrabSystem.Enable();
				_context.CrabGrabbedByPlayerTrackSystem.Enable();
				return;
			}
			_context.CrabSearchRangeSetupSystem.Enable();
			_context.CrabVisionDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.AttackCooldownSystem.Enable();
		}

		public override void OnExit()
		{
			_context.CrabMoveSpeedSetupSystem.Disable();
			_context.CrabBurstMovementSystem.Disable();
			_context.CrabTurnSpeedSystem.Disable();
			_context.CrabHomeRoamSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			if (_isPlayerMode)
			{
				_context.CrabGrabbedByPlayerTrackSystem.Disable();
				_context.PlayerGrabSystem.Disable();
				_context.AttackCooldown = _context.GrabCooldown;
			}
			else
			{
				_context.CrabSearchRangeSetupSystem.Disable();
				_context.CrabVisionDetectingSystem.Disable();
				_context.DetectedPlayersTimeSystem.Disable();
				_context.TargetPlayerPrioritizeSystem.Disable();
				_context.AttackCooldownSystem.Disable();
			}
		}

		public override void OnLogic()
		{
			if (_isPlayerMode)
			{
				ProcessPlayerWandering();
			}
			else
			{
				ProcessItemWandering();
			}
		}

		private void ProcessPlayerWandering()
		{
			if (_context.PlayerGrabSystem.ConsumeHeldPlayerDeparted())
			{
				_enemy.TriggerEvent(CrabEvent.OnGrabbedPlayerDisconnected);
			}
			else if (_context.PlayerGrabSystem.ConsumeHoldTakenByOther())
			{
				_enemy.TriggerEvent(CrabEvent.OnHoldTakenByOther);
			}
			else if (!_context.PlayerGrabSystem.IsHolding)
			{
				_enemy.TriggerEvent(CrabEvent.OnPlayerReleasedEarly);
			}
			else if (_context.IsGrabbedByPlayer)
			{
				_context.PlayerGrabSystem.Clear();
				_enemy.TriggerEvent(CrabEvent.OnPlayerWanderCompleted);
			}
			else if (_context.CurrentStateTime >= _context.PlayerGrabWanderDuration)
			{
				_context.PlayerGrabSystem.Clear();
				_enemy.TriggerEvent(CrabEvent.OnPlayerWanderCompleted);
			}
		}

		private void ProcessItemWandering()
		{
			if (_context.CrabItemCarrySystem.ConsumeHoldTakenByOther())
			{
				_enemy.TriggerEvent(CrabEvent.OnHoldTakenByOther);
			}
			else if (!_context.CrabItemCarrySystem.IsCarryingItem)
			{
				_enemy.TriggerEvent(CrabEvent.OnItemLost);
			}
			else if (_context.CanEnemyTargetPriorityPlayer())
			{
				_enemy.TriggerEvent(CrabEvent.OnTargetAcquired);
			}
			else if (_context.CurrentStateTime >= _context.GrabWanderDuration)
			{
				_enemy.TriggerEvent(CrabEvent.OnItemWanderCompleted);
			}
		}
	}
}
