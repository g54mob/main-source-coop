using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanChasingState : StateBase<HeadmanChasingStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanChasingSettings _settings;

		public HeadmanChasingState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanChasingSettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.Chasing);
			_context.ActiveChasingSubstate = HeadmanChasingStateId.Chase;
			if (_context.Agent != null)
			{
				_context.Agent.speed = _settings.SprintSpeed * _settings.ChasingSpeedCoefficient;
			}
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.AttackTimer += tickDelta;
			if (!_context.HasChaseTarget || !_context.CanEnemyInteractWithChaseTarget())
			{
				_context.HeadmanLowAttackInterestPointSystem.Disable();
				_enemy.TriggerEvent((!_context.ShouldEnterRageWhenNoTargets()) ? HeadmanEvent.OnTargetLost : HeadmanEvent.OnRageStart);
			}
			else
			{
				if (TryHandleLowAttackInterestPointApproach())
				{
					return;
				}
				if (!_context.MoveToTargetPlayer())
				{
					if (!_context.HasPrioritizedTarget)
					{
						_enemy.TriggerEvent((!_context.ShouldEnterRageWhenNoTargets()) ? HeadmanEvent.OnTargetLost : HeadmanEvent.OnRageStart);
					}
				}
				else if (_context.CanStartAttack())
				{
					_context.UseDefaultAreaMaskOnly();
					_enemy.TriggerEvent(HeadmanEvent.OnAttackStart);
				}
			}
		}

		public override void OnExit()
		{
			_context.HeadmanLowAttackInterestPointSystem.Disable();
		}

		private bool TryHandleLowAttackInterestPointApproach()
		{
			if (!_context.ShouldUseLowAttack())
			{
				_context.HeadmanLowAttackInterestPointSystem.Disable();
				return false;
			}
			_context.HeadmanLowAttackInterestPointSystem.Enable();
			if (!_context.HeadmanLowAttackInterestPointSystem.TryResolveReachableInterestPoint())
			{
				return false;
			}
			_context.HeadmanLowAttackInterestPointSystem.MoveToInterestPoint();
			if (_context.HeadmanLowAttackInterestPointSystem.CanStartLowAttackAtInterestPoint())
			{
				_context.UseDefaultAreaMaskOnly();
				_enemy.TriggerEvent(HeadmanEvent.OnAttackStart);
			}
			return true;
		}
	}
}
