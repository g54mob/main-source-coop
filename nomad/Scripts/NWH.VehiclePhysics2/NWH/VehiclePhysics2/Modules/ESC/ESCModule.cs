using System;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.ESC
{
	[Serializable]
	public class ESCModule : VehicleComponent
	{
		[Range(0f, 1f)]
		[Tooltip("    Intensity of stability control.")]
		public float intensity = 0.4f;

		[Tooltip("ESC will not work below this speed.\r\nSetting this to a too low value might cause vehicle to be hard to steer at very low speeds.")]
		public float lowerSpeedThreshold = 4f;

		public override void VC_Update()
		{
			base.VC_Update();
			if (vehicleController.LocalForwardVelocity < lowerSpeedThreshold)
			{
				return;
			}
			float num = Vector3.SignedAngle(vehicleController.vehicleRigidbody.linearVelocity, vehicleController.vehicleTransform.forward, vehicleController.vehicleTransform.up);
			num -= vehicleController.steering.angle * 0.5f;
			float num2 = ((num < 0f) ? (0f - num) : num);
			if (vehicleController.powertrain.engine.revLimiterActive || num2 < 2f)
			{
				return;
			}
			foreach (WheelComponent wheel in vehicleController.powertrain.wheels)
			{
				if (wheel.wheelUAPI.IsGrounded)
				{
					float torque = (0f - num) * Mathf.Sign(wheel.wheelUAPI.transform.position.x) * 50f * intensity;
					wheel.AddBrakeTorque(torque);
				}
			}
		}
	}
}
