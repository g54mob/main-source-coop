using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using Features.PlayerSpawner.Scripts;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.States
{
	public class ParrotMobIdleState : StateBase<ParrotMobStateId>
	{
		private readonly ParrotMobEnemy _enemy;

		private readonly ParrotMobEnemyContext _context;

		private readonly ParrotMobIdleSettings _idleSettings;

		private bool _hadPriorityPlayer;

		private PlayerDataHolder _engagedPriorityPlayer;

		public ParrotMobIdleState(ParrotMobEnemy enemy, ParrotMobEnemyContext context, ParrotMobIdleSettings idleSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_idleSettings = idleSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(ParrotMobStateId.Idle);
			_enemy.SetVisualState(ParrotMobVisualState.Idle);
			_hadPriorityPlayer = HasPriorityPlayer() || _context.IsZoneEngagementActive;
			_engagedPriorityPlayer = (HasPriorityPlayer() ? _context.PriorityPlayer : null);
			_context.DamageProcessSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.PlayerDetectionSyncSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			if (_idleSettings.CanWander)
			{
				_context.RotateTowardsDirectionSystem.ValidateBodyObjectAssigned();
				_context.FindRandomPositionSystem.ConfigureSearchRadius(_idleSettings.WanderRadius);
				_context.NeedToFindTargetPosition = true;
				_context.FindRandomPositionSystem.Enable();
				_context.TargetPositionCompletedResetSystem.Enable();
				_context.MoveSystem.Enable();
				_context.TargetPositionCompletedSystem.Enable();
				_context.RotateTowardsDirectionSystem.Enable();
			}
			else
			{
				_context.NeedToFindTargetPosition = false;
				if (_context.NavMeshAgent.isOnNavMesh)
				{
					_context.NavMeshAgent.ResetPath();
				}
			}
		}

		public override void OnExit()
		{
			_context.FindRandomPositionSystem.Disable();
			_context.TargetPositionCompletedResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.PlayerDetectionSyncSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
		}

		public override void OnLogic()
		{
			_context.TickScreamCooldown(_enemy.GetTickDelta());
			bool num = HasPriorityPlayer();
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (!num)
			{
				if (_hadPriorityPlayer)
				{
					_context.IsZoneEngagementActive = false;
				}
				_hadPriorityPlayer = false;
				_engagedPriorityPlayer = null;
				return;
			}
			bool num2 = !_hadPriorityPlayer;
			bool flag = _engagedPriorityPlayer != null && _engagedPriorityPlayer != priorityPlayer;
			bool isZoneEngagementActive = _context.IsZoneEngagementActive;
			_hadPriorityPlayer = true;
			if (flag)
			{
				_context.IsZoneEngagementActive = false;
			}
			if (num2 || flag || !isZoneEngagementActive)
			{
				_engagedPriorityPlayer = priorityPlayer;
				_enemy.TriggerEvent(ParrotMobEvent.OnPlayerDetected);
				return;
			}
			_engagedPriorityPlayer = priorityPlayer;
			if (!(_context.ScreamCooldownRemaining > 0f))
			{
				_enemy.TriggerEvent(ParrotMobEvent.OnRepeatScream);
			}
		}

		private bool HasPriorityPlayer()
		{
			if (!_context.PlayerTriggerSensor.HasPlayerInside)
			{
				return false;
			}
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer != null)
			{
				return priorityPlayer.NetworkObject != null;
			}
			return false;
		}
	}
}
