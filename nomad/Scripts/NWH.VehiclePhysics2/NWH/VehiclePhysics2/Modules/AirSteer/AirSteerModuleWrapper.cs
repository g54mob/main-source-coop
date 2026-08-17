using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.AirSteer
{
	[Serializable]
	[DisallowMultipleComponent]
	public class AirSteerModuleWrapper : ModuleWrapper
	{
		public AirSteerModule module = new AirSteerModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as AirSteerModule;
		}
	}
}
