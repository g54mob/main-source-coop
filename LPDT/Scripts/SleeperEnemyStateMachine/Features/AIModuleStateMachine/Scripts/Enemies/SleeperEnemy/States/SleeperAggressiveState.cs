using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperAggressiveState : StateBase<SleeperStateId>
	{
		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		private readonly SleeperEnemySettings _settings;

		public SleeperAggressiveState(SleeperEnemy enemy, SleeperEnemyContext context, SleeperEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.Aggressive);
			_context.SetVisualState(SleeperVisualState.Aggressive);
			_context.DistanceToAttack = _settings.DistanceToAttack;
			_context.SleeperSearchRangeSetupSystem.Enable();
			_context.SleeperAggroSoundReactDelaySetupSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SleeperDamageAggrSystem.Enable();
			_context.SleeperSoundOcclusionHearingStrengthSetupSystem.Enable();
			_context.SoundOcclusionMonoSystem.Enable();
			_context.SleeperAggressiveSoundAggroSystem.Enable();
		}

		public override void OnExit()
		{
			_context.SleeperSearchRangeSetupSystem.Disable();
			_context.SleeperAggroSoundReactDelaySetupSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.SleeperDamageAggrSystem.Disable();
			_context.SleeperSoundOcclusionHearingStrengthSetupSystem.Disable();
			_context.SoundOcclusionMonoSystem.Disable();
			_context.SleeperAggressiveSoundAggroSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.AttackCooldown <= 0f && _context.PriorityPlayer != null)
			{
				_enemy.TriggerEvent(SleeperEvent.OnTargetAcquired);
			}
			else if (_context.CurrentStateTime >= _settings.AggroDuration)
			{
				_enemy.TriggerEvent(SleeperEvent.OnAggroTimeout);
			}
		}
	}
}
