using System;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreReconnectSystem : IInitializable, IDisposable
	{
		private readonly PlayerReboundModel _playerReboundModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly CardsOnTableModel _cardsOnTableModel;

		private readonly StorePhaseModel _storePhaseModel;

		public StoreReconnectSystem(PlayerReboundModel playerReboundModel, MultiplayerModel multiplayerModel, CardsOnTableModel cardsOnTableModel, StorePhaseModel storePhaseModel)
		{
			_playerReboundModel = playerReboundModel;
			_multiplayerModel = multiplayerModel;
			_cardsOnTableModel = cardsOnTableModel;
			_storePhaseModel = storePhaseModel;
		}

		public void Initialize()
		{
			_playerReboundModel.OnPlayerRebound += HandlePlayerRebound;
			_storePhaseModel.OnStorePhaseChanged += HandleStorePhaseChanged;
			HandleStorePhaseChanged(_storePhaseModel.IsStoreActive.Value);
		}

		public void Dispose()
		{
			_playerReboundModel.OnPlayerRebound -= HandlePlayerRebound;
			_storePhaseModel.OnStorePhaseChanged -= HandleStorePhaseChanged;
		}

		private void HandleStorePhaseChanged(bool isActive)
		{
			PlayerSessionPrefs.SetPositionSavingSuspended(isActive);
			if (isActive)
			{
				PlayerSessionPrefs.ClearSavedPosition();
			}
		}

		private void HandlePlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			if (!(avatar == null))
			{
				NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
				if (!(networkRunner == null) && networkRunner.IsRunning && !(playerRef != networkRunner.LocalPlayer))
				{
					ResyncCardsOnTable(networkRunner);
				}
			}
		}

		private void ResyncCardsOnTable(NetworkRunner runner)
		{
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid)
				{
					StoreCardBehaviour component = allNetworkObject.GetComponent<StoreCardBehaviour>();
					if (component != null)
					{
						_cardsOnTableModel.RegisterCard(component);
					}
				}
			}
		}
	}
}
