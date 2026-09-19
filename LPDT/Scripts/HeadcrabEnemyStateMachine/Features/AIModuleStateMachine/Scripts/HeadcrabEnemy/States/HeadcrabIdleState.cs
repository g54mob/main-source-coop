using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Sensors;
using Fusion;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.States
{
	public class HeadcrabIdleState : StateBase<HeadcrabStateId>
	{
		private readonly HeadcrabEnemy _enemy;

		private readonly HeadcrabEnemyContext _context;

		private readonly HeadcrabTargetSensor _sensor;

		private readonly BusyByHeadCrabPlayers _busyPlayers;

		public HeadcrabIdleState(HeadcrabEnemy enemy, HeadcrabEnemyContext context, HeadcrabTargetSensor sensor, BusyByHeadCrabPlayers busyPlayers)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_sensor = sensor;
			_busyPlayers = busyPlayers;
		}

		public override void OnEnter()
		{
			PlayerRef targetPlayer = _context.TargetPlayer;
			if (targetPlayer != PlayerRef.None && _busyPlayers != null)
			{
				_busyPlayers.BusyPlayers.Remove(targetPlayer);
			}
			_enemy.SetVisualState(HeadcrabVisualState.Idle);
			_context.TargetPlayer = PlayerRef.None;
			_context.TargetObject = null;
			_context.ChaseElapsed = 0f;
			_context.SnapElapsed = 0f;
			_context.ResnapCooldownElapsed = 0f;
		}

		public override void OnLogic()
		{
			if (_sensor.TryAcquireTarget(out var target))
			{
				_context.TargetPlayer = target;
				if (_busyPlayers != null && !_busyPlayers.BusyPlayers.Contains(target))
				{
					_busyPlayers.BusyPlayers.Add(target);
				}
				_enemy.TriggerEvent(HeadcrabEvent.OnTargetAcquired);
			}
		}
	}
}
