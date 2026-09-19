using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorDamageAggroState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		public AnchorDamageAggroState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.DamageAggro);
			_context.SetVisualState(AnchorVisualState.Chase);
			_context.AnchorPlayerGrabSystem.Disable();
			_context.AnchorHookControlSystem.ResetHook();
			_context.AnchorDamageReactionSystem.Enable();
			_context.AnchorMoveSpeedSetupSystem.Enable();
			_context.FindTargetPlayerPositionSystem.Enable();
			_context.FindTargetPlayerPositionResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.AnchorVisionDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.AnchorDamageReactionSystem.Disable();
			_context.AnchorMoveSpeedSetupSystem.Disable();
			_context.FindTargetPlayerPositionSystem.Disable();
			_context.FindTargetPlayerPositionResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.AnchorVisionDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (!_context.HasLivePriorityPlayer())
			{
				_enemy.TriggerEvent(AnchorEvent.OnDamageAggroTimeout);
			}
			else if (!(_context.CurrentStateTime < _context.DamageAggroDuration))
			{
				if (_context.DetectedPlayers.Contains(_context.PriorityPlayer))
				{
					_enemy.TriggerEvent(AnchorEvent.OnTargetAcquired);
				}
				else
				{
					_enemy.TriggerEvent(AnchorEvent.OnDamageAggroTimeout);
				}
			}
		}
	}
}
