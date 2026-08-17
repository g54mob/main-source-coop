namespace NomadDrive.Features.Interaction.Interactables
{
	public enum StaticHingeState : byte
	{
		Closed = 0,
		Opened = 1,
		LockedClosed = 2,
		StuckClosed = 3,
		LockedAndStuckClosed = 4
	}
}
