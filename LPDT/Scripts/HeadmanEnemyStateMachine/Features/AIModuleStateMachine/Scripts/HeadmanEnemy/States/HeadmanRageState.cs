using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanRageState : StateBase<HeadmanRageStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanRageSettings _settings;

		public HeadmanRageState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanRageSettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.Rage);
			_context.ClearTarget();
			_context.RoarsPerformed = 0;
			_context.RageIsApproachingCenter = false;
			_context.ActiveRageSubstate = HeadmanRageStateId.Rage;
			InitializeRageSession();
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.RageTimeLeft -= tickDelta;
			_context.RageWanderTimer += tickDelta;
			_context.RageApproachTimer += tickDelta;
			if (!_context.HasLastSeenPlayerPosition)
			{
				_enemy.TriggerEvent(HeadmanEvent.OnRageFinished);
				return;
			}
			if (_context.Agent != null)
			{
				_context.Agent.speed = _settings.RageWanderSpeed;
			}
			if (_context.RageApproachTimer >= _settings.RageApproachInterval)
			{
				Vector3 closestReachablePointToLastSeen = GetClosestReachablePointToLastSeen();
				_context.MoveToPosition(closestReachablePointToLastSeen);
				_context.RageIsApproachingCenter = true;
				_context.RageApproachTimer = 0f;
			}
			else if (_context.RageWanderTimer >= _settings.RageWanderPositionUpdateTime)
			{
				_context.MoveToPosition(_context.GetRandomNavmeshPosition(_settings.RageRadius));
				_context.RageWanderTimer = 0f;
			}
		}

		private void InitializeRageSession()
		{
			if (!_context.RageSessionInitialized)
			{
				Vector3 lastSeenPlayerPosition = _context.transform.position;
				if (_context.LastPlayerToChase != PlayerRef.None && _context.TryGetPlayerTrackingWorldPosition(_context.LastPlayerToChase, out var worldPosition))
				{
					lastSeenPlayerPosition = worldPosition;
				}
				_context.LastSeenPlayerPosition = lastSeenPlayerPosition;
				_context.HasLastSeenPlayerPosition = true;
				_context.RageTimeLeft = _settings.RageDuration;
				_context.RageWanderTimer = 0f;
				_context.RageApproachTimer = 0f;
				_context.RageSessionInitialized = true;
				_context.RageIsApproachingCenter = false;
			}
		}

		private Vector3 GetClosestReachablePointToLastSeen()
		{
			if (_context.TryGetBlockingSafeZoneOnRage(out var safeZone) && safeZone != null && safeZone.InteractionPoints != null && safeZone.InteractionPoints.Count > 0)
			{
				Transform transform = safeZone.InteractionPoints[Random.Range(0, safeZone.InteractionPoints.Count)];
				if (transform != null && NavMesh.SamplePosition(transform.position, out var hit, _settings.RageRadius, (_context.Agent != null) ? _context.Agent.areaMask : (-1)))
				{
					return hit.position;
				}
			}
			if (NavMesh.SamplePosition(_context.HasLastSeenPlayerPosition ? _context.LastSeenPlayerPosition : _context.transform.position, out var hit2, _settings.RageRadius, (_context.Agent != null) ? _context.Agent.areaMask : (-1)))
			{
				return hit2.position;
			}
			return _context.transform.position;
		}
	}
}
