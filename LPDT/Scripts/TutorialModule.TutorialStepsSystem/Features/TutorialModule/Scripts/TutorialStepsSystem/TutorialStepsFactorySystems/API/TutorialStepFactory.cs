using System;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.API
{
	public class TutorialStepFactory
	{
		private readonly DiContainer _diContainer;

		public TutorialStepFactory(DiContainer diContainer)
		{
			_diContainer = diContainer;
		}

		public ITutorialStep Create(Type type)
		{
			return _diContainer.Instantiate(type) as ITutorialStep;
		}
	}
}
