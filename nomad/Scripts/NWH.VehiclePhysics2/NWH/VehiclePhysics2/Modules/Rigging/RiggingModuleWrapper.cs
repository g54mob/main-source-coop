using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.Rigging
{
	[Serializable]
	[DisallowMultipleComponent]
	public class RiggingModuleWrapper : ModuleWrapper
	{
		public RiggingModule module = new RiggingModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as RiggingModule;
		}
	}
}
