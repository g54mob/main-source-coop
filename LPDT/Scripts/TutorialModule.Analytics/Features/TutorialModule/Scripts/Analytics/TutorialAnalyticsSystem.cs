using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.TutorialModule.Scripts.TutorialStepsSystem;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Zenject;

namespace Features.TutorialModule.Scripts.Analytics
{
	public class TutorialAnalyticsSystem : IInitializable, IDisposable
	{
		private readonly CurrentTutorialStepModel _currentTutorialStepModel;

		private readonly TutorialAnalyticsConfiguration _tutorialAnalyticsConfiguration;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private readonly BaseTutorialService _baseTutorialService;

		public TutorialAnalyticsSystem(CurrentTutorialStepModel currentTutorialStepModel, TutorialAnalyticsConfiguration tutorialAnalyticsConfiguration, GameAnalyticsEventSendService gameAnalyticsEventSendService, BaseTutorialService baseTutorialService)
		{
			_currentTutorialStepModel = currentTutorialStepModel;
			_tutorialAnalyticsConfiguration = tutorialAnalyticsConfiguration;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_baseTutorialService = baseTutorialService;
		}

		public void Initialize()
		{
			_currentTutorialStepModel.OnStepEnd += OnTutorialStepEnd;
			_baseTutorialService.OnTutorialStarted += TrackBaseTutorialStart;
		}

		public void Dispose()
		{
			_currentTutorialStepModel.OnStepEnd -= OnTutorialStepEnd;
			_baseTutorialService.OnTutorialStarted -= TrackBaseTutorialStart;
		}

		private void OnTutorialStepEnd(TutorialStep tutorialStepType, ITutorialStep step)
		{
			if (!_tutorialAnalyticsConfiguration.ExcludedSteps.Contains(tutorialStepType))
			{
				_tutorialAnalyticsConfiguration.AnalyticsStepNameMap.TryGetValue(tutorialStepType, out var value);
				_gameAnalyticsEventSendService.TrackTutorialStepFinished(value);
			}
		}

		private void TrackBaseTutorialStart()
		{
			_gameAnalyticsEventSendService.TrackTutorialStarted();
		}
	}
}
