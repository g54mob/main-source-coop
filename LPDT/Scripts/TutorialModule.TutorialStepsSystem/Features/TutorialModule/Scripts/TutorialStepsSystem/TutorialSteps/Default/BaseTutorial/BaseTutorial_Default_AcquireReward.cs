using Features.Movement.Scripts;
using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_AcquireReward : BaseTutorial_Default_RewardBase
	{
		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLineHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public BaseTutorial_Default_AcquireReward(SpawnedRewardsModel spawnedRewardsModel, BaseTutorialChosenRewardDataHolder baseTutorialChosenRewardDataHolder, TutorialStepsConfiguration tutorialStepsConfiguration, IGuideLineBuildService guideLineBuildService, PlayerMovableModel playerMovableModel, ITipService tipService)
			: base(spawnedRewardsModel, baseTutorialChosenRewardDataHolder, tutorialStepsConfiguration)
		{
			_guideLineBuildService = guideLineBuildService;
			_playerMovableModel = playerMovableModel;
			_tipService = tipService;
		}

		protected override void OnRewardEnsured()
		{
			base.OnRewardEnsured();
			BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardGrabbable.OnGrab += base.EndStep;
			BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardGrabbable.ForceOutline(enable: true);
			_guideLineHandle = _guideLineBuildService.StartTrackingPath(BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardNetworkObject.transform, _playerMovableModel.LocalMovable.Rigidbody.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
			_tipHandle = _tipService.CreateTip(TipType.Arrow, BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardGrabbable.GameObject.transform, TipFollowFlags.Position, 1.5f);
			_grabTipHandle = _tipService.CreateTip(TipType.GrabLeftClick, BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardGrabbable.GameObject.transform, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position, 0.3f);
			SetStepPoi(BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardGrabbable.Rigidbody.transform);
		}

		public override void Dispose()
		{
			BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardGrabbable.OnGrab -= base.EndStep;
			BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardGrabbable.ForceOutline(enable: false);
			_guideLineBuildService.StopTrackingPath(_guideLineHandle);
			_tipService.KillTip(_tipHandle);
			_tipService.KillTip(_grabTipHandle);
		}
	}
}
