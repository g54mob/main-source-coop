namespace NomadDrive.Features.Planting
{
	public enum PlantPotInteractionState
	{
		EmptyNoItem = 0,
		EmptyWithSeed = 1,
		PlantedNoWater = 2,
		PlantedWithWater = 3,
		PlantedWithInsufficientWater = 4,
		NeedsWaterNoItem = 5,
		NeedsWaterWithWater = 6,
		NeedsWaterWithInsufficientWater = 7,
		Growing = 8,
		FullyGrown = 9
	}
}
