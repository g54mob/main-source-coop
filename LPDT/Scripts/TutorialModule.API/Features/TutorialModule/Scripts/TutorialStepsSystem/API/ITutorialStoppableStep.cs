using System;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.API
{
	public interface ITutorialStoppableStep : ITutorialStep, IDisposable
	{
		event Action OnStopTutorial;
	}
}
