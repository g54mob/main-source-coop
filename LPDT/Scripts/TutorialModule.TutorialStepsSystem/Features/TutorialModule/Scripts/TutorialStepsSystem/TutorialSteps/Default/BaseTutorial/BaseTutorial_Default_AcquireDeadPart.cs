using System;
using Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy;
using Features.GameUpdaterModule;
using Features.Movement.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_AcquireDeadPart : BaseTutorialStep
	{
		private readonly BaseTutorialEnemiesDataHolder _baseTutorialEnemiesDataHolder;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLineHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _grabTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_AcquireDeadPart(BaseTutorialEnemiesDataHolder baseTutorialEnemiesDataHolder, IGuideLineBuildService guideLineBuildService, PlayerMovableModel playerMovableModel, IGameUpdater gameUpdater, ITipService tipService)
		{
			_baseTutorialEnemiesDataHolder = baseTutorialEnemiesDataHolder;
			_guideLineBuildService = guideLineBuildService;
			_playerMovableModel = playerMovableModel;
			_gameUpdater = gameUpdater;
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
			_gameUpdater.OnUpdate -= TrackDeadPartGrabbed;
			if ((UnityEngine.Object)(object)_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy != null)
			{
				_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart.ParentGrabbable.ForceOutline(enable: false);
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
			if (!((UnityEngine.Object)(object)playerTutorialGuideEnemy == null) && !(playerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart == null))
			{
				_gameUpdater.OnUpdate += TrackDeadPartGrabbed;
				_guideLineHandle = _guideLineBuildService.StartTrackingPath(playerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart.UpperPos, _playerMovableModel.LocalMovable.Rigidbody.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
				_tipHandle = _tipService.CreateTip(TipType.Arrow, playerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart.UpperPos, TipFollowFlags.Position, 1.25f);
				_grabTipHandle = _tipService.CreateTip(TipType.GrabLeftClick, playerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart.UpperPos, TipFollowFlags.FaceCameraEntire | TipFollowFlags.Position, 0.2f);
				playerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart.ParentGrabbable.ForceOutline(enable: true);
				SetStepPoi(playerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart.UpperPos);
			}
		}

		private void TrackDeadPartGrabbed()
		{
			if (_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart.ParentGrabbable.GrabbedByPlayersCount > 0)
			{
				EndStep();
			}
		}
	}
}
