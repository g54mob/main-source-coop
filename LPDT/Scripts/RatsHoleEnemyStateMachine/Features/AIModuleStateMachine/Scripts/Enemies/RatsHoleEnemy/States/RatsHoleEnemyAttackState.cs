using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.Settings;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerSpawner.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States
{
	public class RatsHoleEnemyAttackState : StateBase<RatsHoleEnemyStateId>
	{
		private const float PriorityLostGraceSeconds = 0.4f;

		private const float AttackPerformingExitRangePadding = 0.25f;

		private readonly RatsHoleEnemy _enemy;

		private readonly RatsHoleEnemyContext _context;

		private readonly RatsHoleEnemyAttackSettings _attackSettings;

		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private float _priorityLostTimer;

		private bool _hitApplied;

		public RatsHoleEnemyAttackState(RatsHoleEnemy enemy, RatsHoleEnemyContext context, RatsHoleEnemyAttackSettings attackSettings, PlayerDamageablesTrackModel playerDamageablesTrackModel)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_attackSettings = attackSettings;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
		}

		public override void OnEnter()
		{
			ResetAttackState();
			_enemy.SnapBodyRotationTowardPriorityPlayer();
			_context.AttackReactor.TryDealBaseAttackDamage += OnAttackAnimationHit;
			_context.AttackReactor.TryAttackAnimationFinished += CompleteAttackPerforming;
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		private void ResetAttackState()
		{
			_enemy.SetCurrentStateId(RatsHoleEnemyStateId.Attack);
			_context.SetVisualState(RatsHoleEnemyVisualState.Attack);
			_priorityLostTimer = 0f;
			_hitApplied = false;
			_context.SetIsAttackPerforming(value: false);
			_context.StopMovement();
			_context.SetSmoothedVelocity(0f);
			_context.TargetSearchRange = _context.AggroRange;
		}

		public override void OnExit()
		{
			CompleteAttackPerforming();
			_context.AttackReactor.TryDealBaseAttackDamage -= OnAttackAnimationHit;
			_context.AttackReactor.TryAttackAnimationFinished -= CompleteAttackPerforming;
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			float num = ((_enemy.Runner != null) ? _enemy.Runner.DeltaTime : Time.deltaTime);
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				_priorityLostTimer += num;
				if (_priorityLostTimer >= 0.4f)
				{
					_enemy.LosePriorityTarget();
				}
				return;
			}
			_priorityLostTimer = 0f;
			if (_enemy.IsPriorityPlayerOutsideGate())
			{
				CompleteAttackPerforming();
				_enemy.LosePriorityTarget();
				return;
			}
			if (!_enemy.CanInteractWithPriorityPlayer())
			{
				_enemy.LosePriorityTarget();
				return;
			}
			if (_enemy.IsPriorityPlayerInSafeZone())
			{
				CompleteAttackPerforming();
				_enemy.LoseSafeZonePriorityTargetToPatrolFallback();
				return;
			}
			Vector3 position = priorityPlayer.NetworkObject.transform.position;
			float rangePadding = (_context.IsAttackPerforming ? 0.25f : 0f);
			if (!IsInAttackRange(position, rangePadding))
			{
				if (!_context.IsAttackPerforming)
				{
					_enemy.TriggerEvent(RatsHoleEnemyEvent.OnOutOfAttackRange);
				}
			}
			else if (!_context.IsAttackPerforming)
			{
				float currentTime = GetCurrentTime();
				if (!(currentTime < _context.NextAttackAllowedTime))
				{
					StartAttackPresentation(currentTime);
				}
			}
		}

		private float GetCurrentTime()
		{
			if (!(_enemy.Runner != null))
			{
				return Time.time;
			}
			return _enemy.Runner.SimulationTime;
		}

		private void StartAttackPresentation(float currentTime)
		{
			_hitApplied = false;
			_enemy.SnapBodyRotationTowardPriorityPlayer();
			_context.RequestAttackPresentation();
			_context.SetIsAttackPerforming(value: true);
			_context.NextAttackAllowedTime = currentTime + _attackSettings.AttackFrequency;
		}

		private void OnAttackAnimationHit()
		{
			if (!_enemy.HasStateAuthority || !_context.IsAttackPerforming || _hitApplied)
			{
				return;
			}
			_hitApplied = true;
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (!(priorityPlayer?.NetworkObject == null) && _enemy.CanInteractWithPriorityPlayer() && !_enemy.IsPriorityPlayerOutsideGate() && !_enemy.IsPriorityPlayerInSafeZone())
			{
				Vector3 position = priorityPlayer.NetworkObject.transform.position;
				if (IsInAttackRange(position))
				{
					DealDamage(priorityPlayer, position);
				}
			}
		}

		private bool IsInAttackRange(Vector3 playerPosition, float rangePadding = 0f)
		{
			Vector3 vector = playerPosition - _enemy.transform.position;
			float magnitude = new Vector2(vector.x, vector.z).magnitude;
			float num = _attackSettings.AttackRange + rangePadding;
			if (magnitude > num)
			{
				return false;
			}
			if (vector.y < 0f)
			{
				return Mathf.Abs(vector.y) <= _attackSettings.MaxAttackVerticalDelta;
			}
			return vector.y <= _attackSettings.MaxAttackVerticalDelta;
		}

		private void DealDamage(PlayerDataHolder target, Vector3 playerPosition)
		{
			int playerId = target.NetworkObject.InputAuthority.PlayerId;
			if (_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(playerId, out var value))
			{
				Vector3 direction = playerPosition - _enemy.transform.position;
				direction.y = 0f;
				if (direction.sqrMagnitude > 0.0001f)
				{
					direction.Normalize();
				}
				else
				{
					direction = _enemy.transform.forward;
				}
				value.Damage(new DamageData
				{
					Damage = _attackSettings.Damage,
					Position = _enemy.transform.position,
					Direction = direction,
					DamageDealerPlayerID = _enemy.Object.StateAuthority.PlayerId,
					Force = _attackSettings.ForceStrength,
					ForceMode = ForceMode.Impulse,
					Source = DamageDataSourceExtensions.ForEnemyAttack(_enemy.transform, EnemyType.RatsHoleEnemy.ToString(), DamageType.Melee)
				});
			}
		}

		private void CompleteAttackPerforming()
		{
			_context.SetIsAttackPerforming(value: false);
		}
	}
}
