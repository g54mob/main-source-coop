using NWH.VehiclePhysics2;
using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Sandbox.Test
{
	public class VehicleFunctionsTest : MonoBehaviour
	{
		[FormerlySerializedAs("_vehicleController")]
		[SerializeField]
		private VehicleController vehicleController;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				vehicleController.brakes.brakeOffThrottleIntensity = 0f;
			}
		}
	}
}
