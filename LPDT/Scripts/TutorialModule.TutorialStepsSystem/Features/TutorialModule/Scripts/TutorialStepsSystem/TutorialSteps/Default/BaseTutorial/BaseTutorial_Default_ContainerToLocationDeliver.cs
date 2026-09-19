using System;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_ContainerToLocationDeliver : BaseTutorialStep
	{
		private readonly BaseTutorialTargetTrackerDataHolder _baseTutorialTargetTrackerDataHolder;

		private readonly BaseTutorialTargetEventClass _baseTutorialTargetEventClass;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly BaseTutorialContainerDataHolder _baseTutorialContainerDataHolder;

		private readonly ITipService _tipService;

		private GuideLineHandle _pathHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _cartPushTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_ContainerToLocationDeliver(BaseTutorialTargetTrackerDataHolder baseTutorialTargetTrackerDataHolder, BaseTutorialTargetEventClass baseTutorialTargetEventClass, IGuideLineBuildService guideLineBuildService, BaseTutorialContainerDataHolder baseTutorialContainerDataHolder, ITipService tipService)
		{
			_baseTutorialTargetTrackerDataHolder = baseTutorialTargetTrackerDataHolder;
			_baseTutorialTargetEventClass = baseTutorialTargetEventClass;
			_guideLineBuildService = guideLineBuildService;
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if (_baseTutorialTargetTrackerDataHolder.TargetReachTrackers.TryGetValue(BaseTutorialTargetType.ContainerDeliver, out var value))
			{
				InitializeContainerDeliverTracker(value);
			}
			else
			{
				_baseTutorialTargetTrackerDataHolder.OnTargetReachTrackerAdded += InitializeContainerDeliverTracker;
			}
			_baseTutorialTargetEventClass.OnTargetReached += OnTargetReached;
			_baseTutorialContainerDataHolder.ContainerEntity.ContainerNavmeshObstacle.carving = false;
		}

		public override void Dispose()
		{
			_baseTutorialTargetTrackerDataHolder.OnTargetReachTrackerAdded -= InitializeContainerDeliverTracker;
			_baseTutorialTargetEventClass.OnTargetReached -= OnTargetReached;
			if (_baseTutorialTargetTrackerDataHolder.TargetReachTrackers.TryGetValue(BaseTutorialTargetType.ContainerDeliver, out var value))
			{
				value.Enabled = false;
			}
			_guideLineBuildService.StopTrackingPath(_pathHandle);
			_baseTutorialContainerDataHolder.ContainerEntity.ContainerNavmeshObstacle.carving = true;
			_tipService.KillTip(_tipHandle);
			_tipService.KillTip(_cartPushTipHandle);
		}

		private void EndStep()
		{
			OnStepEnd?.Invoke();
		}

		public override void DeActivateStep()
		{
			Dispose();
		}

		public override void SkipStep()
		{
			OnStepEnd?.Invoke();
		}

		public override void RevertStep()
		{
		}

		private void InitializeContainerDeliverTracker(BaseTutorialTargetReachTracker targetReachTracker)
		{
			if (targetReachTracker.TargetType == BaseTutorialTargetType.ContainerDeliver)
			{
				targetReachTracker.Enabled = true;
				_pathHandle = _guideLineBuildService.StartTrackingPath(targetReachTracker.transform, _baseTutorialContainerDataHolder.ContainerEntity.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
				_tipHandle = _tipService.CreateTip(TipType.DestinationPulsing, targetReachTracker.transform, TipFollowFlags.None, 0.2f, project: true);
				if (_baseTutorialContainerDataHolder.ContainerEntity.TipHolders.TryGetValue(TipType.CartPush, out var value))
				{
					_cartPushTipHandle = _tipService.CreateTip(TipType.CartPush, value, TipFollowFlags.Position | TipFollowFlags.Rotation);
				}
				SetStepPoi(targetReachTracker.transform);
			}
		}

		private void OnTargetReached(BaseTutorialTargetType baseTutorialTargetType)
		{
			if (baseTutorialTargetType == BaseTutorialTargetType.ContainerDeliver)
			{
				EndStep();
			}
		}
	}
}
