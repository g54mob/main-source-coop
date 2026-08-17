using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.ABS
{
	[Serializable]
	[DisallowMultipleComponent]
	public class ABSModuleWrapper : ModuleWrapper
	{
		public ABSModule module = new ABSModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as ABSModule;
		}
	}
}
