using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial;
using Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy;
using Features.CoroutineUtils.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_EnemyEncounter : BaseTutorialStep
	{
		private readonly IEnemySpawnService _enemySpawnService;

		private readonly BaseTutorialEnemiesDataHolder _baseTutorialEnemiesDataHolder;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private Coroutine _pirateSpawnRoutine;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_EnemyEncounter(IEnemySpawnService enemySpawnService, BaseTutorialEnemiesDataHolder baseTutorialEnemiesDataHolder, ICoroutineRunner coroutineRunner, TutorialStepsConfiguration tutorialStepsConfiguration)
		{
			_enemySpawnService = enemySpawnService;
			_baseTutorialEnemiesDataHolder = baseTutorialEnemiesDataHolder;
			_coroutineRunner = coroutineRunner;
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
		}

		public override async void ActivateStep()
		{
			base.ActivateStep();
			IEnemyBehaviour enemyBehaviour = await _enemySpawnService.SpawnEnemy(EnemyType.PlayerTutorialGuide);
			_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy = enemyBehaviour as PlayerTutorialGuideEnemy;
			_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.SwitchCanBeRevived(canBeRevived: false);
			if (_baseTutorialEnemiesDataHolder.GuideRunAwayPosition.HasValue)
			{
				InvokeGuideRunAway(_baseTutorialEnemiesDataHolder.GuideRunAwayPosition);
			}
			else
			{
				_baseTutorialEnemiesDataHolder.OnGuideRunAwayPositionChanged += InvokeGuideRunAway;
			}
			_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.OnIsDeadChanged += OnPlayerGuideDead;
			_pirateSpawnRoutine = _coroutineRunner.StartCoroutine(SpawnPirateWithGap(_tutorialStepsConfiguration.PirateSpawnGapAfterPlayer));
		}

		public override void Dispose()
		{
			_baseTutorialEnemiesDataHolder.OnGuideRunAwayPositionChanged -= InvokeGuideRunAway;
			if ((UnityEngine.Object)(object)_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy != null)
			{
				_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.OnIsDeadChanged -= OnPlayerGuideDead;
			}
			if (_pirateSpawnRoutine != null)
			{
				_coroutineRunner.StopCoroutine(_pirateSpawnRoutine);
			}
		}

		private IEnumerator SpawnPirateWithGap(float gap)
		{
			yield return new WaitForSeconds(gap);
			SpawnPirateWithGapAsync().Forget();
		}

		private async UniTask SpawnPirateWithGapAsync()
		{
			BaseTutorialEnemiesDataHolder baseTutorialEnemiesDataHolder = _baseTutorialEnemiesDataHolder;
			baseTutorialEnemiesDataHolder.PirateEnemy = (await _enemySpawnService.SpawnEnemy(EnemyType.PirateTutorial)) as PirateEnemy;
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

		private void InvokeGuideRunAway(Vector3? position)
		{
			if (position.HasValue)
			{
				_baseTutorialEnemiesDataHolder.PlayerTutorialGuideEnemy.SetDestination(position.Value);
			}
		}

		private void OnPlayerGuideDead(bool isDead)
		{
			if (isDead)
			{
				EndStep();
			}
		}
	}
}
