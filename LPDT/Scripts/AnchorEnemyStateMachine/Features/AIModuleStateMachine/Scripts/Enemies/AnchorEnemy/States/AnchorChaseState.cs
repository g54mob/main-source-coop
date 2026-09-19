using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorChaseState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		public AnchorChaseState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.Chase);
			_context.SetVisualState(AnchorVisualState.Chase);
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
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.AttackCooldownSystem.Enable();
			_context.AnchorThrowDecisionSystem.Enable();
			_context.AnchorMeleeDecisionSystem.Enable();
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
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.AttackCooldownSystem.Disable();
			_context.AnchorThrowDecisionSystem.Disable();
			_context.AnchorMeleeDecisionSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			bool num = _context.HasLivePriorityPlayer();
			if (num)
			{
				_context.ClearAuxTarget();
			}
			if (!num && !DriveToAuxTarget())
			{
				_enemy.TriggerEvent(AnchorEvent.OnTargetLost);
			}
			else if (_context.AnchorMeleeDecisionSystem.CanMelee)
			{
				_enemy.TriggerEvent(AnchorEvent.OnMeleeAttack);
			}
			else if (_context.AnchorThrowDecisionSystem.CanThrow)
			{
				_enemy.TriggerEvent(AnchorEvent.OnThrowStarted);
			}
		}

		private bool DriveToAuxTarget()
		{
			Transform auxTargetTransform = _context.AuxTargetTransform;
			if (auxTargetTransform == null)
			{
				return false;
			}
			_context.SetTargetPosition(auxTargetTransform.position);
			_context.SetTargetPositionCompleted(isCompleted: false);
			return true;
		}
	}
}
