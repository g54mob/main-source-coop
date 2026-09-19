using System.Threading.Tasks;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;

namespace Features.StoreModule.Scripts
{
	public class SpawnMonkeyPorterStoreReward : StoreRewardBase
	{
		private readonly MonkeyPorterRunModel _monkeyPorterRunModel;

		public override StoreRewardApplyMoment ApplyMoment => StoreRewardApplyMoment.OnPlayerReadyInStore;

		public SpawnMonkeyPorterStoreReward(MonkeyPorterRunModel monkeyPorterRunModel)
		{
			_monkeyPorterRunModel = monkeyPorterRunModel;
		}

		public override Task ApplyReward(StoreRewardContext context)
		{
			_monkeyPorterRunModel.IsOwned.Value = true;
			return Task.CompletedTask;
		}
	}
}
