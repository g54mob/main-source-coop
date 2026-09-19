using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class StoreSessionCloseAnalyticsSystem : IInitializable, IDisposable
	{
		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly StoreAnalyticsQuotaSynchronizedModel _storeAnalyticsQuotaSynchronizedModel;

		private int _reportedSessionId = -1;

		public StoreSessionCloseAnalyticsSystem(StoreAnalyticsQuotaSynchronizedModel storeAnalyticsQuotaSynchronizedModel, MultiplayerModel multiplayerModel, GameAnalyticsEventSendService gameAnalyticsEventSendService)
		{
			_storeAnalyticsQuotaSynchronizedModel = storeAnalyticsQuotaSynchronizedModel;
			_multiplayerModel = multiplayerModel;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
		}

		public void Dispose()
		{
			_storeAnalyticsQuotaSynchronizedModel.OnSessionStateChanged -= OnSessionStateChanged;
		}

		public void Initialize()
		{
			_storeAnalyticsQuotaSynchronizedModel.OnSessionStateChanged += OnSessionStateChanged;
		}

		private void OnSessionStateChanged()
		{
			if (_storeAnalyticsQuotaSynchronizedModel.Phase == StoreAnalyticsPhase.Completed)
			{
				_reportedSessionId = -1;
			}
			else if (_storeAnalyticsQuotaSynchronizedModel.Phase == StoreAnalyticsPhase.AwaitingQuota && _reportedSessionId != _storeAnalyticsQuotaSynchronizedModel.SessionId)
			{
				NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
				if (!(networkRunner == null) && networkRunner.IsRunning)
				{
					_reportedSessionId = _storeAnalyticsQuotaSynchronizedModel.SessionId;
					bool hasEnoughQuota = _gameAnalyticsEventSendService.CanSendBatch(AnalyticsEventQuotaGroup.Shop, _storeAnalyticsQuotaSynchronizedModel.RequiredEventCount);
					_storeAnalyticsQuotaSynchronizedModel.ReportLocalQuota(networkRunner.LocalPlayer.PlayerId, _storeAnalyticsQuotaSynchronizedModel.SessionId, hasEnoughQuota);
				}
			}
		}
	}
}
