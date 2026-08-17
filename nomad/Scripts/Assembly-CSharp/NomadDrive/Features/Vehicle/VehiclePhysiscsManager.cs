using NWH.Common.CoM;
using NWH.VehiclePhysics2;
using UnityEngine;

namespace NomadDrive.Features.Vehicle
{
	public class VehiclePhysiscsManager : MonoBehaviour, IVehiclePhysiscsManager
	{
		[SerializeField]
		private VehicleController vehicleController;

		private void Awake()
		{
			vehicleController = GetComponent<VehicleController>();
		}

		private void OnValidate()
		{
			vehicleController = GetComponent<VehicleController>();
		}

		public void SetMassAffector(float newMass, GameObject affectedObjectSlot)
		{
			affectedObjectSlot.GetComponent<MassAffector>().mass = newMass;
		}

		public void ImpactForceAtPosition(Vector3 forceDirection, Vector3 position, float forceAmount)
		{
			vehicleController.vehicleRigidbody.AddForceAtPosition(forceDirection * forceAmount, position, ForceMode.Impulse);
		}
	}
}
