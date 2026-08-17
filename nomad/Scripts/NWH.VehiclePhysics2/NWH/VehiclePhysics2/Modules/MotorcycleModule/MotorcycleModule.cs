using System;
using NWH.Common.Utility;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.MotorcycleModule
{
	[Serializable]
	public class MotorcycleModule : VehicleComponent
	{
		[Tooltip("Maximum angle delta in [deg] per [s] for given speed [m/s].")]
		public AnimationCurve leanAngleMaxDelta = new AnimationCurve(new Keyframe(0f, 220f), new Keyframe(90f, 100f));

		[Tooltip("Maximum lean angle [deg] for given speed [m/s].")]
		public AnimationCurve maxLeanAngle = new AnimationCurve(new Keyframe(0f, 33f), new Keyframe(90f, 33f));

		[Tooltip("Lean angle addition given the lateral tire slip.\r\nAllows the motorcycle to lean and slide sideways when drifting,\r\ninstead of highsiding.")]
		public float leanAngleSlipCoefficient = -30f;

		[Tooltip("Maximum torque the lean controller can apply to the Rigidbody.\r\nToo small value will result in lack of lean control on the vehicle\r\nin extreme cases, but can be more realistic as the motorcycle will be able to\r\nfall over, highside, etc.")]
		public float maxLeanTorque = 7000f;

		[Tooltip("Lean PID controller proportional gain.")]
		public float gainProportional = 4f;

		[Tooltip("Lean PID controller integral gain.")]
		public float gainIntegral = 5f;

		[Tooltip("Lean PID controller derivative gain.")]
		public float gainDerivative = 1f;

		[Tooltip("Lean PID controller proportional gain.")]
		public float leanPIDCoefficient = 50f;

		[Tooltip("Transform representing the upper forks and handlebars.")]
		public Transform handlebarsTransform;

		[Tooltip("Transform representing the rear swingarm.")]
		public Transform swingarmTransform;

		public bool useHitNormalAsUp;

		private float _leanTorque;

		private float _turningRadius;

		private float _leanAngleCurrent;

		private float _leanAngleTarget;

		private float _leanAngleTargetSmoothed;

		private float _leanAngleSlipContribution;

		private Transform _transform;

		private Rigidbody _rb;

		private PIDController _leanPIDController;

		private float _gravity;

		private float _speed;

		private float _absSpeed;

		private WheelComponent _frontWheel;

		private WheelComponent _rearWheel;

		private Quaternion _handlebarInitRotation;

		private Vector3 _transformForward;

		private Vector3 _transformUp;

		private Vector3 _up;

		public bool FrontWheelGrounded => _frontWheel.wheelUAPI.IsGrounded;

		public bool RearWheelGrounded => _rearWheel.wheelUAPI.IsGrounded;

		public bool IsGrounded
		{
			get
			{
				if (FrontWheelGrounded)
				{
					return RearWheelGrounded;
				}
				return false;
			}
		}

		public bool IsWheelie
		{
			get
			{
				if (!FrontWheelGrounded)
				{
					return RearWheelGrounded;
				}
				return false;
			}
		}

		public bool IsStoppie
		{
			get
			{
				if (!RearWheelGrounded)
				{
					return FrontWheelGrounded;
				}
				return false;
			}
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.vehicleRigidbody.angularDamping = 0f;
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.vehicleRigidbody.angularDamping = 100f;
				Vector3 localEulerAngles = vehicleController.transform.localEulerAngles;
				localEulerAngles.x = 0f;
				vehicleController.transform.localEulerAngles = localEulerAngles;
				return true;
			}
			return false;
		}

		protected override void VC_Initialize()
		{
			_rb = vehicleController.vehicleRigidbody;
			_transform = vehicleController.vehicleTransform;
			_leanPIDController = new PIDController(gainProportional, gainIntegral, gainDerivative, 0f - maxLeanTorque, maxLeanTorque);
			_frontWheel = vehicleController.powertrain.wheels[0];
			_rearWheel = vehicleController.powertrain.wheels[1];
			if (handlebarsTransform != null)
			{
				_handlebarInitRotation = handlebarsTransform.localRotation;
			}
			base.VC_Initialize();
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (useHitNormalAsUp)
			{
				_up = ((_frontWheel.wheelUAPI.IsGrounded ? _frontWheel.wheelUAPI.HitNormal : Vector3.up) + (_rearWheel.wheelUAPI.IsGrounded ? _rearWheel.wheelUAPI.HitNormal : Vector3.up)).normalized;
			}
			else
			{
				_up = Vector3.up;
			}
			_leanPIDController.GainProportional = gainProportional;
			_leanPIDController.GainIntegral = gainIntegral;
			_leanPIDController.GainDerivative = gainDerivative;
			_leanPIDController.minValue = 0f - maxLeanTorque;
			_leanPIDController.maxValue = maxLeanTorque;
			_transformForward = _transform.forward;
			_transformUp = _transform.up;
			if (swingarmTransform != null)
			{
				swingarmTransform.LookAt(_rearWheel.wheelUAPI.WheelPosition, _transformUp);
			}
			if (handlebarsTransform != null)
			{
				handlebarsTransform.localRotation = _handlebarInitRotation * Quaternion.AngleAxis(_frontWheel.wheelUAPI.SteerAngle, Vector3.up);
			}
			_gravity = 0f - Physics.gravity.y;
			_speed = vehicleController.Speed;
			_absSpeed = Mathf.Abs(_speed);
			if (Vector3.Dot(_transformUp, _up) > 0.2f)
			{
				Vector3 normalized = Vector3.ProjectOnPlane(_up, _transformForward).normalized;
				Vector3 normalized2 = Vector3.ProjectOnPlane(_transformUp, _transformForward).normalized;
				_leanAngleCurrent = Vector3.SignedAngle(normalized2, normalized, _transformForward);
				_leanAngleTarget = vehicleController.input.Steering * maxLeanAngle.Evaluate(_absSpeed);
				_leanAngleSlipContribution = Mathf.Clamp(_rearWheel.wheelUAPI.LateralSlip, -1f, 1f);
				_leanAngleSlipContribution *= Mathf.Clamp01(vehicleController.Speed * 0.5f);
				_leanAngleTarget += _leanAngleSlipContribution * leanAngleSlipCoefficient;
				float num = leanAngleMaxDelta.Evaluate(_absSpeed);
				_leanAngleTargetSmoothed = Mathf.MoveTowardsAngle(_leanAngleTargetSmoothed, _leanAngleTarget, num * Time.fixedDeltaTime);
				_leanPIDController.GainProportional = gainProportional * leanPIDCoefficient;
				_leanPIDController.GainIntegral = gainIntegral * leanPIDCoefficient;
				_leanPIDController.GainDerivative = gainDerivative * leanPIDCoefficient;
				_leanPIDController.maxValue = maxLeanTorque;
				_leanPIDController.ProcessVariable = _leanAngleCurrent;
				_leanPIDController.SetPoint = _leanAngleTargetSmoothed;
				_leanTorque = 0f - _leanPIDController.ControlVariable(Time.fixedDeltaTime);
				_rb.AddTorque(_transformForward * _leanTorque);
			}
			else
			{
				_leanAngleCurrent = _leanAngleTarget;
			}
		}

		private float GetNeutralSteerAngle(float wheelbase, float leanAngle, float casterAngle, float speed)
		{
			if (leanAngle > -1E-05f && leanAngle < 1E-05f)
			{
				return 0f;
			}
			float f = leanAngle * ((float)Math.PI / 180f);
			float num = Mathf.Cos(f);
			float num2 = Mathf.Tan(f);
			float num3 = Mathf.Cos(casterAngle * ((float)Math.PI / 180f));
			return wheelbase * num * num2 * _gravity / num3 * speed * speed;
		}

		private float GetSteerAngleForLeanAngle(float leanAngle, float speed, float wheelbase)
		{
			if (leanAngle < 0.01f && leanAngle > -0.01f)
			{
				return 0f;
			}
			float num = speed / Mathf.Tan(leanAngle * ((float)Math.PI / 180f)) * _gravity;
			_ = wheelbase / Mathf.Sin(leanAngle * ((float)Math.PI / 180f));
			GetSteerAngleForTurningRadius(num, wheelbase);
			return num;
		}

		private float GetIdealLeanAngle(float speed, float steerAngle)
		{
			_turningRadius = GetTurningRadius(steerAngle, vehicleController.wheelbase);
			return Mathf.Abs(Mathf.Atan2(speed * speed, Mathf.Abs(_turningRadius) * _gravity) * 57.29578f) * (0f - Mathf.Sign(_turningRadius));
		}

		private float GetTurningRadius(float steerAngle, float wheelbase)
		{
			float num = Mathf.Tan(steerAngle * ((float)Math.PI / 180f));
			if (num < 1E-05f && num > -1E-05f)
			{
				return Mathf.Sign(steerAngle) * 1000000f;
			}
			return wheelbase / num * 2f;
		}

		private float GetSteerAngleForTurningRadius(float turningRadius, float wheelbase)
		{
			if (turningRadius < 1E-05f && turningRadius > -1E-05f)
			{
				return 0f;
			}
			return Mathf.Atan(2f * wheelbase / turningRadius) * 57.29578f;
		}
	}
}
