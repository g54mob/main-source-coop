using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.Metrics
{
	[Serializable]
	[DisallowMultipleComponent]
	public class MetricsModuleWrapper : ModuleWrapper
	{
		public MetricsModule module = new MetricsModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as MetricsModule;
		}
	}
}
