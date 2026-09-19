using System;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.API;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.Internal
{
	public class LocationTutorialStepFactoryRegistrationSystem : IInitializable, IDisposable
	{
		private readonly TutorialStepFactoryHolder _tutorialStepFactoryHolder;

		private readonly TutorialStepFactory _tutorialStepFactory;

		public LocationTutorialStepFactoryRegistrationSystem(TutorialStepFactoryHolder tutorialStepFactoryHolder, TutorialStepFactory tutorialStepFactory)
		{
			_tutorialStepFactoryHolder = tutorialStepFactoryHolder;
			_tutorialStepFactory = tutorialStepFactory;
		}

		public void Initialize()
		{
			RegisterLocalFactory();
		}

		public void Dispose()
		{
		}

		private void RegisterLocalFactory()
		{
			_tutorialStepFactoryHolder.SetCurrentTutorialStepFactory(_tutorialStepFactory);
		}
	}
}
