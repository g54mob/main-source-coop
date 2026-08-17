using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.CruiseControl
{
	[Serializable]
	public class CruiseControlModule : VehicleComponent
	{
		public bool cruiseControlActive;

		[Tooltip("    Derivative gain of PID controller.")]
		public float Kd = 0.1f;

		[Tooltip("    Integral gain of PID controller.")]
		public float Ki = 0.25f;

		[Tooltip("    Proportional gain of PID controller.")]
		public float Kp = 0.5f;

		[Tooltip("    Should the speed be set automatically when the module is enabled?")]
		public bool setTargetSpeedOnEnable;

		[Tooltip("    If true cruise control will be disabled if brakes are activated.")]
		public bool deactivateOnBrake;

		[Tooltip(" If true brakes will be applied when speeding.")]
		public bool applyBrakesWhenSpeeding = true;

		[Tooltip("    The speed the vehicle will try to hold.")]
		public float targetSpeed;

		private float _e;

		private float _ed;

		private float _ei;

		private float _eprev;

		private float _output;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.input.inputModifyCallback.AddListener(SetOutput);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.input.inputModifyCallback.RemoveListener(SetOutput);
				return true;
			}
			return false;
		}

		private void SetOutput()
		{
			if (cruiseControlActive)
			{
				vehicleController.input.Vertical = _output;
			}
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			float speedSigned = vehicleController.SpeedSigned;
			float fixedDeltaTime = vehicleController.fixedDeltaTime;
			if (vehicleController.input.states.cruiseControl)
			{
				vehicleController.input.states.cruiseControl = false;
				cruiseControlActive = !cruiseControlActive;
				if (cruiseControlActive && speedSigned > 0f)
				{
					if (Math.Abs(speedSigned - targetSpeed) > 0.05f)
					{
						_ei = 0f;
					}
					targetSpeed = speedSigned;
				}
				else
				{
					ResetCruiseControl();
				}
			}
			if (!cruiseControlActive)
			{
				return;
			}
			if ((deactivateOnBrake && vehicleController.input.states.inputSwappedBrakesRaw > 0.02f) || speedSigned <= 0f)
			{
				ResetCruiseControl();
				return;
			}
			_eprev = _e;
			_e = targetSpeed - speedSigned;
			if (_e > -0.5f && _e < 0.5f)
			{
				_ei = 0f;
			}
			_ei += _e * fixedDeltaTime;
			_ed = (_e - _eprev) / fixedDeltaTime;
			float num = _e * Kp + _ei * Ki + _ed * Kd;
			num = ((num < -1f) ? (-1f) : ((num > 1f) ? 1f : num));
			_output = Mathf.Lerp(_output, num, fixedDeltaTime * 5f);
			if (!applyBrakesWhenSpeeding)
			{
				_output = ((_output < 0f) ? 0f : _output);
			}
		}

		private void ResetCruiseControl()
		{
			cruiseControlActive = false;
			targetSpeed = 0f;
			_e = 0f;
			_ei = 0f;
			_ed = 0f;
			_eprev = 0f;
			_output = 0f;
		}
	}
}
