using Ami.BroAudio;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Locking;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Interaction.Interactables
{
	public class StaticInteractableDoor : StaticInteractable
	{
		[Header("Visual Mode")]
		[SerializeField]
		private DoorVisualMode visualMode;

		[Header("Door Settings")]
		[SerializeField]
		private float openAngle = 90f;

		[SerializeField]
		private float animationDuration = 0.4f;

		[SerializeField]
		private bool useCustomOpenCurve;

		[SerializeField]
		private Ease openEase = Ease.OutQuart;

		[SerializeField]
		private AnimationCurve openCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private bool useCustomCloseCurve;

		[SerializeField]
		private Ease closeEase = Ease.OutQuart;

		[SerializeField]
		private AnimationCurve closeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[Header("Rotation Axis")]
		[SerializeField]
		private DoorRotationAxis rotationAxis = DoorRotationAxis.Y;

		[SerializeField]
		private bool invertDirection;

		[Header("Animator (Visual Mode = Animator)")]
		[SerializeField]
		private Animator doorAnimator;

		[SerializeField]
		private string openTriggerName = "Open";

		[SerializeField]
		private string closeTriggerName = "Close";

		[Tooltip("Optional. Animator state names for the fully-open / fully-closed poses. When set, late joiners snap to the end pose instantly (no replay). Leave empty to fall back to firing the trigger on join.")]
		[SerializeField]
		private string openStateName = "";

		[SerializeField]
		private string closeStateName = "";

		[SerializeField]
		private int animatorLayer;

		[Header("Sound Settings")]
		[SerializeField]
		private SoundID openingSound;

		[SerializeField]
		private SoundID closingSound;

		[Header("Lockable (Optional)")]
		[SerializeField]
		private bool startsLocked;

		[SerializeField]
		private ChestType requiredKeyType;

		[SerializeField]
		private SoundID lockedFeedbackSound;

		[SerializeField]
		private Transform lockedShakeTarget;

		[SerializeField]
		private float lockedShakeStrength = 0.02f;

		[SerializeField]
		private float lockedShakeDuration = 0.25f;

		[SerializeField]
		private float lockedShakeFrequency = 18f;

		[Header("Stuck (Optional)")]
		[SerializeField]
		private bool startsStuck;

		[SerializeField]
		private SoundID stuckFeedbackSound;

		[SerializeField]
		private Transform stuckShakeTarget;

		[SerializeField]
		private float stuckShakeStrength = 0.02f;

		[SerializeField]
		private float stuckShakeDuration = 0.25f;

		[SerializeField]
		private float stuckShakeFrequency = 18f;

		[Header("Events")]
		[SerializeField]
		private UnityEvent onDoorOpen;

		[SerializeField]
		private UnityEvent onDoorClose;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		private StaticInteractionStateMachine<StaticDoorState> _stateMachine;

		private Quaternion _closedRotation;

		private Quaternion _openRotation;

		private StaticDoorState _currentState;

		private Tween _lockedShakeTween;

		private Tween _stuckShakeTween;

		private Tween _animatorGateTween;

		private int _openTriggerHash;

		private int _closeTriggerHash;

		private int _openStateHash;

		private int _closeStateHash;

		private bool _hasOpenStateName;

		private bool _hasCloseStateName;

		public StaticDoorState CurrentDoorState => _currentState;

		public bool IsOpen => _currentState == StaticDoorState.Open;

		protected override bool UseStateMachine => true;

		private bool IsAnimatorMode => visualMode == DoorVisualMode.Animator;

		protected override void Awake()
		{
			if (IsAnimatorMode)
			{
				_openTriggerHash = Animator.StringToHash(openTriggerName);
				_closeTriggerHash = Animator.StringToHash(closeTriggerName);
				_hasOpenStateName = !string.IsNullOrEmpty(openStateName);
				_hasCloseStateName = !string.IsNullOrEmpty(closeStateName);
				if (_hasOpenStateName)
				{
					_openStateHash = Animator.StringToHash(openStateName);
				}
				if (_hasCloseStateName)
				{
					_closeStateHash = Animator.StringToHash(closeStateName);
				}
			}
			else
			{
				Transform transform = ((base.ModelTransform != null) ? base.ModelTransform : base.transform);
				_closedRotation = transform.localRotation;
				float angle = (invertDirection ? (0f - openAngle) : openAngle);
				_openRotation = _closedRotation * Quaternion.AngleAxis(angle, GetAxisVector());
			}
			if (startsLocked && startsStuck)
			{
				_currentState = StaticDoorState.LockedAndStuckClosed;
			}
			else if (startsLocked)
			{
				_currentState = StaticDoorState.LockedClosed;
			}
			else if (startsStuck)
			{
				_currentState = StaticDoorState.StuckClosed;
			}
			base.Awake();
		}

		private new void OnDestroy()
		{
			_lockedShakeTween.Stop();
			_stuckShakeTween.Stop();
			_animatorGateTween.Stop();
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<StaticDoorState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(StaticDoorState.LockedAndStuckClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleLockedAndStuckOpen).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(StaticDoorState.LockedClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleLockedOpen).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(StaticDoorState.StuckClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleStuckOpen).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false))
				.RegisterState(StaticDoorState.Closed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleOpen).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: false))
				.RegisterState(StaticDoorState.Open, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", HandleClose).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: false));
		}

		private StaticDoorState DetermineState()
		{
			return _currentState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void ApplyStateFromManager(byte stateData, bool skipAnimation)
		{
			StaticDoorState staticDoorState = (StaticDoorState)stateData;
			if (staticDoorState == _currentState)
			{
				return;
			}
			StaticDoorState currentState = _currentState;
			_currentState = staticDoorState;
			if (skipAnimation)
			{
				if (IsAnimatorMode)
				{
					ApplyAnimatorStateInstant(staticDoorState);
				}
				else
				{
					((base.ModelTransform != null) ? base.ModelTransform : base.transform).localRotation = ((staticDoorState == StaticDoorState.Open) ? _openRotation : _closedRotation);
				}
				UpdateState();
				return;
			}
			bool num = currentState == StaticDoorState.Open;
			bool flag = staticDoorState == StaticDoorState.Open;
			if (num != flag)
			{
				AnimateToState(staticDoorState);
			}
			else
			{
				UpdateState();
			}
		}

		private void HandleLockedAndStuckOpen()
		{
			if (HasMatchingKey())
			{
				_playerService.EquipmentManager.Consume();
				RequestStateChange(3);
			}
			else
			{
				PlayLockedFeedback();
			}
		}

		private void HandleLockedOpen()
		{
			if (HasMatchingKey())
			{
				_playerService.EquipmentManager.Consume();
				RequestStateChange(0);
			}
			else
			{
				PlayLockedFeedback();
			}
		}

		private void HandleStuckOpen()
		{
			if (HasCrowbar())
			{
				RequestStateChange(1);
			}
			else
			{
				PlayStuckFeedback();
			}
		}

		private void HandleOpen()
		{
			RequestStateChange(1);
		}

		private void HandleClose()
		{
			RequestStateChange(0);
		}

		private bool HasMatchingKey()
		{
			HeldItem heldItem = _playerService?.EquipmentManager?.EquippedEntity;
			if (heldItem == null)
			{
				return false;
			}
			if (heldItem.TryGetComponent<Key>(out var component))
			{
				return component.KeyType == requiredKeyType;
			}
			return false;
		}

		private bool HasCrowbar()
		{
			HeldItem heldItem = _playerService?.EquipmentManager?.EquippedEntity;
			if (heldItem == null)
			{
				return false;
			}
			Crowbar component;
			return heldItem.TryGetComponent<Crowbar>(out component);
		}

		private void PlayLockedFeedback()
		{
			_uiFeedbackManager?.CreateFloatingMessage("@interaction.is_locked", FeedbackType.Warning);
			if (lockedFeedbackSound.IsValid())
			{
				AudioManager?.PlayOneShot(lockedFeedbackSound, base.transform.position);
			}
			PlayLockedShake();
		}

		private void PlayStuckFeedback()
		{
			_uiFeedbackManager?.CreateFloatingMessage("@interaction.is_stuck", FeedbackType.Warning);
			if (stuckFeedbackSound.IsValid())
			{
				AudioManager?.PlayOneShot(stuckFeedbackSound, base.transform.position);
			}
			PlayStuckShake();
		}

		private void PlayLockedShake()
		{
			if (!_lockedShakeTween.isAlive && !(lockedShakeStrength <= 0f) && !(lockedShakeDuration <= 0f))
			{
				Transform target = ((lockedShakeTarget != null) ? lockedShakeTarget : base.transform);
				_lockedShakeTween = Tween.ShakeLocalPosition(target, Vector3.one * lockedShakeStrength, lockedShakeDuration, lockedShakeFrequency);
			}
		}

		private void PlayStuckShake()
		{
			if (!_stuckShakeTween.isAlive && !(stuckShakeStrength <= 0f) && !(stuckShakeDuration <= 0f))
			{
				Transform target = ((stuckShakeTarget != null) ? stuckShakeTarget : base.transform);
				_stuckShakeTween = Tween.ShakeLocalPosition(target, Vector3.one * stuckShakeStrength, stuckShakeDuration, stuckShakeFrequency);
			}
		}

		private Vector3 GetAxisVector()
		{
			return rotationAxis switch
			{
				DoorRotationAxis.X => Vector3.right, 
				DoorRotationAxis.Y => Vector3.up, 
				DoorRotationAxis.Z => Vector3.forward, 
				_ => Vector3.up, 
			};
		}

		private void AnimateToState(StaticDoorState state)
		{
			SetInteractionAvailability(newValue: false);
			if (IsAnimatorMode)
			{
				AnimateAnimatorToState(state);
				return;
			}
			Transform target = ((base.ModelTransform != null) ? base.ModelTransform : base.transform);
			if (state == StaticDoorState.Open)
			{
				PlaySound(openingSound);
				(useCustomOpenCurve ? Tween.LocalRotation(target, _openRotation, animationDuration, openCurve) : Tween.LocalRotation(target, _openRotation, animationDuration, openEase)).OnComplete(this, delegate(StaticInteractableDoor staticInteractableDoor)
				{
					staticInteractableDoor.onDoorOpen?.Invoke();
					staticInteractableDoor.SetInteractionAvailability(newValue: true);
					staticInteractableDoor.UpdateState();
				});
			}
			else
			{
				PlaySound(closingSound);
				(useCustomCloseCurve ? Tween.LocalRotation(target, _closedRotation, animationDuration, closeCurve) : Tween.LocalRotation(target, _closedRotation, animationDuration, closeEase)).OnComplete(this, delegate(StaticInteractableDoor staticInteractableDoor)
				{
					staticInteractableDoor.onDoorClose?.Invoke();
					staticInteractableDoor.SetInteractionAvailability(newValue: true);
					staticInteractableDoor.UpdateState();
				});
			}
		}

		private void AnimateAnimatorToState(StaticDoorState state)
		{
			_animatorGateTween.Stop();
			if (state == StaticDoorState.Open)
			{
				if (doorAnimator != null)
				{
					doorAnimator.SetTrigger(_openTriggerHash);
				}
				PlaySound(openingSound);
				_animatorGateTween = Tween.Delay(animationDuration);
				_animatorGateTween.OnComplete(this, delegate(StaticInteractableDoor target)
				{
					target.onDoorOpen?.Invoke();
					target.SetInteractionAvailability(newValue: true);
					target.UpdateState();
				});
			}
			else
			{
				if (doorAnimator != null)
				{
					doorAnimator.SetTrigger(_closeTriggerHash);
				}
				PlaySound(closingSound);
				_animatorGateTween = Tween.Delay(animationDuration);
				_animatorGateTween.OnComplete(this, delegate(StaticInteractableDoor target)
				{
					target.onDoorClose?.Invoke();
					target.SetInteractionAvailability(newValue: true);
					target.UpdateState();
				});
			}
		}

		private void ApplyAnimatorStateInstant(StaticDoorState state)
		{
			if (!(doorAnimator == null))
			{
				bool flag = state == StaticDoorState.Open;
				if (flag ? _hasOpenStateName : _hasCloseStateName)
				{
					int stateNameHash = (flag ? _openStateHash : _closeStateHash);
					doorAnimator.Play(stateNameHash, animatorLayer, 1f);
					doorAnimator.Update(0f);
				}
				else
				{
					doorAnimator.SetTrigger(flag ? _openTriggerHash : _closeTriggerHash);
				}
			}
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				AudioManager?.PlayOneShot(sound, base.transform.position);
			}
		}

		public void Open()
		{
			if (!IsOpen)
			{
				HandleOpen();
			}
		}

		public void Close()
		{
			if (IsOpen)
			{
				HandleClose();
			}
		}
	}
}
