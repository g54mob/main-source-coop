using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	public class RatsHoleContentEntry
	{
		public NetworkPrefabId PrefabId { get; }

		public LevelObjectType LevelObjectType { get; }

		public ItemType ItemType { get; }

		public ushort CurrencyValue { get; }

		public ushort MaxCurrencyValue { get; }

		public bool IsCollectable { get; }

		public RatsHoleContentEntry(NetworkPrefabId prefabId, LevelObjectType levelObjectType, ItemType itemType, ushort currencyValue, ushort maxCurrencyValue, bool isCollectable)
		{
			PrefabId = prefabId;
			LevelObjectType = levelObjectType;
			ItemType = itemType;
			CurrencyValue = currencyValue;
			MaxCurrencyValue = maxCurrencyValue;
			IsCollectable = isCollectable;
		}

		public RatsHoleContentSlot ToSlot()
		{
			return new RatsHoleContentSlot
			{
				PrefabId = PrefabId,
				LevelObjectType = LevelObjectType,
				ItemType = ItemType,
				CurrencyValue = CurrencyValue,
				MaxCurrencyValue = MaxCurrencyValue,
				IsCollectable = IsCollectable
			};
		}
	}
}
