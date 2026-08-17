using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Interactables;
using NomadDrive.Features.Vehicle.Networking;
using NomadDrive.Features.Vehicle.Parts.Brakelight;
using NomadDrive.Features.Vehicle.Parts.Headlights;
using NomadDrive.Features.Vehicle.Parts.SignalLight;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleLightsManager : VehicleModule
	{
		private const string HEADLIGHTS = "Headlights";

		private const string BRAKELIGHTS = "Brakelights";

		private const string SIGNALS = "Signal Lights";

		private const string ADDITIONAL = "Additional Lights";

		private const string STATE = "Runtime State";

		[SerializeField]
		private HeadlightButton headlightButton;

		[SerializeField]
		private HeadlightSlot leftHeadlightSlot;

		[SerializeField]
		private HeadlightSlot rightHeadlightSlot;

		[SerializeField]
		private BrakelightSlot leftBrakelightSlot;

		[SerializeField]
		private BrakelightSlot rightBrakelightSlot;

		[SerializeField]
		private WarningLightButton warningLightButton;

		[SerializeField]
		private List<VehicleSignalLightSlot> signalLightSlots = new List<VehicleSignalLightSlot>();

		[SerializeField]
		private float signalBlinkInterval = 0.4f;

		[SerializeField]
		private List<AdditionalLightEntry> additionalLights = new List<AdditionalLightEntry>();

		public HeadLightState CurrentHeadlightState;

		public SignalMode CurrentSignalMode;

		public bool IsBatteryUseful;

		public bool IsIgnited;

		public bool IsBraking;

		private bool _isHazardActive;

		private SignalDirection _directionalSignal;

		private bool _signalBlinkPhase;

		private CancellationTokenSource _signalCts;

		private VehicleNetworkSync _networkSync;

		private VehicleBatteryModule _batteryModule;

		private readonly VehicleLightAnimator _animator = new VehicleLightAnimator();

		public Headlight InstalledLeftHeadlight
		{
			get
			{
				if (!(leftHeadlightSlot != null))
				{
					return null;
				}
				return leftHeadlightSlot.InstalledHeadlight;
			}
		}

		public Headlight InstalledRightHeadlight
		{
			get
			{
				if (!(rightHeadlightSlot != null))
				{
					return null;
				}
				return rightHeadlightSlot.InstalledHeadlight;
			}
		}

		public Brakelight InstalledLeftBrakelight
		{
			get
			{
				if (!(leftBrakelightSlot != null))
				{
					return null;
				}
				return leftBrakelightSlot.InstalledBrakelight;
			}
		}

		public Brakelight InstalledRightBrakelight
		{
			get
			{
				if (!(rightBrakelightSlot != null))
				{
					return null;
				}
				return rightBrakelightSlot.InstalledBrakelight;
			}
		}

		public IReadOnlyList<VehicleSignalLightSlot> SignalLightSlots => signalLightSlots;

		public IReadOnlyList<AdditionalLightEntry> AdditionalLights => additionalLights;

		public VehicleLightAnimator Animator => _animator;

		public bool IsHazardActive => _isHazardActive;

		public int InstalledSignalLightCount
		{
			get
			{
				if (signalLightSlots == null)
				{
					return 0;
				}
				int num = 0;
				foreach (VehicleSignalLightSlot signalLightSlot in signalLightSlots)
				{
					if (signalLightSlot != null && signalLightSlot.InstalledSignalLight != null)
					{
						num++;
					}
				}
				return num;
			}
		}

		private async void Start()
		{
			_networkSync = base.VehicleManager.NetworkSync;
			_batteryModule = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (_batteryModule != null)
			{
				IsBatteryUseful = _batteryModule.IsBatteryUseful;
				_batteryModule.RegisterConsumer(IsLowBeamActive, GetLowBeamMultiplier);
				_batteryModule.RegisterConsumer(IsHighBeamActive, GetHighBeamMultiplier);
			}
			WarnIfCriticalReferencesMissing();
			await UniTask.DelayFrame(3, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			if (!(this == null))
			{
				SyncVisualsToCurrentState();
			}
		}

		private void WarnIfCriticalReferencesMissing()
		{
			_ = headlightButton == null;
			if (leftHeadlightSlot == null)
			{
				_ = rightHeadlightSlot == null;
			}
			_ = warningLightButton == null;
			_ = _batteryModule == null;
		}

		protected override void SubscribeEvents()
		{
			if (headlightButton != null)
			{
				headlightButton.OnHeadlightButtonOff.AddListener(HandleHeadlightOff);
				headlightButton.OnLowBeamHeadlightButtonActivated.AddListener(HandleHeadlightLow);
				headlightButton.OnHighBeamHeadlightButtonActivated.AddListener(HandleHeadlightHigh);
			}
			if (leftHeadlightSlot != null)
			{
				leftHeadlightSlot.OnHeadlightInstalled.AddListener(OnHeadlightInstalled);
				leftHeadlightSlot.OnHeadlightRemoved.AddListener(OnHeadlightRemoved);
			}
			if (rightHeadlightSlot != null)
			{
				rightHeadlightSlot.OnHeadlightInstalled.AddListener(OnHeadlightInstalled);
				rightHeadlightSlot.OnHeadlightRemoved.AddListener(OnHeadlightRemoved);
			}
			if (leftBrakelightSlot != null)
			{
				leftBrakelightSlot.OnBrakelightInstalled.AddListener(OnBrakelightInstalled);
				leftBrakelightSlot.OnBrakelightRemoved.AddListener(OnBrakelightRemoved);
			}
			if (rightBrakelightSlot != null)
			{
				rightBrakelightSlot.OnBrakelightInstalled.AddListener(OnBrakelightInstalled);
				rightBrakelightSlot.OnBrakelightRemoved.AddListener(OnBrakelightRemoved);
			}
			foreach (VehicleSignalLightSlot signalLightSlot in signalLightSlots)
			{
				if (!(signalLightSlot == null))
				{
					signalLightSlot.OnSignalLightInstalled.AddListener(OnSignalLightInstalled);
					signalLightSlot.OnSignalLightRemoved.AddListener(OnSignalLightRemoved);
				}
			}
			if (warningLightButton != null)
			{
				warningLightButton.OnWarningLightButtonOn.AddListener(HandleWarningOn);
				warningLightButton.OnWarningLightButtonOff.AddListener(HandleWarningOff);
			}
			base.EventBus.OnBatteryUsefulChanged += OnBatteryUsefulChanged;
			base.EventBus.OnBatteryDepleted += OnBatteryDepleted;
			base.EventBus.OnEngineStarted += OnEngineStarted;
			base.EventBus.OnEngineStopped += OnEngineStopped;
			base.EventBus.OnBrakePressed += OnBrakePressedEvent;
			base.EventBus.OnBrakeReleased += OnBrakeReleasedEvent;
		}

		protected override void UnsubscribeEvents()
		{
			if (headlightButton != null)
			{
				headlightButton.OnHeadlightButtonOff.RemoveListener(HandleHeadlightOff);
				headlightButton.OnLowBeamHeadlightButtonActivated.RemoveListener(HandleHeadlightLow);
				headlightButton.OnHighBeamHeadlightButtonActivated.RemoveListener(HandleHeadlightHigh);
			}
			if (leftHeadlightSlot != null)
			{
				leftHeadlightSlot.OnHeadlightInstalled.RemoveListener(OnHeadlightInstalled);
				leftHeadlightSlot.OnHeadlightRemoved.RemoveListener(OnHeadlightRemoved);
			}
			if (rightHeadlightSlot != null)
			{
				rightHeadlightSlot.OnHeadlightInstalled.RemoveListener(OnHeadlightInstalled);
				rightHeadlightSlot.OnHeadlightRemoved.RemoveListener(OnHeadlightRemoved);
			}
			if (leftBrakelightSlot != null)
			{
				leftBrakelightSlot.OnBrakelightInstalled.RemoveListener(OnBrakelightInstalled);
				leftBrakelightSlot.OnBrakelightRemoved.RemoveListener(OnBrakelightRemoved);
			}
			if (rightBrakelightSlot != null)
			{
				rightBrakelightSlot.OnBrakelightInstalled.RemoveListener(OnBrakelightInstalled);
				rightBrakelightSlot.OnBrakelightRemoved.RemoveListener(OnBrakelightRemoved);
			}
			foreach (VehicleSignalLightSlot signalLightSlot in signalLightSlots)
			{
				if (!(signalLightSlot == null))
				{
					signalLightSlot.OnSignalLightInstalled.RemoveListener(OnSignalLightInstalled);
					signalLightSlot.OnSignalLightRemoved.RemoveListener(OnSignalLightRemoved);
				}
			}
			if (warningLightButton != null)
			{
				warningLightButton.OnWarningLightButtonOn.RemoveListener(HandleWarningOn);
				warningLightButton.OnWarningLightButtonOff.RemoveListener(HandleWarningOff);
			}
			base.EventBus.OnBatteryUsefulChanged -= OnBatteryUsefulChanged;
			base.EventBus.OnBatteryDepleted -= OnBatteryDepleted;
			base.EventBus.OnEngineStarted -= OnEngineStarted;
			base.EventBus.OnEngineStopped -= OnEngineStopped;
			base.EventBus.OnBrakePressed -= OnBrakePressedEvent;
			base.EventBus.OnBrakeReleased -= OnBrakeReleasedEvent;
			if (_batteryModule != null)
			{
				_batteryModule.UnregisterConsumer(IsLowBeamActive);
				_batteryModule.UnregisterConsumer(IsHighBeamActive);
			}
			StopSignalLoop();
			_animator.StopAll();
		}

		public void SetDirectionalSignal(SignalDirection direction)
		{
			if (_directionalSignal != direction)
			{
				_directionalSignal = direction;
				RecomputeSignalMode();
			}
		}

		public void RefreshLightVisuals(bool instant)
		{
			ApplyHeadlightVisuals(instant);
			ApplyBrakelightTargetState(instant);
			ApplyAdditionalLights(instant);
			ApplySignalState(_signalBlinkPhase);
		}

		public void ForceTestBrake(bool on)
		{
			IsBraking = on;
			ApplyBrakelightTargetState();
		}

		public void ForceTestIgnition(bool on)
		{
			IsIgnited = on;
			ApplyBrakelightTargetState();
			ApplyAdditionalLights();
		}

		public void ForceTestBattery(bool useful)
		{
			IsBatteryUseful = useful;
			ApplyHeadlightVisuals();
			ApplyBrakelightTargetState();
			ApplyAdditionalLights();
		}

		public void ForceTestHeadlight(HeadLightState state)
		{
			CurrentHeadlightState = state;
			ApplyHeadlightVisuals();
			ApplyBrakelightTargetState();
		}

		public void ForceTestHazard(bool on)
		{
			_isHazardActive = on;
			RecomputeSignalMode();
		}

		private void HandleHeadlightOff()
		{
			CurrentHeadlightState = HeadLightState.Off;
			ApplyHeadlightVisuals();
			ApplyBrakelightTargetState();
			base.EventBus.FireHeadlightStateChanged(HeadLightState.Off);
		}

		private async void HandleHeadlightLow()
		{
			CurrentHeadlightState = HeadLightState.Low;
			ApplyHeadlightVisuals();
			ApplyBrakelightTargetState();
			base.EventBus.FireHeadlightStateChanged(HeadLightState.Low);
			ObjectivesEventBus.Raise(ObjectiveSignal.HeadlightsTurnedOn, this);
			if (_batteryModule != null)
			{
				await _batteryModule.ConsumeBatteryCondition();
			}
		}

		private async void HandleHeadlightHigh()
		{
			CurrentHeadlightState = HeadLightState.High;
			ApplyHeadlightVisuals();
			ApplyBrakelightTargetState();
			base.EventBus.FireHeadlightStateChanged(HeadLightState.High);
			ObjectivesEventBus.Raise(ObjectiveSignal.HeadlightsTurnedOn, this);
			if (_batteryModule != null)
			{
				await _batteryModule.ConsumeBatteryCondition();
			}
		}

		private void ApplyHeadlightVisuals(bool instant = false)
		{
			ApplyHeadlightToSide(leftHeadlightSlot?.InstalledHeadlight, instant);
			ApplyHeadlightToSide(rightHeadlightSlot?.InstalledHeadlight, instant);
		}

		private void ApplyHeadlightToSide(Headlight headlight, bool instant)
		{
			if (!(headlight == null))
			{
				bool flag = !IsBatteryUseful || CurrentHeadlightState == HeadLightState.Off;
				float targetIntensity;
				float value;
				float targetEmissionIntensity;
				if (flag)
				{
					targetIntensity = 0f;
					value = 0f;
					targetEmissionIntensity = 0f;
				}
				else if (CurrentHeadlightState == HeadLightState.Low)
				{
					targetIntensity = headlight.LowBeamIntensity;
					value = headlight.LowBeamRange;
					targetEmissionIntensity = headlight.LowBeamEmissionIntensity;
				}
				else
				{
					targetIntensity = headlight.HighBeamIntensity;
					value = headlight.HighBeamRange;
					targetEmissionIntensity = headlight.HighBeamEmissionIntensity;
				}
				float duration = (instant ? 0f : (flag ? headlight.OffFadeDuration : headlight.OnFadeDuration));
				_animator.Apply(new LightAnimationRequest
				{
					light = headlight.Light,
					targetIntensity = targetIntensity,
					targetRange = value,
					emissionRenderer = headlight.EmissionRenderer,
					emissionMaterialIndex = headlight.EmissionMaterialIndex,
					targetEmissionIntensity = targetEmissionIntensity,
					additionalLights = headlight.AdditionalLights,
					duration = duration,
					ease = Ease.OutQuad,
					customEase = headlight.TransitionEase
				});
			}
		}

		private void OnHeadlightInstalled(Headlight headlight)
		{
			ApplyHeadlightToSide(headlight, instant: true);
		}

		private void OnHeadlightRemoved()
		{
			ApplyHeadlightVisuals();
		}

		private void OnBrakelightInstalled(Brakelight brakelight)
		{
			if (!(brakelight == null))
			{
				ApplyBrakelightIntensityFor(brakelight, instant: true);
			}
		}

		private void OnBrakelightRemoved(Brakelight brakelight)
		{
			if (!(brakelight == null))
			{
				_animator.Clear(brakelight.Light);
				_animator.ClearEmission(brakelight.EmissionRenderer, brakelight.EmissionMaterialIndex);
			}
		}

		private void ApplyBrakelightTargetState(bool instant = false)
		{
			ApplyBrakelightIntensityFor(leftBrakelightSlot?.InstalledBrakelight, instant);
			ApplyBrakelightIntensityFor(rightBrakelightSlot?.InstalledBrakelight, instant);
		}

		private void ApplyBrakelightIntensityFor(Brakelight brakelight, bool instant)
		{
			if (!(brakelight == null) && !(brakelight.Light == null))
			{
				float targetIntensity;
				float targetEmissionIntensity;
				if (!IsBatteryUseful)
				{
					targetIntensity = 0f;
					targetEmissionIntensity = 0f;
				}
				else if (IsBraking)
				{
					targetIntensity = brakelight.BrakeIntensity;
					targetEmissionIntensity = brakelight.BrakeEmissionIntensity;
				}
				else
				{
					targetIntensity = brakelight.IdleIntensity;
					targetEmissionIntensity = brakelight.IdleEmissionIntensity;
				}
				float duration = (instant ? 0f : brakelight.FadeDuration);
				_animator.Apply(new LightAnimationRequest
				{
					light = brakelight.Light,
					targetIntensity = targetIntensity,
					emissionRenderer = brakelight.EmissionRenderer,
					emissionMaterialIndex = brakelight.EmissionMaterialIndex,
					targetEmissionIntensity = targetEmissionIntensity,
					duration = duration,
					ease = Ease.OutQuad,
					customEase = brakelight.TransitionEase
				});
			}
		}

		private void OnBrakePressedEvent()
		{
			if (!IsBraking)
			{
				IsBraking = true;
				ApplyBrakelightTargetState();
			}
		}

		private void OnBrakeReleasedEvent()
		{
			if (IsBraking)
			{
				IsBraking = false;
				ApplyBrakelightTargetState();
			}
		}

		private void HandleWarningOn()
		{
			if (!_isHazardActive)
			{
				_isHazardActive = true;
				RecomputeSignalMode();
			}
		}

		private void HandleWarningOff()
		{
			if (_isHazardActive)
			{
				_isHazardActive = false;
				RecomputeSignalMode();
			}
		}

		private void RecomputeSignalMode()
		{
			SignalMode signalMode = ((!_isHazardActive) ? (_directionalSignal switch
			{
				SignalDirection.Left => SignalMode.Left, 
				SignalDirection.Right => SignalMode.Right, 
				_ => SignalMode.Off, 
			}) : SignalMode.Hazard);
			SetSignalMode(signalMode);
		}

		private void SetSignalMode(SignalMode mode)
		{
			if (CurrentSignalMode != mode)
			{
				CurrentSignalMode = mode;
				StopSignalLoop();
				if (mode == SignalMode.Off)
				{
					_signalBlinkPhase = false;
					ApplySignalState(on: false);
				}
				else
				{
					_signalCts = new CancellationTokenSource();
					SignalBlinkLoopAsync(_signalCts.Token).Forget();
				}
			}
		}

		private void StopSignalLoop()
		{
			if (_signalCts != null)
			{
				_signalCts.Cancel();
				_signalCts.Dispose();
				_signalCts = null;
			}
		}

		private async UniTaskVoid SignalBlinkLoopAsync(CancellationToken ct)
		{
			_signalBlinkPhase = false;
			while (!ct.IsCancellationRequested)
			{
				_signalBlinkPhase = !_signalBlinkPhase;
				ApplySignalState(_signalBlinkPhase);
				try
				{
					await UniTask.Delay(TimeSpan.FromSeconds(signalBlinkInterval), ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
				}
				catch (OperationCanceledException)
				{
					break;
				}
			}
		}

		private void ApplySignalState(bool on)
		{
			int num = 0;
			foreach (VehicleSignalLightSlot signalLightSlot in signalLightSlots)
			{
				if (!(signalLightSlot == null))
				{
					VehicleSignalLight installedSignalLight = signalLightSlot.InstalledSignalLight;
					if (!(installedSignalLight == null))
					{
						bool flag = on && MatchesSide(installedSignalLight);
						ApplySignalLightVisual(installedSignalLight, flag);
						num++;
					}
				}
			}
		}

		private bool MatchesSide(VehicleSignalLight signalLight)
		{
			return CurrentSignalMode switch
			{
				SignalMode.Hazard => true, 
				SignalMode.Left => signalLight.SignalLightSide == SignalLightSide.Left, 
				SignalMode.Right => signalLight.SignalLightSide == SignalLightSide.Right, 
				_ => false, 
			};
		}

		private void ApplySignalLightVisual(VehicleSignalLight signalLight, bool on, bool instant = false)
		{
			float targetIntensity = (on ? signalLight.OnIntensity : 0f);
			float targetEmissionIntensity = (on ? signalLight.OnEmissionIntensity : 0f);
			float duration = (instant ? 0f : (on ? signalLight.BlinkAttackDuration : signalLight.BlinkReleaseDuration));
			_animator.Apply(new LightAnimationRequest
			{
				light = signalLight.Light,
				targetIntensity = targetIntensity,
				emissionRenderer = signalLight.EmissionRenderer,
				emissionMaterialIndex = signalLight.EmissionMaterialIndex,
				targetEmissionIntensity = targetEmissionIntensity,
				duration = duration,
				ease = Ease.Linear
			});
		}

		private void OnSignalLightInstalled(VehicleSignalLight signalLight)
		{
			if (!(signalLight == null))
			{
				bool flag = CurrentSignalMode != SignalMode.Off && _signalBlinkPhase && MatchesSide(signalLight);
				ApplySignalLightVisual(signalLight, flag, instant: true);
			}
		}

		private void OnSignalLightRemoved()
		{
		}

		private void ApplyAdditionalLights(bool instant = false)
		{
			foreach (AdditionalLightEntry additionalLight in additionalLights)
			{
				if (additionalLight != null)
				{
					int num = additionalLight.binding switch
					{
						AdditionalLightBinding.OnWhenIgnited => IsIgnited ? 1 : 0, 
						AdditionalLightBinding.OnWithBattery => IsBatteryUseful ? 1 : 0, 
						_ => 0, 
					};
					float targetIntensity = ((num != 0) ? additionalLight.intensity : 0f);
					float targetEmissionIntensity = ((num != 0) ? additionalLight.emissionIntensity : 0f);
					float duration = (instant ? 0f : additionalLight.fadeDuration);
					_animator.Apply(new LightAnimationRequest
					{
						light = additionalLight.light,
						targetIntensity = targetIntensity,
						emissionRenderer = additionalLight.emissionRenderer,
						emissionMaterialIndex = additionalLight.emissionMaterialIndex,
						targetEmissionIntensity = targetEmissionIntensity,
						duration = duration,
						ease = Ease.OutQuad
					});
				}
			}
		}

		private void OnBatteryUsefulChanged(bool isBatteryUseful)
		{
			IsBatteryUseful = isBatteryUseful;
			ApplyHeadlightVisuals();
			ApplyAdditionalLights();
		}

		private void OnBatteryDepleted()
		{
			IsBatteryUseful = false;
			ApplyHeadlightVisuals();
			ApplyAdditionalLights();
		}

		private void OnEngineStarted()
		{
			IsIgnited = true;
			ApplyBrakelightTargetState();
			ApplyAdditionalLights();
		}

		private void OnEngineStopped()
		{
			IsIgnited = false;
			ApplyBrakelightTargetState();
			ApplyAdditionalLights();
		}

		private void SyncVisualsToCurrentState()
		{
			if (_networkSync != null)
			{
				IsBraking = _networkSync.IsBraking;
			}
			if (headlightButton != null)
			{
				CurrentHeadlightState = headlightButton.HeadLightState;
			}
			if (warningLightButton != null)
			{
				_isHazardActive = warningLightButton.WarningLightState == WarningLightState.On;
			}
			if (_batteryModule != null)
			{
				IsBatteryUseful = _batteryModule.IsBatteryUseful;
			}
			ApplyHeadlightVisuals(instant: true);
			ApplyBrakelightTargetState(instant: true);
			ApplyAdditionalLights(instant: true);
			RecomputeSignalMode();
		}

		private bool IsLowBeamActive()
		{
			return CurrentHeadlightState == HeadLightState.Low;
		}

		private bool IsHighBeamActive()
		{
			return CurrentHeadlightState == HeadLightState.High;
		}

		private float GetLowBeamMultiplier()
		{
			return (_batteryModule?.InstalledBattery?.batteryConfig?.lowBeamUsageMultiplier).GetValueOrDefault();
		}

		private float GetHighBeamMultiplier()
		{
			return (_batteryModule?.InstalledBattery?.batteryConfig?.highBeamUsageMultiplier).GetValueOrDefault();
		}
	}
}
