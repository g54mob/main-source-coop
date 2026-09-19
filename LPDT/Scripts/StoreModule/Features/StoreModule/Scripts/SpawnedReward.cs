using Features.GrabModule.Scripts;
using Fusion;

namespace Features.StoreModule.Scripts
{
	public class SpawnedReward
	{
		public NetworkObject SpawnedRewardNetworkObject { get; set; }

		public IPointGrabable SpawnedRewardGrabbable { get; set; }

		public CardItemType SpawnedRewardType { get; set; }
	}
}
