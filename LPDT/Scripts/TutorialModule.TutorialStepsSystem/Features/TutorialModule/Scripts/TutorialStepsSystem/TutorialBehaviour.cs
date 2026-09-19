using System;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using JetBrains.Annotations;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	[PublicAPI]
	public abstract class TutorialBehaviour
	{
		private readonly CurrentTutorialStepModel _currentTutorialStepsModel;

		protected readonly TutorialStepsQuery _tutorialStepsQuery;

		public event Action OnTutorialStarted;

		public event Action OnTutorialEnded;

		protected TutorialBehaviour(TutorialStepsQuery tutorialStepsQuery, CurrentTutorialStepModel currentTutorialStepsModel)
		{
			_tutorialStepsQuery = tutorialStepsQuery;
			_currentTutorialStepsModel = currentTutorialStepsModel;
		}

		public virtual void Initialize()
		{
			TutorialStepsQuery tutorialStepsQuery = _tutorialStepsQuery;
			tutorialStepsQuery.OnStepEnded = (Action)Delegate.Combine(tutorialStepsQuery.OnStepEnded, new Action(ActivateNextStep));
			TutorialStepsQuery tutorialStepsQuery2 = _tutorialStepsQuery;
			tutorialStepsQuery2.OnQueryEnded = (Action)Delegate.Combine(tutorialStepsQuery2.OnQueryEnded, new Action(EndTutorial));
		}

		public virtual void Dispose()
		{
			TutorialStepsQuery tutorialStepsQuery = _tutorialStepsQuery;
			tutorialStepsQuery.OnStepEnded = (Action)Delegate.Remove(tutorialStepsQuery.OnStepEnded, new Action(ActivateNextStep));
			TutorialStepsQuery tutorialStepsQuery2 = _tutorialStepsQuery;
			tutorialStepsQuery2.OnQueryEnded = (Action)Delegate.Remove(tutorialStepsQuery2.OnQueryEnded, new Action(EndTutorial));
		}

		public virtual void ActivateFirstStep()
		{
			this.OnTutorialStarted?.Invoke();
			_tutorialStepsQuery.RestartQuery();
			_tutorialStepsQuery.PopStep();
		}

		public virtual void ActivateNextStep()
		{
			_tutorialStepsQuery.PopStep();
		}

		public virtual void ActivateStep(TutorialStep step)
		{
			_tutorialStepsQuery.GoToStep(step);
		}

		public void EndTutorial()
		{
			_tutorialStepsQuery.RestartQuery();
			this.OnTutorialEnded?.Invoke();
		}

		public void SkipTutorial()
		{
			_tutorialStepsQuery.SkipQueryToEnd();
			this.OnTutorialEnded?.Invoke();
		}

		public void SkipCurrentStep()
		{
			_tutorialStepsQuery.SkipCurrentStep();
		}

		public void SkipToStep(TutorialStep tutorialStep)
		{
			_tutorialStepsQuery.SkipQueryToStep(tutorialStep);
		}
	}
}
