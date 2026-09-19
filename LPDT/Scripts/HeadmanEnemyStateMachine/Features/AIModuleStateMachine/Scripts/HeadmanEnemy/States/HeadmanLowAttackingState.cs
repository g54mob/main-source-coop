using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanLowAttackingState : StateBase<HeadmanAttackingStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanSlowedSettings _slowedSettings;

		private readonly HeadmanChasingSettings _chasingSettings;

		private bool _hasInterestApproachPoint;

		private bool _reachedInterestApproachPoint;

		private bool _presentationStarted;

		public HeadmanLowAttackingState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanSlowedSettings slowedSettings, HeadmanChasingSettings chasingSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_slowedSettings = slowedSettings;
			_chasingSettings = chasingSettings;
		}

		public override void OnEnter()
		{
			_context.CurrentAttackElapsed = 0f;
			_context.AttackDamageApplied = false;
			_context.AttackExitSent = false;
			_reachedInterestApproachPoint = false;
			_hasInterestApproachPoint = false;
			_presentationStarted = false;
			_context.HeadmanLowAttackInterestPointSystem.Enable();
			if (!_context.HeadmanLowAttackInterestPointSystem.TryResolveReachableInterestPoint())
			{
				if (_context.CheckDistanceEnoughToAttack())
				{
					BeginLowAttackPresentation();
				}
				else
				{
					_enemy.SetVisualState(HeadmanVisualState.Chasing);
				}
				return;
			}
			_hasInterestApproachPoint = true;
			_context.HeadmanLowAttackInterestPointSystem.MoveToInterestPoint();
			if (_context.HeadmanLowAttackInterestPointSystem.HasReachedInterestPoint() && _context.CheckDistanceEnoughToAttack())
			{
				BeginLowAttackPresentation();
			}
			else
			{
				_enemy.SetVisualState(HeadmanVisualState.Chasing);
			}
		}

		public override void OnLogic()
		{
			_context.AttackTimer = 0f;
			if (!_context.HasChaseTarget || !_context.CanEnemyInteractWithChaseTarget())
			{
				_enemy.TriggerEvent(HeadmanEvent.OnTargetLost);
				return;
			}
			if (_context.TryGetTargetTrackingWorldPosition(out var worldPosition))
			{
				_context.RotateTowards(worldPosition);
			}
			if (!_presentationStarted)
			{
				if (_hasInterestApproachPoint && !_reachedInterestApproachPoint)
				{
					_context.HeadmanLowAttackInterestPointSystem.MoveToInterestPoint();
					if (!_context.HeadmanLowAttackInterestPointSystem.HasReachedInterestPoint())
					{
						return;
					}
					_reachedInterestApproachPoint = true;
				}
				if (!_context.CheckDistanceEnoughToAttack())
				{
					if (!_hasInterestApproachPoint)
					{
						_context.MoveToTargetPlayer();
					}
					return;
				}
				BeginLowAttackPresentation();
			}
			if (!_presentationStarted)
			{
				return;
			}
			_context.CurrentAttackElapsed += _enemy.GetTickDelta();
			if (!_context.AttackDamageApplied && _context.CurrentAttackElapsed >= _chasingSettings.LowAttackWindupDuration)
			{
				_context.AttackDamageApplied = true;
				if (_context.TryDealLowAttackDamage())
				{
					_enemy.RaiseAttackSound();
				}
			}
			if (!_context.AttackExitSent && _context.CurrentAttackElapsed >= _chasingSettings.LowAttackWindupDuration + _chasingSettings.LowAttackActiveDuration)
			{
				_context.AttackExitSent = true;
				_enemy.TriggerEvent(HeadmanEvent.OnAttackFinished);
			}
		}

		public override void OnExit()
		{
			_context.SlowedTimer = _slowedSettings.SlowedDuration;
			_context.HeadmanLowAttackInterestPointSystem.Disable();
		}

		private void BeginLowAttackPresentation()
		{
			if (!_presentationStarted && _context.CheckDistanceEnoughToAttack())
			{
				_reachedInterestApproachPoint = true;
				_presentationStarted = true;
				_context.CurrentAttackElapsed = 0f;
				_context.AttackDamageApplied = false;
				_context.AttackExitSent = false;
				_enemy.SetVisualState(HeadmanVisualState.AttackingLow);
			}
		}
	}
}
