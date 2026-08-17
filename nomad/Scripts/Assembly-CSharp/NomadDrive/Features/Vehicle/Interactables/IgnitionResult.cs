namespace NomadDrive.Features.Vehicle.Interactables
{
	public enum IgnitionResult
	{
		Success = 0,
		EngineNotInstalled = 1,
		BatteryNotInstalled = 2,
		EngineBroken = 3,
		BatteryBroken = 4,
		EngineOverheated = 5,
		FuelEmpty = 6,
		HandbrakeEngaged = 7
	}
}
