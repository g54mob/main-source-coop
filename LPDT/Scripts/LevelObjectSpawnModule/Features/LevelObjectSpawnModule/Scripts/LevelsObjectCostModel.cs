using System.Collections.Generic;

namespace Features.LevelObjectSpawnModule.Scripts
{
	public class LevelsObjectCostModel
	{
		public int TotalCost { get; private set; }

		public Dictionary<LevelObjectType, int> CostByType { get; private set; } = new Dictionary<LevelObjectType, int>();

		public List<PreparedSpawnItemData> PreparedSpawnItems { get; set; } = new List<PreparedSpawnItemData>();

		public void SetCostData(int totalCost, Dictionary<LevelObjectType, int> costByType)
		{
			TotalCost = totalCost;
			CostByType = costByType ?? new Dictionary<LevelObjectType, int>();
		}

		public void Clear()
		{
			TotalCost = 0;
			CostByType.Clear();
		}
	}
}
