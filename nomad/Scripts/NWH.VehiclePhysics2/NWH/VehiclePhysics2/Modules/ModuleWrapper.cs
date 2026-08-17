using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules
{
	[Serializable]
	[RequireComponent(typeof(VehicleController))]
	[DefaultExecutionOrder(200)]
	public abstract class ModuleWrapper : MonoBehaviour
	{
		public abstract VehicleComponent GetModule();

		public abstract void SetModule(VehicleComponent vehicleComponent);

		private void Reset()
		{
			InitModule();
			GetModule().VC_SetDefaults();
		}

		private void InitModule()
		{
			VehicleController component = GetComponent<VehicleController>();
			VehicleComponent module = GetModule();
			module.VC_SetVehicleController(component);
			module.VC_LoadStateFromStateSettings();
		}
	}
}
