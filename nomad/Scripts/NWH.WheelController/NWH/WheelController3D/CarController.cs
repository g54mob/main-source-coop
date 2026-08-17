using System.Collections.Generic;
using NWH.Common.Vehicles;
using UnityEngine;

namespace NWH.WheelController3D
{
	[RequireComponent(typeof(Rigidbody))]
	public class CarController : Vehicle
	{
		public float maxBrakeTorque = 3000f;

		public float maxMotorTorque = 1000f;

		public float maxSteeringAngle = 35f;

		public float minSteeringAngle = 20f;

		public List<_Wheel> wheels;

		protected float smoothXAxis;

		protected float xAxis;

		protected float xAxisVelocity;

		protected float yAxis;

		private Rigidbody _rigidbody;

		public override void Awake()
		{
			base.Awake();
			_rigidbody = GetComponent<Rigidbody>();
			if (_rigidbody == null)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			for (int i = 0; i < wheels.Count; i++)
			{
				WheelUAPI wheelUAPI = wheels[i].wheelUAPI;
				wheelUAPI.BrakeTorque = maxBrakeTorque;
				wheelUAPI.MotorTorque = 0f;
				wheelUAPI.SteerAngle = 0f;
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			xAxis = Input.GetAxis("Horizontal");
			yAxis = Input.GetAxis("Vertical");
			bool key = Input.GetKey(KeyCode.Space);
			smoothXAxis = Mathf.SmoothDamp(smoothXAxis, xAxis, ref xAxisVelocity, 0.12f);
			for (int i = 0; i < wheels.Count; i++)
			{
				_Wheel wheel = wheels[i];
				WheelUAPI wheelUAPI = wheel.wheelUAPI;
				wheelUAPI.BrakeTorque = 0f;
				wheelUAPI.MotorTorque = 0f;
				if (wheel.handbrake && key)
				{
					wheelUAPI.BrakeTorque = maxBrakeTorque;
				}
				if ((base.SpeedSigned < -0.4f && yAxis > 0.1f) || (base.SpeedSigned > 0.4f && yAxis < -0.1f))
				{
					wheelUAPI.BrakeTorque = maxBrakeTorque * Mathf.Abs(yAxis);
				}
				if ((wheel.power && base.SpeedSigned >= -0.5f && yAxis > 0.1f) || (base.SpeedSigned <= 0.5f && yAxis < -0.1f))
				{
					wheelUAPI.MotorTorque = maxMotorTorque * yAxis;
				}
				if (wheel.steer)
				{
					wheelUAPI.SteerAngle = Mathf.Lerp(maxSteeringAngle, minSteeringAngle, base.Speed * 0.04f) * smoothXAxis;
				}
			}
		}

		private void Reset()
		{
			maxBrakeTorque = 3000f;
			maxMotorTorque = 1000f;
			maxSteeringAngle = 35f;
			minSteeringAngle = 20f;
			wheels = new List<_Wheel>();
			WheelUAPI[] componentsInChildren = GetComponentsInChildren<WheelUAPI>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				WheelUAPI wheelUAPI = componentsInChildren[i];
				wheels.Add(new _Wheel
				{
					wheelUAPI = wheelUAPI,
					steer = (i < 2),
					power = true,
					handbrake = (i > 1)
				});
			}
		}
	}
}
