namespace NomadDrive.Features.Vehicle.UI
{
	public readonly struct DrivingActionEntry
	{
		public readonly string InputId;

		public readonly bool Enabled;

		public DrivingActionEntry(string inputId, bool enabled)
		{
			InputId = inputId;
			Enabled = enabled;
		}
	}
}
