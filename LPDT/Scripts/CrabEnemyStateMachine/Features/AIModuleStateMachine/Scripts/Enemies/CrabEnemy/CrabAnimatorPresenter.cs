using FMODUnity;
using Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Animation;
using Features.AudioServiceModule.Scripts;
using Fusion;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class CrabAnimatorPresenter : NetworkBehaviour
	{
		private const float MovementAnimSpeedPerUnitMoveSpeed = 0.75f;

		private const float LateralSignDeadZone = 0.05f;

		private static readonly int DirectionHash = Animator.StringToHash("Direction");

		private static readonly int HandClosedHash = Animator.StringToHash("HandClosed");

		private static readonly int LeftHandClosedHash = Animator.StringToHash("LeftHandClosed");

		private static readonly int RightHandClosedHash = Animator.StringToHash("RightHandClosed");

		private static readonly int DetachingLeftHash = Animator.StringToHash("DetachingLeft");

		private static readonly int DetachingRightHash = Animator.StringToHash("DetachingRight");

		private static readonly int MovementAnimSpeedHash = Animator.StringToHash("MovementAnimSpeed");

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private Transform _bodyTransform;

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[SerializeField]
		private EventReference _detachingSound;

		private CrabEnemyContext _context;

		private IAudioService _audioService;

		private StateMachine<CrabAnimationStateId> _animationFsm;

		private CrabAnimationStateId _activeAnimationState;

		private float _lastLateralSign = 1f;

		private bool _fsmInitialized;

		public CrabClawSide ActiveClaw
		{
			get
			{
				if (!(_context != null))
				{
					return CrabClawSide.None;
				}
				return _context.ActiveClaw;
			}
		}

		[Inject]
		private void InjectDependencies(CrabEnemyContext context, IAudioService audioService)
		{
			_context = context;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			BuildAnimationFsm();
			_animationFsm.Init();
			_activeAnimationState = CrabAnimationStateId.Locomotion;
			_fsmInitialized = true;
			SyncAnimationState(force: false);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_fsmInitialized = false;
			_animationFsm = null;
			_activeAnimationState = CrabAnimationStateId.None;
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && !(_context == null))
			{
				UpdateNetworkedDirection();
			}
		}

		public override void Render()
		{
			if (_fsmInitialized && !(_context == null) && !(_animator == null))
			{
				SyncAnimationState(force: false);
				_animationFsm.OnLogic();
			}
		}

		public void SetHandClosed(float value)
		{
			if (_animator != null)
			{
				_animator.SetFloat(HandClosedHash, value);
			}
		}

		public void SetHandClosedForClaw(CrabClawSide claw, bool closed)
		{
			if (!(_animator == null))
			{
				switch (claw)
				{
				case CrabClawSide.Left:
					_animator.SetBool(LeftHandClosedHash, closed);
					break;
				case CrabClawSide.Right:
					_animator.SetBool(RightHandClosedHash, closed);
					break;
				}
			}
		}

		public void SetLeftHandClosed(bool closed)
		{
			if (_animator != null)
			{
				_animator.SetBool(LeftHandClosedHash, closed);
			}
		}

		public void SetRightHandClosed(bool closed)
		{
			if (_animator != null)
			{
				_animator.SetBool(RightHandClosedHash, closed);
			}
		}

		public void SetDetachingLeft(bool active)
		{
			if (_animator != null)
			{
				_animator.SetBool(DetachingLeftHash, active);
			}
		}

		public void SetDetachingRight(bool active)
		{
			if (_animator != null)
			{
				_animator.SetBool(DetachingRightHash, active);
			}
		}

		public void ApplyLocomotionParams()
		{
			if (!(_animator == null) && !(_context == null))
			{
				_animator.SetFloat(DirectionHash, _context.Direction);
				float moveSpeed = _context.MoveSpeed;
				float value = ((moveSpeed > 0.01f) ? (moveSpeed * 0.75f) : 1f);
				_animator.SetFloat(MovementAnimSpeedHash, value);
			}
		}

		public void ApplyStoppedLocomotionParams()
		{
			if (!(_animator == null))
			{
				_animator.SetFloat(DirectionHash, 0f);
				_animator.SetFloat(MovementAnimSpeedHash, 1f);
			}
		}

		public void PlayDetachingSound()
		{
			if (_audioService != null && !_detachingSound.IsNull && !(_soundSource == null))
			{
				_audioService.PlayOneShot(_detachingSound, _soundSource);
			}
		}

		private void BuildAnimationFsm()
		{
			_animationFsm = new StateMachine<CrabAnimationStateId>();
			_animationFsm.AddState(CrabAnimationStateId.Locomotion, new CrabAnimationLocomotionState(this));
			_animationFsm.AddState(CrabAnimationStateId.CarryingItem, new CrabAnimationCarryingItemState(this));
			_animationFsm.AddState(CrabAnimationStateId.HoldingPlayer, new CrabAnimationHoldingPlayerState(this));
			_animationFsm.AddState(CrabAnimationStateId.DetachingLeft, new CrabAnimationDetachingLeftState(this));
			_animationFsm.AddState(CrabAnimationStateId.DetachingRight, new CrabAnimationDetachingRightState(this));
			_animationFsm.AddState(CrabAnimationStateId.Rest, new CrabAnimationRestState(this));
			_animationFsm.AddState(CrabAnimationStateId.Fear, new CrabAnimationFearState(this));
			_animationFsm.SetStartState(CrabAnimationStateId.Locomotion);
		}

		private void SyncAnimationState(bool force)
		{
			CrabAnimationStateId crabAnimationStateId = MapVisualState(_context.VisualState);
			if (force || crabAnimationStateId != _activeAnimationState)
			{
				_animationFsm.RequestStateChange(crabAnimationStateId);
				_activeAnimationState = crabAnimationStateId;
			}
		}

		private static CrabAnimationStateId MapVisualState(CrabVisualState visualState)
		{
			return visualState switch
			{
				CrabVisualState.CarryingItem => CrabAnimationStateId.CarryingItem, 
				CrabVisualState.HoldingPlayer => CrabAnimationStateId.HoldingPlayer, 
				CrabVisualState.DetachingLeft => CrabAnimationStateId.DetachingLeft, 
				CrabVisualState.DetachingRight => CrabAnimationStateId.DetachingRight, 
				CrabVisualState.Rest => CrabAnimationStateId.Rest, 
				CrabVisualState.Fear => CrabAnimationStateId.Fear, 
				_ => CrabAnimationStateId.Locomotion, 
			};
		}

		private void UpdateNetworkedDirection()
		{
			if (_context.NavMeshAgent == null || !_context.NavMeshAgent.isActiveAndEnabled)
			{
				_context.SetDirection(0f);
				return;
			}
			Vector3 velocity = _context.NavMeshAgent.velocity;
			velocity.y = 0f;
			float magnitude = velocity.magnitude;
			float num = Mathf.Max(_context.MoveSpeed, 0.01f);
			float num2 = Mathf.Clamp01(magnitude / num);
			if (num2 < 0.01f)
			{
				_context.SetDirection(0f);
				return;
			}
			Transform transform = ((_bodyTransform != null) ? _bodyTransform : base.transform);
			float f = Vector3.Dot(velocity.normalized, transform.right);
			if (Mathf.Abs(f) > 0.05f)
			{
				_lastLateralSign = Mathf.Sign(f);
			}
			_context.SetDirection(_lastLateralSign * num2);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
