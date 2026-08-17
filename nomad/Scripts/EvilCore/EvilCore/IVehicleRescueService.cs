namespace EvilCore
{
	public interface IVehicleRescueService
	{
		bool CanRescueVehicle { get; }

		void RequestRescueVehicle();
	}
}
