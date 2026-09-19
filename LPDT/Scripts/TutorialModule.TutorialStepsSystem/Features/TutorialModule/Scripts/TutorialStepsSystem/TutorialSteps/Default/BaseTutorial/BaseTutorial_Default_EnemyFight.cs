using System;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial;
using Features.Movement.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.TutorialModule.Scripts.GuideModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_EnemyFight : BaseTutorialStep
	{
		private readonly BaseTutorialEnemiesDataHolder _baseTutorialEnemiesDataHolder;

		private readonly IGuideLineBuildService _guideLineBuildService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly ITipService _tipService;

		private GuideLineHandle _guideLineHandle = GuideLineHandle.Invalid;

		private TipHandle _tipHandle = TipHandle.Invalid;

		private TipHandle _leftRightMovementTipHandle = TipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_EnemyFight(BaseTutorialEnemiesDataHolder baseTutorialEnemiesDataHolder, IGuideLineBuildService guideLineBuildService, PlayerMovableModel playerMovableModel, ITipService tipService)
		{
			_baseTutorialEnemiesDataHolder = baseTutorialEnemiesDataHolder;
			_guideLineBuildService = guideLineBuildService;
			_playerMovableModel = playerMovableModel;
			_tipService = tipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			if (_baseTutorialEnemiesDataHolder.PirateEnemy != null)
			{
				InitializePirateEnemyFight(_baseTutorialEnemiesDataHolder.PirateEnemy);
			}
			else
			{
				_baseTutorialEnemiesDataHolder.OnPirateEnemyChanged += InitializePirateEnemyFight;
			}
		}

		public override void Dispose()
		{
			_baseTutorialEnemiesDataHolder.OnPirateEnemyChanged -= InitializePirateEnemyFight;
			if (_baseTutorialEnemiesDataHolder.PirateEnemy != null)
			{
				_baseTutorialEnemiesDataHolder.PirateEnemy.OnDeath -= OnEnemyKilled;
				_baseTutorialEnemiesDataHolder.PirateEnemy.CastedContext.PirateStatEntity.GetStat(EntityStatType.Health).OnReachedMinValue -= DisablePirateOutline;
			}
			_guideLineBuildService.StopTrackingPath(_guideLineHandle);
			_tipService.KillTip(_tipHandle);
			_tipService.KillTip(_leftRightMovementTipHandle);
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

		private void InitializePirateEnemyFight(PirateEnemy enemyBehaviour)
		{
			if (!(enemyBehaviour == null))
			{
				if (enemyBehaviour.CastedContext.IsDead)
				{
					EndStep();
					return;
				}
				enemyBehaviour.CastedContext.Damageable.IsDamageBlocked = false;
				enemyBehaviour.OnDeath += OnEnemyKilled;
				enemyBehaviour.CastedContext.PirateStatEntity.GetStat(EntityStatType.Health).OnReachedMinValue += DisablePirateOutline;
				_guideLineHandle = _guideLineBuildService.StartTrackingPath(enemyBehaviour.NetworkObject.transform, _playerMovableModel.LocalMovable.Rigidbody.transform, GuideLineBuildType.PathFinding, GuideLineUpdateFrequencyType.EveryFrame);
				_tipHandle = _tipService.CreateTip(TipType.Arrow, enemyBehaviour.CastedContext.TipHolder, TipFollowFlags.Position | TipFollowFlags.Rotation);
				_leftRightMovementTipHandle = _tipService.CreateTip(TipType.LeftRightMouseMovement, enemyBehaviour.CastedContext.LeftRightMovementTipHolder, TipFollowFlags.Position | TipFollowFlags.Rotation);
				SetStepPoi(enemyBehaviour.NetworkObject.transform);
				_baseTutorialEnemiesDataHolder.PirateEnemy.SwitchOutline(outlineEnabled: true);
			}
		}

		private void OnEnemyKilled(IEnemyBehaviour enemyBehaviour)
		{
			EndStep();
		}

		private void DisablePirateOutline()
		{
			_baseTutorialEnemiesDataHolder.PirateEnemy?.SwitchOutline(outlineEnabled: false);
		}
	}
}
