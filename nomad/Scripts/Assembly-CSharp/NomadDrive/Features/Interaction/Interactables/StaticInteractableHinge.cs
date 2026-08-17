using System;
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
	public class StaticInteractableHinge : StaticInteractable
	{
		[Header("Hinge Settings")]
		[SerializeField]
		private float openAngle = 90f;

		[SerializeField]
		private DoorRotationAxis rotationAxis = DoorRotationAxis.Y;

		[SerializeField]
		private bool invertDirection;

		[Header("Animation Settings")]
		[SerializeField]
		private float openingDuration = 0.5f;

		[SerializeField]
		private float closingDuration = 0.5f;

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
		public UnityEvent OnOpened = new UnityEvent();

		public UnityEvent OnClosed = new UnityEvent();

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		private StaticInteractionStateMachine<StaticHingeState> _stateMachine;

		private Sequence _currentSequence;

		private Quaternion _closedRotation;

		private Quaternion _openRotation;

		private StaticHingeState _currentState;

		private Action _onAnimateComplete;

		private Tween _lockedShakeTween;

		private Tween _stuckShakeTween;

		public bool IsOpen => _currentState == StaticHingeState.Opened;

		protected override bool UseStateMachine => true;

		protected override void Awake()
		{
			_closedRotation = base.transform.localRotation;
			float angle = (invertDirection ? (0f - openAngle) : openAngle);
			_openRotation = _closedRotation * Quaternion.AngleAxis(angle, GetAxisVector());
			if (startsLocked && startsStuck)
			{
				_currentState = StaticHingeState.LockedAndStuckClosed;
			}
			else if (startsLocked)
			{
				_currentState = StaticHingeState.LockedClosed;
			}
			else if (startsStuck)
			{
				_currentState = StaticHingeState.StuckClosed;
			}
			base.Awake();
		}

		private new void OnDestroy()
		{
			_currentSequence.Stop();
			_lockedShakeTween.Stop();
			_stuckShakeTween.Stop();
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<StaticHingeState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(StaticHingeState.LockedAndStuckClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleLockedAndStuckOpen).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(StaticHingeState.LockedClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleLockedOpen).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(StaticHingeState.StuckClosed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", HandleStuckOpen).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false))
				.RegisterState(StaticHingeState.Closed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", Open).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
					.WithInteractionLabelVisibility(visible: false))
				.RegisterState(StaticHingeState.Opened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", Close).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
					.WithInteractionLabelVisibility(visible: false));
		}

		private StaticHingeState DetermineState()
		{
			return _currentState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void ApplyStateFromManager(byte stateData, bool skipAnimation)
		{
			StaticHingeState staticHingeState = (StaticHingeState)stateData;
			if (staticHingeState == _currentState)
			{
				return;
			}
			StaticHingeState currentState = _currentState;
			_currentState = staticHingeState;
			if (skipAnimation)
			{
				base.transform.localRotation = ((staticHingeState == StaticHingeState.Opened) ? _openRotation : _closedRotation);
				UpdateState();
				return;
			}
			bool num = currentState == StaticHingeState.Opened;
			bool flag = staticHingeState == StaticHingeState.Opened;
			if (num != flag)
			{
				if (flag)
				{
					HandleOpening();
				}
				else
				{
					HandleClosing();
				}
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

		private void HandleOpening()
		{
			PlaySound(openingSound);
			OnOpened.Invoke();
			AnimateHinge(_openRotation, openingDuration, delegate
			{
				UpdateState();
			});
		}

		private void HandleClosing()
		{
			PlaySound(closingSound);
			AnimateHinge(_closedRotation, closingDuration, delegate
			{
				OnClosed.Invoke();
				UpdateState();
			});
		}

		private void AnimateHinge(Quaternion targetRotation, float duration, Action onComplete)
		{
			SetInteractionAvailability(newValue: false);
			_onAnimateComplete = onComplete;
			_currentSequence.Stop();
			_currentSequence = Sequence.Create().Chain(Tween.LocalRotation(base.transform, targetRotation, duration, Ease.OutBounce)).ChainCallback(this, delegate(StaticInteractableHinge target)
			{
				target.SetInteractionAvailability(newValue: true);
				target._onAnimateComplete?.Invoke();
			});
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				AudioManager?.PlayOneShot(sound, base.transform.position);
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

		public void Open()
		{
			if (!IsOpen)
			{
				RequestStateChange(1);
			}
		}

		public void Close()
		{
			if (IsOpen)
			{
				RequestStateChange(0);
			}
		}

		public void SetDurations(float openingDuration, float closingDuration)
		{
			this.openingDuration = openingDuration;
			this.closingDuration = closingDuration;
		}

		public void SetSounds(SoundID openingSound, SoundID closingSound)
		{
			this.openingSound = openingSound;
			this.closingSound = closingSound;
		}
	}
}
