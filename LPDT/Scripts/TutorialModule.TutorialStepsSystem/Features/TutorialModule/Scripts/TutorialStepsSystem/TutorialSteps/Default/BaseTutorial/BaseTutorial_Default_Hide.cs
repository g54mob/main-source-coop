using System;
using Features.Movement.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using Features.TutorialModule.Scripts.UITipsModule;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_Hide : BaseTutorialStep
	{
		private readonly BaseTutorialTargetTrackerDataHolder _baseTutorialTargetTrackerDataHolder;

		private readonly BaseTutorialTargetEventClass _baseTutorialTargetEventClass;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly ITipService _tipService;

		private readonly IUITipService _uiTipService;

		private readonly BaseTutorialUIDataHolder _baseTutorialUIDataHolder;

		private GuideLineHandle _guideLinePathHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private UITipHandle _ctrlTipHandle = UITipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_Hide(BaseTutorialTargetTrackerDataHolder baseTutorialTargetTrackerDataHolder, BaseTutorialTargetEventClass baseTutorialTargetEventClass, IGuideLineBuildService guideLineBuildService, PlayerMovableModel playerMovableModel, ITipService tipService, IUITipService uiTipService, BaseTutorialUIDataHolder baseTutorialUIDataHolder)
		{
			_baseTutorialTargetTrackerDataHolder = baseTutorialTargetTrackerDataHolder;
			_baseTutorialTargetEventClass = baseTutorialTargetEventClass;
			_guideLineBuildService = guideLineBuildService;
			_playerMovableModel = playerMovableModel;
			_tipService = tipService;
			_uiTipService = uiTipService;
			_baseTutorialUIDataHolder = baseTutorialUIDataHolder;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			_baseTutorialTargetEventClass.OnTargetReached += OnTargetReached;
			if (_baseTutorialTargetTrackerDataHolder.TargetReachTrackers.TryGetValue(BaseTutorialTargetType.PlayerHide, out var value))
			{
				InitializeContainerDeliverTracker(value);
			}
			else
			{
				_baseTutorialTargetTrackerDataHolder.OnTargetReachTrackerAdded += InitializeContainerDeliverTracker;
			}
			if (_baseTutorialUIDataHolder.CtrlTipHolder != null)
			{
				InitializeUITips(_baseTutorialUIDataHolder.CtrlTipHolder);
			}
			else
			{
				_baseTutorialUIDataHolder.OnCtrlTipHolderChanged += InitializeUITips;
			}
		}

		public override void Dispose()
		{
			_baseTutorialTargetTrackerDataHolder.OnTargetReachTrackerAdded -= InitializeContainerDeliverTracker;
			_baseTutorialTargetEventClass.OnTargetReached -= OnTargetReached;
			_baseTutorialUIDataHolder.OnCtrlTipHolderChanged -= InitializeUITips;
			if (_baseTutorialTargetTrackerDataHolder.TargetReachTrackers.TryGetValue(BaseTutorialTargetType.PlayerHide, out var value))
			{
				value.Enabled = false;
			}
			_guideLineBuildService.StopTrackingPath(_guideLinePathHandle);
			_tipService.KillTip(_tipHandle);
			_uiTipService.KillTip(_ctrlTipHandle);
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
			if (targetReachTracker.TargetType == BaseTutorialTargetType.PlayerHide)
			{
				targetReachTracker.Enabled = true;
				_guideLinePathHandle = _guideLineBuildService.StartTrackingPath(targetReachTracker.transform, _playerMovableModel.LocalMovable.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
				_tipHandle = _tipService.CreateTip(TipType.DestinationPulsing, targetReachTracker.transform, TipFollowFlags.None, 0.2f, project: true);
				SetStepPoi(targetReachTracker.transform);
			}
		}

		private void OnTargetReached(BaseTutorialTargetType baseTutorialTargetType)
		{
			if (baseTutorialTargetType == BaseTutorialTargetType.PlayerHide)
			{
				EndStep();
			}
		}

		private void InitializeUITips(RectTransform rectTransform)
		{
			if (!(rectTransform == null))
			{
				_ctrlTipHandle = _uiTipService.CreateTip(UITipType.Ctrl, rectTransform.anchoredPosition);
			}
		}
	}
}
