using Features.AIModuleStateMachine.Scripts.SharkEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public class SharkHuntState : StateBase<SharkStateId>
	{
		private readonly SharkEnemy _enemy;

		private readonly SharkEnemyContext _context;

		private readonly SharkWaterTargetSensor _sensor;

		private readonly SharkMovementSettings _movement;

		private readonly SharkTargetingSettings _targeting;

		private readonly SharkAttackSettings _attack;

		public SharkHuntState(SharkEnemy enemy, SharkEnemyContext context, SharkWaterTargetSensor sensor, SharkMovementSettings movement, SharkTargetingSettings targeting, SharkAttackSettings attack)
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
			_enemy.RaiseStartHuntSound();
			_enemy.SetVisualState(SharkVisualState.Hunting);
			_context.LineOfSightLostElapsed = 0f;
			if (_context.Agent.enabled)
			{
				_context.Agent.speed = _movement.ActiveMovementSpeed;
			}
		}

		public override void OnLogic()
		{
			_context.AttackCooldownElapsed += _enemy.GetTickDelta();
			if (!_context.CanEnemyInteractWithTargetPlayer())
			{
				_enemy.TriggerEvent(SharkEvent.OnTargetLost);
				return;
			}
			if (_context.TargetObject == null)
			{
				_enemy.LosePlayerTarget();
				return;
			}
			Vector3 position = _context.TargetObject.transform.position;
			Vector3 position2 = _enemy.transform.position;
			float num = HorizontalDist(position2, position);
			bool flag = position.y - position2.y >= _targeting.MinHighGroundDelta;
			if (num > _targeting.MaxHuntRadius)
			{
				_enemy.LosePlayerTarget();
				return;
			}
			if (_targeting.RequireHuntVolume && _context.HuntBounds != null && !_context.HuntBounds.bounds.Contains(position))
			{
				_enemy.LosePlayerTarget();
				return;
			}
			if (!_context.IsChaseTargetReachableOnNavMesh(position, _movement.MaxHuntNavMeshPathLength, _movement.NearestNavigationPointRange))
			{
				_enemy.LosePlayerTarget();
				return;
			}
			if (_sensor.IsPlayerProtectedFromAttack(position, position2))
			{
				_enemy.LosePlayerTarget();
				return;
			}
			if (!_context.TryGetPlayerObject(_context.TargetPlayer, out var playerObject) || playerObject != _context.TargetObject)
			{
				_enemy.LosePlayerTarget();
				return;
			}
			if (!UpdateLineOfSightState())
			{
				_enemy.LosePlayerTarget();
				return;
			}
			float sampleRange = Mathf.Max(_movement.NearestNavigationPointRange, _attack.AttackRange * 1.5f);
			if (flag)
			{
				if (num <= _attack.AttackRange && _context.AttackCooldownElapsed >= _attack.AttackCooldown)
				{
					bool useRealBite;
					if (_sensor.IsTargetAirborne(_context.TargetPlayer))
					{
						if (_sensor.IsPlayerProtectedFromAttack(position, position2))
						{
							_enemy.LosePlayerTarget();
						}
						else
						{
							_enemy.TryCommitPlayerBite();
						}
					}
					else if (_sensor.TryResolveJumpBiteOverSurface(position, out useRealBite) && useRealBite)
					{
						if (_sensor.IsPlayerProtectedFromAttack(position, position2))
						{
							_enemy.LosePlayerTarget();
						}
						else
						{
							_enemy.TryCommitPlayerBite();
						}
					}
					else
					{
						_enemy.TriggerEvent(SharkEvent.OnHuntWithdraw);
					}
				}
				else
				{
					_context.TrySetChaseDestinationNear(position, sampleRange);
				}
			}
			else
			{
				_context.TrySetChaseDestinationNear(position, sampleRange);
				if (num <= _attack.AttackRange && _context.AttackCooldownElapsed >= _attack.AttackCooldown)
				{
					_enemy.TryCommitPlayerBite();
				}
			}
		}

		private static float HorizontalDist(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		private bool UpdateLineOfSightState()
		{
			if (!_targeting.UseLineOfSight)
			{
				_context.LineOfSightLostElapsed = 0f;
				return true;
			}
			if (_sensor.HasLineOfSightToTargetPlayer())
			{
				_context.LineOfSightLostElapsed = 0f;
				return true;
			}
			_context.LineOfSightLostElapsed += _enemy.GetTickDelta();
			return _context.LineOfSightLostElapsed < _targeting.LoseAggroAfterLosLostTime;
		}
	}
}
