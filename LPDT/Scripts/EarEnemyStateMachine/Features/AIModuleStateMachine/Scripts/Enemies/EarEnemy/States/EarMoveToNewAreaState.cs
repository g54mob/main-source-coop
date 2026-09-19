using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States
{
	public class EarMoveToNewAreaState : StateBase<EarStateId>
	{
		private readonly EarEnemy _enemy;

		private readonly EarEnemyContext _context;

		private readonly EarEnemySettings _settings;

		public EarMoveToNewAreaState(EarEnemy enemy, EarEnemyContext context, EarEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(EarStateId.MoveToNewArea);
			_context.SetVisualState(EarVisualState.MoveToNewArea);
			_context.SetMoveSpeed(_settings.WanderSpeed);
			_context.NavMeshAgent.stoppingDistance = 0f;
			_context.SetTargetPositionCompleted(isCompleted: false);
			_context.NeedToFindTargetPosition = true;
			_context.MoveSystem.Enable();
			_context.FindRandomSafePositionSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
		}

		public override void OnExit()
		{
			_context.MoveSystem.Disable();
			_context.FindRandomSafePositionSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
		}

		public override void OnLogic()
		{
			if (!_context.NeedToFindTargetPosition && _context.TargetPositionCompleted)
			{
				_enemy.TriggerEvent(EarEvent.OnNewAreaReached);
			}
		}
	}
}
