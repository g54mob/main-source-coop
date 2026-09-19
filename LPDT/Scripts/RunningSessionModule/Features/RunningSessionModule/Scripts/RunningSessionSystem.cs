using System;
using System.Linq;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.RunningSessionModule.Scripts
{
	public class RunningSessionSystem : IInitializable, IDisposable
	{
		private readonly IRunningSessionService _runningSessionService;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly MultiplayerModel _multiplayerModel;

		public RunningSessionSystem(IRunningSessionService runningSessionService, NetworkRunnerEventBus eventBus, MultiplayerModel multiplayerModel)
		{
			_runningSessionService = runningSessionService;
			_eventBus = eventBus;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnSessionListUpdatedEvent>(ProcessSavedSessionCode);
			_eventBus.Subscribe<OnSuccessfullyStartGameEvent>(ProcessGameStart);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnSessionListUpdatedEvent>(ProcessSavedSessionCode);
			_eventBus.Unsubscribe<OnSuccessfullyStartGameEvent>(ProcessGameStart);
		}

		private void ProcessSavedSessionCode(OnSessionListUpdatedEvent sessionListUpdatedEvent)
		{
			if (!_runningSessionService.TryGetSavedSessionCode(out var sessionCode))
			{
				_runningSessionService.InitializeSessionCode(null);
			}
			else if (sessionListUpdatedEvent.SessionList.Any((SessionInfo x) => x.Name == sessionCode))
			{
				_runningSessionService.InitializeSessionCode(sessionCode);
			}
			else
			{
				_runningSessionService.SetSessionCodeSaved(null);
			}
		}

		private void ProcessGameStart(OnSuccessfullyStartGameEvent successfullyStartGameEvent)
		{
			InitializeRunningSessionCode();
			IncreaseCurrentGameIndex();
		}

		private void InitializeRunningSessionCode()
		{
			_runningSessionService.SetSessionCodeSaved(_multiplayerModel.NetworkRunner.SessionInfo.Name);
		}

		private void IncreaseCurrentGameIndex()
		{
			_runningSessionService.IncreaseCurrentGameIndex();
		}
	}
}
