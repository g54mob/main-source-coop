using EvilCore;

namespace NomadDrive.Features.WorldGeneration
{
	public static class NomadDriveLoadingStates
	{
		public const int TerrainGenerating = 30;

		public const int PoiSpawning = 40;

		public const int RoadBuilding = 50;

		public const int LootSpawning = 70;

		public static void RegisterAll(IGameLoadingManager manager)
		{
			manager.RegisterStateDescription(30, "@loading.creating_world");
			manager.RegisterStateDescription(40, "@loading.spawning_locations");
			manager.RegisterStateDescription(50, "@loading.building_roads");
			manager.RegisterStateDescription(70, "@loading.finalizing");
		}
	}
}
