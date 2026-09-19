using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.LevelModule.Scripts;

namespace Features.StoreModule.Scripts
{
	public class StoreStatsModel : ISessionCleanup, ILevelCleanup
	{
		public List<int> InitializedPlayerIds { get; } = new List<int>();

		public void Cleanup()
		{
			InitializedPlayerIds.Clear();
		}
	}
}
