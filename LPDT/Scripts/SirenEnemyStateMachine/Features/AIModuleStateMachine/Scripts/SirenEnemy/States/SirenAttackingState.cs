using System.Collections;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings;
using Features.DamageableTrackModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.States
{
	public class SirenAttackingState : StateBase<SirenCombatStateId>
	{
		private readonly SirenEnemy _enemy;

		private readonly SirenEnemyContext _context;

		private readonly SirenAttackSettings _attackSettings;

		private readonly SirenChaseSettings _chaseSettings;

		private readonly PlayerStatesConfiguration _playerStatesConfiguration;

		private readonly PlayerDamageablesTrackModel _playerDamageables;

		private readonly IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private readonly float _raycastThreshold;

		private readonly float _forceStrength;

		private Coroutine _routine;

		public SirenAttackingState(SirenEnemy enemy, SirenEnemyContext context, SirenEnemySettings enemySettings, SirenAttackSettings attackSettings, SirenChaseSettings chaseSettings, PlayerDamageablesTrackModel playerDamageables, PlayerStatesConfiguration playerStatesConfiguration, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_attackSettings = attackSettings;
			_chaseSettings = chaseSettings;
			_raycastThreshold = enemySettings.RaycastThreshold;
			_forceStrength = enemySettings.ForceStrength;
			_playerDamageables = playerDamageables;
			_playerStatesConfiguration = playerStatesConfiguration;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SirenVisualState.Attacking);
			_enemy.SetLookAtPresentationActive(active: true);
			_enemy.RaiseScreamSound(play: true);
			_enemy.RaiseSongSound(play: false);
			_enemy.RaiseScreamParticle();
			_routine = _enemy.StartCoroutine(AttackRoutine());
		}

		public override void OnExit()
		{
			if (_routine != null)
			{
				_enemy.StopCoroutine(_routine);
				_routine = null;
			}
			_enemy.RaiseScreamSound(play: false);
		}

		private IEnumerator AttackRoutine()
		{
			for (float attackTimer = 0f; attackTimer <= _attackSettings.AttackTime; attackTimer += Time.deltaTime)
			{
				if (!_context.HasTarget)
				{
					_enemy.TriggerEvent(SirenEvent.OnTargetLost);
					yield break;
				}
				if (!_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(_context.TargetPlayer.PlayerId))
				{
					_enemy.TriggerEvent(SirenEvent.OnTargetLost);
					yield break;
				}
				float statValue = _context.GetStatValue(EntityStatType.HardDetectionDistance);
				if (IsTargetOutsideAttackHoldRange() || !_context.IsTargetLookingAtMe(statValue, _raycastThreshold, angleCullEnabled: false))
				{
					_enemy.TriggerEvent(SirenEvent.OnTargetLost);
					yield break;
				}
				yield return null;
			}
			ApplyDamageToTarget();
			_enemy.TriggerEvent(SirenEvent.OnTargetLost);
		}

		private bool IsTargetOutsideAttackHoldRange()
		{
			return _context.GetDistanceToTarget() >= _chaseSettings.DistanceToAttack + 1f;
		}

		private void ApplyDamageToTarget()
		{
			if (_context.HasTarget)
			{
				int playerId = _context.TargetPlayer.PlayerId;
				if (_enemyPlayerAttackabilityService.CanEnemyAttackPlayer(playerId) && _playerDamageables.AllPlayerDamageables.TryGetValue(playerId, out var value))
				{
					value.Damage(new DamageData
					{
						Damage = _context.GetStatValue(EntityStatType.Damage),
						Direction = value.Transform.forward * -1f,
						Force = _forceStrength * _playerStatesConfiguration.StunThrowMultiplier,
						DamageDealerPlayerID = playerId,
						ForceMode = ForceMode.Impulse,
						IsStunning = true,
						Source = DamageDataSourceExtensions.ForEnemyAttack(_context.transform, EnemyType.Siren.ToString(), DamageType.Melee)
					});
				}
			}
		}
	}
}
