using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanRageInteractedState : StateBase<HeadmanRageStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		public HeadmanRageInteractedState(HeadmanEnemy enemy, HeadmanEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.RageInteracted);
			_context.RageInteractedTimer = 0f;
			_context.RageIsApproachingCenter = false;
			_context.SetMovementEnabled(enabled: false);
			_context.ActiveRageSubstate = HeadmanRageStateId.Interacted;
			if (_enemy.HasStateAuthority)
			{
				int randomIndex = Random.Range(0, 100);
				SafeZoneType safeZoneType = SafeZoneType.None;
				if (_context.TryGetBlockingSafeZoneOnRage(out var safeZone) && safeZone != null)
				{
					safeZoneType = safeZone.SafeZoneType;
				}
				_enemy.SetRageInteractedPresentation(safeZoneType, randomIndex);
			}
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.RageInteractedTimer += tickDelta;
			if (_context.HasLastSeenPlayerPosition)
			{
				_context.RotateTowards(_context.LastSeenPlayerPosition);
			}
		}
	}
}
