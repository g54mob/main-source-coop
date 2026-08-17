using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.MotorcycleModule
{
	[Serializable]
	[DisallowMultipleComponent]
	public class MotorcycleModuleWrapper : ModuleWrapper
	{
		public MotorcycleModule module = new MotorcycleModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as MotorcycleModule;
		}
	}
}
