using System;
using NWH.VehiclePhysics2.Powertrain;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.ArcadeModule
{
	[Serializable]
	public class ArcadeModule : VehicleComponent
	{
		[Tooltip("Torque that will be applied to the Rigidbody to try and reach the steering angle,\r\nirrelevant of the tire slip. Also works in air.")]
		public float artificialSteerStrength = 1f;

		[Tooltip("Strength of drift assist.")]
		public float driftAssistStrength = 1f;

		[Tooltip("angle that the vehicle will attempt to hold when drifting.\r\nForce is applied if the angle goes over this value. If the angle is below the drift angle, no force is applied.")]
		public float targetDriftAngle = 45f;

		[Tooltip("angle that will be added to targetDriftAngle based on the steering input.\r\nIf the vehicle is drifting and there is steering input, drift angle will increase.")]
		public float steerAngleContribution = 10f;

		[Tooltip("Maximum force that will be applied to the rear axle to keep the vehicle at or below the target drift angle.")]
		public float maxDriftAssistForce = 800f;

		private float _driftAngle;

		private float _prevDriftError;

		public float DriftAngle => _driftAngle;

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (!base.IsActive || !vehicleController.IsFullyGrounded() || vehicleController.SpeedSigned < 1f)
			{
				return;
			}
			if (artificialSteerStrength > 0f && vehicleController.Speed > 1f)
			{
				float num = vehicleController.input.Steering * artificialSteerStrength * vehicleController.vehicleRigidbody.mass * (0f - Physics.gravity.y) * 0.4f;
				num *= Mathf.Clamp01(vehicleController.Speed / 5f);
				vehicleController.vehicleRigidbody.AddTorque(new Vector3(0f, num, 0f));
			}
			if (driftAssistStrength > 0f && vehicleController.Speed > 1f && vehicleController.powertrain.wheelGroups.Count == 2)
			{
				Vector3 normalized = vehicleController.vehicleRigidbody.linearVelocity.normalized;
				Vector3 forward = vehicleController.transform.forward;
				_driftAngle = Vector3.SignedAngle(normalized, forward, vehicleController.transform.up);
				_driftAngle = Mathf.Sign(_driftAngle) * Mathf.Clamp(Mathf.Abs(Mathf.Clamp(_driftAngle, -90f, 90f)), 0f, 1f / 0f);
				WheelGroup wheelGroup = vehicleController.powertrain.wheelGroups[1];
				if (wheelGroup.Wheels.Count == 2)
				{
					WheelComponent leftWheel = wheelGroup.LeftWheel;
					WheelComponent rightWheel = wheelGroup.RightWheel;
					Vector3 position = (leftWheel.wheelUAPI.transform.position + rightWheel.wheelUAPI.transform.position) * 0.5f;
					float f = targetDriftAngle + vehicleController.input.Steering * steerAngleContribution;
					float num2 = Mathf.Abs(_driftAngle) - Mathf.Abs(f);
					float num3 = (num2 - _prevDriftError) / Time.fixedDeltaTime;
					Vector3 vector = vehicleController.transform.right * (Mathf.Clamp(num2 + num3, 0f, 90f) * Mathf.Sign(_driftAngle) * maxDriftAssistForce);
					vector *= Mathf.Clamp01(vehicleController.Speed / 3f);
					vehicleController.vehicleRigidbody.AddForceAtPosition(vector * driftAssistStrength, position);
					_prevDriftError = num2;
				}
			}
		}
	}
}
