using System;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class LocationTutorialSystem : IDisposable
	{
		private readonly ITutorialStartupService _tutorialService;

		public LocationTutorialSystem(ITutorialStartupService tutorialService)
		{
			_tutorialService = tutorialService;
		}

		public void Dispose()
		{
			_tutorialService.Dispose();
		}
	}
}
