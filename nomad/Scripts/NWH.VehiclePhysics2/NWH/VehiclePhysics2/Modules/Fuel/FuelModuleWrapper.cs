using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.Fuel
{
	[Serializable]
	[DisallowMultipleComponent]
	public class FuelModuleWrapper : ModuleWrapper
	{
		public FuelModule module = new FuelModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as FuelModule;
		}
	}
}
