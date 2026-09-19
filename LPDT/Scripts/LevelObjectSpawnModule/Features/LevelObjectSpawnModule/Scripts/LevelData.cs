using System;
using System.Collections.Generic;

namespace Features.LevelObjectSpawnModule.Scripts
{
	[Serializable]
	public class LevelData
	{
		public LevelObjects LevelObjects;

		public List<LevelObjectsSpawnData> LevelObjectsSpawnPoints;
	}
}
