using System;
using System.Collections.Generic;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class StoreByLevelData
	{
		public List<StoreArcData> Arcs;

		public List<LevelRewardSetting> LevelRewardsSettings;
	}
}
