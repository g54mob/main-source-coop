using EvilCore;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Vehicle.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class DriverCabinLight : VehicleInteractable, IInitialize
	{
		private StaticInteractionStateMachine<DriverCabinLightState> _stateMachine;

		private Light _light;

		public UnityEvent OnLightButtonOn { get; } = new UnityEvent();

		public UnityEvent OnLightButtonOff { get; } = new UnityEvent();

		[field: SerializeField]
		internal DriverCabinLightState driverCabinLightState { get; set; }

		public bool IsInitialized { get; set; }

		protected override bool UseStateMachine => true;

		protected override bool HasNetworkState => true;

		protected override void Awake()
		{
			base.Awake();
			_light = GetComponentInChildren<Light>();
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<DriverCabinLightState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(DriverCabinLightState.Off, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.on", OnButtonPressed).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(DriverCabinLightState.On, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.off", OffButtonPressed).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true));
		}

		private DriverCabinLightState DetermineState()
		{
			return driverCabinLightState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public void Init()
		{
			driverCabinLightState = DriverCabinLightState.Off;
			if (_light != null)
			{
				_light.enabled = false;
			}
			IsInitialized = true;
			UpdateState();
		}

		public void RefreshLightState()
		{
			if (driverCabinLightState.Equals(DriverCabinLightState.On))
			{
				OnLightButtonOn.Invoke();
			}
			else
			{
				OnLightButtonOff.Invoke();
			}
		}

		public override void ApplyStateFromNetwork(byte stateData, bool skipAnimation)
		{
			if ((DriverCabinLightState)stateData != driverCabinLightState)
			{
				driverCabinLightState = (DriverCabinLightState)stateData;
				if (_light != null)
				{
					_light.enabled = stateData == 1;
				}
				UpdateState();
				if (stateData == 1)
				{
					OnLightButtonOn.Invoke();
				}
				else
				{
					OnLightButtonOff.Invoke();
				}
			}
		}

		private void OnButtonPressed()
		{
			if (RequireUsableBatteryOrWarn())
			{
				RequestStateChange(1);
			}
		}

		private void OffButtonPressed()
		{
			RequestStateChange(0);
		}
	}
}
