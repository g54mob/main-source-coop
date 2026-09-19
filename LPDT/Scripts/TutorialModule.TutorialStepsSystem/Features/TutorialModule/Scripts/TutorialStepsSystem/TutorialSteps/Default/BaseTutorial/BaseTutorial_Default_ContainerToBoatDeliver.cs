using System;
using Features.HingeModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_ContainerToBoatDeliver : BaseTutorialStep
	{
		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly BaseTutorialContainerDataHolder _baseTutorialContainerDataHolder;

		private readonly ITipService _tipService;

		private GuideLineHandle _pathHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _cartPushTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_ContainerToBoatDeliver(IGuideLineBuildService guideLineBuildService, BaseTutorialContainerDataHolder baseTutorialContainerDataHolder, ITipService tipService)
		{
			_guideLineBuildService = guideLineBuildService;
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if (_baseTutorialContainerDataHolder.ContainerEntity != null)
			{
				InitializeContainerToBoatDeliver(_baseTutorialContainerDataHolder.ContainerEntity);
			}
			else
			{
				_baseTutorialContainerDataHolder.OnContainerPointGrabbableChanged += InitializeContainerToBoatDeliver;
			}
			_baseTutorialContainerDataHolder.ContainerEntity.ContainerNavmeshObstacle.carving = false;
		}

		public override void Dispose()
		{
			_baseTutorialContainerDataHolder.OnContainerPointGrabbableChanged -= InitializeContainerToBoatDeliver;
			_guideLineBuildService.StopTrackingPath(_pathHandle);
			if (_baseTutorialContainerDataHolder.ContainerEntity != null)
			{
				foreach (BoatHingeControllerData boatHingeControllersDatum in _baseTutorialContainerDataHolder.ContainerEntity.BoatHingeControllersData)
				{
					boatHingeControllersDatum.OnHingeConnectedChanged -= OnContainerHingeConnectedChanged;
				}
				_baseTutorialContainerDataHolder.ContainerEntity.ContainerNavmeshObstacle.carving = true;
			}
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

		private void InitializeContainerToBoatDeliver(TutorialContainerEntity tutorialContainerEntity)
		{
			if (tutorialContainerEntity == null)
			{
				return;
			}
			tutorialContainerEntity.ContainerNavmeshObstacle.carving = false;
			foreach (BoatHingeControllerData boatHingeControllersDatum in tutorialContainerEntity.BoatHingeControllersData)
			{
				if (boatHingeControllersDatum.IsHingeConnected)
				{
					EndStep();
					return;
				}
				boatHingeControllersDatum.OnHingeConnectedChanged += OnContainerHingeConnectedChanged;
			}
			_pathHandle = _guideLineBuildService.StartTrackingPath(_baseTutorialContainerDataHolder.BoatEntity.HingeController, _baseTutorialContainerDataHolder.ContainerEntity.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
			_tipHandle = _tipService.CreateTip(TipType.DestinationPulsing, _baseTutorialContainerDataHolder.BoatEntity.HingeController, TipFollowFlags.None, 0.2f, project: true);
			if (_baseTutorialContainerDataHolder.ContainerEntity.TipHolders.TryGetValue(TipType.CartPush, out var value))
			{
				_cartPushTipHandle = _tipService.CreateTip(TipType.CartPush, value, TipFollowFlags.Position | TipFollowFlags.Rotation);
			}
			SetStepPoi(_baseTutorialContainerDataHolder.BoatEntity.HingeController);
		}

		private void OnContainerHingeConnectedChanged(bool isConnected)
		{
			if (isConnected)
			{
				EndStep();
			}
		}
	}
}
