using EvilCore;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Vehicle.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class WarningLightButton : VehicleInteractable, IInitialize
	{
		private StaticInteractionStateMachine<WarningLightState> _stateMachine;

		public UnityEvent OnWarningLightButtonOn { get; } = new UnityEvent();

		public UnityEvent OnWarningLightButtonOff { get; } = new UnityEvent();

		[field: SerializeField]
		public WarningLightState WarningLightState { get; private set; } = WarningLightState.Off;

		public bool IsInitialized { get; set; }

		protected override bool UseStateMachine => true;

		protected override bool HasNetworkState => true;

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<WarningLightState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(WarningLightState.Off, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.on", OnButtonPressed).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(WarningLightState.On, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.off", OffButtonPressed).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true));
		}

		private WarningLightState DetermineState()
		{
			return WarningLightState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public void Init()
		{
			WarningLightState = WarningLightState.Off;
			IsInitialized = true;
			UpdateState();
		}

		public override void ApplyStateFromNetwork(byte stateData, bool skipAnimation)
		{
			if ((WarningLightState)stateData != WarningLightState)
			{
				WarningLightState = (WarningLightState)stateData;
				UpdateState();
				if (stateData == 0)
				{
					OnWarningLightButtonOn.Invoke();
				}
				else
				{
					OnWarningLightButtonOff.Invoke();
				}
			}
		}

		private void OnButtonPressed()
		{
			RequestStateChange(0);
		}

		private void OffButtonPressed()
		{
			RequestStateChange(1);
		}
	}
}
