namespace NomadDrive.Features.Interaction.Interactables
{
	public enum StaticDoorState : byte
	{
		Closed = 0,
		Open = 1,
		LockedClosed = 2,
		StuckClosed = 3,
		LockedAndStuckClosed = 4
	}
}
