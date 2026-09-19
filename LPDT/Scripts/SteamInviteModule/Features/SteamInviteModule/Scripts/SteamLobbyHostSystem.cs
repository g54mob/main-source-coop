using System;
using Features.MultiplayerSessionServices.Scripts;
using NetworkServices.NetworkEvents;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;
using UnityEngine;
using Zenject;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamLobbyHostSystem : IInitializable, IDisposable
	{
		private readonly NetworkRunnerEventBus _eventBus;

		private readonly SteamModel _steamModel;

		private readonly SteamLobbyModel _steamLobbyModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ISteamInviteService _steamInviteService;

		private readonly MultiplayerSessionConfig _multiplayerSessionConfig;

		private Callback<LobbyCreated_t> _lobbyCreated;

		private string _pendingSessionCode;

		private string _pendingSessionRegion;

		public SteamLobbyHostSystem(NetworkRunnerEventBus eventBus, SteamModel steamModel, SteamLobbyModel steamLobbyModel, MultiplayerModel multiplayerModel, ISteamInviteService steamInviteService, MultiplayerSessionConfig multiplayerSessionConfig)
		{
			_eventBus = eventBus;
			_steamModel = steamModel;
			_steamLobbyModel = steamLobbyModel;
			_multiplayerModel = multiplayerModel;
			_steamInviteService = steamInviteService;
			_multiplayerSessionConfig = multiplayerSessionConfig;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnSuccessfullyStartGameEvent>(OnFusionSessionStarted);
			_steamModel.OnInitialized += TryCreatePendingSteamLobby;
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnSuccessfullyStartGameEvent>(OnFusionSessionStarted);
			_steamModel.OnInitialized -= TryCreatePendingSteamLobby;
			_lobbyCreated?.Dispose();
			_steamLobbyModel.Clear();
		}

		private void OnFusionSessionStarted(OnSuccessfullyStartGameEvent eventData)
		{
			if (eventData.StartedAsHost)
			{
				_pendingSessionCode = eventData.SessionName;
				_pendingSessionRegion = eventData.SessionRegion;
				TryCreatePendingSteamLobby();
			}
		}

		private void TryCreatePendingSteamLobby()
		{
			if (!string.IsNullOrEmpty(_pendingSessionCode) && _steamModel.IsSteamInitialized)
			{
				_steamLobbyModel.Clear();
				_lobbyCreated?.Dispose();
				_lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
				_steamInviteService.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _multiplayerSessionConfig.MaxPlayersInRoom);
			}
		}

		private void OnLobbyCreated(LobbyCreated_t callback)
		{
			if (callback.m_eResult != EResult.k_EResultOK)
			{
				Debug.LogWarning($"[SteamInvite] Steam lobby creation failed: {callback.m_eResult}");
				return;
			}
			CSteamID lobbyId = new CSteamID(callback.m_ulSteamIDLobby);
			string text = _pendingSessionCode;
			if (string.IsNullOrEmpty(text) && _multiplayerModel.NetworkRunner != null)
			{
				text = _multiplayerModel.NetworkRunner.SessionInfo.Name;
			}
			string text2 = _pendingSessionRegion;
			if (string.IsNullOrEmpty(text2) && _multiplayerModel.NetworkRunner != null)
			{
				text2 = _multiplayerModel.NetworkRunner.SessionInfo.Region;
			}
			_steamInviteService.CreateSteamLobby(lobbyId, text, text2);
		}
	}
}
