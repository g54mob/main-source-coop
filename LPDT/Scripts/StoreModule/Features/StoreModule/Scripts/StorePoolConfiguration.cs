using System.Collections.Generic;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	[CreateAssetMenu(fileName = "StorePoolConfiguration_Default", menuName = "Configurations/StoreModule/StorePoolConfiguration")]
	public class StorePoolConfiguration : ScriptableObject
	{
		public const int DEAD_PLAYER_CARD_ID = -1;

		public const int HEAL_POTION_CARD_ID = -2;

		public StoreCardBehaviour StoreCardPrefab;

		public StoreTableBehaviour StoreTablePrefab;

		[field: SerializeField]
		public List<StoreCardData> RewardPoolWithChances { get; private set; }

		[field: SerializeField]
		public StoreCardData DeadPlayerStoreCardData { get; private set; }

		[field: SerializeField]
		public StoreCardData HealPotionStoreCardData { get; private set; }

		public StoreCardData ResolveCardData(int cardDataId)
		{
			if (cardDataId == -1)
			{
				return DeadPlayerStoreCardData;
			}
			if (cardDataId == -2)
			{
				return HealPotionStoreCardData;
			}
			if (cardDataId >= 0 && RewardPoolWithChances != null && cardDataId < RewardPoolWithChances.Count)
			{
				return RewardPoolWithChances[cardDataId];
			}
			return null;
		}
	}
}
