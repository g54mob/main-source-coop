using System;
using Features.GameUpdaterModule;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_LocationContainerInteraction : BaseTutorialStep
	{
		private readonly BaseTutorialLocationContainerDataHolder _baseTutorialContainerDataHolder;

		private readonly IGameUpdater _gameUpdater;

		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private readonly ITipService _tipService;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_LocationContainerInteraction(BaseTutorialLocationContainerDataHolder baseTutorialContainerDataHolder, IGameUpdater gameUpdater, TutorialStepsConfiguration tutorialStepsConfiguration, ITipService tipService)
		{
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
			_gameUpdater = gameUpdater;
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			_gameUpdater.OnUpdate += Update;
			if (_baseTutorialContainerDataHolder.LocationContainerEntity != null)
			{
				InitializeContainerTip(_baseTutorialContainerDataHolder.LocationContainerEntity);
			}
			else
			{
				_baseTutorialContainerDataHolder.OnLocationContainerEntityChanged += InitializeContainerTip;
			}
		}

		public override void Dispose()
		{
			_gameUpdater.OnUpdate -= Update;
			_baseTutorialContainerDataHolder.OnLocationContainerEntityChanged -= InitializeContainerTip;
			if (_baseTutorialContainerDataHolder.LocationContainerEntity != null)
			{
				_baseTutorialContainerDataHolder.LocationContainerEntity.DoorGrabbable.OnGrab -= OnDoorGrabbed;
				_baseTutorialContainerDataHolder.LocationContainerEntity.DoorGrabbable.ForceOutline(enable: false);
			}
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

		private void Update()
		{
			if (_baseTutorialContainerDataHolder.LocationContainerEntity != null && _baseTutorialContainerDataHolder.LocationContainerEntity.OpenAngle >= _tutorialStepsConfiguration.LocationContainerOpenThreshold)
			{
				EndStep();
			}
		}

		private void InitializeContainerTip(IBaseTutorialLocationContainerEntity baseTutorialLocationContainerEntity)
		{
			if (baseTutorialLocationContainerEntity != null)
			{
				if (baseTutorialLocationContainerEntity.TipHolders.TryGetValue(TipType.Arrow, out var value))
				{
					_tipHandle = _tipService.CreateTip(TipType.Arrow, value, TipFollowFlags.Position | TipFollowFlags.Rotation);
				}
				if (baseTutorialLocationContainerEntity.TipHolders.TryGetValue(TipType.GrabLeftClick, out var value2))
				{
					_grabTipHandle = _tipService.CreateTip(TipType.GrabLeftClick, value2, TipFollowFlags.Position | TipFollowFlags.Rotation);
				}
				baseTutorialLocationContainerEntity.DoorGrabbable.ForceOutline(enable: true);
				baseTutorialLocationContainerEntity.DoorGrabbable.OnGrab += OnDoorGrabbed;
				SetStepPoi(_baseTutorialContainerDataHolder.LocationContainerEntity.ContentGrabbable.Rigidbody.transform);
			}
		}

		private void OnDoorGrabbed()
		{
			if (_baseTutorialContainerDataHolder.LocationContainerEntity != null)
			{
				_baseTutorialContainerDataHolder.LocationContainerEntity.DoorGrabbable.OnGrab -= OnDoorGrabbed;
				_baseTutorialContainerDataHolder.LocationContainerEntity.DoorGrabbable.ForceOutline(enable: false);
			}
			_tipService.KillTip(_grabTipHandle);
		}
	}
}
