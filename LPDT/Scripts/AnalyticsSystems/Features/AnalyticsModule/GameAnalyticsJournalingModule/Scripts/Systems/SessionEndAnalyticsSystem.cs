using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.DisconnectHandlerModule.Scripts.Data;
using Features.GameCycle.Scripts.SessionCleanup;
using Global.StateMachinesModule.Scripts;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class SessionEndAnalyticsSystem : ISessionEndAnalyticsService, IInitializable, IDisposable, ISessionCleanup
	{
		private readonly SessionEndAnalyticsContextModel _contextModel;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private readonly DisconnectRequestEventClass _disconnectRequestEventClass;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private bool _sessionEndSent;

		public SessionEndAnalyticsSystem(SessionEndAnalyticsContextModel contextModel, GameAnalyticsEventSendService gameAnalyticsEventSendService, DisconnectRequestEventClass disconnectRequestEventClass, NetworkRunnerEventBus networkRunnerEventBus, GameFlowStateMachine gameFlowStateMachine)
		{
			_contextModel = contextModel;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_disconnectRequestEventClass = disconnectRequestEventClass;
			_networkRunnerEventBus = networkRunnerEventBus;
			_gameFlowStateMachine = gameFlowStateMachine;
		}

		public void Initialize()
		{
			_disconnectRequestEventClass.OnRequestDisconnect += OnDisconnectRequested;
			_networkRunnerEventBus.Subscribe<OnDisconnectedFromServerEvent>(OnDisconnectedFromServer);
		}

		public void Dispose()
		{
			_disconnectRequestEventClass.OnRequestDisconnect -= OnDisconnectRequested;
			_networkRunnerEventBus.Unsubscribe<OnDisconnectedFromServerEvent>(OnDisconnectedFromServer);
		}

		public void Cleanup()
		{
			_sessionEndSent = false;
			_contextModel.Cleanup();
		}

		public void PrepareForApplicationQuit()
		{
			if (_contextModel.GetCurrentReason() == SessionEndAnalyticsReason.Unknown)
			{
				_contextModel.TrySetReason(SessionEndAnalyticsReason.AltF4);
			}
		}

		public bool TrySendSessionEnd(bool treatUnknownReasonAsDisconnect = false)
		{
			if (_sessionEndSent)
			{
				return false;
			}
			if (treatUnknownReasonAsDisconnect && _contextModel.GetCurrentReason() == SessionEndAnalyticsReason.Unknown)
			{
				_contextModel.TrySetReason(SessionEndAnalyticsReason.Disconnect);
			}
			_sessionEndSent = true;
			SessionEndAnalyticsReason currentReason = _contextModel.GetCurrentReason();
			SessionEndAnalyticsPlace currentPlace = _contextModel.GetCurrentPlace();
			_gameAnalyticsEventSendService.TrackSessionEnd(currentReason, currentPlace);
			_contextModel.Reset();
			return true;
		}

		private void OnDisconnectRequested(DisconnectRequestReason _)
		{
			_contextModel.TrySetReason(SessionEndAnalyticsReason.Normal);
		}

		private void OnDisconnectedFromServer(OnDisconnectedFromServerEvent _)
		{
			if (_contextModel.GetCurrentReason() != SessionEndAnalyticsReason.Normal)
			{
				_contextModel.TrySetReason(SessionEndAnalyticsReason.Disconnect);
				TrySendSessionEnd();
			}
		}

		public bool ShouldSendOnShutdown()
		{
			return _gameFlowStateMachine.CurrentState != GameFlowState.MenuGameState;
		}
	}
}
