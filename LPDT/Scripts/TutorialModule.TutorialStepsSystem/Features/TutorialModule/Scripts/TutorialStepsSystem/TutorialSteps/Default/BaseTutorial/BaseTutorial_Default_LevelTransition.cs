using System;
using Features.QuotaModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_LevelTransition : BaseTutorialStep
	{
		private readonly BaseTutorialBellDataHolder _baseTutorialBellDataHolder;

		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLinePathHandle = GuideLineHandle.Invalid;

		private TipHandle _waveTipHandle = TipHandle.Invalid;

		private TipHandle _leftRightTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_LevelTransition(BaseTutorialBellDataHolder baseTutorialBellDataHolder, QuotaCompletionModel quotaCompletionModel, ITipService tipService)
		{
			_baseTutorialBellDataHolder = baseTutorialBellDataHolder;
			_quotaCompletionModel = quotaCompletionModel;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if (_quotaCompletionModel.IsBellActivated.Value)
			{
				EndStep();
			}
			else
			{
				_quotaCompletionModel.OnBellActivated += OnBellActivated;
			}
			if (_baseTutorialBellDataHolder.BellEntity != null)
			{
				InitializeBellEntity(_baseTutorialBellDataHolder.BellEntity);
			}
			else
			{
				_baseTutorialBellDataHolder.OnBellEntityChanged += InitializeBellEntity;
			}
		}

		public override void Dispose()
		{
			_quotaCompletionModel.OnBellActivated -= OnBellActivated;
			_baseTutorialBellDataHolder.OnBellEntityChanged -= InitializeBellEntity;
			_tipService.KillTip(_waveTipHandle);
			_tipService.KillTip(_leftRightTipHandle);
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

		private void OnBellActivated(bool isActivated)
		{
			if (isActivated)
			{
				EndStep();
			}
		}

		private void InitializeBellEntity(BaseTutorialBellEntity baseTutorialBellEntity)
		{
			if (!(baseTutorialBellEntity == null))
			{
				if (baseTutorialBellEntity.TipHolders.TryGetValue(TipType.LeftRightWave, out var value))
				{
					_waveTipHandle = _tipService.CreateTip(TipType.LeftRightWave, value, TipFollowFlags.Position | TipFollowFlags.Rotation | TipFollowFlags.FaceCameraY);
				}
				if (baseTutorialBellEntity.TipHolders.TryGetValue(TipType.LeftRightMouseMovement, out var value2))
				{
					_leftRightTipHandle = _tipService.CreateTip(TipType.LeftRightMouseMovement, value2, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position);
				}
				SetStepPoi(_baseTutorialBellDataHolder.BellEntity.ClapperGrabbable.transform);
			}
		}
	}
}
