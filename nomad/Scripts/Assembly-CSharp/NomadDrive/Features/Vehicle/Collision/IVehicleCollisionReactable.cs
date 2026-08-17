namespace NomadDrive.Features.Vehicle.Collision
{
	public interface IVehicleCollisionReactable
	{
		void OnHitByVehicle(VehicleCollisionData data);
	}
}
