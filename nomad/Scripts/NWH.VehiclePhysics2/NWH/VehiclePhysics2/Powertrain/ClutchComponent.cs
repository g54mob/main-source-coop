using System;
using NWH.Common;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.Powertrain
{
	[Serializable]
	public class ClutchComponent : PowertrainComponent
	{
		public enum ClutchControlType
		{
			Automatic = 0,
			UserInput = 1,
			Manual = 2
		}

		[Tooltip("    RPM at which automatic clutch will try to engage.")]
		[FormerlySerializedAs("baseEngagementRPM")]
		[ShowInTelemetry]
		[ShowInSettings("Engagement RPM", 900f, 2000f, 100f)]
		public float engagementRPM = 1200f;

		public float throttleEngagementOffsetRPM = 400f;

		[Range(0f, 1f)]
		[Tooltip("Clutch engagement in range [0,1] where 1 is fully engaged clutch.\r\nAffected by Slip Torque field as the clutch can transfer [clutchEngagement * slipTorque] Nm\r\nmeaning that higher value of slipTorque will result in more sensitive clutch.")]
		[ShowInTelemetry]
		public float clutchInput;

		[Tooltip("Curve representing pedal travel vs. clutch engagement. Should start at 0,0 and end at 1,1.")]
		[FormerlySerializedAs("clutchEngagementCurve")]
		public AnimationCurve engagementCurve = new AnimationCurve();

		public ClutchControlType controlType;

		[ShowInSettings("Engagement Range", 200f, 1000f, 100f)]
		[Tooltip("The RPM range in which the clutch will go from disengaged to engaged and vice versa. \r\nE.g. if set to 400 and engagementRPM is 1000, 1000 will mean clutch is fully disengaged and\r\n1400 fully engaged. Setting it too low might cause clutch to hunt/oscillate.")]
		public float engagementRange = 400f;

		[SerializeField]
		[Tooltip("Torque at which the clutch will slip / maximum torque that the clutch can transfer.\r\nThis value also affects clutch engagement as higher slip value will result in clutch\r\nthat grabs higher up / sooner. Too high slip torque value combined with low inertia of\r\npowertrain components might cause instability in powertrain solver.")]
		[ShowInSettings("Slip Torque", 10f, 5000f, 100f)]
		public float slipTorque = 500f;

		[Tooltip("Amount of torque that will be passed through clutch even when completely disengaged to emulate torque converter creep on automatic transmissions.Should be higher than rolling resistance of the wheels to get the vehicle rolling.")]
		[ShowInSettings("Creep Torque", 0f, 100f, 10f)]
		public float creepTorque;

		public float creepSpeedLimit = 1f;

		[NonSerialized]
		private float _clutchEngagement;

		public float Engagement => _clutchEngagement;

		public override void VC_Validate(VehicleController vc)
		{
			base.VC_Validate(vc);
			if (vc.powertrain.engine.engineType == EngineComponent.EngineType.ICE && engagementRPM <= vc.powertrain.engine.idleRPM)
			{
				PC_LogWarning(vc, "Clutch engagement RPM is too low on vehicle " + vc.name + ". Clutch might stay engaged while in idle. Increase clutch engagement RPM to be larger than engine idle RPM.");
			}
			if (engagementCurve == null || engagementCurve.keys.Length < 2)
			{
				PC_LogWarning(vc, "Clutch engagement curve is not set. A simple [0,0] to [1,1] curve can be used.");
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			inertia = 0.02f;
			slipTorque = vehicleController.powertrain.engine.EstimatedPeakTorque * 1.5f;
			SetDefaultClutchEngagementCurve();
			base.Output = vehicleController.powertrain.transmission;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				clutchInput = 0f;
				_clutchEngagement = 0f;
				return true;
			}
			return false;
		}

		private void SetDefaultClutchEngagementCurve()
		{
			engagementCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
		}

		public override float QueryAngularVelocity(float angularVelocity, float dt)
		{
			inputAngularVelocity = angularVelocity;
			if (inputNameHash == 0 || outputNameHash == 0)
			{
				return inputAngularVelocity;
			}
			if (controlType == ClutchControlType.Automatic)
			{
				EngineComponent engine = vehicleController.powertrain.engine;
				if (vehicleController.powertrain.engine.OutputRPM < engine.idleRPM)
				{
					clutchInput = 0f;
				}
				else if (vehicleController.powertrain.transmission.isShifting)
				{
					float shiftProgress = vehicleController.powertrain.transmission.shiftProgress;
					clutchInput = Mathf.Abs(Mathf.Cos((float)Math.PI * shiftProgress));
				}
				else
				{
					float inputSwappedThrottle = vehicleController.input.InputSwappedThrottle;
					float num = engagementRPM + throttleEngagementOffsetRPM * (inputSwappedThrottle * inputSwappedThrottle);
					float num2 = (Mathf.Max(base.InputRPM, base.OutputRPM) - num) / engagementRange;
					if (num2 > clutchInput)
					{
						clutchInput = Mathf.SmoothStep(clutchInput, num2, dt * 15f);
					}
					else
					{
						clutchInput = num2;
					}
					clutchInput = Mathf.Clamp(clutchInput, 0f, 1f);
					if (engine.OutputRPM > engine.idleRPM * 1.1f && vehicleController.Speed > 3f)
					{
						clutchInput = 1f;
					}
				}
			}
			else if (controlType == ClutchControlType.UserInput)
			{
				clutchInput = vehicleController.input.Clutch;
			}
			outputAngularVelocity = inputAngularVelocity * _clutchEngagement;
			float num3 = base.Output.QueryAngularVelocity(outputAngularVelocity, dt) * _clutchEngagement;
			float num4 = angularVelocity * (1f - _clutchEngagement);
			return num3 + num4;
		}

		public override float QueryInertia()
		{
			if (outputNameHash == 0)
			{
				return inertia;
			}
			return inertia + base.Output.QueryInertia() * _clutchEngagement;
		}

		public override float ForwardStep(float torque, float inertiaSum, float dt)
		{
			inputTorque = torque;
			inputInertia = inertiaSum;
			if (outputNameHash == 0)
			{
				return torque;
			}
			_clutchEngagement = engagementCurve.Evaluate(clutchInput);
			_clutchEngagement = Mathf.Clamp01(_clutchEngagement);
			float num = inertia * 0.5f;
			outputInertia = (inertiaSum + num) * _clutchEngagement + num;
			float range = slipTorque * _clutchEngagement;
			outputTorque = inputTorque;
			MathUtility.ClampWithRemainder(ref outputTorque, in range, out var remainder);
			ApplyCreepTorque(ref outputTorque, creepTorque);
			return Mathf.Clamp(_output.ForwardStep(outputTorque, outputInertia, dt) * _clutchEngagement, 0f - slipTorque, slipTorque) + remainder;
		}

		private void ApplyCreepTorque(ref float torque, float creepTorque)
		{
			if (creepTorque != 0f && vehicleController.powertrain.engine.IsRunning && vehicleController.Speed < creepSpeedLimit && torque < creepTorque && torque > 0f - creepTorque)
			{
				torque = creepTorque;
			}
		}
	}
}
