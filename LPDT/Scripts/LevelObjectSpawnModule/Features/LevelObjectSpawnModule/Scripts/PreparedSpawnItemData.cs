using System.Collections.Generic;
using Features.ItemsModule.Scripts;

namespace Features.LevelObjectSpawnModule.Scripts
{
	public class PreparedSpawnItemData
	{
		public LevelObjectSpawnPointData SpawnPoint { get; }

		public LevelObjectData LevelObjectData { get; }

		public List<ItemData> ItemDataList { get; }

		public PreparedSpawnItemData(LevelObjectSpawnPointData spawnPoint, LevelObjectData levelObjectData, List<ItemData> itemDataList)
		{
			SpawnPoint = spawnPoint;
			LevelObjectData = levelObjectData;
			ItemDataList = itemDataList;
		}
	}
}
