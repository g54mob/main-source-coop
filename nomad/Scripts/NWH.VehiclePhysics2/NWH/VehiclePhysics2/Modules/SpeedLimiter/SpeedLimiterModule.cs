using System;
using NWH.Common.Utility;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.SpeedLimiter
{
	[Serializable]
	public class SpeedLimiterModule : VehicleComponent
	{
		public enum SpeedUnits
		{
			ms = 0,
			kmh = 1,
			mph = 2
		}

		public bool active;

		[Tooltip("    Speed limit above which the throttle will be cut.")]
		public float speedLimit;

		[Tooltip("    Units which will be used for speed limiter. Defaults to m/s.")]
		public SpeedUnits speedUnits;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.powerModifiers.Add(SpeedPowerLimiter);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				active = false;
				vehicleController.powertrain.engine.powerModifiers.Remove(SpeedPowerLimiter);
				return true;
			}
			return false;
		}

		public float SpeedPowerLimiter()
		{
			if (!base.IsActive || speedLimit == 0f)
			{
				active = false;
				return 1f;
			}
			float num = 0f;
			if (speedUnits == SpeedUnits.ms)
			{
				num = speedLimit;
			}
			else if (speedUnits == SpeedUnits.kmh)
			{
				num = UnitConverter.Speed_kmhToMs(speedLimit);
			}
			else if (speedUnits == SpeedUnits.mph)
			{
				num = UnitConverter.Speed_mphToMs(speedLimit);
			}
			if (vehicleController.Speed > num)
			{
				active = true;
				return 0f;
			}
			active = false;
			return 1f;
		}
	}
}
