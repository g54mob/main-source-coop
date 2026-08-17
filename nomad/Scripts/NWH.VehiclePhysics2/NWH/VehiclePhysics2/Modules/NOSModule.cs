using System;
using NWH.VehiclePhysics2.Modules.NOS;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules
{
	[Serializable]
	public class NOSModule : VehicleComponent
	{
		[Tooltip("    Capacity of NOS bottle.")]
		public float capacity = 2f;

		[Tooltip("    Current charge of NOS bottle.")]
		public float charge = 2f;

		[Tooltip("    Can NOS be used while in reverse?")]
		public bool disableInReverse = true;

		[Tooltip("    Can NOS be used while there is no throttle input / engine is idling?")]
		public bool disableOffThrottle = true;

		[Range(1f, 3f)]
		[Tooltip("Makes engine sound louder while NOS is active.\r\nVolume range of the engine running sound component will get multiplied by this value.")]
		public float engineVolumeCoefficient = 1.5f;

		[Range(1f, 3f)]
		[Tooltip("    Value that will be used as base intensity of Exhaust Smoke effect while NOS is active.")]
		public float exhaustEmissionCoefficient = 2f;

		[Tooltip("    Maximum flow of NOS in kg/s.")]
		public float flow = 0.1f;

		[Range(1f, 5f)]
		[Tooltip("Power of the engine will be multiplied by this value when NOS is active to get the final engine power.")]
		public float powerCoefficient = 2f;

		[SerializeField]
		public NOSSoundComponent nosSoundComponent = new NOSSoundComponent();

		public bool IsUsingNOS
		{
			get
			{
				if (state.isEnabled && vehicleController.input.Boost && charge > 0f && (vehicleController.powertrain.transmission.Gear >= 0 || !disableInReverse))
				{
					if (vehicleController.powertrain.engine.ThrottlePosition < 0.5f)
					{
						return !disableOffThrottle;
					}
					return true;
				}
				return false;
			}
		}

		protected override void VC_Initialize()
		{
			nosSoundComponent.nosModule = this;
			vehicleController.soundManager.AddAndOnboardNewComponent(nosSoundComponent);
			base.VC_Initialize();
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.powerModifiers.Add(NOSPowerModifier);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.powertrain.engine.powerModifiers.Remove(NOSPowerModifier);
				return true;
			}
			return false;
		}

		public float NOSPowerModifier()
		{
			if (!IsUsingNOS)
			{
				return 1f;
			}
			charge -= flow * vehicleController.fixedDeltaTime;
			charge = ((charge < 0f) ? 0f : ((charge > capacity) ? capacity : charge));
			if (charge <= 0f)
			{
				return 1f;
			}
			if (vehicleController.effectsManager.exhaustFlash.IsActive)
			{
				vehicleController.effectsManager.exhaustFlash.Flash(triggerEvent: false);
			}
			return powerCoefficient;
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			nosSoundComponent.VC_SetDefaults();
		}
	}
}
