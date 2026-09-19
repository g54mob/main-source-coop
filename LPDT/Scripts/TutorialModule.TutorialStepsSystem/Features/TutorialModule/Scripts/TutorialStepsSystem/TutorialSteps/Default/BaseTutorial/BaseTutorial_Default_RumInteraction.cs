using Features.GrabModule.Scripts;
using Features.RumModule.Scripts;
using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using Features.TutorialModule.Scripts.UITipsModule;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_RumInteraction : BaseTutorial_Default_RewardBase
	{
		private readonly IUITipService _uiTipService;

		private readonly BaseTutorialUIDataHolder _baseTutorialUIDataHolder;

		private RumController _cachedRumController;

		private IPointGrabable _cachedRewardGrabbable;

		private UITipHandle _mouseUpUITipHandle = UITipHandle.Invalid;

		private UITipHandle _interactUITipHandle = UITipHandle.Invalid;

		public BaseTutorial_Default_RumInteraction(SpawnedRewardsModel spawnedRewardsModel, BaseTutorialChosenRewardDataHolder baseTutorialChosenRewardDataHolder, TutorialStepsConfiguration tutorialStepsConfiguration, IUITipService uiTipService, BaseTutorialUIDataHolder baseTutorialUIDataHolder)
			: base(spawnedRewardsModel, baseTutorialChosenRewardDataHolder, tutorialStepsConfiguration)
		{
			_uiTipService = uiTipService;
			_baseTutorialUIDataHolder = baseTutorialUIDataHolder;
		}

		protected override void OnRewardEnsured()
		{
			base.OnRewardEnsured();
			BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardNetworkObject.TryGetComponent<RumController>(out var component);
			_cachedRumController = component;
			if (_cachedRumController.IsRewardApplied)
			{
				EndStep();
				return;
			}
			_cachedRumController.OnIsRewardAppliedChanged += OnIsRewardAppliedChanged;
			_cachedRewardGrabbable = BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardGrabbable;
			_cachedRewardGrabbable.OnGrab += RefreshTips;
			_cachedRewardGrabbable.OnUnGrab += HideTips;
			_cachedRumController.RumBottleInteractable.OnToggleChanged += RefreshTips;
			if (_cachedRewardGrabbable.GrabbedByPlayersCount > 0)
			{
				RefreshTips();
			}
			SetStepPoi(BaseTutorialChosenRewardDataHolder.ChosenReward.SpawnedRewardNetworkObject.transform);
		}

		public override void Dispose()
		{
			_cachedRumController.OnIsRewardAppliedChanged -= OnIsRewardAppliedChanged;
			_cachedRewardGrabbable.OnGrab -= RefreshTips;
			_cachedRewardGrabbable.OnUnGrab -= HideTips;
			_cachedRumController.RumBottleInteractable.OnToggleChanged -= RefreshTips;
			_uiTipService.KillTip(_mouseUpUITipHandle);
			_uiTipService.KillTip(_interactUITipHandle);
		}

		private void RefreshTips()
		{
			if (_cachedRewardGrabbable.GrabbedByPlayersCount <= 0)
			{
				HideTips();
			}
			else if (_cachedRumController.RumBottleInteractable.IsToggledOn)
			{
				KillTip(ref _interactUITipHandle);
				ShowTip(ref _mouseUpUITipHandle, UITipType.MouseUp);
			}
			else
			{
				KillTip(ref _mouseUpUITipHandle);
				ShowTip(ref _interactUITipHandle, UITipType.InteractRightClick);
			}
		}

		private void HideTips()
		{
			KillTip(ref _mouseUpUITipHandle);
			KillTip(ref _interactUITipHandle);
		}

		private void ShowTip(ref UITipHandle uiTipHandle, UITipType uiTipType)
		{
			if (!uiTipHandle.IsValid)
			{
				uiTipHandle = _uiTipService.CreateTip(uiTipType, _baseTutorialUIDataHolder.MouseUpTipHolder.anchoredPosition);
			}
		}

		private void KillTip(ref UITipHandle uiTipHandle)
		{
			_uiTipService.KillTip(uiTipHandle);
			uiTipHandle = UITipHandle.Invalid;
		}

		private void OnIsRewardAppliedChanged(bool isRewardApplied)
		{
			if (isRewardApplied)
			{
				EndStep();
			}
		}
	}
}
