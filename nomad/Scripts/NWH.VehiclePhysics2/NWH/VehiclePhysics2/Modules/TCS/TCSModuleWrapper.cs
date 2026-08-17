using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.TCS
{
	[Serializable]
	[DisallowMultipleComponent]
	public class TCSModuleWrapper : ModuleWrapper
	{
		public TCSModule module = new TCSModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as TCSModule;
		}
	}
}
