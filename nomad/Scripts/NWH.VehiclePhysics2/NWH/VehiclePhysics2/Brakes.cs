using System;
using System.Collections.Generic;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2
{
	[Serializable]
	public class Brakes : VehicleComponent
	{
		public delegate float BrakeTorqueModifier();

		public enum HandbrakeType
		{
			Standard = 0,
			Latching = 1
		}

		[Range(0f, 1f)]
		[Tooltip("    Strength of off-throttle braking in percentage [0 to 1] of max braking torque.")]
		public float brakeOffThrottleIntensity;

		[Tooltip("Collection of functions that modify the braking performance of the vehicle. Used for modules such as ABS where brakes need to be overriden or their effect reduced/increase. Return 1 for neutral modifier while returning 0 will disable the brakes completely. All brake torque modifiers will be multiplied in order to get the final brake torque coefficient.")]
		public List<BrakeTorqueModifier> brakeTorqueModifiers = new List<BrakeTorqueModifier>();

		[Tooltip("    Should brakes be applied when vehicle is disabled?")]
		[FormerlySerializedAs("brakeWhileAsleep")]
		[ShowInSettings("Brake While Disabled")]
		public bool brakeWhileDisabled = true;

		[Tooltip("    If true vehicle will break when in neutral and no throttle is applied.")]
		[ShowInSettings("Brake While Idle")]
		public bool brakeWhileIdle = true;

		[Tooltip("Should the vehicle apply brakes when the movement direction is opposite of input direction?")]
		public bool brakeOnReverseDirection;

		[Tooltip("Max brake torque that can be applied to each wheel. To adjust braking on per-axle basis change brake coefficients under Axle settings")]
		[ShowInSettings("Max. Torque")]
		public float maxTorque = 7000f;

		[ShowInSettings("Handbrake Type")]
		public HandbrakeType handbrakeType;

		[Tooltip("    Current value of the handbrake. 0 = inactive, 1 = maximum strength.\r\n    Handbrake strength will also be affected by per wheel group handbrake settings.")]
		public float handbrakeValue;

		[Range(0f, 1f)]
		[Tooltip("    Higher smoothing will result in brakes being applied more gradually.")]
		public float actuationTime = 0.1f;

		[Tooltip("    Called each time brakes are activated.")]
		public UnityEvent onBrakesActivate = new UnityEvent();

		[Tooltip("    Called each time brakes are released.")]
		public UnityEvent onBrakesDeactivate = new UnityEvent();

		private bool _isBraking;

		private bool _wasBraking;

		private float _handbrakeInput;

		private bool _handbrakeActive;

		private bool _handbrakeWasReset;

		private float _brakeInput;

		private float _throttleInput;

		public bool IsBraking
		{
			get
			{
				return _isBraking;
			}
			set
			{
				_isBraking = value;
			}
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			_isBraking = false;
			for (int i = 0; i < vehicleController.powertrain.wheelCount; i++)
			{
				vehicleController.powertrain.wheels[i].wheelUAPI.BrakeTorque = 0f;
			}
			float num = SumBrakeTorqueModifiers();
			if (actuationTime < 1E-05f)
			{
				actuationTime = 1E-05f;
			}
			_brakeInput = Mathf.MoveTowards(_brakeInput, vehicleController.input.InputSwappedBrakes, 1f / actuationTime * vehicleController.fixedDeltaTime);
			_throttleInput = vehicleController.input.InputSwappedThrottle;
			if (num <= 0.02f && _brakeInput <= 0.02f)
			{
				return;
			}
			_handbrakeInput = vehicleController.input.Handbrake;
			if (handbrakeType == HandbrakeType.Standard)
			{
				handbrakeValue = _handbrakeInput;
				_handbrakeActive = _handbrakeInput > 0.02f;
			}
			else
			{
				if (_handbrakeInput < 0.02f)
				{
					_handbrakeWasReset = true;
				}
				if (_handbrakeInput > 0.02f && !_handbrakeActive && _handbrakeWasReset)
				{
					_handbrakeActive = true;
					_handbrakeWasReset = false;
				}
				if (_handbrakeInput > 0.02f && _handbrakeActive && _handbrakeWasReset)
				{
					_handbrakeActive = false;
					_handbrakeWasReset = false;
				}
				if (_handbrakeActive)
				{
					handbrakeValue = ((_handbrakeInput > handbrakeValue) ? _handbrakeInput : handbrakeValue);
				}
				else
				{
					handbrakeValue = 0f;
				}
			}
			if (handbrakeValue > 0.02f)
			{
				AddBrakeTorqueAllWheels(handbrakeValue * num * maxTorque, isHandbrake: true);
			}
			float num2 = 0f;
			int gear = vehicleController.powertrain.transmission.Gear;
			if (brakeOffThrottleIntensity != 0f && _throttleInput < 0.02f)
			{
				num2 += brakeOffThrottleIntensity * maxTorque;
				_isBraking = true;
			}
			if (brakeOnReverseDirection)
			{
				float num3 = ((gear >= 0) ? 1f : (-1f));
				if ((_throttleInput * num3 > 0.2f && vehicleController.SpeedSigned < -0.2f) || (_throttleInput * num3 < -0.2f && vehicleController.SpeedSigned > 0.2f))
				{
					num2 += maxTorque;
				}
			}
			if (brakeWhileIdle && _throttleInput < 0.02f && gear == 0 && vehicleController.Speed < 0.2f)
			{
				num2 += num * maxTorque;
				_isBraking = true;
			}
			if (_brakeInput > 0.02f)
			{
				num2 += _brakeInput * num * maxTorque;
				_isBraking = true;
			}
			AddBrakeTorqueAllWheels(num2);
			if (_isBraking && !_wasBraking)
			{
				onBrakesActivate.Invoke();
			}
			else if (!_isBraking && _wasBraking)
			{
				onBrakesDeactivate.Invoke();
			}
			_wasBraking = _isBraking;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				if (brakeWhileDisabled)
				{
					float num = SumBrakeTorqueModifiers();
					_isBraking = true;
					AddBrakeTorqueAllWheels(num * maxTorque);
				}
				return true;
			}
			return false;
		}

		private void ResetBrakeTorque()
		{
			for (int i = 0; i < vehicleController.powertrain.wheelCount; i++)
			{
				vehicleController.powertrain.wheels[i].wheelUAPI.BrakeTorque = 0f;
			}
		}

		public void AddBrakeTorqueAllWheels(float brakeTorque, bool isHandbrake = false)
		{
			brakeTorque = ((brakeTorque < 0f) ? 0f : ((brakeTorque > maxTorque) ? maxTorque : brakeTorque));
			for (int i = 0; i < vehicleController.powertrain.wheelCount; i++)
			{
				vehicleController.powertrain.wheels[i].AddBrakeTorque(brakeTorque, isHandbrake);
			}
			if (brakeTorque > 1f && !isHandbrake)
			{
				_isBraking = true;
			}
		}

		private float SumBrakeTorqueModifiers()
		{
			if (brakeTorqueModifiers.Count == 0)
			{
				return 1f;
			}
			float num = 1f;
			int count = brakeTorqueModifiers.Count;
			for (int i = 0; i < count; i++)
			{
				num *= brakeTorqueModifiers[i]();
			}
			return Mathf.Clamp(num, 0f, 1f / 0f);
		}
	}
}
