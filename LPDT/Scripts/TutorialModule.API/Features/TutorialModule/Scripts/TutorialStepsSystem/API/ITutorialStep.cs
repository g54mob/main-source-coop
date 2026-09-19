using System;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.API
{
	public interface ITutorialStep : IDisposable
	{
		ITutorialCustomData CustomData { get; }

		event Action OnStepEnd;

		void ActivateStep();

		void DeActivateStep();

		void SkipStep();

		void RevertStep();
	}
}
