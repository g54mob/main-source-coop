using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.FlipOver
{
	[Serializable]
	[DisallowMultipleComponent]
	public class FlipOverModuleWrapper : ModuleWrapper
	{
		public FlipOverModule module = new FlipOverModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as FlipOverModule;
		}
	}
}
