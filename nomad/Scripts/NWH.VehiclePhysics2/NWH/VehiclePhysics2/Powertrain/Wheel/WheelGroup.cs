using System;
using System.Collections.Generic;
using NWH.Common.Vehicles;
using UnityEngine;

namespace NWH.VehiclePhysics2.Powertrain.Wheel
{
	[Serializable]
	public class WheelGroup
	{
		public string name;

		[Tooltip("    Should Ackerman steering angle be added to the axle?\r\n    angle is auto-calculated.")]
		[ShowInSettings("Add Ackerman")]
		public bool addAckerman = true;

		[ShowInSettings("ARB Force", 0f, 12000f, 1000f)]
		public float antiRollBarForce;

		[Tooltip("If set to 1 axle will receive full brake torque as set by Max Torque parameter under Brake section while 0 means no breaking at all.")]
		[Range(0f, 1f)]
		[ShowInSettings("Brake Coeff.", 0f, 1f, 0.1f)]
		public float brakeCoefficient = 1f;

		[Range(0f, 2f)]
		[Tooltip("    If set to 1 axle will receive full brake torque when handbrake is used.")]
		[ShowInSettings("Brake Coeff.", 0f, 1f, 0.1f)]
		public float handbrakeCoefficient;

		[Tooltip("Setting to true will override camber settings and camber will be calculated from position of the (imaginary) axle object instead.")]
		[ShowInSettings]
		public bool isSolid;

		[Tooltip("    Track width of the axle. 0 if wheel count is not 2.")]
		public float trackWidth;

		[Tooltip("Determines what percentage of the steer angle will be applied to the wheel. If set to negative value wheels will turn in direction opposite of input.")]
		[Range(-1f, 1f)]
		[ShowInSettings("Steer Coeff.", -1f, 1f, 0.1f)]
		public float steerCoefficient;

		public bool applyCasterAngle;

		[Tooltip("Positive caster means that whe wheel will be angled towards the front of the vehicle while negative  caster will angle the wheel in opposite direction (shopping cart wheel).")]
		[Range(-8f, 8f)]
		[ShowInTelemetry]
		[SerializeField]
		private float _casterAngle;

		public bool applyToeAngle;

		[Tooltip("Positive toe angle means that the wheels will face inwards (front of the wheel angled toward longitudinal center of the vehicle).")]
		[Range(-8f, 8f)]
		[SerializeField]
		[ShowInTelemetry]
		private float _toeAngle;

		[SerializeField]
		private List<WheelComponent> wheels = new List<WheelComponent>();

		private float _camber;

		private float _arbForce;

		public VehicleController vc;

		public float ToeAngle
		{
			get
			{
				return _toeAngle;
			}
			set
			{
				_toeAngle = value;
				ApplyGeometryValues();
			}
		}

		public float CasterAngle
		{
			get
			{
				return _casterAngle;
			}
			set
			{
				_casterAngle = value;
				ApplyGeometryValues();
			}
		}

		public WheelComponent LeftWheel
		{
			get
			{
				if (wheels.Count != 0)
				{
					return wheels[0];
				}
				return null;
			}
		}

		public WheelComponent RightWheel
		{
			get
			{
				if (wheels.Count > 1)
				{
					return wheels[1];
				}
				return null;
			}
		}

		public WheelComponent Wheel
		{
			get
			{
				if (wheels.Count != 0)
				{
					return wheels[0];
				}
				return null;
			}
		}

		public List<WheelComponent> Wheels => wheels;

		public void Initialize()
		{
			FindBelongingWheels();
			ApplyGeometryValues();
		}

		public void FindBelongingWheels()
		{
			int thisGroupIndex = vc.powertrain.wheelGroups.IndexOf(this);
			wheels.Clear();
			foreach (WheelComponent item in FindWheelsBelongingToGroup(ref vc.powertrain.wheels, thisGroupIndex))
			{
				AddWheel(item);
			}
			if (wheels.Count == 2)
			{
				trackWidth = Vector3.Distance(LeftWheel.wheelUAPI.transform.position, RightWheel.wheelUAPI.transform.position);
			}
		}

		public void Update()
		{
			int count = wheels.Count;
			if (antiRollBarForce > 0f && count == 2)
			{
				CalculateARB();
			}
			if (isSolid && count == 2 && trackWidth != 0f)
			{
				WheelComponent wheelComponent = wheels[0];
				WheelComponent wheelComponent2 = wheels[1];
				float springLength = wheelComponent.wheelUAPI.SpringLength;
				float y = wheelComponent2.wheelUAPI.SpringLength - springLength;
				_camber = Mathf.Atan2(y, trackWidth) * 57.29578f;
				wheelComponent.wheelUAPI.Camber = 0f - _camber;
				wheelComponent2.wheelUAPI.Camber = _camber;
			}
		}

		public void CalculateARB()
		{
			WheelUAPI wheelUAPI = Wheels[0].wheelUAPI;
			WheelUAPI wheelUAPI2 = Wheels[1].wheelUAPI;
			if (wheelUAPI.IsGrounded && wheelUAPI2.IsGrounded)
			{
				float springLength = wheelUAPI.SpringLength;
				float springLength2 = wheelUAPI2.SpringLength;
				float num = springLength - springLength2;
				_arbForce = num * antiRollBarForce;
				if (wheelUAPI.IsGrounded || wheelUAPI2.IsGrounded)
				{
					wheelUAPI.TargetRigidbody.AddForceAtPosition(wheelUAPI.transform.up * (0f - _arbForce), wheelUAPI.transform.position);
					wheelUAPI2.TargetRigidbody.AddForceAtPosition(wheelUAPI2.transform.up * _arbForce, wheelUAPI2.transform.position);
				}
			}
		}

		public void ApplyGeometryValues()
		{
			foreach (WheelComponent wheel in Wheels)
			{
				if (applyCasterAngle || applyToeAngle)
				{
					Vector3 localEulerAngles = wheel.wheelUAPI.transform.localEulerAngles;
					if (wheel.wheelUAPI.transform.localPosition.x >= 0f)
					{
						wheel.wheelUAPI.transform.localEulerAngles = new Vector3(applyCasterAngle ? (0f - _casterAngle) : localEulerAngles.x, applyToeAngle ? (0f - _toeAngle) : localEulerAngles.y, localEulerAngles.z);
					}
					else
					{
						wheel.wheelUAPI.transform.localEulerAngles = new Vector3(applyCasterAngle ? (0f - _casterAngle) : localEulerAngles.x, applyToeAngle ? _toeAngle : localEulerAngles.y, localEulerAngles.z);
					}
				}
			}
		}

		public void AddWheel(WheelComponent wheel)
		{
			Wheels.Add(wheel);
			wheel.wheelGroup = this;
		}

		public List<WheelComponent> FindWheelsBelongingToGroup(ref List<WheelComponent> wheels, int thisGroupIndex)
		{
			List<WheelComponent> list = new List<WheelComponent>();
			foreach (WheelComponent wheel in wheels)
			{
				if (wheel.wheelGroupSelector.index == thisGroupIndex)
				{
					list.Add(wheel);
				}
			}
			return list;
		}

		public void RemoveWheel(WheelComponent wheel)
		{
			Wheels.Remove(wheel);
		}

		public void SetWheels(List<WheelComponent> wheels)
		{
			this.wheels = wheels;
			foreach (WheelComponent wheel in wheels)
			{
				wheel.wheelGroup = this;
			}
		}
	}
}
