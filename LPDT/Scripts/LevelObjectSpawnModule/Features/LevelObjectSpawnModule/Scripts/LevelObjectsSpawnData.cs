using System;

namespace Features.LevelObjectSpawnModule.Scripts
{
	[Serializable]
	public class LevelObjectsSpawnData
	{
		public LevelObjectType ObjectType;

		public int MinObjectCount;

		public int MaxObjectCount;
	}
}
