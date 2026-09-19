using Features.AIModuleStateMachine.Scripts.SharkEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public class SharkAttractionInvestigateState : StateBase<SharkStateId>
	{
		private const float RepathInterval = 1.5f;

		private readonly SharkEnemy _enemy;

		private readonly SharkEnemyContext _context;

		private readonly SharkWaterTargetSensor _sensor;

		private readonly SharkMovementSettings _movement;

		private readonly SharkTargetingSettings _targeting;

		private float _repathTimer;

		private bool _hasApproachPoint;

		public SharkAttractionInvestigateState(SharkEnemy enemy, SharkEnemyContext context, SharkWaterTargetSensor sensor, SharkMovementSettings movement, SharkTargetingSettings targeting)
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
				_context.Agent.speed = _movement.ActiveMovementSpeed;
			}
			_repathTimer = 0f;
			_hasApproachPoint = _context.TrySetZoneApproachDestination(_movement);
		}

		public override void OnLogic()
		{
			if (!_hasApproachPoint)
			{
				_repathTimer += _enemy.GetTickDelta();
				if (_repathTimer >= 1.5f)
				{
					_repathTimer = 0f;
					_hasApproachPoint = _context.TrySetZoneApproachDestination(_movement);
				}
				return;
			}
			if (HasArrivedOrStuck())
			{
				_enemy.TriggerEvent(SharkEvent.OnAttractionZoneExited);
				return;
			}
			Vector3 position = _enemy.transform.position;
			if ((!_sensor.TryAcquireTarget(position, out var target, out var targetObject) || !TryStartHunt(target, targetObject)) && _sensor.TryAcquireMimicTarget(position, out var mimicObject))
			{
				_context.TargetPlayer = PlayerRef.None;
				_context.TargetObject = mimicObject;
				_enemy.TriggerEvent(SharkEvent.OnMimicTargetAcquired);
			}
		}

		private bool HasArrivedOrStuck()
		{
			if (_context.Agent == null || !_context.Agent.isOnNavMesh || _context.Agent.pathPending)
			{
				return false;
			}
			if (_context.Agent.pathStatus == NavMeshPathStatus.PathComplete)
			{
				return _context.Agent.remainingDistance <= _context.Agent.stoppingDistance;
			}
			return true;
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
