using Features.AIModule.Scripts.AttractionZone;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.States
{
	public class MimicAttractionInvestigateState : StateBase<MimicStateId>
	{
		private const int SamplingAttempts = 12;

		private readonly MimicEnemy _enemy;

		private readonly MimicEnemyContext _context;

		private readonly IEnemyAttractionZoneService _attractionZoneService;

		public MimicAttractionInvestigateState(MimicEnemy enemy, MimicEnemyContext context, IEnemyAttractionZoneService attractionZoneService)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_attractionZoneService = attractionZoneService;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(MimicStateId.AttractionInvestigate);
			_enemy.SetVisualState(MimicVisualState.MoveToRandomPos);
			_context.MimicMoveStatesSetupSystem.Enable();
			_context.TimeToAttackSetupSystem.Enable();
			_context.DistanceToAttackSetupSystem.Enable();
			_context.ReadyForAttackTrackSystem.Enable();
			_context.CompleteDistanceSetupSystem.Enable();
			_context.TargetSearchRangeSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.MimicMoveStatesSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AttackCooldownSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
			_context.PlayerVisibleSystem.Enable();
			_context.MimicAnalyticsSystem.Enable();
			SetApproachTarget();
		}

		public override void OnExit()
		{
			_context.MimicMoveStatesSetupSystem.Disable();
			_context.TimeToAttackSetupSystem.Disable();
			_context.DistanceToAttackSetupSystem.Disable();
			_context.ReadyForAttackTrackSystem.Disable();
			_context.CompleteDistanceSetupSystem.Disable();
			_context.TargetSearchRangeSetupSystem.Disable();
			_context.MoveSystem.Disable();
			_context.MimicMoveStatesSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AttackCooldownSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.ReplicateTargetSystem.Disable();
			_context.PlayerVisibleSystem.Disable();
			_context.MimicAnalyticsSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.IsReadyForAttack)
			{
				_context.IsReadyForAttack = false;
				_enemy.TriggerEvent(MimicEvent.OnReadyForAttack);
			}
			else if (_context.PriorityPlayer != null)
			{
				_enemy.TriggerEvent(MimicEvent.OnTargetAcquired);
			}
			else if (_context.TargetPositionCompleted)
			{
				_enemy.TriggerEvent(MimicEvent.OnAttractionZoneExited);
			}
		}

		private void SetApproachTarget()
		{
			if (_attractionZoneService.TrySampleApproachPointDirect(_context.PendingAttractionZone.Origin, _context.PendingAttractionZone.ApproachRadius, 12, out var point))
			{
				_context.NeedToFindTargetPosition = false;
				_context.SetTargetPosition(point);
				_context.SetTargetPositionCompleted(isCompleted: false);
			}
		}
	}
}
