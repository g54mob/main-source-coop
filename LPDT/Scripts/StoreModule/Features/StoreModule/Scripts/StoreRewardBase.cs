using System.Threading.Tasks;

namespace Features.StoreModule.Scripts
{
	public abstract class StoreRewardBase
	{
		public abstract StoreRewardApplyMoment ApplyMoment { get; }

		public abstract Task ApplyReward(StoreRewardContext context);
	}
}
