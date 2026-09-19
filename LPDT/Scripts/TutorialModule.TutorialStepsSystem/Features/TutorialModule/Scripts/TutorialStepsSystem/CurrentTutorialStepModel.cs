using System;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class CurrentTutorialStepModel
	{
		private TutorialStep _currentStepType;

		public ITutorialStep CurrentStep { get; private set; }

		public event Action<TutorialStep, ITutorialStep> OnStepStart;

		public event Action<TutorialStep, ITutorialStep> OnStepInitialized;

		public event Action<TutorialStep, ITutorialStep> OnStepEnd;

		public void SetStepStarted(ITutorialStep step, TutorialStep tutorialStep)
		{
			CurrentStep = step;
			_currentStepType = tutorialStep;
			this.OnStepStart?.Invoke(_currentStepType, CurrentStep);
		}

		public void SetStepInitialized()
		{
			this.OnStepInitialized?.Invoke(_currentStepType, CurrentStep);
		}

		public void SetStepEnded()
		{
			TutorialStep currentStepType = _currentStepType;
			ITutorialStep currentStep = CurrentStep;
			_currentStepType = TutorialStep.None;
			CurrentStep = null;
			this.OnStepEnd?.Invoke(currentStepType, currentStep);
		}
	}
}
