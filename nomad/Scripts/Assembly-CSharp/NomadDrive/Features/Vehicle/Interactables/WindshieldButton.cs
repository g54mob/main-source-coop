using NomadDrive.Features.Vehicle.Enums;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class WindshieldButton : VehicleInteractable
	{
		[SerializeField]
		private ControlRotationAxis rotationAxis;

		[SerializeField]
		private float stepAngleDegrees = 25f;

		private Quaternion _defaultRotation;

		private Vector3 _axisVector;

		private float _currentAngle;

		public UnityEvent OnWiperButtonOff { get; } = new UnityEvent();

		public UnityEvent<float> OnWiperSlowButtonOn { get; } = new UnityEvent<float>();

		public UnityEvent<float> OnWiperFastButtonOn { get; } = new UnityEvent<float>();

		public WiperState WiperState { get; private set; }

		protected override bool UseStateMachine => false;

		protected override bool HasNetworkState => true;

		protected override void Awake()
		{
			_defaultRotation = base.ModelTransform.localRotation;
			base.Awake();
			_axisVector = GetAxisVector(rotationAxis);
			CreateBasicInteraction(ActivateSlow, "@interaction.on");
		}

		public override void ApplyStateFromNetwork(byte stateData, bool skipAnimation)
		{
			if ((WiperState)stateData == WiperState && !skipAnimation)
			{
				return;
			}
			WiperState = (WiperState)stateData;
			if (skipAnimation)
			{
				ApplyStateImmediate((WiperState)stateData);
				return;
			}
			switch (stateData)
			{
			case 0:
				DeactivateActions();
				break;
			case 1:
				ActivateSlowActions();
				break;
			case 2:
				ActivateFastActions();
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

		private void ApplyStateImmediate(WiperState state)
		{
			switch (state)
			{
			case WiperState.Off:
				SetAngle(0f);
				SetBasicInteraction(ActivateSlow, "@interaction.on");
				OnWiperButtonOff.Invoke();
				break;
			case WiperState.Slow:
				SetAngle(stepAngleDegrees);
				SetBasicInteraction(ActivateFast, "@interaction.fast");
				OnWiperSlowButtonOn.Invoke(1f);
				break;
			case WiperState.Fast:
				SetAngle(stepAngleDegrees * 2f);
				SetBasicInteraction(Deactivate, "@interaction.off");
				OnWiperFastButtonOn.Invoke(1.5f);
				break;
			}
		}

		private void ActivateSlow()
		{
			if (RequireUsableBatteryOrWarn())
			{
				RequestStateChange(1);
			}
		}

		private void ActivateFast()
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

		private void ActivateSlowActions()
		{
			Tween.CompleteAll(this);
			float endValue = stepAngleDegrees;
			Tween.Custom(this, _currentAngle, endValue, 0.25f, delegate(WindshieldButton _, float angle)
			{
				SetAngle(angle);
			}, Ease.Linear).OnComplete(delegate
			{
				SetBasicInteraction(ActivateFast, "@interaction.fast");
				OnWiperSlowButtonOn.Invoke(1f);
			});
		}

		private void ActivateFastActions()
		{
			Tween.CompleteAll(this);
			float endValue = stepAngleDegrees * 2f;
			Tween.Custom(this, _currentAngle, endValue, 0.25f, delegate(WindshieldButton _, float angle)
			{
				SetAngle(angle);
			}, Ease.Linear).OnComplete(delegate
			{
				SetBasicInteraction(Deactivate, "@interaction.off");
				OnWiperFastButtonOn.Invoke(1.5f);
			});
		}

		private void DeactivateActions()
		{
			Tween.CompleteAll(this);
			Tween.Custom(this, _currentAngle, 0f, 0.25f, delegate(WindshieldButton _, float angle)
			{
				SetAngle(angle);
			}, Ease.OutElastic).OnComplete(delegate
			{
				SetBasicInteraction(ActivateSlow, "@interaction.on");
				OnWiperButtonOff.Invoke();
			});
		}
	}
}
