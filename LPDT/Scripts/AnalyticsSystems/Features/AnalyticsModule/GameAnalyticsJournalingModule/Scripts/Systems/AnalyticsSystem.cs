using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class AnalyticsSystem : IInitializable, IDisposable
	{
		private readonly SessionAnalyticsModel _sessionAnalyticsModel;

		private readonly GameAnalyticsEventSendService _analytics;

		private readonly CustomPlayerEventsSynchronizedModel _customPlayerEventsSynchronizedModel;

		private readonly MultiplayerModel _multiplayerModel;

		public AnalyticsSystem(SessionAnalyticsModel sessionAnalyticsModel, GameAnalyticsEventSendService analytics, CustomPlayerEventsSynchronizedModel customPlayerEventsSynchronizedModel, MultiplayerModel multiplayerModel)
		{
			_sessionAnalyticsModel = sessionAnalyticsModel;
			_analytics = analytics;
			_customPlayerEventsSynchronizedModel = customPlayerEventsSynchronizedModel;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_sessionAnalyticsModel.OnLocationEntered += OnLocationEntered;
			_sessionAnalyticsModel.OnEnemyTargetRegistered += OnEnemyTargetRegistered;
			_customPlayerEventsSynchronizedModel.OnPlayerCustomEvent += ProcessPlayerCustomEvent;
		}

		public void Dispose()
		{
			_sessionAnalyticsModel.OnLocationEntered -= OnLocationEntered;
			_sessionAnalyticsModel.OnEnemyTargetRegistered -= OnEnemyTargetRegistered;
			_customPlayerEventsSynchronizedModel.OnPlayerCustomEvent -= ProcessPlayerCustomEvent;
		}

		private void ProcessPlayerCustomEvent(CustomPlayerEventRequest customPlayerEventRequest)
		{
			if (customPlayerEventRequest.OwnerPlayerId == -1 || _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == customPlayerEventRequest.OwnerPlayerId)
			{
				_analytics.NewDesignEvent(customPlayerEventRequest.EvenName, customPlayerEventRequest.QuotaGroup);
			}
		}

		private void OnLocationEntered()
		{
			_analytics.TrackLocationEntered();
		}

		private void OnEnemyTargetRegistered(int player)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_customPlayerEventsSynchronizedModel.SendPlayerEvent(player, "enemy:encountered");
			}
		}
	}
}
