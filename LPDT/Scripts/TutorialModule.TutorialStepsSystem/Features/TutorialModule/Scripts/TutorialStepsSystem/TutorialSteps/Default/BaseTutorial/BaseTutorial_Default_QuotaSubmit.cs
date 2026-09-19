using System;
using Features.QuotaModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_QuotaSubmit : BaseTutorialStep
	{
		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private readonly BaseTutorialContainerDataHolder _baseTutorialContainerDataHolder;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly QuotaCompletionModel _quotaCompletionModel;

		private GuideLineHandle _guideLinePathHandle = GuideLineHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_QuotaSubmit(TutorialStepsConfiguration tutorialStepsConfiguration, BaseTutorialContainerDataHolder baseTutorialContainerDataHolder, IGuideLineBuildService guideLineBuildService, QuotaCompletionModel quotaCompletionModel)
		{
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
			_baseTutorialContainerDataHolder = baseTutorialContainerDataHolder;
			_guideLineBuildService = guideLineBuildService;
			_quotaCompletionModel = quotaCompletionModel;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if (_baseTutorialContainerDataHolder.BoatEntity != null)
			{
				InitializeBoatEntity(_baseTutorialContainerDataHolder.BoatEntity);
			}
			else
			{
				_baseTutorialContainerDataHolder.OnBoatEntityChanged += InitializeBoatEntity;
			}
			if (_quotaCompletionModel.IsQuotaCompleted.Value)
			{
				EndStep();
			}
			else
			{
				_quotaCompletionModel.OnQuotaCompleted += OnQuotaCompleted;
			}
		}

		public override void Dispose()
		{
			_baseTutorialContainerDataHolder.OnBoatEntityChanged -= InitializeBoatEntity;
			_quotaCompletionModel.OnQuotaCompleted -= OnQuotaCompleted;
			_guideLineBuildService.StopTrackingPath(_guideLinePathHandle);
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

		private void OnQuotaCompleted(bool isQuotaCompleted)
		{
			if (isQuotaCompleted)
			{
				EndStep();
			}
		}

		private void InitializeBoatEntity(BaseTutorialBoatEntity baseTutorialBoatEntity)
		{
			if (!(baseTutorialBoatEntity == null))
			{
				_guideLinePathHandle = _guideLineBuildService.StartTrackingPath(baseTutorialBoatEntity.QuotaSubmitTip, _baseTutorialContainerDataHolder.ContainerEntity.HandleGrabPoint, GuideLineBuildType.StraightArc, GuideLineUpdateFrequencyType.EveryFrame);
				baseTutorialBoatEntity.QuotaContainerTrigger.IsQuotaCollectBlocked = false;
				SetStepPoi(baseTutorialBoatEntity.QuotaSubmitTip);
			}
		}
	}
}
