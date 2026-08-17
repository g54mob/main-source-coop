namespace NomadDrive.Features.SaveSystem
{
	public enum SaveLinkKind : byte
	{
		None = 0,
		EquipmentHand = 1,
		ObjectSlot = 2,
		VehicleSlot = 3,
		SnappingPlane = 4,
		NetworkedTransformChild = 5,
		PlantPotHarvest = 6,
		SeatOccupant = 7
	}
}
