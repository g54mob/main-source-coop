using System;
using System.Linq;
using Features.MultiplayerSessionServices.Scripts;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreSessionCloseAnalyticsSystem : IInitializable, IDisposable
	{
		private readonly CardsOnTableModel _cardsOnTableModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly StoreDraftMoneyModel _storeDraftMoneyModel;

		private readonly IStoreReadyRoster _storeReadyRoster;

		public StoreSessionCloseAnalyticsSystem(IStoreReadyRoster storeReadyRoster, MultiplayerModel multiplayerModel, CardsOnTableModel cardsOnTableModel, StoreDraftMoneyModel storeDraftMoneyModel)
		{
			_storeReadyRoster = storeReadyRoster;
			_multiplayerModel = multiplayerModel;
			_cardsOnTableModel = cardsOnTableModel;
			_storeDraftMoneyModel = storeDraftMoneyModel;
		}

		public void Dispose()
		{
			_storeReadyRoster.OnPlayerIdsChanged -= OnStoreReadyPlayersChanged;
		}

		public void Initialize()
		{
			_storeReadyRoster.OnPlayerIdsChanged += OnStoreReadyPlayersChanged;
		}

		private void OnStoreReadyPlayersChanged()
		{
			int num = _multiplayerModel.NetworkRunner.ActivePlayers.Count();
			if (_storeReadyRoster.ReadyPlayers.Count == num)
			{
				bool isSharedModeMasterClient = _multiplayerModel.NetworkRunner.IsSharedModeMasterClient;
				StoreCardsSnapshotDebug.LogStoreClose(StoreCardsSnapshotDebug.Collect(_cardsOnTableModel), isSharedModeMasterClient, isSharedModeMasterClient ? _storeDraftMoneyModel : null);
			}
		}
	}
}
