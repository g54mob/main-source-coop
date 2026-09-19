using System;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;
using Zenject;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamLobbyAutoJoinSystem : IInitializable, IDisposable
	{
		private readonly SteamModel _steamModel;

		private readonly SteamLobbyModel _steamLobbyModel;

		private readonly SteamLobbySyncModel _steamLobbySyncModel;

		private readonly ISteamInviteService _steamInviteService;

		private Callback<LobbyEnter_t> _lobbyEntered;

		public SteamLobbyAutoJoinSystem(SteamModel steamModel, SteamLobbyModel steamLobbyModel, SteamLobbySyncModel steamLobbySyncModel, ISteamInviteService steamInviteService)
		{
			_steamModel = steamModel;
			_steamLobbyModel = steamLobbyModel;
			_steamLobbySyncModel = steamLobbySyncModel;
			_steamInviteService = steamInviteService;
		}

		public void Initialize()
		{
			_steamLobbySyncModel.OnSteamLobbyIdChanged += OnSteamLobbyIdChanged;
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
			_steamLobbySyncModel.OnSteamLobbyIdChanged -= OnSteamLobbyIdChanged;
			_steamModel.OnInitialized -= RegisterSteamCallbacks;
			_lobbyEntered?.Dispose();
		}

		private void RegisterSteamCallbacks()
		{
			_steamModel.OnInitialized -= RegisterSteamCallbacks;
			_lobbyEntered?.Dispose();
			_lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
			TryJoinSyncedLobby();
		}

		private void OnSteamLobbyIdChanged(ulong steamLobbyId)
		{
			TryJoinSyncedLobby();
		}

		private void TryJoinSyncedLobby()
		{
			ulong steamLobbyId = _steamLobbySyncModel.SteamLobbyId;
			if (steamLobbyId != 0L && _steamModel.IsSteamInitialized && (!_steamLobbyModel.HasActiveLobby || _steamLobbyModel.CurrentLobbyId.Value.m_SteamID != steamLobbyId))
			{
				_steamInviteService.JoinLobby(new CSteamID(steamLobbyId));
			}
		}

		private void OnLobbyEntered(LobbyEnter_t callback)
		{
			if (callback.m_ulSteamIDLobby == _steamLobbySyncModel.SteamLobbyId && (!_steamLobbyModel.HasActiveLobby || _steamLobbyModel.CurrentLobbyId.Value.m_SteamID != callback.m_ulSteamIDLobby))
			{
				_steamLobbyModel.SetCurrentLobby(new CSteamID(callback.m_ulSteamIDLobby));
				_steamInviteService.RefreshConnectRichPresence();
			}
		}
	}
}
