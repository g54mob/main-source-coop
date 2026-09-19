using System;
using Features.Movement.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_AcquireContainer2 : BaseTutorialStep
	{
		private readonly BaseTutorialContainerDataHolder _baseTutorialContainerDataHolder;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLinePathHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_AcquireContainer2(BaseTutorialContainerDataHolder baseTutorialContainerDataHolder, IGuideLineBuildService guideLineBuildService, PlayerMovableModel playerMovableModel, ITipService tipService)
		{
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
			_guideLineBuildService = guideLineBuildService;
			_playerMovableModel = playerMovableModel;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if (_baseTutorialContainerDataHolder.ContainerEntity != null)
			{
				InitializeContainerGrabTracking(_baseTutorialContainerDataHolder.ContainerEntity);
			}
			else
			{
				_baseTutorialContainerDataHolder.OnContainerPointGrabbableChanged += InitializeContainerGrabTracking;
			}
		}

		public override void Dispose()
		{
			_baseTutorialContainerDataHolder.OnContainerPointGrabbableChanged -= InitializeContainerGrabTracking;
			if (_baseTutorialContainerDataHolder.ContainerEntity != null)
			{
				_baseTutorialContainerDataHolder.ContainerEntity.ContainerNavmeshObstacle.carving = true;
				_baseTutorialContainerDataHolder.ContainerEntity.ContainerItemsGrabber.CartGrabbable.OnGrab -= EndStep;
				_baseTutorialContainerDataHolder.ContainerEntity.ContainerItemsGrabber.CartGrabbable.ForceOutline(enable: false);
			}
			_guideLineBuildService.StopTrackingPath(_guideLinePathHandle);
			_tipService.KillTip(_tipHandle);
			_tipService.KillTip(_grabTipHandle);
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

		private void InitializeContainerGrabTracking(TutorialContainerEntity containerEntity)
		{
			if (!(containerEntity == null))
			{
				containerEntity.ContainerItemsGrabber.CartGrabbable.OnGrab += EndStep;
				_baseTutorialContainerDataHolder.ContainerEntity.ContainerNavmeshObstacle.carving = false;
				_guideLinePathHandle = _guideLineBuildService.StartTrackingPath(containerEntity.transform, _playerMovableModel.LocalMovable.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
				_tipHandle = _tipService.CreateTip(TipType.DestinationPulsing, containerEntity.transform, TipFollowFlags.None, 0.2f, project: true);
				if (_baseTutorialContainerDataHolder.ContainerEntity.TipHolders.TryGetValue(TipType.GrabLeftClick, out var value))
				{
					_grabTipHandle = _tipService.CreateTip(TipType.GrabLeftClick, value, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position);
				}
				containerEntity.ContainerItemsGrabber.CartGrabbable.ForceOutline(enable: true);
				SetStepPoi(containerEntity.ContainerItemsGrabber.transform);
			}
		}
	}
}
