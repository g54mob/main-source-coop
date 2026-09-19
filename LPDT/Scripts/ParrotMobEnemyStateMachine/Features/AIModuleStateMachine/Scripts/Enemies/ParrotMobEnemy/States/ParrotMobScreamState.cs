using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.States
{
	public class ParrotMobScreamState : StateBase<ParrotMobStateId>
	{
		private readonly ParrotMobEnemy _enemy;

		private readonly ParrotMobEnemyContext _context;

		private readonly ParrotMobScreamSettings _screamSettings;

		private float _elapsed;

		public ParrotMobScreamState(ParrotMobEnemy enemy, ParrotMobEnemyContext context, ParrotMobScreamSettings screamSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_screamSettings = screamSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(ParrotMobStateId.Scream);
			_enemy.SetVisualState(ParrotMobVisualState.Scream);
			if (_context.NavMeshAgent.isOnNavMesh)
			{
				_context.NavMeshAgent.ResetPath();
			}
			_context.DamageProcessSystem.Enable();
			_context.PlayerDetectionSyncSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_elapsed = 0f;
		}

		public override void OnExit()
		{
			_context.PlayerDetectionSyncSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_enemy.SetVisualState(ParrotMobVisualState.Idle);
			_context.ScreamCooldownRemaining = _screamSettings.ScreamCooldown;
		}

		public override void OnLogic()
		{
			_elapsed += _enemy.GetTickDelta();
			if (!(_elapsed < _screamSettings.ScreamDuration))
			{
				_enemy.TriggerEvent(ParrotMobEvent.OnScreamCompleted);
			}
		}
	}
}
