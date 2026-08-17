using System;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Powertrain;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using UnityEngine;

namespace NWH.VehiclePhysics2
{
	[Serializable]
	public class Steering : VehicleComponent
	{
		[Tooltip("Only used if limitSteeringRate is true.Will limit wheels so that they can only steer up to the set degreelimit per second. E.g. 60 degrees per second will mean that the wheels that have 30 degree steer angle willtake 1 second to steer from full left to full right.")]
		[ShowInSettings("deg/s Limit", 50f, 500f, 10f)]
		public float degreesPerSecondLimit = 180f;

		[Tooltip("    If true direct steering input will be used, without any modification.")]
		[ShowInSettings("Raw Input")]
		public bool useRawInput;

		public AnimationCurve linearity = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));

		[Range(0f, 90f)]
		[Tooltip("    Maximum steering angle at the wheels.")]
		[ShowInSettings("Max. Steer Angle", 5f, 50f, 5f)]
		public float maximumSteerAngle = 25f;

		[Tooltip("    Should wheels return to neutral position when there is no input?")]
		[ShowInSettings("Return to Center")]
		public bool returnToCenter = true;

		[Tooltip("Curve that shows how the steering angle behaves at certain speed.\r\nX axis represents velocity in range 0 to 100m/s (normalized to 0,1).\r\nY axis represents 0 to maximumSteerAngle (normalized to 0,1).")]
		public AnimationCurve speedSensitiveSteeringCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.3f, 0.6f, 0f, -0.6f), new Keyframe(1f, 0.1f, 0.5f, 0f));

		public AnimationCurve speedSensitiveSmoothingCurve = new AnimationCurve(new Keyframe(0f, 0.05f), new Keyframe(1f, 0.15f));

		[Tooltip("    Steering wheel transform that will be rotated when steering. Optional.")]
		public Transform steeringWheel;

		[Tooltip("Steer angle will be multiplied by this value to get steering wheel angle. Ignored if steering wheel is null.\r\nIf you want the steering wheel to rotate in opposite direction use negative value.")]
		public float steeringWheelTurnRatio = 5f;

		private Vector3 _initialSteeringWheelRotation;

		private float _steerVelocity;

		private float _targetAngle;

		[Tooltip("    Current steer angle.")]
		[ShowInTelemetry]
		public float angle;

		[Tooltip("    angle added to the user set angle, used mostly for motorcycle balancing.\r\n    To add angle to the current steer angle use this instead of angle, since this goes around smoothing and clamping.")]
		public float externallyAddedAngle;

		protected override void VC_Initialize()
		{
			if (steeringWheel != null)
			{
				_initialSteeringWheelRotation = steeringWheel.transform.localRotation.eulerAngles;
			}
			_targetAngle = 0f;
			_steerVelocity = 0f;
			vehicleController.wheelbase = CalculateWheelbase();
			base.VC_Initialize();
		}

		public void InitializeSteeringWheel(Transform steeringWheel)
		{
			if (steeringWheel == null)
			{
				this.steeringWheel = null;
				_initialSteeringWheelRotation = Vector3.zero;
			}
			else
			{
				this.steeringWheel = steeringWheel;
				_initialSteeringWheelRotation = steeringWheel.transform.localRotation.eulerAngles;
			}
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			CalculateSteerAngles();
			VisualUpdate();
		}

		public virtual void CalculateSteerAngles()
		{
			float steering = vehicleController.input.Steering;
			float smoothTime = speedSensitiveSmoothingCurve.Evaluate(vehicleController.Speed / 50f);
			if (!useRawInput && !returnToCenter && steering > -0.02f && steering < 0.02f)
			{
				return;
			}
			if (useRawInput)
			{
				angle = steering * maximumSteerAngle;
			}
			else
			{
				float time = ((steering < 0f) ? (0f - steering) : steering);
				float num = ((!(steering < 0f)) ? 1 : (-1));
				float target = speedSensitiveSteeringCurve.Evaluate(vehicleController.Speed / 50f) * maximumSteerAngle * linearity.Evaluate(time) * num;
				_targetAngle = Mathf.SmoothDamp(_targetAngle, target, ref _steerVelocity, smoothTime);
				angle = Mathf.MoveTowards(angle, _targetAngle, degreesPerSecondLimit * vehicleController.fixedDeltaTime);
			}
			foreach (WheelGroup wheelGroup in vehicleController.powertrain.wheelGroups)
			{
				float num2 = (angle + externallyAddedAngle) * wheelGroup.steerCoefficient;
				if (wheelGroup.Wheels.Count == 2 && vehicleController.wheelbase > 1E-05f && wheelGroup.addAckerman)
				{
					float f = num2 * ((float)Math.PI / 180f);
					float num3 = Mathf.Sin(f);
					float num4 = Mathf.Cos(f);
					float num5 = Mathf.Atan(4f * wheelGroup.trackWidth * num3 / (2f * vehicleController.wheelbase * num4 - wheelGroup.trackWidth * num3));
					float num6 = Mathf.Atan(4f * wheelGroup.trackWidth * num3 / (2f * vehicleController.wheelbase * num4 + wheelGroup.trackWidth * num3));
					if (num2 < 0f)
					{
						wheelGroup.RightWheel.wheelUAPI.SteerAngle = num5 * 57.29578f;
						wheelGroup.LeftWheel.wheelUAPI.SteerAngle = num6 * 57.29578f;
					}
					else
					{
						wheelGroup.LeftWheel.wheelUAPI.SteerAngle = num6 * 57.29578f;
						wheelGroup.RightWheel.wheelUAPI.SteerAngle = num5 * 57.29578f;
					}
					continue;
				}
				foreach (WheelComponent wheel in wheelGroup.Wheels)
				{
					wheel.wheelUAPI.SteerAngle = num2;
				}
			}
		}

		public float CalculateWheelbase()
		{
			float result = -1f;
			switch (vehicleController.powertrain.wheelCount)
			{
			case 4:
				result = Vector3.Distance(vehicleController.powertrain.wheels[0].wheelUAPI.transform.position, vehicleController.powertrain.wheels[2].wheelUAPI.transform.position);
				break;
			case 2:
				result = Vector3.Distance(vehicleController.powertrain.wheels[0].wheelUAPI.transform.position, vehicleController.powertrain.wheels[1].wheelUAPI.transform.position);
				break;
			}
			return result;
		}

		public virtual void VisualUpdate()
		{
			if (steeringWheel != null)
			{
				float num = angle * steeringWheelTurnRatio;
				steeringWheel.transform.localRotation = Quaternion.Euler(_initialSteeringWheelRotation);
				steeringWheel.transform.Rotate(Vector3.forward, num);
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			speedSensitiveSteeringCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.3f, 0.4f, -0.6f, -0.6f), new Keyframe(1f, 0.2f, -0.1f, 0.1f));
			linearity = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));
		}
	}
}
