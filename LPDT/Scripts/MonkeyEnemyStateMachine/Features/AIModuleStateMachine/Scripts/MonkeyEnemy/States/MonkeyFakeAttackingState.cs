using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyFakeAttackingState : StateBase<MonkeyPlayerInteractionStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyPlayerInteractionSettings _interactionSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		private bool _hasPlayedIntimidation;

		public MonkeyFakeAttackingState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyPlayerInteractionSettings interactionSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_interactionSettings = interactionSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.FakeAttacking);
			_enemy.ResetStateTimerRandom(_interactionSettings.MinStateDuration, _interactionSettings.MaxStateDuration);
			_enemy.RaiseMoveMiddleAnimation();
			_enemy.RaiseMoveAnimation();
			_hasPlayedIntimidation = false;
			_context.EnableMovement();
			_context.Agent.speed = _interactionSettings.RunAroundSpeed;
			_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
			_context.FakeAttackTargetPosition = ResolveBackoffPosition();
		}

		public override void OnLogic()
		{
			if (!_context.HasTarget || !_context.IsTargetAlive() || !_enemy.CanEnemyInteractWithTarget())
			{
				_enemy.LoseInterestInTarget();
				return;
			}
			if (_enemy.IsStarving)
			{
				_enemy.TriggerEvent(MonkeyEvent.OnCombatRequired);
				return;
			}
			Vector3 targetPosition = _context.GetTargetPosition();
			if (Vector3.Distance(_context.transform.position, _context.FakeAttackTargetPosition) > _interactionSettings.FakeAttackDestinationTolerance)
			{
				_context.MoveToPosition(_context.FakeAttackTargetPosition);
			}
			else
			{
				_context.StopAgent();
				_context.FacePosition(targetPosition, _movementSettings.RotationSpeed, _enemy.GetTickDelta());
				if (!_hasPlayedIntimidation)
				{
					_enemy.RaiseFakeAttackAnimation();
					_hasPlayedIntimidation = true;
				}
			}
			if (_enemy.AdvanceStateTimer())
			{
				_enemy.TriggerEvent(MonkeyEvent.OnInteractionStepCompleted);
			}
		}

		private Vector3 ResolveBackoffPosition()
		{
			Vector3 targetPosition = _context.GetTargetPosition();
			Vector3 normalized = (_context.transform.position - targetPosition).normalized;
			normalized.y = 0f;
			Vector3 vector = targetPosition + normalized * _interactionSettings.FakeAttackBackoffDistance;
			if (!NavMesh.SamplePosition(vector, out var hit, _interactionSettings.FakeAttackBackoffDistance, -1))
			{
				return vector;
			}
			return hit.position;
		}
	}
}
