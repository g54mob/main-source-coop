using System;
using RSG.Muffin.PlatformStatusSubmodule.Scripts.API;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;
using Zenject;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamPlayerGroupSyncSystem : IInitializable, IDisposable
	{
		private readonly SteamModel _steamModel;

		private readonly SteamLobbyModel _steamLobbyModel;

		private readonly IPlatformStatusService _platformStatusService;

		private Callback<LobbyChatUpdate_t> _lobbyChatUpdate;

		public SteamPlayerGroupSyncSystem(SteamModel steamModel, SteamLobbyModel steamLobbyModel, IPlatformStatusService platformStatusService)
		{
			_steamModel = steamModel;
			_steamLobbyModel = steamLobbyModel;
			_platformStatusService = platformStatusService;
		}

		public void Initialize()
		{
			if (_steamModel.IsSteamInitialized)
			{
				RegisterSteamCallbacks();
			}
			else
			{
				_steamModel.OnInitialized += RegisterSteamCallbacks;
			}
		}

		public void Dispose()
		{
			_steamModel.OnInitialized -= RegisterSteamCallbacks;
			_lobbyChatUpdate?.Dispose();
		}

		private void RegisterSteamCallbacks()
		{
			_steamModel.OnInitialized -= RegisterSteamCallbacks;
			_lobbyChatUpdate?.Dispose();
			_lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
		}

		private void OnLobbyChatUpdate(LobbyChatUpdate_t callback)
		{
			if (_steamLobbyModel.HasActiveLobby)
			{
				CSteamID value = _steamLobbyModel.CurrentLobbyId.Value;
				if (value.m_SteamID == callback.m_ulSteamIDLobby)
				{
					int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(value);
					_platformStatusService.UpdatePlayerGroup(value.ToString(), numLobbyMembers);
				}
			}
		}
	}
}
