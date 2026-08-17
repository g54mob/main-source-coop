using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.CruiseControl
{
	[Serializable]
	[DisallowMultipleComponent]
	public class CruiseControlModuleWrapper : ModuleWrapper
	{
		public CruiseControlModule module = new CruiseControlModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as CruiseControlModule;
		}
	}
}
