using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States
{
	public class EarWanderingState : StateBase<EarStateId>
	{
		private readonly EarEnemy _enemy;

		private readonly EarEnemyContext _context;

		private readonly EarEnemySettings _settings;

		public EarWanderingState(EarEnemy enemy, EarEnemyContext context, EarEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(EarStateId.Wandering);
			_context.SetVisualState(EarVisualState.Wandering);
			_context.SetMoveSpeed(_settings.WanderSpeed);
			_context.NavMeshAgent.stoppingDistance = 0f;
			_context.NeedToFindTargetPosition = true;
			_context.FindRandomPositionSystem.ConfigureSearchRadius(_settings.WanderRadius);
			_context.FindRandomPositionSystem.Enable();
			_context.TargetPositionCompletedResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.SoundOcclusionMonoSystem.Enable();
			_context.EarSoundOcclusionHearingStrengthSetupSystem.Enable();
			_context.EarWanderingSoundAggroSystem.Enable();
			_context.EarDamageReactionSystem.Enable();
		}

		public override void OnExit()
		{
			_context.FindRandomPositionSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.SoundOcclusionMonoSystem.Disable();
			_context.EarSoundOcclusionHearingStrengthSetupSystem.Disable();
			_context.EarWanderingSoundAggroSystem.Disable();
			_context.EarDamageReactionSystem.Disable();
		}

		public override void OnLogic()
		{
		}
	}
}
