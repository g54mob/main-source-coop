using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.SpeedLimiter
{
	[Serializable]
	[DisallowMultipleComponent]
	public class SpeedLimiterModuleWrapper : ModuleWrapper
	{
		public SpeedLimiterModule module = new SpeedLimiterModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as SpeedLimiterModule;
		}
	}
}
