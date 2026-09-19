using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States
{
	public class EarAggroState : StateBase<EarStateId>
	{
		private readonly EarEnemy _enemy;

		private readonly EarEnemyContext _context;

		private readonly EarEnemySettings _settings;

		private readonly PlayerSoundSourceGateService _playerSoundSourceGateService;

		public EarAggroState(EarEnemy enemy, EarEnemyContext context, EarEnemySettings settings, PlayerSoundSourceGateService playerSoundSourceGateService)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
			_playerSoundSourceGateService = playerSoundSourceGateService;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(EarStateId.Aggro);
			_context.SetVisualState(EarVisualState.Aggro);
			_context.SetMoveSpeed(_settings.AggroSpeed);
			_context.NavMeshAgent.stoppingDistance = 0f;
			_context.SetDestinationToReachablePoint(_context.SoundTargetPosition);
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SoundOcclusionMonoSystem.Enable();
			_context.EarSoundOcclusionHearingStrengthSetupSystem.Enable();
			_context.EarAggroSoundAggroSystem.Enable();
			_context.EarDamageReactionSystem.Enable();
		}

		public override void OnExit()
		{
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.SoundOcclusionMonoSystem.Disable();
			_context.EarSoundOcclusionHearingStrengthSetupSystem.Disable();
			_context.EarAggroSoundAggroSystem.Disable();
			_context.EarDamageReactionSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_playerSoundSourceGateService.IsSoundFromOutsideGatePlayer(_context.TargetHeardSound))
			{
				_enemy.TriggerEvent(EarEvent.OnSoundTargetLost);
			}
			else if (_context.TargetPositionCompleted || _context.CurrentStateTime >= _settings.AggroReachTimeout)
			{
				_enemy.TriggerEvent(EarEvent.OnReachedSoundPoint);
			}
		}
	}
}
