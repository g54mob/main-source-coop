using System;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class LevelRewardSetting
	{
		public string CardId;

		public int CardPrice;

		public float Weight;

		public int MaxDuplicates = 1;
	}
}
