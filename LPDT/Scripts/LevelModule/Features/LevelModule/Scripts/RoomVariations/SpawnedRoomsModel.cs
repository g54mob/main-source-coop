namespace Features.LevelModule.Scripts.RoomVariations
{
	public class SpawnedRoomsModel
	{
		public int? RoomSpawnTasksCount { get; set; }

		public int SpawnedRoomsCount { get; set; }

		public bool IsAllTasksCompleted
		{
			get
			{
				if (RoomSpawnTasksCount.HasValue)
				{
					return RoomSpawnTasksCount.Value == SpawnedRoomsCount;
				}
				return false;
			}
		}

		public void Reset()
		{
			RoomSpawnTasksCount = null;
			SpawnedRoomsCount = 0;
		}
	}
}
