using System;
using System.Collections.Generic;

namespace NWH.VehiclePhysics2.Modules
{
	[Serializable]
	public class ModuleManager : ManagerVehicleComponent
	{
		protected override void FillComponentList()
		{
			if (_components == null)
			{
				_components = new List<VehicleComponent>();
			}
			else
			{
				_components.Clear();
			}
			ModuleWrapper[] components = vehicleController.GetComponents<ModuleWrapper>();
			if (components != null && components.Length != 0)
			{
				for (int i = 0; i < components.Length; i++)
				{
					_components.Add(components[i].GetModule());
				}
			}
		}
	}
}
