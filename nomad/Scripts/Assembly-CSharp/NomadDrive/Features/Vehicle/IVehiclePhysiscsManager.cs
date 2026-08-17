using UnityEngine;

namespace NomadDrive.Features.Vehicle
{
	public interface IVehiclePhysiscsManager
	{
		void ImpactForceAtPosition(Vector3 forceDirection, Vector3 position, float forceAmount);

		void SetMassAffector(float newMass, GameObject affectedObjectSlot);
	}
}
