using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class PirateIdleState : StateBase<PirateStateId>
	{
		private readonly PirateEnemy _pirateEnemy;

		private readonly PirateEnemyContext _pirateEnemyContext;

		private bool _hasWanderDestination;

		private readonly PlayerMovableModel _playerMovableModel;

		public PirateIdleState(PirateEnemy pirateEnemy, PirateEnemyContext pirateEnemyContext, PlayerMovableModel playerMovableModel)
			: base(false, false)
		{
			_pirateEnemy = pirateEnemy;
			_pirateEnemyContext = pirateEnemyContext;
			_playerMovableModel = playerMovableModel;
		}

		public override void OnEnter()
		{
			_pirateEnemyContext.ApplyWalkingSpeed();
			_pirateEnemy.ResetFleeTimer();
			_pirateEnemyContext.TargetPlayer = PlayerRef.None;
			_pirateEnemyContext.ResetIdleWanderTimer();
			_pirateEnemyContext.IdleChangeAreaTimer = 0f;
			_hasWanderDestination = false;
			_pirateEnemyContext.TargetDetector.OnTargetDetectedInRange += OnTargetDetected;
		}

		public override void OnExit()
		{
			_pirateEnemyContext.TargetDetector.OnTargetDetectedInRange -= OnTargetDetected;
		}

		private void OnTargetDetected(PlayerRef playerRef)
		{
			if (_pirateEnemyContext.IsPlayerAlive(playerRef.PlayerId) && _pirateEnemyContext.IsPlayerVisible(playerRef) && !_pirateEnemyContext.IsPlayerStealthedByHeadwear(playerRef.PlayerId) && (!_playerMovableModel.AllCharacterMovables.TryGetValue(playerRef, out var value) || (!(value == null) && !_pirateEnemyContext.IsPositionInSafeZone(value.CameraPositionTransform.position))))
			{
				_pirateEnemyContext.TargetPlayer = playerRef;
				_pirateEnemy.TriggerEvent(PirateEvent.OnTargetAcquired);
			}
		}

		public override void OnLogic()
		{
			float deltaTime = GetDeltaTime();
			if (TryAcquireAuxTarget(deltaTime))
			{
				return;
			}
			_pirateEnemyContext.IdleChangeAreaTimer += deltaTime;
			if (_pirateEnemyContext.IdleChangeAreaTimer >= _pirateEnemyContext.PirateConfiguration.IdleChangeAreaUpdateFrequency)
			{
				_pirateEnemy.RaiseChangeAreaTriggered(_pirateEnemy.transform.position);
				_pirateEnemyContext.IdleChangeAreaTimer = 0f;
			}
			if (!_hasWanderDestination || _pirateEnemyContext.EnemyMovableBase.HasReachedEnd())
			{
				if (_hasWanderDestination)
				{
					_pirateEnemyContext.EnemyMovableBase.ResetPath();
					_hasWanderDestination = false;
				}
				_hasWanderDestination = TryMoveToWanderPosition();
			}
		}

		private bool TryAcquireAuxTarget(float deltaTime)
		{
			if (_pirateEnemyContext.TargetPlayer != PlayerRef.None)
			{
				return false;
			}
			if (!_pirateEnemyContext.TryAcquireAuxTarget(deltaTime))
			{
				return false;
			}
			_pirateEnemyContext.EnemyMovableBase.ResetPath();
			_hasWanderDestination = false;
			_pirateEnemy.TriggerEvent(PirateEvent.OnTargetAcquired);
			return true;
		}

		private bool TryMoveToWanderPosition()
		{
			if (!_pirateEnemyContext.TryGetIdleWanderPosition(out var position))
			{
				return false;
			}
			if (!_pirateEnemyContext.EnemyMovableBase.IsCanMoveToPoint(position))
			{
				return false;
			}
			_pirateEnemyContext.RegisterIdleWanderTargetPosition(position);
			_pirateEnemyContext.EnemyMovableBase.MoveToPoint(position);
			return true;
		}

		private float GetDeltaTime()
		{
			if (!(_pirateEnemy.Runner != null))
			{
				return Time.deltaTime;
			}
			return _pirateEnemy.Runner.DeltaTime;
		}
	}
}
