using System;

namespace NWH.VehiclePhysics2.Modules.ModuleTemplate
{
	[Serializable]
	public class ModuleTemplateWrapper : ModuleWrapper
	{
		public ModuleTemplate module = new ModuleTemplate();

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as ModuleTemplate;
		}
	}
}
