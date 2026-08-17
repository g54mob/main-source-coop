using System;

namespace NWH.VehiclePhysics2.Modules.ArcadeModule
{
	[Serializable]
	public class ArcadeModuleWrapper : ModuleWrapper
	{
		public ArcadeModule module = new ArcadeModule();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as ArcadeModule;
		}
	}
}
