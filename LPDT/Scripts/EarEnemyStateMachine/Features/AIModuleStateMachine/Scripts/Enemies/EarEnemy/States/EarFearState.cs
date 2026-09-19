using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States
{
	public class EarFearState : StateBase<EarStateId>
	{
		private readonly EarEnemy _enemy;

		private readonly EarEnemyContext _context;

		private readonly EarEnemySettings _settings;

		public EarFearState(EarEnemy enemy, EarEnemyContext context, EarEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(EarStateId.Fear);
			_context.SetVisualState(EarVisualState.Fear);
			_context.SetMoveSpeed(_settings.AggroSpeed);
			_context.NavMeshAgent.stoppingDistance = 0f;
			_context.NeedToFindTargetPosition = false;
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.EnemyFearFleeSystem.OnCompleted += CompleteFear;
			_context.EnemyFearFleeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.EnemyFearFleeSystem.OnCompleted -= CompleteFear;
			_context.EnemyFearFleeSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		private void CompleteFear()
		{
			if (_enemy.IsDespawnAfterFear)
			{
				_enemy.RequestDespawn();
				return;
			}
			_enemy.PositionOnFearEnd = _enemy.transform.position;
			_enemy.MarkFearCompleted();
			_enemy.TriggerEvent(EarEvent.OnFearEscapeCompleted);
		}
	}
}
