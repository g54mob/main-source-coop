using System;
using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using Features.StoreModule.Scripts;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class StoreReadyShopVoteSystem : IInitializable, IDisposable
	{
		private readonly IStoreReadyRoster _storeReadyRoster;

		private readonly ShopVoteModel _shopVoteModel;

		private readonly MultiplayerModel _multiplayerModel;

		public StoreReadyShopVoteSystem(IStoreReadyRoster storeReadyRoster, ShopVoteModel shopVoteModel, MultiplayerModel multiplayerModel)
		{
			_storeReadyRoster = storeReadyRoster;
			_shopVoteModel = shopVoteModel;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_storeReadyRoster.OnPlayerReady += OnPlayerReady;
		}

		public void Dispose()
		{
			_storeReadyRoster.OnPlayerReady -= OnPlayerReady;
		}

		private void OnPlayerReady(int playerId)
		{
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && !_shopVoteModel.HasVotedToLeave.Value)
			{
				_shopVoteModel.HasVotedToLeave.Value = true;
			}
		}
	}
}
