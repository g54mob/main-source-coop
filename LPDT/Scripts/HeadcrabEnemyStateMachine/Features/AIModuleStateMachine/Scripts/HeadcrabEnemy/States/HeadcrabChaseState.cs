using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings;
using Fusion;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.States
{
	public class HeadcrabChaseState : StateBase<HeadcrabStateId>
	{
		private readonly HeadcrabEnemy _enemy;

		private readonly HeadcrabEnemyContext _context;

		private readonly HeadcrabChaseSettings _settings;

		private readonly BusyByHeadCrabPlayers _busyPlayers;

		public HeadcrabChaseState(HeadcrabEnemy enemy, HeadcrabEnemyContext context, HeadcrabChaseSettings settings, BusyByHeadCrabPlayers busyPlayers)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
			_busyPlayers = busyPlayers;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadcrabVisualState.Chasing);
			_context.ChaseElapsed = 0f;
			_enemy.RaiseFoundPlayerSound();
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.ChaseElapsed += tickDelta;
			DetectionType detectionType;
			if (!_context.CanEnemyInteractWithTargetPlayer())
			{
				if (_context.TargetPlayer != PlayerRef.None && _busyPlayers != null)
				{
					_busyPlayers.BusyPlayers.Remove(_context.TargetPlayer);
				}
				_enemy.TriggerEvent(HeadcrabEvent.OnTargetLost);
			}
			else if (_context.TargetDetector == null || _context.TargetPlayer == PlayerRef.None)
			{
				if (_context.TargetPlayer != PlayerRef.None && _busyPlayers != null)
				{
					_busyPlayers.BusyPlayers.Remove(_context.TargetPlayer);
				}
				_enemy.TriggerEvent(HeadcrabEvent.OnTargetLost);
			}
			else if (!_context.TargetDetector.IsPlayerDetected(_context.TargetPlayer, out detectionType))
			{
				if (_busyPlayers != null)
				{
					_busyPlayers.BusyPlayers.Remove(_context.TargetPlayer);
				}
				_enemy.TriggerEvent(HeadcrabEvent.OnTargetLost);
			}
			else if (_context.ChaseElapsed >= _settings.ChasingTime)
			{
				_enemy.TriggerEvent(HeadcrabEvent.OnChaseTimeout);
			}
		}
	}
}
