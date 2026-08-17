using System;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.GroundDetection;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using UnityEngine;

namespace NWH.VehiclePhysics2.Powertrain
{
	[Serializable]
	public class WheelComponent : PowertrainComponent
	{
		[NonSerialized]
		[ShowInTelemetry]
		public int surfaceMapIndex = -1;

		[ShowInTelemetry]
		public SurfacePreset surfacePreset;

		public WheelUAPI wheelUAPI;

		public WheelGroupSelector wheelGroupSelector = new WheelGroupSelector();

		[NonSerialized]
		public WheelGroup wheelGroup;

		private float _initialRollingResistance;

		protected override void VC_Initialize()
		{
			_initialRollingResistance = wheelUAPI.RollingResistanceTorque;
			base.VC_Initialize();
		}

		public override void VC_Validate(VehicleController vc)
		{
			if (wheelUAPI == null)
			{
				PC_LogWarning(vc, "WheelUAPI is null. Make sure to assign a wheel under VehicleController > PWR > Wheels. If the vehicle has been set up pre 10.0b, wheels will need re-assigning.");
			}
		}

		public void AddBrakeTorque(float torque, bool isHandbrake = false)
		{
			float brakeTorque = wheelUAPI.BrakeTorque;
			if (wheelGroup != null)
			{
				torque *= (isHandbrake ? wheelGroup.handbrakeCoefficient : wheelGroup.brakeCoefficient);
			}
			brakeTorque = ((!(torque < 0f)) ? (brakeTorque + torque) : (brakeTorque + 0f));
			if (brakeTorque > vehicleController.brakes.maxTorque)
			{
				brakeTorque = vehicleController.brakes.maxTorque;
			}
			if (brakeTorque < 0f)
			{
				brakeTorque = 0f;
			}
			wheelUAPI.BrakeTorque = brakeTorque;
		}

		public override float QueryAngularVelocity(float angularVelocity, float dt)
		{
			inputAngularVelocity = (outputAngularVelocity = wheelUAPI.AngularVelocity);
			return outputAngularVelocity;
		}

		public override float QueryInertia()
		{
			float num = Mathf.Clamp(vehicleController.fixedDeltaTime, 0.01f, 0.05f) / 0.005f;
			return 0.5f * wheelUAPI.Mass * wheelUAPI.Radius * wheelUAPI.Radius * num;
		}

		public void ApplyRollingResistanceMultiplier(float multiplier)
		{
			wheelUAPI.RollingResistanceTorque = _initialRollingResistance * multiplier;
		}

		public override float ForwardStep(float torque, float inertiaSum, float dt)
		{
			inputTorque = torque;
			inputInertia = inertiaSum;
			outputTorque = inputTorque;
			outputInertia = wheelUAPI.Mass * wheelUAPI.Radius * wheelUAPI.Radius + inertiaSum;
			wheelUAPI.MotorTorque = outputTorque;
			wheelUAPI.Inertia = outputInertia;
			wheelUAPI.AutoSimulate = false;
			wheelUAPI.Step();
			return wheelUAPI.CounterTorque;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				wheelUAPI.BrakeTorque = 0f;
				wheelUAPI.MotorTorque = 0f;
				wheelUAPI.AutoSimulate = true;
				return true;
			}
			return false;
		}

		public void SetWheelGroup(int wheelGroupIndex)
		{
			wheelGroupSelector.index = wheelGroupIndex;
		}
	}
}
