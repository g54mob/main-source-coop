using System.Threading.Tasks;
using Features.CartUpgradesModule.Scripts.Core;

namespace Features.StoreModule.Scripts
{
	public class CartUpgradeStoreReward : StoreRewardBase
	{
		private readonly ICartUpgradeService _cartUpgradeService;

		private readonly CartUpgradeModule _cartUpgradeModule;

		public override StoreRewardApplyMoment ApplyMoment => StoreRewardApplyMoment.OnPlayerReadyInStore;

		public CartUpgradeStoreReward(ICartUpgradeService cartUpgradeService, CartUpgradeModule cartUpgradeModule)
		{
			_cartUpgradeService = cartUpgradeService;
			_cartUpgradeModule = cartUpgradeModule;
		}

		public override Task ApplyReward(StoreRewardContext context)
		{
			_cartUpgradeService.ApplyModule(_cartUpgradeModule);
			return Task.CompletedTask;
		}
	}
}
