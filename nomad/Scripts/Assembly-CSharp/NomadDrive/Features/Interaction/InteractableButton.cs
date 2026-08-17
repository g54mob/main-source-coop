using EvilCore.UI.Scripts;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction
{
	public class InteractableButton : Interactable
	{
		public UnityEvent onButtonPressed;

		public float buttonPressTime = 1f;

		public Vector3 pressingDirection = Vector3.down;

		private Vector3 _initialPosition;

		private InteractionStateMachine<ButtonState> _stateMachine;

		protected override bool UseStateMachine => true;

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<ButtonState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(ButtonState.Ready);
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(ButtonState.Ready, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.press", PressButton).WithCrosshair(CrosshairType.Interact));
		}

		private void PressButton()
		{
			_initialPosition = base.transform.localPosition;
			SetInteractionAvailability(newValue: false);
			Sequence.Create().Chain(Tween.LocalPosition(base.transform, -base.transform.up, buttonPressTime)).ChainCallback(this, delegate(InteractableButton target)
			{
				target.onButtonPressed.Invoke();
			})
				.Chain(Tween.LocalPosition(base.transform, _initialPosition, buttonPressTime))
				.ChainCallback(this, delegate(InteractableButton target)
				{
					target.SetInteractionAvailability(newValue: true);
				});
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
