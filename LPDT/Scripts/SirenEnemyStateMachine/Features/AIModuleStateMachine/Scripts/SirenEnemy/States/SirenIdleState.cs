using Features.AIModuleStateMachine.Scripts.SirenEnemy.Sensors;
using Fusion;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.States
{
	public class SirenIdleState : StateBase<SirenStateId>
	{
		private readonly SirenEnemy _enemy;

		private readonly SirenEnemyContext _context;

		private readonly SirenTargetSensor _sensor;

		public SirenIdleState(SirenEnemy enemy, SirenEnemyContext context, SirenTargetSensor sensor)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_sensor = sensor;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SirenVisualState.Idle);
			_enemy.SetLookAtPresentationActive(active: false);
			_enemy.RaiseSongSound(play: true);
			_enemy.RaiseScreamSound(play: false);
			_enemy.RaiseHitSound(play: false);
			_sensor.Reset();
			_context.TargetPlayer = PlayerRef.None;
			_context.ChaseElapsed = 0f;
			_context.LostTargetElapsed = 0f;
			_context.IsLosingTarget = false;
			_enemy.SetCurrentTargetPlayerId(-1);
		}

		public override void OnLogic()
		{
			if (_sensor.TryAcquireTarget())
			{
				_enemy.TriggerEvent(SirenEvent.OnTargetAcquired);
			}
		}
	}
}
