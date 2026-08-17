using EvilCore;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class DoorWindowButton : VehicleInteractable, IInitialize
	{
		public UnityEvent onButtonPressed;

		[SerializeField]
		private float pressedZ = 0.42f;

		[SerializeField]
		private float unpressedZ = 0.425f;

		private Vector3 _originalPosition;

		private StaticInteractionStateMachine<DoorWindowButtonState> _stateMachine;

		public bool IsInitialized { get; set; }

		protected override bool UseStateMachine => true;

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<DoorWindowButtonState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DoorWindowButtonState.Ready);
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(DoorWindowButtonState.Ready, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.press", PressButton).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false));
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DoorWindowButtonState.Ready);
		}

		protected override void Awake()
		{
			base.Awake();
			_originalPosition = base.transform.localPosition;
			Init();
		}

		public void Init()
		{
			IsInitialized = true;
		}

		public override void OnHovered()
		{
			base.OnHovered();
			ExecuteUnpressAnimation();
		}

		private void PressButton()
		{
			ExecutePressAnimation();
			onButtonPressed.Invoke();
			ExecuteUnpressAnimation();
			UpdateState();
		}

		private void ExecutePressAnimation()
		{
			Tween.LocalPosition(base.transform, new Vector3(_originalPosition.x, _originalPosition.y, pressedZ), 0.15f, Ease.OutCubic);
		}

		private void ExecuteUnpressAnimation()
		{
			Tween.LocalPosition(base.transform, new Vector3(_originalPosition.x, _originalPosition.y, unpressedZ), 0.15f, Ease.OutCubic);
		}
	}
}
