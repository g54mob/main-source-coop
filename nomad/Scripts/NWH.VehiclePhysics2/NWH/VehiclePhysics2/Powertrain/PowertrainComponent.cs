using System;
using NWH.Common.Utility;
using UnityEngine;

namespace NWH.VehiclePhysics2.Powertrain
{
	[Serializable]
	public abstract class PowertrainComponent : VehicleComponent
	{
		[Tooltip("    Name of the component. Only unique names should be used on the same vehicle.")]
		[SerializeField]
		public string name = "";

		[Range(0.0002f, 2f)]
		[Tooltip("Angular inertia of the component. Higher inertia value will result in a powertrain that is slower to spin up, but\r\nalso slower to spin down. Too high values will result in (apparent) sluggish response while too low values will\r\nresult in vehicle being easy to stall.")]
		public float inertia = 0.05f;

		public float inputTorque;

		public float outputTorque;

		public float inputAngularVelocity;

		public float outputAngularVelocity;

		public float inputInertia;

		public float outputInertia;

		[NonSerialized]
		protected PowertrainComponent _input;

		public int inputNameHash;

		[NonSerialized]
		protected PowertrainComponent _output;

		public int outputNameHash;

		protected float _damage;

		public PowertrainComponent Input
		{
			get
			{
				return _input;
			}
			set
			{
				if (value == null || value == this)
				{
					_input = null;
					inputNameHash = 0;
				}
				else
				{
					_input = value;
				}
			}
		}

		public PowertrainComponent Output
		{
			get
			{
				return _output;
			}
			set
			{
				if (value == this)
				{
					Debug.LogWarning(name + ": PowertrainComponent Output can not be self.");
					outputNameHash = 0;
					_output = null;
					return;
				}
				if (_output != null)
				{
					_output.inputNameHash = 0;
					_output._input = null;
				}
				_output = value;
				if (_output != null)
				{
					outputNameHash = _output.name.GetHashCode();
					_output._input = this;
					_output.inputNameHash = name.GetHashCode();
				}
				else
				{
					outputNameHash = 0;
				}
			}
		}

		public float Damage
		{
			get
			{
				return _damage;
			}
			set
			{
				_damage = Mathf.Clamp01(value);
			}
		}

		public float InputRPM => UnitConverter.AngularVelocityToRPM(inputAngularVelocity);

		public float OutputRPM => UnitConverter.AngularVelocityToRPM(outputAngularVelocity);

		protected override void VC_Initialize()
		{
			if (inertia < 1E-05f)
			{
				inertia = 1E-05f;
			}
			LoadComponentFromHash(in vehicleController, ref _output, in outputNameHash);
			LoadComponentFromHash(in vehicleController, ref _input, in inputNameHash);
			base.VC_Initialize();
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				inputAngularVelocity = 0f;
				outputAngularVelocity = 0f;
				inputTorque = 0f;
				outputTorque = 0f;
				return true;
			}
			return false;
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			inertia = 0.02f;
		}

		public override void VC_Validate(VehicleController vc)
		{
			base.VC_Validate(vc);
			if (inertia < 0.0001f)
			{
				inertia = 0.0001f;
				Debug.LogWarning(vc.name + " " + name + ": Inertia must be larger than 0.0.0001f. Setting to 0.0.0001f.");
			}
			if (outputNameHash == 0)
			{
				PC_LogWarning(vc, "Output not set. This might be a result of the 10.20f update, in which case the powertrain outputs need to be re-assigned.");
			}
		}

		public virtual float QueryAngularVelocity(float angularVelocity, float dt)
		{
			inputAngularVelocity = angularVelocity;
			if (outputNameHash == 0)
			{
				return angularVelocity;
			}
			outputAngularVelocity = angularVelocity;
			return _output.QueryAngularVelocity(outputAngularVelocity, dt);
		}

		public virtual float QueryInertia()
		{
			if (outputNameHash == 0)
			{
				return inertia;
			}
			float num = inertia;
			float num2 = _output.QueryInertia();
			return num + num2;
		}

		public virtual float ForwardStep(float torque, float inertiaSum, float dt)
		{
			inputTorque = torque;
			inputInertia = inertiaSum;
			if (outputNameHash == 0)
			{
				return torque;
			}
			outputTorque = inputTorque;
			outputInertia = inertiaSum + inertia;
			return _output.ForwardStep(outputTorque, outputInertia, dt);
		}

		public void PC_LogWarning(VehicleController vc, string message)
		{
			vc.VC_LogWarning(name + " [" + GetType()?.Name + "] > " + message);
		}

		public static float TorqueToPowerInKW(in float angularVelocity, in float torque)
		{
			return torque * angularVelocity / 1000f;
		}

		public static float PowerInKWToTorque(in float angularVelocity, in float powerInKW)
		{
			float num = powerInKW * 1000f;
			float num2 = Mathf.Abs(angularVelocity);
			float num3 = ((num2 > -1f && num2 < 1f) ? 1f : angularVelocity);
			return num / num3;
		}

		public float CalculateOutputPowerInKW()
		{
			return GetPowerInKW(in outputTorque, in outputAngularVelocity);
		}

		public static float GetPowerInKW(in float torque, in float angularVelocity)
		{
			return torque * angularVelocity / 1000f;
		}

		protected static void LoadComponentFromHash(in VehicleController vc, ref PowertrainComponent component, in int hashCode)
		{
			if (component == null && hashCode != 0)
			{
				component = vc.powertrain.Inspector_GetPowertrainComponentFromNameHash(hashCode);
			}
		}
	}
}
