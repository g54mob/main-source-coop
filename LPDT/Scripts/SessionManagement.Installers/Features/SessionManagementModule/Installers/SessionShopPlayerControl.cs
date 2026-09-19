using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.SessionManagementModule.Models;
using Features.StoreModule.Scripts;

namespace Features.SessionManagementModule.Installers
{
	public sealed class SessionShopPlayerControl : IShopPlayerControl
	{
		private readonly IPlayerStateService _playerStateService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly StoreSeaterRegistry _storeSeaterRegistry;

		public SessionShopPlayerControl(IPlayerStateService playerStateService, MultiplayerModel multiplayerModel, StoreSeaterRegistry storeSeaterRegistry)
		{
			_playerStateService = playerStateService;
			_multiplayerModel = multiplayerModel;
			_storeSeaterRegistry = storeSeaterRegistry;
		}

		public UniTask ReleaseGrabsAsync()
		{
			return _storeSeaterRegistry.ReleaseGrabsAsync();
		}

		public UniTask EnsureBodyAuthorityAsync()
		{
			return _storeSeaterRegistry.EnsureBodyAuthorityAsync();
		}

		public UniTask ResetRagdollAsync()
		{
			return _storeSeaterRegistry.ResetRagdollAsync();
		}

		public UniTask SeatAtStoreSeatAsync()
		{
			return _storeSeaterRegistry.SeatAtStoreSeatAsync();
		}

		public void LeaveStoreSeat()
		{
			bool num = _playerStateService.GetPlayerState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId) == PlayerState.Store;
			_storeSeaterRegistry.ClearSeatRequest();
			if (num)
			{
				_storeSeaterRegistry.RestorePreStoreState();
			}
		}
	}
}
