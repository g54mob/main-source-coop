using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyCauldronedState : StateBase<MonkeyStateId>
	{
		private enum CauldronedPhase
		{
			None = 0,
			Idle = 1,
			Jump = 2
		}

		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyCauldronSettings _cauldronSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		private readonly EnemyHeadwearModel _enemyHeadwearModel;

		private CauldronedPhase _phase;

		private float _phaseTimer;

		private int _jumpCount;

		private bool _isFinishing;

		public MonkeyCauldronedState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyCauldronSettings cauldronSettings, MonkeyMovementSettings movementSettings, EnemyHeadwearModel enemyHeadwearModel)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_cauldronSettings = cauldronSettings;
			_movementSettings = movementSettings;
			_enemyHeadwearModel = enemyHeadwearModel;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.Stun);
			_enemy.ResetStateTimerRandom(_cauldronSettings.MinDuration, _cauldronSettings.MaxDuration);
			_jumpCount = 0;
			_isFinishing = false;
			_context.ClearTargetItem();
			_context.AnimationEvent.OnAttack += OnJumpLanded;
			StartIdlePhase();
		}

		public override void OnLogic()
		{
			uint raw = _enemy.NetworkObject.Id.Raw;
			if (!_enemyHeadwearModel.IsWearing(raw))
			{
				if (_enemyHeadwearModel.TryConsumeRemover(raw, out var playerId))
				{
					_enemy.AttackRemover(playerId);
				}
				else
				{
					_enemy.TriggerEvent(MonkeyEvent.OnRecovered);
				}
				return;
			}
			if (_enemy.AdvanceStateTimer())
			{
				_isFinishing = true;
			}
			AdvanceJumpCycle();
		}

		public override void OnExit()
		{
			_context.AnimationEvent.OnAttack -= OnJumpLanded;
			_enemy.StopMoveAngrySound();
			_context.StopAgent();
			_context.Agent.speed = _movementSettings.WanderingSpeed;
			uint raw = _enemy.NetworkObject.Id.Raw;
			if (_enemyHeadwearModel.TryGet(raw, out var headwear) && headwear != null)
			{
				_enemyHeadwearModel.Set(raw, isWearing: false);
				Vector3 impulse = Vector3.up * _cauldronSettings.UpImpulse + _enemy.transform.forward * _cauldronSettings.ForwardImpulse;
				headwear.DropFromEnemy(impulse);
			}
		}

		private void AdvanceJumpCycle()
		{
			_phaseTimer -= _enemy.GetTickDelta();
			if (_isFinishing && _phase == CauldronedPhase.Jump && HasReachedJumpPeak())
			{
				_enemy.TriggerEvent(MonkeyEvent.OnRecovered);
			}
			else if (!(_phaseTimer > 0f))
			{
				switch (_phase)
				{
				case CauldronedPhase.Idle:
					StartJumpPhase();
					break;
				case CauldronedPhase.Jump:
					StartIdlePhase();
					break;
				}
			}
		}

		private bool HasReachedJumpPeak()
		{
			return _phaseTimer <= _cauldronSettings.JumpMoveDuration * (1f - _cauldronSettings.JumpPeakNormalizedTime);
		}

		private void StartIdlePhase()
		{
			_enemy.StopMoveAngrySound();
			_context.StopAgent();
			_enemy.RaiseIdleAnimation();
			_phase = CauldronedPhase.Idle;
			_phaseTimer = _cauldronSettings.IdleDuration;
		}

		private void StartJumpPhase()
		{
			Vector3 randomNavmeshPosition = _context.GetRandomNavmeshPosition(_context.transform.position, _cauldronSettings.JumpRadius);
			_context.SnapFacePosition(randomNavmeshPosition);
			_enemy.RaiseAttackJumpAnimation(_cauldronSettings.JumpAnimationStartNormalizedTime);
			_jumpCount++;
			if (IsAngrySoundJump())
			{
				_enemy.RaiseMoveAngrySound();
			}
			_context.Agent.speed = _cauldronSettings.JumpSpeed;
			_context.MoveToPosition(randomNavmeshPosition);
			_phase = CauldronedPhase.Jump;
			_phaseTimer = _cauldronSettings.JumpMoveDuration;
		}

		private bool IsAngrySoundJump()
		{
			if (_cauldronSettings.AngrySoundJumpInterval > 0)
			{
				return _jumpCount % _cauldronSettings.AngrySoundJumpInterval == 0;
			}
			return false;
		}

		private void OnJumpLanded()
		{
			_enemy.RaiseHitSound();
		}
	}
}
