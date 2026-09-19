using System;
using Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy;
using Features.Movement.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_AcquireGuideCarryItem : BaseTutorialStep
	{
		private readonly BaseTutorialEnemiesDataHolder _baseTutorialEnemiesDataHolder;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLineHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_AcquireGuideCarryItem(BaseTutorialEnemiesDataHolder baseTutorialEnemiesDataHolder, IGuideLineBuildService guideLineBuildService, PlayerMovableModel playerMovableModel, ITipService tipService)
		{
			_baseTutorialEnemiesDataHolder = baseTutorialEnemiesDataHolder;
			_guideLineBuildService = guideLineBuildService;
			_playerMovableModel = playerMovableModel;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if ((UnityEngine.Object)(object)_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy != null)
			{
				InitializeGuideDeadPartPath(_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy);
			}
			else
			{
				_baseTutorialEnemiesDataHolder.OnPlayerTutorialGuideEnemyChanged += InitializeGuideDeadPartPath;
			}
		}

		public override void Dispose()
		{
			_baseTutorialEnemiesDataHolder.OnPlayerTutorialGuideEnemyChanged -= InitializeGuideDeadPartPath;
			if ((UnityEngine.Object)(object)_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy != null)
			{
				_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.CastedContext.CarryItemGrabbable.OnGrab -= EndStep;
				_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.CastedContext.CarryItemGrabbable.ForceOutline(enable: false);
			}
			_guideLineBuildService.StopTrackingPath(_guideLineHandle);
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

		private void InitializeGuideDeadPartPath(PlayerTutorialGuideEnemy playerTutorialGuideEnemy)
		{
			if (!((UnityEngine.Object)(object)playerTutorialGuideEnemy == null))
			{
				playerTutorialGuideEnemy.CastedContext.CarryItemGrabbable.OnGrab += EndStep;
				_guideLineHandle = _guideLineBuildService.StartTrackingPath(playerTutorialGuideEnemy.CastedContext.CarryItemGrabbable.Rigidbody.transform, _playerMovableModel.LocalMovable.Rigidbody.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
				_tipHandle = _tipService.CreateTip(TipType.Arrow, playerTutorialGuideEnemy.CastedContext.CarryItemGrabbable.Rigidbody.transform, TipFollowFlags.Position, 1f);
				_grabTipHandle = _tipService.CreateTip(TipType.GrabLeftClick, playerTutorialGuideEnemy.CastedContext.CarryItemGrabbable.Rigidbody.transform, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position, 0.2f);
				playerTutorialGuideEnemy.CastedContext.CarryItemGrabbable.ForceOutline(enable: true);
				SetStepPoi(playerTutorialGuideEnemy.CastedContext.CarryItemGrabbable.Rigidbody.transform);
			}
		}
	}
}
