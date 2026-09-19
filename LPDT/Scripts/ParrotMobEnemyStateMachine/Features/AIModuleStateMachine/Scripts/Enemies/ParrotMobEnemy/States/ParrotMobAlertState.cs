using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using Features.PlayerSpawner.Scripts;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.States
{
	public class ParrotMobAlertState : StateBase<ParrotMobStateId>
	{
		private readonly ParrotMobEnemy _enemy;

		private readonly ParrotMobEnemyContext _context;

		private readonly ParrotMobAlertSettings _alertSettings;

		private readonly ParrotMobAudioController _audioController;

		public ParrotMobAlertState(ParrotMobEnemy enemy, ParrotMobEnemyContext context, ParrotMobAlertSettings alertSettings, ParrotMobAudioController audioController)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_alertSettings = alertSettings;
			_audioController = audioController;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(ParrotMobStateId.Alert);
			_enemy.SetVisualState(ParrotMobVisualState.Alert);
			_context.IsZoneEngagementActive = true;
			if (_context.NavMeshAgent.isOnNavMesh)
			{
				_context.NavMeshAgent.ResetPath();
			}
			_context.DamageProcessSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.PlayerDetectionSyncSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_audioController.PlayAlert();
		}

		public override void OnExit()
		{
			_context.StateDurationTimeSystem.Disable();
			_context.PlayerDetectionSyncSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
		}

		public override void OnLogic()
		{
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (!_context.PlayerTriggerSensor.HasPlayerInside || priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				_context.IsZoneEngagementActive = false;
				_enemy.TriggerEvent(ParrotMobEvent.OnPlayerLost);
			}
			else if (!(_context.CurrentStateTime < _alertSettings.AlertDuration))
			{
				_enemy.TriggerEvent(ParrotMobEvent.OnAlertCompleted);
			}
		}
	}
}
