using Features.AIModuleStateMachine.Scripts.SharkEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public class SharkIdleState : StateBase<SharkStateId>
	{
		private readonly SharkEnemy _enemy;

		private readonly SharkEnemyContext _context;

		private readonly SharkWaterTargetSensor _sensor;

		private readonly SharkMovementSettings _movement;

		private readonly SharkTargetingSettings _targeting;

		public SharkIdleState(SharkEnemy enemy, SharkEnemyContext context, SharkWaterTargetSensor sensor, SharkMovementSettings movement, SharkTargetingSettings targeting)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_sensor = sensor;
			_movement = movement;
			_targeting = targeting;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SharkVisualState.Idle);
			_context.TargetPlayer = PlayerRef.None;
			_context.TargetObject = null;
			if (_context.Agent.enabled)
			{
				_context.Agent.speed = _movement.IdleMovementSpeed;
				if (_context.Agent.isOnNavMesh)
				{
					_context.Agent.ResetPath();
				}
			}
			_context.UpdateWanderUpdateFrequency(_movement);
			_context.WanderTimer = 0f;
			_context.IdleChangeAreaTimer = 0f;
			_context.LineOfSightLostElapsed = 0f;
			_context.TrySetWanderDestination(_movement);
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.WanderTimer += tickDelta;
			_context.IdleChangeAreaTimer += tickDelta;
			if (_context.IdleChangeAreaTimer >= _movement.IdleChangeAreaUpdateFrequency)
			{
				_enemy.RaiseChangeAreaTriggered(_enemy.transform.position);
				_context.IdleChangeAreaTimer = 0f;
			}
			if (_context.WanderTimer >= _context.WanderUpdateFrequency)
			{
				if (_context.TrySetWanderDestination(_movement))
				{
					_context.WanderTimer = 0f;
					_context.UpdateWanderUpdateFrequency(_movement);
				}
				else
				{
					_context.WanderTimer = _context.WanderUpdateFrequency * 0.5f;
				}
			}
			Vector3 position = _enemy.transform.position;
			if ((!_sensor.TryAcquireTarget(position, out var target, out var targetObject) || !TryStartHunt(target, targetObject)) && _sensor.TryAcquireMimicTarget(position, out var mimicObject))
			{
				_context.TargetPlayer = PlayerRef.None;
				_context.TargetObject = mimicObject;
				_enemy.TriggerEvent(SharkEvent.OnMimicTargetAcquired);
			}
		}

		private bool TryStartHunt(PlayerRef target, NetworkObject obj)
		{
			Vector3 position = _enemy.transform.position;
			Vector3 position2 = obj.transform.position;
			if (position2.y - position.y >= _targeting.MinHighGroundDelta)
			{
				return false;
			}
			if (_sensor.IsPlayerProtectedFromAttack(position2, position))
			{
				return false;
			}
			if (!_context.IsChaseTargetReachableOnNavMesh(position2, _movement.MaxHuntNavMeshPathLength, _movement.NearestNavigationPointRange))
			{
				return false;
			}
			_enemy.BeginPlayerHunt(target, obj);
			return true;
		}
	}
}
