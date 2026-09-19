using System;
using Features.GameUpdaterModule;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_QuotaCollect : BaseTutorialStep
	{
		private readonly BaseTutorialContainerDataHolder _baseTutorialContainerDataHolder;

		private readonly IGameUpdater _gameUpdater;

		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly BaseTutorialLocationContainerDataHolder _baseTutorialLocationContainerDataHolder;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLinePathHandle = GuideLineHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_QuotaCollect(BaseTutorialContainerDataHolder baseTutorialContainerDataHolder, IGameUpdater gameUpdater, TutorialStepsConfiguration tutorialStepsConfiguration, IGuideLineBuildService guideLineBuildService, BaseTutorialLocationContainerDataHolder baseTutorialLocationContainerDataHolder, ITipService tipService)
		{
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
			_gameUpdater = gameUpdater;
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
			_guideLineBuildService = guideLineBuildService;
			_baseTutorialLocationContainerDataHolder = baseTutorialLocationContainerDataHolder;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if (_baseTutorialLocationContainerDataHolder.LocationContainerEntity != null)
			{
				InitializeLocationContainerTrail(_baseTutorialLocationContainerDataHolder.LocationContainerEntity);
			}
			else
			{
				_baseTutorialLocationContainerDataHolder.OnLocationContainerEntityChanged += InitializeLocationContainerTrail;
			}
			_gameUpdater.OnUpdate += Update;
		}

		public override void Dispose()
		{
			_gameUpdater.OnUpdate -= Update;
			_baseTutorialLocationContainerDataHolder.OnLocationContainerEntityChanged -= InitializeLocationContainerTrail;
			if (_baseTutorialLocationContainerDataHolder.LocationContainerEntity != null)
			{
				_baseTutorialLocationContainerDataHolder.LocationContainerEntity.ContentGrabbable.ForceOutline(enable: false);
				_baseTutorialLocationContainerDataHolder.LocationContainerEntity.ContentGrabbable.OnGrab += OnContentGrabbed;
			}
			_guideLineBuildService.StopTrackingPath(_guideLinePathHandle);
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

		private void Update()
		{
			if (!(_baseTutorialContainerDataHolder.ContainerEntity == null) && GetContainerCurrency() >= _tutorialStepsConfiguration.QuotaInContainerThreshold)
			{
				EndStep();
			}
		}

		private ushort GetContainerCurrency()
		{
			ushort num = 0;
			foreach (IPointGrabable item in _baseTutorialContainerDataHolder.ContainerEntity.ContainerItemsGrabber.Items)
			{
				if (item.GameObject.TryGetComponent<MonoItem>(out var component))
				{
					num += component.CurrencyValue;
				}
			}
			return num;
		}

		private void InitializeLocationContainerTrail(IBaseTutorialLocationContainerEntity baseTutorialLocationContainerEntity)
		{
			if (baseTutorialLocationContainerEntity != null)
			{
				_guideLinePathHandle = _guideLineBuildService.StartTrackingPath(_baseTutorialContainerDataHolder.ContainerEntity.ContainerTipPoint, baseTutorialLocationContainerEntity.ContentGrabbable.NetworkObject.transform, GuideLineBuildType.StraightArc, GuideLineUpdateFrequencyType.EveryFrame, () => baseTutorialLocationContainerEntity.ContentGrabbable.NetworkObject == null);
				_grabTipHandle = _tipService.CreateTip(TipType.GrabLeftClick, baseTutorialLocationContainerEntity.ContentGrabbable.NetworkObject.transform, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position, 1f);
				baseTutorialLocationContainerEntity.ContentGrabbable.ForceOutline(enable: true);
				baseTutorialLocationContainerEntity.ContentGrabbable.OnGrab += OnContentGrabbed;
				SetStepPoi(baseTutorialLocationContainerEntity.ContentGrabbable.NetworkObject.transform);
			}
		}

		private void OnContentGrabbed()
		{
			if (_baseTutorialLocationContainerDataHolder.LocationContainerEntity != null)
			{
				_baseTutorialLocationContainerDataHolder.LocationContainerEntity.ContentGrabbable.ForceOutline(enable: false);
				_baseTutorialLocationContainerDataHolder.LocationContainerEntity.ContentGrabbable.OnGrab += OnContentGrabbed;
			}
			_tipService.KillTip(_grabTipHandle);
		}
	}
}
