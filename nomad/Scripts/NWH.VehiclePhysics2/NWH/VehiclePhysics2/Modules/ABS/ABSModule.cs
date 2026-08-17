using System;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.VehiclePhysics2.Modules.ABS
{
	[Serializable]
	public class ABSModule : VehicleComponent
	{
		[Tooltip("    Called each frame while ABS is a active.")]
		public UnityEvent absActivated = new UnityEvent();

		[Tooltip("    Is ABS currently active?")]
		public bool active;

		[Tooltip("    ABS will not work below this speed.")]
		public float lowerSpeedThreshold = 1f;

		[Range(0.001f, 1f)]
		[Tooltip("Range in which brake torque will be reduced. Larger value means less sensitive ABS.")]
		public float slipRange = 0.2f;

		[Range(0f, 1f)]
		[Tooltip("Longitudinal slip required for ABS to trigger.")]
		public float slipThreshold = 0.16f;

		public override bool VC_Enable(bool calledByParent)
		{
			if (!base.VC_Enable(calledByParent))
			{
				return false;
			}
			slipRange = ((slipRange < 1E-05f) ? 1E-05f : slipRange);
			vehicleController.brakes.brakeTorqueModifiers.Add(BrakeTorqueModifier);
			return true;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (!base.VC_Disable(calledByParent))
			{
				return false;
			}
			active = false;
			vehicleController.brakes.brakeTorqueModifiers.Remove(BrakeTorqueModifier);
			return true;
		}

		public float BrakeTorqueModifier()
		{
			active = false;
			if (!base.IsActive)
			{
				return 1f;
			}
			if (vehicleController.Speed < lowerSpeedThreshold)
			{
				return 1f;
			}
			if (!vehicleController.brakes.IsActive)
			{
				return 1f;
			}
			if (vehicleController.powertrain.engine.revLimiterActive)
			{
				return 1f;
			}
			if (vehicleController.input.Handbrake > 0.02f)
			{
				return 1f;
			}
			for (int i = 0; i < vehicleController.powertrain.wheelCount; i++)
			{
				WheelUAPI wheelUAPI = vehicleController.powertrain.wheels[i].wheelUAPI;
				if (wheelUAPI.IsGrounded && !(wheelUAPI.LongitudinalSlip < slipThreshold))
				{
					active = true;
					absActivated.Invoke();
					return Mathf.Clamp01(wheelUAPI.LongitudinalSlip - slipThreshold) / slipRange;
				}
			}
			return 1f;
		}
	}
}
