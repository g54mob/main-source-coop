using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanChasingStartState : StateBase<HeadmanChasingStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanChasingSettings _settings;

		public HeadmanChasingStartState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanChasingSettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.ChasingStart);
			_context.ActiveChasingSubstate = HeadmanChasingStateId.Start;
			_context.RestoreDefaultAreaMask();
			_enemy.RaiseNoticePlayerSound();
			if (_context.Agent != null)
			{
				_context.Agent.speed = _settings.SprintSpeed * _settings.ChasingSpeedCoefficient;
			}
		}
	}
}
