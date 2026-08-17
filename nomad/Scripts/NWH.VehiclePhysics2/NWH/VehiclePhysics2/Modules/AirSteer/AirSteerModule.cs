using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.AirSteer
{
	[Serializable]
	public class AirSteerModule : VehicleComponent
	{
		[Tooltip("Torque applied around the Y axis to steer the vehicle while in the air (nose left/right).\r\nActivated with steering input.")]
		public float yawTorque = 10000f;

		[Tooltip("Torque applied around the X axis to steer the vehicle while in the air (nose up, down).\r\nActivated with throttle / brake input.\r\nTorque from the changes in the wheel angular velocity will get applied independently of this setting\r\nby the WheelController.")]
		public float pitchTorque = 10000f;

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (!vehicleController.IsGrounded())
			{
				Vector3 zero = Vector3.zero;
				zero.x = (vehicleController.input.Throttle - vehicleController.input.Brakes) * pitchTorque;
				zero.y = vehicleController.input.Steering * yawTorque;
				vehicleController.vehicleRigidbody.AddRelativeTorque(zero);
			}
		}
	}
}
