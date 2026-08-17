using System;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using Mirror;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction.Interactables
{
	public class StaticInteractableRail : StaticInteractable
	{
		[Header("Rail Settings")]
		[SerializeField]
		private Transform connectedTransform;

		[SerializeField]
		private Vector3 railDirection;

		[SerializeField]
		private float railDistance;

		[Header("Animation Settings")]
		[SerializeField]
		private float openingDuration = 0.5f;

		[SerializeField]
		private float closingDuration = 0.5f;

		[SerializeField]
		private AnimationEaseConfig openingEaseConfig = new AnimationEaseConfig();

		[SerializeField]
		private AnimationEaseConfig closingEaseConfig = new AnimationEaseConfig();

		[Header("Sound Settings")]
		[SerializeField]
		private SoundID openingSound;

		[SerializeField]
		private SoundID closingSound;

		[Header("Events")]
		public UnityEvent OnOpened = new UnityEvent();

		public UnityEvent OnClosed = new UnityEvent();

		private StaticInteractionStateMachine<StaticRailState> _stateMachine;

		private Vector3 _initialPosition;

		private Vector3 _targetPosition;

		private Sequence _currentSequence;

		private StaticRailState _currentState;

		private Action _onAnimateComplete;

		public bool IsOpen => _currentState == StaticRailState.Opened;

		protected override bool UseStateMachine => true;

		protected override void Awake()
		{
			InitializeRail();
			base.Awake();
		}

		private new void OnDestroy()
		{
			_currentSequence.Stop();
		}

		private void InitializeRail()
		{
			_initialPosition = base.transform.localPosition;
			_targetPosition = _initialPosition + railDirection * railDistance;
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<StaticRailState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(StaticRailState.Closed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.open", Open).WithCrosshair(CrosshairType.Interact)).RegisterState(StaticRailState.Opened, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.close", Close).WithCrosshair(CrosshairType.Interact));
		}

		private StaticRailState DetermineState()
		{
			return _currentState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void ApplyStateFromManager(byte stateData, bool skipAnimation)
		{
			StaticRailState staticRailState = (StaticRailState)stateData;
			if (staticRailState == _currentState)
			{
				return;
			}
			_currentState = staticRailState;
			if (skipAnimation)
			{
				if (staticRailState == StaticRailState.Opened)
				{
					base.transform.localPosition = _targetPosition;
				}
				else
				{
					base.transform.localPosition = _initialPosition;
				}
				UpdateState();
			}
			else if (staticRailState == StaticRailState.Opened)
			{
				HandleOpening();
			}
			else
			{
				HandleClosing();
			}
		}

		private void HandleOpening()
		{
			PlaySoundNetworked(openingSound);
			OnOpened.Invoke();
			AnimateRail(_targetPosition, openingDuration, openingEaseConfig, delegate
			{
				UpdateState();
			});
		}

		private void HandleClosing()
		{
			PlaySoundNetworked(closingSound);
			AnimateRail(_initialPosition, closingDuration, closingEaseConfig, delegate
			{
				OnClosed.Invoke();
				UpdateState();
			});
		}

		private void AnimateRail(Vector3 targetPos, float duration, AnimationEaseConfig easeConfig, Action onComplete)
		{
			SetInteractionAvailability(newValue: false);
			_onAnimateComplete = onComplete;
			_currentSequence.Stop();
			_currentSequence = Sequence.Create().Chain(easeConfig.CreateLocalPositionTween(base.transform, targetPos, duration)).ChainCallback(this, delegate(StaticInteractableRail target)
			{
				target.SetInteractionAvailability(newValue: true);
				target._onAnimateComplete?.Invoke();
			});
		}

		private void PlaySoundNetworked(SoundID sound)
		{
			if (sound.IsValid() && NetworkServer.active)
			{
				NetworkAudioRelay?.PlayOneShot(sound, base.transform.position);
			}
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
