using System;
using System.Collections.Generic;
using System.Linq;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems.Extensions;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.StoreModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class StoreSessionAnalyticsHostCoordinatorSystem : IInitializable, IDisposable
	{
		private readonly IStoreReadyRoster _storeReadyRoster;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LevelModel _levelModel;

		private readonly CardsOnTableModel _cardsOnTableModel;

		private readonly StoreAnalyticsQuotaSynchronizedModel _storeAnalyticsQuotaSynchronizedModel;

		private readonly CustomPlayerEventsSynchronizedModel _customPlayerEventsSynchronizedModel;

		private readonly Dictionary<int, bool> _quotaResponses = new Dictionary<int, bool>();

		private readonly List<string> _offerItemNames = new List<string>();

		private readonly List<string> _purchaseItemNames = new List<string>();

		private List<string> _pendingEventNames;

		private bool _quotaRoundInProgress;

		private int _nextSessionId = 1;

		public StoreSessionAnalyticsHostCoordinatorSystem(IStoreReadyRoster storeReadyRoster, MultiplayerModel multiplayerModel, LevelModel levelModel, CardsOnTableModel cardsOnTableModel, StoreAnalyticsQuotaSynchronizedModel storeAnalyticsQuotaSynchronizedModel, CustomPlayerEventsSynchronizedModel customPlayerEventsSynchronizedModel)
		{
			_storeReadyRoster = storeReadyRoster;
			_multiplayerModel = multiplayerModel;
			_levelModel = levelModel;
			_cardsOnTableModel = cardsOnTableModel;
			_storeAnalyticsQuotaSynchronizedModel = storeAnalyticsQuotaSynchronizedModel;
			_customPlayerEventsSynchronizedModel = customPlayerEventsSynchronizedModel;
		}

		public void Initialize()
		{
			_storeReadyRoster.OnPlayerIdsChanged += OnStoreReadyPlayersChanged;
			_storeAnalyticsQuotaSynchronizedModel.OnQuotaResponseReceived += OnQuotaResponseReceived;
		}

		public void Dispose()
		{
			_storeReadyRoster.OnPlayerIdsChanged -= OnStoreReadyPlayersChanged;
			_storeAnalyticsQuotaSynchronizedModel.OnQuotaResponseReceived -= OnQuotaResponseReceived;
		}

		private void OnStoreReadyPlayersChanged()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient && _storeReadyRoster.ReadyPlayers.Count == networkRunner.ActivePlayers.Count() && !_quotaRoundInProgress && _storeAnalyticsQuotaSynchronizedModel.Phase != StoreAnalyticsPhase.AwaitingQuota)
			{
				StartQuotaRound();
			}
		}

		private void StartQuotaRound()
		{
			_cardsOnTableModel.CollectShopOfferItemNames(_offerItemNames);
			_cardsOnTableModel.CollectShopPurchaseItemNames(_purchaseItemNames);
			_pendingEventNames = _levelModel.BuildShopStoreAnalyticsEventNames(_offerItemNames, _purchaseItemNames);
			_quotaResponses.Clear();
			_quotaRoundInProgress = true;
			int sessionId = _nextSessionId++;
			_storeAnalyticsQuotaSynchronizedModel.BeginQuotaRound(sessionId, _pendingEventNames.Count);
		}

		private void OnQuotaResponseReceived(StoreAnalyticsQuotaResponseData data)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient && _quotaRoundInProgress && data.SessionId == _storeAnalyticsQuotaSynchronizedModel.SessionId)
			{
				_quotaResponses[data.PlayerId] = data.HasEnoughQuota;
				int num = networkRunner.ActivePlayers.Count();
				if (_quotaResponses.Count >= num)
				{
					TryDispatchStoreEvents(networkRunner);
				}
			}
		}

		private void TryDispatchStoreEvents(NetworkRunner runner)
		{
			int num = PickSenderPlayerId(runner);
			if (num < 0)
			{
				Debug.LogWarning("[StoreAnalytics] No peer has enough analytics quota for store events. Skipping Shop events.");
				CompleteQuotaRound();
				return;
			}
			if (_pendingEventNames != null)
			{
				foreach (string pendingEventName in _pendingEventNames)
				{
					_customPlayerEventsSynchronizedModel.SendPlayerEvent(num, pendingEventName, AnalyticsEventQuotaGroup.Shop);
				}
			}
			CompleteQuotaRound();
		}

		private int PickSenderPlayerId(NetworkRunner runner)
		{
			int playerId = runner.LocalPlayer.PlayerId;
			if (_quotaResponses.TryGetValue(playerId, out var value) && value)
			{
				return playerId;
			}
			foreach (PlayerRef item in runner.ActivePlayers.OrderBy((PlayerRef player) => player.PlayerId))
			{
				if (item.PlayerId != playerId && _quotaResponses.TryGetValue(item.PlayerId, out var value2) && value2)
				{
					return item.PlayerId;
				}
			}
			return -1;
		}

		private void CompleteQuotaRound()
		{
			_quotaRoundInProgress = false;
			_pendingEventNames = null;
			_storeAnalyticsQuotaSynchronizedModel.CompleteRound();
		}
	}
}
