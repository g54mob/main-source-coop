using NWH.VehiclePhysics2.SetupWizard;
using UnityEngine;

namespace NWH.VehiclePhysics2.Tests
{
	public class VehicleSetupWizardTest : MonoBehaviour
	{
		private void Start()
		{
			VehicleSetupWizard component = GetComponent<VehicleSetupWizard>();
			if (component != null)
			{
				VehicleController vehicleController = VehicleSetupWizard.RunSetup(component.gameObject, component.wheelGameObjects, component.wheelControllerType);
				if (vehicleController != null)
				{
					VehicleSetupWizard.RunConfiguration(vehicleController, component.preset);
				}
				Object.Destroy(this);
				Object.Destroy(component);
			}
			else
			{
				Debug.LogWarning("VehicleSetupWizard does not exist.");
			}
		}
	}
}
