using Features.AIModuleStateMachine.Scripts.Core.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States
{
	public abstract class RatsHoleEnemyHoleAbsorbDespawnStateBase : StateBase<RatsHoleEnemyStateId>
	{
		private const float HoleReturnTimeoutSeconds = 12f;

		private readonly RatsHoleEnemyContext _context;

		private readonly FearHoleAbsorbAnimationSettings _fearHoleAbsorbAnimationSettings;

		private readonly RatsHoleEnemyStateId _stateId;

		private readonly RatsHoleEnemyVisualState _visualState;

		private bool _hasDestination;

		private float _absorbVisualStartTime = -1f;

		protected RatsHoleEnemy Enemy { get; }

		protected RatsHoleEnemyHoleAbsorbDespawnStateBase(RatsHoleEnemy enemy, RatsHoleEnemyContext context, FearHoleAbsorbAnimationSettings fearHoleAbsorbAnimationSettings, RatsHoleEnemyStateId stateId, RatsHoleEnemyVisualState visualState)
			: base(false, false)
		{
			Enemy = enemy;
			_context = context;
			_fearHoleAbsorbAnimationSettings = fearHoleAbsorbAnimationSettings;
			_stateId = stateId;
			_visualState = visualState;
		}

		public override void OnEnter()
		{
			Enemy.SetCurrentStateId(_stateId);
			_context.SetVisualState(_visualState);
			_context.SetPriorityPlayer(null);
			_context.MoveSpeedSetupSystem.Enable();
			_context.ResumeMovement();
			_context.CurrentStateTime = 0f;
			_absorbVisualStartTime = -1f;
			_hasDestination = TryPrepareDestination();
			if (!_hasDestination)
			{
				CompleteAbsorbDespawn();
				return;
			}
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.MoveSpeedSetupSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_hasDestination = false;
			_absorbVisualStartTime = -1f;
			_context.ClearHoleAbsorbWorldPosition();
		}

		public override void OnLogic()
		{
			if (_hasDestination && !TryInterruptForReacquire())
			{
				bool flag = _context.CurrentStateTime >= 12f;
				if (_context.TargetPositionCompleted || flag)
				{
					StartAbsorbVisualForDespawn();
					CompleteAbsorbDespawn();
				}
			}
		}

		protected abstract bool TryPrepareDestination();

		protected virtual bool TryInterruptForReacquire()
		{
			return false;
		}

		private void StartAbsorbVisualForDespawn()
		{
			if (!(_absorbVisualStartTime >= 0f))
			{
				_absorbVisualStartTime = _context.CurrentStateTime;
				DisableMovementSystemsForAbsorb();
				_context.StopMovement();
				if (_context.HasHoleAbsorbWorldPosition)
				{
					Enemy.PlayHoleAbsorbVisualToTargetRpc(_fearHoleAbsorbAnimationSettings.ScaleDuration, _fearHoleAbsorbAnimationSettings.EndScale, (int)_fearHoleAbsorbAnimationSettings.ScaleEase, _context.HoleAbsorbWorldPosition);
				}
				else
				{
					Enemy.PlayHoleAbsorbVisualRpc(_fearHoleAbsorbAnimationSettings.ScaleDuration, _fearHoleAbsorbAnimationSettings.EndScale, (int)_fearHoleAbsorbAnimationSettings.ScaleEase);
				}
			}
		}

		private void DisableMovementSystemsForAbsorb()
		{
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
		}

		private void CompleteAbsorbDespawn()
		{
			Enemy.RequestDespawn();
		}
	}
}
