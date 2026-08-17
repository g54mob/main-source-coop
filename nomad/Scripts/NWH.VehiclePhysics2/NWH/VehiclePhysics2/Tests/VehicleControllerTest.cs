using System.Collections.Generic;
using UnityEngine;

namespace NWH.VehiclePhysics2.Tests
{
	[RequireComponent(typeof(VehicleController))]
	public class VehicleControllerTest : MonoBehaviour
	{
		public VehicleController vehicleController;

		private List<VehicleComponent> components = new List<VehicleComponent>();

		private void Awake()
		{
			vehicleController = GetComponent<VehicleController>();
		}

		private void RandomlyEnableDisableComponent()
		{
			int index = Random.Range(0, components.Count);
			bool num = Random.Range(0f, 1f) > 0.5f;
			VehicleComponent vehicleComponent = components[index];
			if (num)
			{
				Debug.Log("Enable " + vehicleComponent.GetType().Name);
				vehicleComponent.state.lodIndex = 0;
				vehicleComponent.UpdateLOD();
			}
			else
			{
				Debug.Log("Disable " + vehicleComponent.GetType().Name);
				vehicleComponent.state.lodIndex = 3;
				vehicleComponent.UpdateLOD();
			}
		}

		public void RunStateTest()
		{
			components = new List<VehicleComponent>();
			components.Add(vehicleController.steering);
			components.Add(vehicleController.powertrain);
			components.Add(vehicleController.brakes);
			components.Add(vehicleController.groundDetection);
			components.Add(vehicleController.moduleManager);
			components.Add(vehicleController.effectsManager);
			components.AddRange(vehicleController.effectsManager.Components);
			components.Add(vehicleController.soundManager);
			components.AddRange(vehicleController.soundManager.Components);
			InvokeRepeating("RandomlyEnableDisableComponent", 0.05f, 0.2f);
		}

		public void RunTests()
		{
			RunStateTest();
		}

		public void StopTests()
		{
			CancelInvoke("RandomlyEnableDisableComponent");
		}
	}
}
