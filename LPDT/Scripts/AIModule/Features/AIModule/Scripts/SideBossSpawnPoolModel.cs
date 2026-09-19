using System.Collections.Generic;

namespace Features.AIModule.Scripts
{
	public class SideBossSpawnPoolModel
	{
		public List<SideBossSpawnConfiguration> RemainingPool { get; } = new List<SideBossSpawnConfiguration>();

		public Dictionary<SideBossSpawnConfiguration, float> SpawnWeights { get; } = new Dictionary<SideBossSpawnConfiguration, float>();

		public void Clear()
		{
			RemainingPool.Clear();
			SpawnWeights.Clear();
		}
	}
}
