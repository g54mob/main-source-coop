using System.Threading.Tasks;
using Features.SkinChangeModule.Scripts;
using Features.SkinConfiguration.Scripts;

namespace Features.StoreModule.Scripts
{
	public class ApplySkinStoreReward : StoreRewardBase
	{
		private readonly SkinChangeService _skinChangeService;

		private readonly SkinPartType _skinPartType;

		private readonly SkinType _skinId;

		public override StoreRewardApplyMoment ApplyMoment => StoreRewardApplyMoment.OnPlayerReadyInStore;

		public ApplySkinStoreReward(SkinChangeService skinChangeService, SkinPartType skinPartType, SkinType skinId)
		{
			_skinChangeService = skinChangeService;
			_skinPartType = skinPartType;
			_skinId = skinId;
		}

		public override Task ApplyReward(StoreRewardContext context)
		{
			_skinChangeService.ChangeSkinPart(context.TargetPlayerId, _skinPartType, _skinId);
			return Task.CompletedTask;
		}
	}
}
