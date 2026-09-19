using Features.DamageableTrackModule.Scripts;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class PirateAttractionInvestigateState : StateBase<PirateStateId>
	{
		private const int SamplingAttempts = 12;

		private const float RepathInterval = 1.5f;

		private readonly PirateEnemy _enemy;

		private readonly PirateEnemyContext _context;

		private readonly PlayerMovableModel _playerMovableModel;

		private bool _hasDestination;

		private float _repathTimer;

		public PirateAttractionInvestigateState(PirateEnemy enemy, PirateEnemyContext context, PlayerMovableModel playerMovableModel)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_playerMovableModel = playerMovableModel;
		}

		public override void OnEnter()
		{
			_context.ApplyWalkingSpeed();
			_context.TargetPlayer = PlayerRef.None;
			_hasDestination = false;
			_repathTimer = 0f;
			_context.TargetDetector.OnTargetDetectedInRange += OnTargetDetected;
			if (_context.Damageable != null)
			{
				_context.Damageable.OnDamaged += OnDamaged;
			}
			MoveToApproachPoint();
		}

		public override void OnExit()
		{
			_context.TargetDetector.OnTargetDetectedInRange -= OnTargetDetected;
			if (_context.Damageable != null)
			{
				_context.Damageable.OnDamaged -= OnDamaged;
			}
		}

		public override void OnLogic()
		{
			if (_hasDestination)
			{
				if (_context.EnemyMovableBase.HasReachedEnd())
				{
					_enemy.TriggerEvent(PirateEvent.OnAttractionZoneExited);
				}
				return;
			}
			_repathTimer += GetDeltaTime();
			if (!(_repathTimer < 1.5f))
			{
				_repathTimer = 0f;
				MoveToApproachPoint();
			}
		}

		private void MoveToApproachPoint()
		{
			if (_context.TryGetAttractionApproachPoint(12, out var point) && _context.EnemyMovableBase.IsCanMoveToPoint(point))
			{
				_context.RegisterIdleWanderTargetPosition(point);
				_context.EnemyMovableBase.MoveToPoint(point);
				_hasDestination = true;
			}
		}

		private void OnTargetDetected(PlayerRef playerRef)
		{
			if (_context.IsPlayerAlive(playerRef.PlayerId) && _context.IsPlayerVisible(playerRef) && (!_playerMovableModel.AllCharacterMovables.TryGetValue(playerRef, out var value) || (!(value == null) && !_context.IsPositionInSafeZone(value.CameraPositionTransform.position))))
			{
				_context.TargetPlayer = playerRef;
				_enemy.TriggerEvent(PirateEvent.OnTargetAcquired);
			}
		}

		private void OnDamaged(DamageData damageData)
		{
			_enemy.TriggerEvent(PirateEvent.OnAttractionZoneExited);
		}

		private float GetDeltaTime()
		{
			if (!(_enemy.Runner != null))
			{
				return Time.deltaTime;
			}
			return _enemy.Runner.DeltaTime;
		}
	}
}
