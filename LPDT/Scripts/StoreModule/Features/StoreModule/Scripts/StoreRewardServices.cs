using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.CartUpgradesModule.Scripts.Core;
using Features.SkinChangeModule.Scripts;

namespace Features.StoreModule.Scripts
{
	public class StoreRewardServices
	{
		public ICardItemSpawner CardItemSpawner { get; }

		public SkinChangeService SkinChangeService { get; }

		public ICartUpgradeService CartUpgradeService { get; }

		public MonkeyPorterRunModel MonkeyPorterRunModel { get; }

		public StoreRewardServices(ICardItemSpawner cardItemSpawner, SkinChangeService skinChangeService, ICartUpgradeService cartUpgradeService, MonkeyPorterRunModel monkeyPorterRunModel)
		{
			CardItemSpawner = cardItemSpawner;
			SkinChangeService = skinChangeService;
			CartUpgradeService = cartUpgradeService;
			MonkeyPorterRunModel = monkeyPorterRunModel;
		}
	}
}
