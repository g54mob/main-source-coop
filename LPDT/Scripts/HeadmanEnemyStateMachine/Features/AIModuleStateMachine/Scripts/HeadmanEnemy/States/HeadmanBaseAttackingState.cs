using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanBaseAttackingState : StateBase<HeadmanAttackingStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanSlowedSettings _slowedSettings;

		private readonly HeadmanChasingSettings _chasingSettings;

		public HeadmanBaseAttackingState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanSlowedSettings slowedSettings, HeadmanChasingSettings chasingSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_slowedSettings = slowedSettings;
			_chasingSettings = chasingSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.AttackingBase);
			_context.CurrentAttackElapsed = 0f;
			_context.AttackDamageApplied = false;
			_context.AttackExitSent = false;
		}

		public override void OnLogic()
		{
			_context.AttackTimer = 0f;
			_context.CurrentAttackElapsed += _enemy.GetTickDelta();
			if (!_context.AttackDamageApplied && _context.CurrentAttackElapsed >= _chasingSettings.BaseAttackWindupDuration)
			{
				_context.AttackDamageApplied = true;
				if (_context.TryDealBaseAttackDamage())
				{
					_enemy.RaiseAttackSound();
				}
			}
			Vector3 worldPosition;
			Vector3 availablePosition;
			if (!_context.AttackExitSent && _context.CurrentAttackElapsed >= _chasingSettings.BaseAttackWindupDuration + _chasingSettings.BaseAttackActiveDuration)
			{
				_context.AttackExitSent = true;
				_enemy.TriggerEvent(HeadmanEvent.OnAttackFinished);
			}
			else if (!_context.HasChaseTarget || !_context.CanEnemyInteractWithChaseTarget())
			{
				_enemy.TriggerEvent(HeadmanEvent.OnTargetLost);
			}
			else if (_context.TryGetTargetTrackingWorldPosition(out worldPosition) && _context.IsTargetPositionAvailable(worldPosition, out availablePosition) && _context.Agent != null && Vector3.Distance(availablePosition, _context.Agent.pathEndPosition) >= 0.1f)
			{
				_context.Agent.SetDestination(availablePosition);
			}
		}

		public override void OnExit()
		{
			_context.SlowedTimer = _slowedSettings.SlowedDuration;
		}
	}
}
