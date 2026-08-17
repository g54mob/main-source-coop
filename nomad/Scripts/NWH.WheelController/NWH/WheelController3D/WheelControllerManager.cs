using System.Collections.Generic;
using NWH.Common.Vehicles;
using UnityEngine;

namespace NWH.WheelController3D
{
	[DefaultExecutionOrder(60)]
	public class WheelControllerManager : MonoBehaviour
	{
		public bool wakeWheelsOnConnectedBodies = true;

		private List<WheelUAPI> _wheels = new List<WheelUAPI>();

		private int _wheelCount;

		private Rigidbody _rb;

		private Joint[] _joints;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
			_wheels = new List<WheelUAPI>();
			_wheelCount = 0;
		}

		private void FixedUpdate()
		{
			bool flag = false;
			for (int i = 0; i < _wheels.Count; i++)
			{
				float motorTorque = _wheels[i].MotorTorque;
				if (motorTorque > 0.01f || motorTorque < -0.01f)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				WakeAllWheels();
			}
		}

		public void WakeAllWheels()
		{
			if (_wheels.Count > 0)
			{
				for (int i = 0; i < _wheels.Count; i++)
				{
					_wheels[i].WakeFromSleep();
				}
			}
			if (!wakeWheelsOnConnectedBodies)
			{
				return;
			}
			_joints = _rb.GetComponentsInChildren<Joint>();
			Joint[] joints = _joints;
			foreach (Joint joint in joints)
			{
				if (joint.connectedBody != null && joint.connectedBody != _rb)
				{
					WheelControllerManager component = joint.connectedBody.GetComponent<WheelControllerManager>();
					if ((bool)component)
					{
						component.WakeAllWheels();
					}
				}
			}
		}

		public void Register(WheelUAPI wheel)
		{
			if (!_wheels.Contains(wheel))
			{
				_wheels.Add(wheel);
				_wheelCount++;
			}
		}

		public void Deregister(WheelUAPI wheel)
		{
			if (_wheels.Contains(wheel))
			{
				_wheels.Remove(wheel);
				_wheelCount--;
			}
		}
	}
}
