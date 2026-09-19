using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.ProgressSavingModule.Scripts.Implementation;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class SessionStartAnalyticsSystem : IInitializable, IDisposable
	{
		private readonly SentAnalyticsModel _sentAnalyticsModel;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private readonly ISavingService _savingService;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		public SessionStartAnalyticsSystem(SentAnalyticsModel sentAnalyticsModel, GameAnalyticsEventSendService gameAnalyticsEventSendService, ISavingService savingService, NetworkRunnerEventBus networkRunnerEventBus)
		{
			_sentAnalyticsModel = sentAnalyticsModel;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_savingService = savingService;
			_networkRunnerEventBus = networkRunnerEventBus;
		}

		public void Initialize()
		{
			_networkRunnerEventBus.Subscribe<OnStartGamePhaseEvent>(OnStartGamePhase);
		}

		public void Dispose()
		{
			_networkRunnerEventBus.Unsubscribe<OnStartGamePhaseEvent>(OnStartGamePhase);
		}

		private void OnStartGamePhase(OnStartGamePhaseEvent startGamePhaseEvent)
		{
			int num = _sentAnalyticsModel.IncrementSessionStartedCount();
			_savingService.SaveDataForGroup(SavingGroup.Analytics);
			_gameAnalyticsEventSendService.TrackSessionStart(num == 1, startGamePhaseEvent.PlayerCount);
		}
	}
}
