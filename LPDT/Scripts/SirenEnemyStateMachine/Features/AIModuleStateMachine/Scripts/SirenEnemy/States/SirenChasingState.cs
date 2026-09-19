using Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.States
{
	public class SirenChasingState : StateBase<SirenCombatStateId>
	{
		private readonly SirenEnemy _enemy;

		private readonly SirenEnemyContext _context;

		private readonly SirenChaseSettings _chaseSettings;

		private readonly float _raycastThreshold;

		public SirenChasingState(SirenEnemy enemy, SirenEnemyContext context, SirenEnemySettings enemySettings, SirenChaseSettings chaseSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_chaseSettings = chaseSettings;
			_raycastThreshold = enemySettings.RaycastThreshold;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SirenVisualState.Chasing);
			_enemy.SetCurrentTargetPlayerId(_context.TargetPlayer.PlayerId);
			_enemy.SetLookAtPresentationActive(active: true);
			_context.ChaseElapsed = 0f;
			_context.LostTargetElapsed = 0f;
			_context.IsLosingTarget = false;
			_enemy.RaiseSetPlayerRotation(_context.TargetPlayer.PlayerId);
			_enemy.RaiseEnterFollowMode(_context.TargetPlayer.PlayerId);
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.ChaseElapsed += tickDelta;
			if (!_context.CanEnemyInteractWithTargetPlayer())
			{
				_enemy.TriggerEvent(SirenEvent.OnTargetLost);
			}
			else
			{
				if (_context.ChaseElapsed < _chaseSettings.MinChaseTimeBeforeAttack)
				{
					return;
				}
				if (_context.ChaseElapsed > _chaseSettings.MaxChasingTime)
				{
					_enemy.TriggerEvent(SirenEvent.OnTargetLost);
					return;
				}
				float statValue = _context.GetStatValue(EntityStatType.HardDetectionDistance);
				if (!IsTargetOutsideDetectionRange() && _context.IsTargetLookingAtMe(statValue, _raycastThreshold, angleCullEnabled: true))
				{
					_context.IsLosingTarget = false;
					_context.LostTargetElapsed = 0f;
					return;
				}
				if (!_context.IsLosingTarget)
				{
					_context.IsLosingTarget = true;
					_context.LostTargetElapsed = 0f;
					return;
				}
				_context.LostTargetElapsed += tickDelta;
				if (_context.LostTargetElapsed >= _chaseSettings.MinLookLostTime)
				{
					_enemy.TriggerEvent(SirenEvent.OnTargetLost);
				}
			}
		}

		private bool IsTargetOutsideDetectionRange()
		{
			return _context.GetDistanceToTarget() >= _context.TargetDetector.DetectionRadius + 1f;
		}
	}
}
