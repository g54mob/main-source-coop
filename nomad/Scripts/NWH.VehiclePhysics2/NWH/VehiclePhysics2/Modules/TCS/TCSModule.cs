using System;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.VehiclePhysics2.Modules.TCS
{
	[Serializable]
	public class TCSModule : VehicleComponent
	{
		public bool active;

		[Tooltip("    Speed under which TCS will not work.")]
		public float lowerSpeedThreshold = 2f;

		[Range(0f, 1f)]
		[Tooltip("    Longitudinal slip threshold at which TCS will activate.")]
		public float slipThreshold = 0.1f;

		[Tooltip("    Called each frame while TCS is active.")]
		public UnityEvent onTCSActive = new UnityEvent();

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.powerModifiers.Add(TCSPowerLimiter);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				active = false;
				vehicleController.powertrain.engine.powerModifiers.Remove(TCSPowerLimiter);
				return true;
			}
			return false;
		}

		public float TCSPowerLimiter()
		{
			active = false;
			if (!base.IsActive)
			{
				return 1f;
			}
			foreach (WheelComponent wheel in vehicleController.powertrain.wheels)
			{
				if (wheel.wheelUAPI.IsGrounded && !vehicleController.powertrain.transmission.isShifting && (0f - wheel.wheelUAPI.LongitudinalSlip) * Mathf.Sign(vehicleController.LocalForwardVelocity) > slipThreshold)
				{
					active = true;
					onTCSActive.Invoke();
					return 0.01f;
				}
			}
			return 1f;
		}
	}
}
