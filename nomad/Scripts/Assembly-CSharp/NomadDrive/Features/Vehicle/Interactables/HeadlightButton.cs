using NomadDrive.Features.Vehicle.Enums;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class HeadlightButton : VehicleInteractable
	{
		[SerializeField]
		private ControlRotationAxis rotationAxis;

		[SerializeField]
		private float stepAngleDegrees = 25f;

		private Quaternion _defaultRotation;

		private Vector3 _axisVector;

		private float _currentAngle;

		public UnityEvent OnHeadlightButtonOff { get; } = new UnityEvent();

		public UnityEvent OnLowBeamHeadlightButtonActivated { get; } = new UnityEvent();

		public UnityEvent OnHighBeamHeadlightButtonActivated { get; } = new UnityEvent();

		public HeadLightState HeadLightState { get; private set; }

		protected override bool UseStateMachine => false;

		protected override bool HasNetworkState => true;

		protected override void Awake()
		{
			_defaultRotation = base.ModelTransform.localRotation;
			base.Awake();
			_axisVector = GetAxisVector(rotationAxis);
			CreateBasicInteraction(ActivateLowBeam, "@interaction.on");
		}

		public override void ApplyStateFromNetwork(byte stateData, bool skipAnimation)
		{
			if ((HeadLightState)stateData == HeadLightState && !skipAnimation)
			{
				return;
			}
			HeadLightState = (HeadLightState)stateData;
			if (skipAnimation)
			{
				ApplyStateImmediate((HeadLightState)stateData);
				return;
			}
			switch (stateData)
			{
			case 0:
				DeactivateActions();
				break;
			case 1:
				ActivateLowBeamActions();
				break;
			case 2:
				ActivateHighBeamActions();
				break;
			}
		}

		private static Vector3 GetAxisVector(ControlRotationAxis axis)
		{
			return axis switch
			{
				ControlRotationAxis.X => Vector3.right, 
				ControlRotationAxis.Y => Vector3.up, 
				ControlRotationAxis.Z => Vector3.forward, 
				_ => Vector3.right, 
			};
		}

		private void SetAngle(float angle)
		{
			_currentAngle = angle;
			base.ModelTransform.localRotation = _defaultRotation * Quaternion.AngleAxis(angle, _axisVector);
		}

		private void ApplyStateImmediate(HeadLightState state)
		{
			switch (state)
			{
			case HeadLightState.Off:
				SetAngle(0f);
				SetBasicInteraction(ActivateLowBeam, "@interaction.on");
				OnHeadlightButtonOff.Invoke();
				break;
			case HeadLightState.Low:
				SetAngle(stepAngleDegrees);
				SetBasicInteraction(ActivateHighBeam, "@interaction.high");
				OnLowBeamHeadlightButtonActivated.Invoke();
				break;
			case HeadLightState.High:
				SetAngle(stepAngleDegrees * 2f);
				SetBasicInteraction(Deactivate, "@interaction.off");
				OnHighBeamHeadlightButtonActivated.Invoke();
				break;
			}
		}

		private void ActivateLowBeam()
		{
			if (RequireUsableBatteryOrWarn())
			{
				RequestStateChange(1);
			}
		}

		private void ActivateHighBeam()
		{
			if (RequireUsableBatteryOrWarn())
			{
				RequestStateChange(2);
			}
		}

		private void Deactivate()
		{
			RequestStateChange(0);
		}

		private void ActivateLowBeamActions()
		{
			Tween.CompleteAll(this);
			float endValue = stepAngleDegrees;
			Tween.Custom(this, _currentAngle, endValue, 0.25f, delegate(HeadlightButton _, float angle)
			{
				SetAngle(angle);
			}, Ease.Linear).OnComplete(delegate
			{
				SetBasicInteraction(ActivateHighBeam, "@interaction.high");
				OnLowBeamHeadlightButtonActivated.Invoke();
			});
		}

		private void ActivateHighBeamActions()
		{
			Tween.CompleteAll(this);
			float endValue = stepAngleDegrees * 2f;
			Tween.Custom(this, _currentAngle, endValue, 0.25f, delegate(HeadlightButton _, float angle)
			{
				SetAngle(angle);
			}, Ease.Linear).OnComplete(delegate
			{
				SetBasicInteraction(Deactivate, "@interaction.off");
				OnHighBeamHeadlightButtonActivated.Invoke();
			});
		}

		private void DeactivateActions()
		{
			Tween.CompleteAll(this);
			Tween.Custom(this, _currentAngle, 0f, 0.35f, delegate(HeadlightButton _, float angle)
			{
				SetAngle(angle);
			}, Ease.OutElastic).OnComplete(delegate
			{
				SetBasicInteraction(ActivateLowBeam, "@interaction.on");
				OnHeadlightButtonOff.Invoke();
			});
		}
	}
}
