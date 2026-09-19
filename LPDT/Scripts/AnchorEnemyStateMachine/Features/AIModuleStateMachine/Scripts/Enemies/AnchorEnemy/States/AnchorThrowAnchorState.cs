using Features.PlayerSpawner.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorThrowAnchorState : StateBase<AnchorStateId>
	{
		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		private bool _launched;

		private float _launchTime;

		private int _launchTargetPlayerId = -1;

		public AnchorThrowAnchorState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.ThrowAnchor);
			_context.SetVisualState(AnchorVisualState.ThrowAnchor);
			_context.SetSmoothedVelocity(0f);
			_context.ClearThrowRelease();
			_launched = false;
			_launchTime = 0f;
			_launchTargetPlayerId = -1;
			_context.AnchorDamageReactionSystem.Enable();
			_context.AnchorMoveSpeedSetupSystem.Enable();
			_context.AnchorFaceTargetSystem.Enable();
			_context.AnchorVisionDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.AnchorHookControlSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.AttackCooldown = _context.ThrowCooldown;
			_context.AnchorDamageReactionSystem.Disable();
			_context.AnchorMoveSpeedSetupSystem.Disable();
			_context.AnchorFaceTargetSystem.Disable();
			_context.AnchorVisionDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (!_launched)
			{
				if (!_context.HasLivePriorityPlayer())
				{
					_context.AnchorHookControlSystem.ResetHook();
					_enemy.TriggerEvent(AnchorEvent.OnThrowCancelled);
					return;
				}
				PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
				bool throwReleaseRequested = _context.ThrowReleaseRequested;
				bool flag = _context.CurrentStateTime >= _context.ThrowWindupDuration;
				if (throwReleaseRequested || flag)
				{
					_context.AnchorHookControlSystem.Launch(GetThrowTargetPosition(priorityPlayer));
					_launchTargetPlayerId = priorityPlayer.NetworkObject.InputAuthority.PlayerId;
					_context.ClearThrowRelease();
					_context.SetVisualState(AnchorVisualState.ThrowUp);
					_launched = true;
					_launchTime = _context.CurrentStateTime;
				}
				return;
			}
			int hookedPlayerId = _context.AnchorHookControlSystem.HookedPlayerId;
			bool num = _launchTargetPlayerId >= 0 && !_context.IsPlayerSessionActive(_launchTargetPlayerId);
			bool flag2 = hookedPlayerId >= 0 && !_context.IsPlayerSessionActive(hookedPlayerId);
			if (num || flag2)
			{
				_context.AnchorHookControlSystem.ResetHook();
				_enemy.TriggerEvent(AnchorEvent.OnThrowCancelled);
				return;
			}
			if (_context.AnchorHookControlSystem.HitPlayer)
			{
				_enemy.TriggerEvent(AnchorEvent.OnAnchorHitPlayer);
				return;
			}
			float num2 = _context.CurrentStateTime - _launchTime;
			bool num3 = num2 >= _context.AnchorHookControlSystem.MinFlightTimeBeforeSettle && _context.AnchorHookControlSystem.IsAnchorSettled;
			bool flag3 = num2 >= _context.ThrowMissTimeout;
			if (num3 || flag3)
			{
				if (!_context.AnchorHookControlSystem.Missed)
				{
					_context.AnchorHookControlSystem.NotifyEmptyFlightEnded();
				}
				_enemy.TriggerEvent(AnchorEvent.OnAnchorMissed);
			}
		}

		private Vector3 GetThrowTargetPosition(PlayerDataHolder priorityPlayer)
		{
			Vector3 position = _enemy.transform.position;
			Vector3 position2 = priorityPlayer.NetworkObject.transform.position;
			Vector3 vector = position2 - position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = _enemy.transform.forward;
			}
			vector.Normalize();
			Vector3 vector2 = position2;
			vector2.y = position.y;
			float num = Mathf.Min(Vector3.Distance(new Vector3(position.x, 0f, position.z), new Vector3(position2.x, 0f, position2.z)), _context.MaxHookDistance);
			Vector3 result = position + vector * num;
			result.y = _context.AnchorHookControlSystem.LaunchAimHeight + _context.ThrowAimHeightOffset;
			return result;
		}
	}
}
