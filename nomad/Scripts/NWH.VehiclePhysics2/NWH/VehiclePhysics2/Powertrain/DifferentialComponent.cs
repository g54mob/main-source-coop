using System;
using NWH.Common.Vehicles;
using UnityEngine;

namespace NWH.VehiclePhysics2.Powertrain
{
	[Serializable]
	public class DifferentialComponent : PowertrainComponent
	{
		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("    Torque bias between left (A) and right (B) output in [0,1] range.")]
		[ShowInTelemetry]
		[ShowInSettings("Bias A/B", 0f, 1f, 0.1f)]
		public float biasAB = 0.5f;

		[SerializeField]
		[ShowInTelemetry]
		[ShowInSettings("Power Ramp", 0f, 10f, 0.1f)]
		[Tooltip("Stiffness of the differential under acceleration.")]
		public float powerStiffness = 1f;

		[ShowInTelemetry]
		[ShowInSettings("Coast Ramp", 0f, 10f, 0.1f)]
		[Tooltip("Stiffness of the differential under braking.")]
		public float coastStiffness = 0.5f;

		[Tooltip("Slip torque of a differential. Typically in the range of 100-500 Nm for a sports car. Use >1000 for a locking differential.")]
		[ShowInTelemetry]
		[ShowInSettings("LSD Slip Tq", 0f, 2000f, 100f)]
		public float slipTorque = 150f;

		[Tooltip("If true, the torque will be sent to left/right based on the steering input.")]
		public bool differentialSteering;

		[NonSerialized]
		protected PowertrainComponent _outputB;

		public int outputBNameHash;

		public PowertrainComponent OutputB
		{
			get
			{
				return _outputB;
			}
			set
			{
				if (value == this)
				{
					Debug.LogWarning(name + ": PowertrainComponent Output can not be self.");
					outputBNameHash = 0;
					_output = null;
					return;
				}
				if (_outputB != null)
				{
					_outputB.inputNameHash = 0;
					_outputB.Input = null;
				}
				_outputB = value;
				if (_outputB != null)
				{
					outputBNameHash = _outputB.name.GetHashCode();
					_outputB.Input = this;
				}
				else
				{
					outputBNameHash = 0;
				}
			}
		}

		protected override void VC_Initialize()
		{
			PowertrainComponent.LoadComponentFromHash(in vehicleController, ref _outputB, in outputBNameHash);
			base.VC_Initialize();
		}

		public override void VC_Validate(VehicleController vc)
		{
			base.VC_Validate(vc);
			if (outputBNameHash == 0)
			{
				Debug.Log(outputBNameHash);
				PC_LogWarning(vc, "PowertrainComponent output not set. This might be a result of the 10.20f update, in which case the powertrain outputs need to be re-assigned.");
			}
			if (Application.isPlaying && base.Input == null)
			{
				PC_LogWarning(vc, "Differential has no input. Differential that are in no way connected to the engine will not be updated and should be removed or they might cause the wheels attached to them to spin up slower than usual due to the inertia of a dangling/dead differential.");
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			name = "Differential";
			inertia = 0.02f;
		}

		public override float QueryAngularVelocity(float angularVelocity, float dt)
		{
			inputAngularVelocity = angularVelocity;
			if (outputNameHash == 0 || outputBNameHash == 0)
			{
				return angularVelocity;
			}
			outputAngularVelocity = inputAngularVelocity;
			float num = _output.QueryAngularVelocity(outputAngularVelocity, dt);
			float num2 = _outputB.QueryAngularVelocity(outputAngularVelocity, dt);
			return (num + num2) * 0.5f;
		}

		public override float QueryInertia()
		{
			if (outputNameHash == 0 || outputBNameHash == 0)
			{
				return inertia;
			}
			float num = _output.QueryInertia();
			float num2 = _outputB.QueryInertia();
			return inertia + (num + num2);
		}

		public override float ForwardStep(float torque, float inertiaSum, float dt)
		{
			inputTorque = torque;
			inputInertia = inertiaSum;
			if (outputNameHash == 0 || outputBNameHash == 0)
			{
				return torque;
			}
			float num = _output.QueryAngularVelocity(outputAngularVelocity, dt);
			float num2 = _outputB.QueryAngularVelocity(outputAngularVelocity, dt);
			float num3;
			float num4;
			if (differentialSteering)
			{
				float steering = vehicleController.input.Steering;
				num3 = torque * (1f + steering) / 2f;
				num4 = torque * (1f - steering) / 2f;
			}
			else
			{
				float num5 = ((torque > 0f) ? (powerStiffness * 1000f) : (coastStiffness * 1000f));
				float num6 = num - num2;
				float num7 = Mathf.Clamp(num5 * num6, 0f - slipTorque, slipTorque);
				num3 = torque - num7;
				num4 = torque + num7;
				num3 *= 1f - biasAB;
				num4 *= biasAB;
			}
			float num8 = inertiaSum * 0.5f;
			float num9 = inertiaSum * 0.5f;
			outputTorque = num3 + num4;
			outputInertia = num8 + num9;
			return _output.ForwardStep(num3, num8, dt) + _outputB.ForwardStep(num4, num9, dt);
		}
	}
}
