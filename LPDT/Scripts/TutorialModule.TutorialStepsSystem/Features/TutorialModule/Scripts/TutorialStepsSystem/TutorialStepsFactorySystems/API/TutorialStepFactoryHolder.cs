using System;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.API
{
	public class TutorialStepFactoryHolder
	{
		public TutorialStepFactory CurrentLocalTutorialStepFactory { get; private set; }

		public void SetCurrentTutorialStepFactory(TutorialStepFactory tutorialStepFactory)
		{
			CurrentLocalTutorialStepFactory = tutorialStepFactory ?? throw new NullReferenceException();
		}
	}
}
