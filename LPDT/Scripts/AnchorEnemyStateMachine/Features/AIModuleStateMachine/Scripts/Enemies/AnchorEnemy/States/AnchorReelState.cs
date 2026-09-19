using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorReelState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		public AnchorReelState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.Reel);
			_context.SetVisualState(AnchorVisualState.Reel);
			_context.SetSmoothedVelocity(0f);
			_context.AnchorDamageReactionSystem.Enable();
			_context.AnchorMoveSpeedSetupSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.AnchorHookControlSystem.Reel();
		}

		public override void OnExit()
		{
			_context.AnchorDamageReactionSystem.Disable();
			_context.AnchorMoveSpeedSetupSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			float carrierDistanceToRoot = _context.AnchorHookControlSystem.CarrierDistanceToRoot;
			bool hasGrabbedPlayer = _context.AnchorHookControlSystem.HasGrabbedPlayer;
			int hookedPlayerId = _context.AnchorHookControlSystem.HookedPlayerId;
			if ((hookedPlayerId >= 0 && !_context.IsPlayerSessionActive(hookedPlayerId)) || (hasGrabbedPlayer && hookedPlayerId < 0))
			{
				CompleteEmptyReel();
				return;
			}
			float distance;
			bool flag = _context.TryGetPriorityPlayerTrackingHorizontalDistance(out distance);
			if (hasGrabbedPlayer)
			{
				bool num = carrierDistanceToRoot <= _context.HookReleaseDistance;
				bool flag2 = flag && distance <= _context.HandGrabRadius;
				if (num && flag2)
				{
					_enemy.TriggerEvent(AnchorEvent.OnPlayerGrabbed);
				}
				else if (_context.CurrentStateTime >= _context.ReelPlayerTimeout)
				{
					CompleteEmptyReel();
				}
			}
			else
			{
				float arriveDistance = _context.AnchorHookControlSystem.ArriveDistance;
				if (carrierDistanceToRoot <= arriveDistance)
				{
					CompleteEmptyReel();
				}
				else if (_context.CurrentStateTime >= _context.ReelEmptyTimeout)
				{
					CompleteEmptyReel();
				}
			}
		}

		private void CompleteEmptyReel()
		{
			_context.AnchorHookControlSystem.ResetHook();
			_enemy.TriggerEvent(AnchorEvent.OnAnchorMissed);
		}
	}
}
