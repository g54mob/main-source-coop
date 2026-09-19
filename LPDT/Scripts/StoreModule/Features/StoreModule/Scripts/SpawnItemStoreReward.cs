using System.Threading.Tasks;
using Fusion;

namespace Features.StoreModule.Scripts
{
	public class SpawnItemStoreReward : StoreRewardBase
	{
		private readonly ICardItemSpawner _cardItemSpawner;

		private readonly NetworkBehaviour _rewardItemPrefab;

		public override StoreRewardApplyMoment ApplyMoment => StoreRewardApplyMoment.OnMoveToBeach;

		public SpawnItemStoreReward(ICardItemSpawner cardItemSpawner, NetworkBehaviour rewardItemPrefab)
		{
			_cardItemSpawner = cardItemSpawner;
			_rewardItemPrefab = rewardItemPrefab;
		}

		public override async Task ApplyReward(StoreRewardContext context)
		{
			if (!(_rewardItemPrefab == null) && context.CardData != null)
			{
				await _cardItemSpawner.Spawn(context.CardData, context.SpawnPosition, context.SpawnRotation, context.SpawnColor);
			}
		}
	}
}
