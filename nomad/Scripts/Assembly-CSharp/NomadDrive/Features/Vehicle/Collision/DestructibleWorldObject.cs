using UnityEngine;

namespace NomadDrive.Features.Vehicle.Collision
{
	public class DestructibleWorldObject : MonoBehaviour, IVehicleCollisionReactable
	{
		[SerializeField]
		private float minimumForceToDestroy = 2000f;

		public void OnHitByVehicle(VehicleCollisionData data)
		{
			if (data.Force >= minimumForceToDestroy)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
