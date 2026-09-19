using System;
using System.Collections.Generic;

namespace Features.AIModule.Scripts
{
	[Serializable]
	public class SideBossSpawnData
	{
		public int TimeToSpawn = 30;

		public int RespawnBossTimer = 120;

		public List<SideBossSpawnConfiguration> BossSpawnConfigurations = new List<SideBossSpawnConfiguration>();
	}
}
