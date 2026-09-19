using System;
using Fusion;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class StoreCardData
	{
		[SerializeReference]
		public StoreRewardDataBase RewardData;

		public string Id;

		public Sprite CardSprite;

		public Sprite CardMask;

		public bool RandomizeColor;

		public StoreRewardDataBase GetRewardData()
		{
			return RewardData;
		}

		public NetworkBehaviour GetRewardItemPrefab()
		{
			if (RewardData is SpawnItemStoreRewardData spawnItemStoreRewardData)
			{
				return spawnItemStoreRewardData.RewardItemPrefab;
			}
			return null;
		}

		public CardItemType GetRewardCardItemType()
		{
			if (RewardData == null)
			{
				return CardItemType.None;
			}
			return RewardData.GetCardItemType();
		}
	}
}
