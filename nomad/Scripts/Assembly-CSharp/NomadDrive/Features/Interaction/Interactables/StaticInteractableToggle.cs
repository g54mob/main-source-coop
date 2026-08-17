using EvilCore.UI.Scripts;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction.Interactables
{
	public class StaticInteractableToggle : StaticInteractable
	{
		[Header("Toggle Settings")]
		[SerializeField]
		private float toggleDuration = 0.2f;

		[SerializeField]
		private float toggleAngle = 40f;

		[Header("Events")]
		public UnityEvent onToggleOn = new UnityEvent();

		public UnityEvent onToggleOff = new UnityEvent();

		private StaticInteractionStateMachine<StaticToggleState> _stateMachine;

		private Vector3 _defaultRotation;

		private Vector3 _targetRotation;

		private StaticToggleState _currentState;

		public StaticToggleState ToggleState => _currentState;

		public bool IsOn => _currentState == StaticToggleState.On;

		protected override bool UseStateMachine => true;

		protected override void Start()
		{
			base.Start();
			_defaultRotation = base.transform.localEulerAngles;
			_targetRotation = _defaultRotation + new Vector3(0f, toggleAngle, 0f);
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<StaticToggleState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(StaticToggleState.Off, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.on", ToggleOn).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(StaticToggleState.On, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.off", ToggleOff).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false));
		}

		private StaticToggleState DetermineState()
		{
			return _currentState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void ApplyStateFromManager(byte stateData, bool skipAnimation)
		{
			StaticToggleState staticToggleState = (StaticToggleState)stateData;
			if (staticToggleState == _currentState)
			{
				return;
			}
			_currentState = staticToggleState;
			if (skipAnimation)
			{
				if (staticToggleState == StaticToggleState.On)
				{
					base.transform.localRotation = Quaternion.Euler(_targetRotation);
					onToggleOn.Invoke();
				}
				else
				{
					base.transform.localRotation = Quaternion.Euler(_defaultRotation);
					onToggleOff.Invoke();
				}
				UpdateState();
			}
			else
			{
				AnimateToState(staticToggleState);
			}
		}

		private void AnimateToState(StaticToggleState state)
		{
			SetInteractionAvailability(newValue: false);
			if (state == StaticToggleState.On)
			{
				Tween.LocalRotation(base.transform, Quaternion.Euler(_targetRotation), toggleDuration, Ease.OutQuart).OnComplete(this, delegate(StaticInteractableToggle target)
				{
					target.onToggleOn.Invoke();
					target.SetInteractionAvailability(newValue: true);
					target.UpdateState();
				});
			}
			else
			{
				Tween.LocalRotation(base.transform, Quaternion.Euler(_defaultRotation), toggleDuration, Ease.OutQuart).OnComplete(this, delegate(StaticInteractableToggle target)
				{
					target.onToggleOff.Invoke();
					target.SetInteractionAvailability(newValue: true);
					target.UpdateState();
				});
			}
		}

		public void ToggleOn()
		{
			if (!IsOn)
			{
				RequestStateChange(1);
			}
		}

		public void ToggleOff()
		{
			if (IsOn)
			{
				RequestStateChange(0);
			}
		}

		public void Toggle()
		{
			if (IsOn)
			{
				ToggleOff();
			}
			else
			{
				ToggleOn();
			}
		}
	}
}
