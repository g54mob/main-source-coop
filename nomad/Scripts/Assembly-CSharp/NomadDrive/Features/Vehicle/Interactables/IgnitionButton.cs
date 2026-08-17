using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Vehicle.Enums;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class IgnitionButton : VehicleInteractable
	{
		public Transform socketTransform;

		[Header("Audio")]
		[SerializeField]
		private SoundID ignitionSuccessSound;

		[SerializeField]
		private SoundID ignitionFailSound;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		private StaticInteractionStateMachine<IgnitionButtonState> _stateMachine;

		private Sequence _sequence;

		private readonly List<IgnitionResult> _hardBlockers = new List<IgnitionResult>();

		private readonly List<IgnitionResult> _warnings = new List<IgnitionResult>();

		public UnityEvent OnIgnitionButtonOn { get; } = new UnityEvent();

		public UnityEvent OnIgnitionButtonOff { get; } = new UnityEvent();

		public UnityEvent OnStartEngineRequested { get; } = new UnityEvent();

		public UnityEvent OnStopEngineRequested { get; } = new UnityEvent();

		public IgnitionState IgnitionState { get; private set; }

		public IgnitionButtonState CurrentButtonState => _stateMachine?.CurrentState ?? IgnitionButtonState.Extinguished;

		protected override bool UseStateMachine => true;

		protected override bool HasNetworkState => true;

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<IgnitionButtonState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(IgnitionButtonState.Extinguished, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.start_engine", HandleIgnite).WithInteractionLabelVisibility(visible: false).WithCrosshair(CrosshairType.Interact)).RegisterState(IgnitionButtonState.Ignited, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.stop_engine", HandleExtinguish).WithInteractionLabelVisibility(visible: false).WithCrosshair(CrosshairType.Interact));
		}

		private IgnitionButtonState DetermineState()
		{
			if (IgnitionState != IgnitionState.Ignited)
			{
				return IgnitionButtonState.Extinguished;
			}
			return IgnitionButtonState.Ignited;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void ApplyStateFromNetwork(byte stateData, bool skipAnimation)
		{
			IgnitionState ignitionState = IgnitionState;
			IgnitionState = (IgnitionState)stateData;
			UpdateState();
			if (stateData == 1)
			{
				OnIgnitionButtonOn.Invoke();
				if (ignitionState != IgnitionState.Ignited)
				{
					OnStartEngineRequested.Invoke();
				}
				return;
			}
			if (!skipAnimation)
			{
				StopIgnitionRotateAnimation();
			}
			OnIgnitionButtonOff.Invoke();
			if (ignitionState == IgnitionState.Ignited)
			{
				OnStopEngineRequested.Invoke();
			}
		}

		private void StartIgnitionRotateAnimation(float duration)
		{
			_sequence.Stop();
			float x = socketTransform.localEulerAngles.x;
			float z = socketTransform.localEulerAngles.z;
			_sequence = Sequence.Create().Chain(Tween.LocalRotation(socketTransform, Quaternion.Euler(x, 120f, z), duration, Ease.OutBack));
		}

		private void StopIgnitionRotateAnimation()
		{
			_sequence.Stop();
			float x = socketTransform.localEulerAngles.x;
			float z = socketTransform.localEulerAngles.z;
			_sequence = Sequence.Create().Chain(Tween.LocalRotation(socketTransform, Quaternion.Euler(x, 0f, z), 0.25f, Ease.Linear));
		}

		private void HandleIgnite()
		{
			if (_hardBlockers.Count > 0)
			{
				if (ignitionFailSound.IsValid())
				{
					NetworkAudioRelay?.PlayOneShot(ignitionFailSound, base.transform.position);
				}
				ShowBlockerMessages();
				return;
			}
			StartIgnitionRotateAnimation(1f);
			if (ignitionSuccessSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(ignitionSuccessSound, base.transform.position);
			}
			OnStartEngineRequested.Invoke();
			ShowWarningMessages();
		}

		private void HandleExtinguish()
		{
			OnStopEngineRequested.Invoke();
		}

		public void SetIgnitionState(IReadOnlyList<IgnitionResult> hardBlockers, IReadOnlyList<IgnitionResult> warnings)
		{
			_hardBlockers.Clear();
			if (hardBlockers != null)
			{
				_hardBlockers.AddRange(hardBlockers);
			}
			_warnings.Clear();
			if (warnings != null)
			{
				_warnings.AddRange(warnings);
			}
		}

		private void ShowBlockerMessages()
		{
			foreach (IgnitionResult hardBlocker in _hardBlockers)
			{
				string messageKey = GetMessageKey(hardBlocker);
				if (messageKey != null)
				{
					_uiFeedbackManager?.CreateFloatingMessage(messageKey, FeedbackType.Error);
				}
			}
		}

		private void ShowWarningMessages()
		{
			foreach (IgnitionResult warning in _warnings)
			{
				string messageKey = GetMessageKey(warning);
				if (messageKey != null)
				{
					_uiFeedbackManager?.CreateFloatingMessage(messageKey, FeedbackType.Warning);
				}
			}
		}

		private static string GetMessageKey(IgnitionResult result)
		{
			return result switch
			{
				IgnitionResult.EngineNotInstalled => "@vehicle.engine_not_installed", 
				IgnitionResult.EngineBroken => "@vehicle.engine_broken", 
				IgnitionResult.EngineOverheated => "@vehicle.engine_overheated", 
				IgnitionResult.BatteryNotInstalled => "@vehicle.battery_not_installed", 
				IgnitionResult.BatteryBroken => "@vehicle.battery_broken", 
				IgnitionResult.FuelEmpty => "@vehicle.fuel_empty", 
				IgnitionResult.HandbrakeEngaged => "@vehicle.handbrake_engaged", 
				_ => null, 
			};
		}

		public void Ignite()
		{
			if (IgnitionState != IgnitionState.Ignited)
			{
				IgnitionState = IgnitionState.Ignited;
				RequestStateChange(1);
			}
		}

		public void Extinguish()
		{
			if (IgnitionState != IgnitionState.Extinguished)
			{
				IgnitionState = IgnitionState.Extinguished;
				RequestStateChange(2);
			}
		}

		public void RequestStartFromInput(IReadOnlyList<IgnitionResult> warnings)
		{
			_warnings.Clear();
			if (warnings != null)
			{
				_warnings.AddRange(warnings);
			}
			StartIgnitionRotateAnimation(1f);
			if (ignitionSuccessSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(ignitionSuccessSound, base.transform.position);
			}
			OnStartEngineRequested.Invoke();
			ShowWarningMessages();
			PlayInteractSound();
		}

		public void ShowStartBlockedFromInput(IReadOnlyList<IgnitionResult> blockers)
		{
			_hardBlockers.Clear();
			if (blockers != null)
			{
				_hardBlockers.AddRange(blockers);
			}
			if (ignitionFailSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(ignitionFailSound, base.transform.position);
			}
			ShowBlockerMessages();
			PlayInteractSound();
		}

		public void RequestStopFromInput()
		{
			OnStopEngineRequested.Invoke();
			PlayInteractSound();
		}
	}
}
