using System;
using Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_PlayerResurrect : BaseTutorialStep
	{
		private readonly BaseTutorialEnemiesDataHolder _baseTutorialEnemiesDataHolder;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLineHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_PlayerResurrect(BaseTutorialEnemiesDataHolder baseTutorialEnemiesDataHolder, IGuideLineBuildService guideLineBuildService, ITipService tipService)
		{
			_baseTutorialEnemiesDataHolder = baseTutorialEnemiesDataHolder;
			_guideLineBuildService = guideLineBuildService;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if ((UnityEngine.Object)(object)_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy != null)
			{
				InitializeGuideResurrect(_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy);
			}
			else
			{
				_baseTutorialEnemiesDataHolder.OnPlayerTutorialGuideEnemyChanged += InitializeGuideResurrect;
			}
		}

		public override void Dispose()
		{
			_baseTutorialEnemiesDataHolder.OnPlayerTutorialGuideEnemyChanged -= InitializeGuideResurrect;
			_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.OnIsDeadChanged -= OnTutorialGuideResurrect;
			_guideLineBuildService.StopTrackingPath(_guideLineHandle);
			_tipService.KillTip(_tipHandle);
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

		private void InitializeGuideResurrect(PlayerTutorialGuideEnemy playerTutorialGuideEnemy)
		{
			if (!((UnityEngine.Object)(object)playerTutorialGuideEnemy == null) && !(playerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart == null))
			{
				_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.OnIsDeadChanged += OnTutorialGuideResurrect;
				_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.SwitchCanBeRevived(canBeRevived: true);
				_guideLineHandle = _guideLineBuildService.StartTrackingPath(playerTutorialGuideEnemy.CastedContext.DownPos, playerTutorialGuideEnemy.CastedContext.LastSpawnedDeadPart.UpperPos, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
				_tipHandle = _tipService.CreateTip(TipType.Arrow, playerTutorialGuideEnemy.CastedContext.UpperPos, TipFollowFlags.Position, 1.75f);
				SetStepPoi(playerTutorialGuideEnemy.CastedContext.DownPos);
			}
		}

		private void OnTutorialGuideResurrect(bool isDead)
		{
			_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.OnIsDeadChanged -= OnTutorialGuideResurrect;
			if (!isDead)
			{
				EndStep();
			}
		}
	}
}
