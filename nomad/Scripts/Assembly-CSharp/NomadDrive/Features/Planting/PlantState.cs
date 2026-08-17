namespace NomadDrive.Features.Planting
{
	public enum PlantState : byte
	{
		Empty = 0,
		Planted = 1,
		Growing = 2,
		NeedsWater = 3,
		FullyGrown = 4
	}
}
