using Features.AIModuleStateMachine.Scripts.SharkEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public class SharkMimicHuntState : StateBase<SharkStateId>
	{
		private readonly SharkEnemy _enemy;

		private readonly SharkEnemyContext _context;

		private readonly SharkWaterTargetSensor _sensor;

		private readonly SharkMovementSettings _movement;

		private readonly SharkTargetingSettings _targeting;

		private readonly SharkAttackSettings _attack;

		public SharkMimicHuntState(SharkEnemy enemy, SharkEnemyContext context, SharkWaterTargetSensor sensor, SharkMovementSettings movement, SharkTargetingSettings targeting, SharkAttackSettings attack)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_sensor = sensor;
			_movement = movement;
			_targeting = targeting;
			_attack = attack;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SharkVisualState.Hunting);
			_context.AttackCooldownElapsed = 0f;
			if (_context.Agent.enabled)
			{
				_context.Agent.speed = _movement.ActiveMovementSpeed;
			}
		}

		public override void OnLogic()
		{
			_context.AttackCooldownElapsed += _enemy.GetTickDelta();
			if (_context.TargetObject == null || !_context.TargetObject.IsValid)
			{
				_enemy.TriggerEvent(SharkEvent.OnTargetLost);
				return;
			}
			Vector3 position = _context.TargetObject.transform.position;
			Vector3 position2 = _enemy.transform.position;
			float num = HorizontalDist(position2, position);
			if (num > _targeting.MaxHuntRadius)
			{
				_enemy.TriggerEvent(SharkEvent.OnTargetLost);
				return;
			}
			if (_targeting.RequireHuntVolume && _context.HuntBounds != null && !_context.HuntBounds.bounds.Contains(position))
			{
				_enemy.TriggerEvent(SharkEvent.OnTargetLost);
				return;
			}
			if (!_context.IsChaseTargetReachableOnNavMesh(position, _movement.MaxHuntNavMeshPathLength, _movement.NearestNavigationPointRange))
			{
				_enemy.TriggerEvent(SharkEvent.OnTargetLost);
				return;
			}
			if (!_sensor.IsMimicEligibleForAttack(position))
			{
				_enemy.TriggerEvent(SharkEvent.OnTargetLost);
				return;
			}
			float sampleRange = Mathf.Max(_movement.NearestNavigationPointRange, _attack.AttackRange * 1.5f);
			_context.TrySetChaseDestinationNear(position, sampleRange);
			if (num <= _attack.AttackRange && _context.AttackCooldownElapsed >= _attack.AttackCooldown)
			{
				_enemy.TriggerEvent(SharkEvent.OnCommitSimpleAttack);
			}
		}

		private float HorizontalDist(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}
	}
}
