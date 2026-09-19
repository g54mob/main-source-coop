using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using Features.DamageableTrackModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperInvestigateState : StateBase<SleeperStateId>
	{
		private const int OVERLAP_BUFFER_SIZE = 64;

		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		private readonly SleeperEnemySettings _settings;

		private readonly Collider[] _overlapBuffer = new Collider[64];

		private bool _hasProbedSoundPoint;

		private Transform _approachTarget;

		public SleeperInvestigateState(SleeperEnemy enemy, SleeperEnemyContext context, SleeperEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.Investigate);
			_context.SetVisualState(SleeperVisualState.Investigate);
			_context.SetMoveSpeed(_context.GetStatValue(EntityStatType.WalkSpeed));
			_context.NavMeshAgent.stoppingDistance = 0f;
			_context.DistanceToAttack = _settings.DistanceToAttack;
			_hasProbedSoundPoint = false;
			_approachTarget = null;
			_context.SetDestinationToReachablePoint(_context.LastHeardSoundPosition);
			_context.SleeperSearchRangeSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.SleeperVisionDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SleeperDamageAggrSystem.Enable();
		}

		public override void OnExit()
		{
			_hasProbedSoundPoint = false;
			_approachTarget = null;
			_context.SleeperSearchRangeSetupSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.SleeperVisionDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.SleeperDamageAggrSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.AttackCooldown <= 0f && _context.PriorityPlayer != null)
			{
				_enemy.TriggerEvent(SleeperEvent.OnTargetAcquired);
			}
			else if (_context.CurrentStateTime >= _settings.InvestigateTimeout)
			{
				_enemy.TriggerEvent(SleeperEvent.OnInvestigateEmpty);
			}
			else if (!_hasProbedSoundPoint)
			{
				if (!_context.TargetPositionCompleted)
				{
					return;
				}
				_hasProbedSoundPoint = true;
				if (!TryFindClosestAttackable(out var closest))
				{
					_enemy.TriggerEvent(SleeperEvent.OnInvestigateEmpty);
					return;
				}
				_approachTarget = closest;
				if (IsInAttackRange(_approachTarget.position))
				{
					_enemy.TriggerEvent(SleeperEvent.OnReachedSoundPoint);
				}
				else
				{
					_context.SetDestinationToReachablePoint(_approachTarget.position);
				}
			}
			else if (_approachTarget == null)
			{
				_enemy.TriggerEvent(SleeperEvent.OnInvestigateEmpty);
			}
			else if (IsInAttackRange(_approachTarget.position) || _context.TargetPositionCompleted)
			{
				_enemy.TriggerEvent(SleeperEvent.OnReachedSoundPoint);
			}
		}

		private bool IsInAttackRange(Vector3 targetPosition)
		{
			return Vector3.Distance(_enemy.transform.position, targetPosition) <= _settings.DistanceToAttack;
		}

		private bool TryFindClosestAttackable(out Transform closest)
		{
			closest = null;
			Vector3 position = _enemy.transform.position;
			float investigateDamagableRadius = _settings.InvestigateDamagableRadius;
			float num = float.MaxValue;
			int layerMask = _settings.InvestigateOverlapMask;
			int num2 = Physics.OverlapSphereNonAlloc(position, investigateDamagableRadius, _overlapBuffer, layerMask);
			for (int i = 0; i < num2; i++)
			{
				Collider collider = _overlapBuffer[i];
				if (!(collider == null) && !collider.transform.IsChildOf(_enemy.transform) && TryResolveAttackable(collider.gameObject, out var attackable))
				{
					Vector3 vector = attackable.position - position;
					vector.y = 0f;
					float sqrMagnitude = vector.sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						closest = attackable;
					}
				}
			}
			return closest != null;
		}

		private static bool TryResolveAttackable(GameObject source, out Transform attackable)
		{
			attackable = null;
			if (FindComponent<IDamageable>(source, out var component) && component is Component component2)
			{
				attackable = component2.transform;
				return true;
			}
			if (FindComponent<MonoItem>(source, out var component3))
			{
				attackable = component3.transform;
				return true;
			}
			return false;
		}

		private static bool FindComponent<T>(GameObject source, out T component)
		{
			component = source.GetComponent<T>();
			T val = component;
			if (val == null)
			{
				component = source.GetComponentInParent<T>();
			}
			val = component;
			if (val == null)
			{
				component = source.GetComponentInChildren<T>();
			}
			return component != null;
		}
	}
}
