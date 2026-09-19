using System;
using System.Collections;
using Features.CoroutineUtils.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.UITipsModule;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_End : BaseTutorialStep
	{
		private readonly ICoroutineRunner _coroutineRunner;

		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private readonly IUITipService _uiTipService;

		private UITipHandle _tutorialCompleteUiTipHandle = UITipHandle.Invalid;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_End(ICoroutineRunner coroutineRunner, TutorialStepsConfiguration tutorialStepsConfiguration, IUITipService uiTipService)
		{
			_coroutineRunner = coroutineRunner;
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
			_uiTipService = uiTipService;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			_coroutineRunner.StartCoroutine(InvokeEndWithDelay(_tutorialStepsConfiguration.TutorialCompleteStepDuration));
			_tutorialCompleteUiTipHandle = _uiTipService.CreateTip(UITipType.TutorialComplete);
		}

		public override void Dispose()
		{
			_uiTipService.KillTip(_tutorialCompleteUiTipHandle);
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

		private IEnumerator InvokeEndWithDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			EndStep();
		}
	}
}
