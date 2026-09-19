using System;
using RSG.Muffin.PlatformStatusSubmodule.Scripts.API;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamInviteService : ISteamInviteService
	{
		private readonly SteamLobbyModel _steamLobbyModel;

		private readonly SteamLobbySyncModel _steamLobbySyncModel;

		private readonly SteamModel _steamModel;

		private readonly IPlatformStatusService _platformStatusService;

		public event Action OnSteamInitialized;

		public SteamInviteService(SteamLobbyModel steamLobbyModel, SteamLobbySyncModel steamLobbySyncModel, SteamModel steamModel, IPlatformStatusService platformStatusService)
		{
			_steamLobbyModel = steamLobbyModel;
			_steamLobbySyncModel = steamLobbySyncModel;
			_steamModel = steamModel;
			_platformStatusService = platformStatusService;
			_steamModel.OnInitialized += HandleSteamInitialized;
		}

		private void HandleSteamInitialized()
		{
			this.OnSteamInitialized?.Invoke();
		}

		public void InviteToCurrentLobby()
		{
			if (_steamModel.IsSteamInitialized && _steamLobbyModel.HasActiveLobby)
			{
				SteamFriends.ActivateGameOverlayInviteDialog(_steamLobbyModel.CurrentLobbyId.Value);
			}
		}

		public void OpenCurrentLobby()
		{
			if (_steamModel.IsSteamInitialized && _steamLobbyModel.HasActiveLobby)
			{
				RefreshConnectRichPresence();
				if (IsLocalPlayerLobbyOwner())
				{
					SteamMatchmaking.SetLobbyJoinable(_steamLobbyModel.CurrentLobbyId.Value, bLobbyJoinable: true);
				}
			}
		}

		public void CloseCurrentLobby()
		{
			if (_steamModel.IsSteamInitialized && _steamLobbyModel.HasActiveLobby)
			{
				_platformStatusService.UpdateGenericStatus("connect", "");
				if (IsLocalPlayerLobbyOwner())
				{
					SteamMatchmaking.SetLobbyJoinable(_steamLobbyModel.CurrentLobbyId.Value, bLobbyJoinable: false);
				}
			}
		}

		public void LeaveCurrentLobby()
		{
			if (_steamModel.IsSteamInitialized && _steamLobbyModel.HasActiveLobby)
			{
				_platformStatusService.UpdateGenericStatus("connect", "");
				_platformStatusService.ClearPlayerGroup();
				SteamMatchmaking.LeaveLobby(_steamLobbyModel.CurrentLobbyId.Value);
				_steamLobbyModel.Clear();
			}
		}

		public bool IsLocalPlayerLobbyOwner()
		{
			if (!_steamModel.IsSteamInitialized || !_steamLobbyModel.HasActiveLobby)
			{
				return false;
			}
			return SteamMatchmaking.GetLobbyOwner(_steamLobbyModel.CurrentLobbyId.Value) == SteamUser.GetSteamID();
		}

		public void RefreshConnectRichPresence()
		{
			if (_steamModel.IsSteamInitialized && _steamLobbyModel.HasActiveLobby)
			{
				CSteamID value = _steamLobbyModel.CurrentLobbyId.Value;
				int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(value);
				_platformStatusService.UpdateGenericStatus("connect", value);
				_platformStatusService.UpdatePlayerGroup(value.ToString(), numLobbyMembers);
			}
		}

		public void WriteLobbyData(string sessionCode, string sessionRegion)
		{
			if (IsLocalPlayerLobbyOwner())
			{
				CSteamID value = _steamLobbyModel.CurrentLobbyId.Value;
				SteamMatchmaking.SetLobbyData(value, "SessionCode", sessionCode);
				SteamMatchmaking.SetLobbyData(value, "SessionRegion", sessionRegion);
			}
		}

		public void OpenFriendsOverlay()
		{
			if (_steamModel.IsSteamInitialized)
			{
				SteamFriends.ActivateGameOverlay("Friends");
			}
		}

		public void CreateSteamLobby(CSteamID lobbyId, string sessionCode, string sessionRegion)
		{
			_steamLobbyModel.SetCurrentLobby(lobbyId);
			SteamMatchmaking.SetLobbyData(lobbyId, "SessionCode", sessionCode);
			SteamMatchmaking.SetLobbyData(lobbyId, "SessionRegion", sessionRegion);
			_platformStatusService.UpdateGenericStatus("connect", lobbyId);
			SteamMatchmaking.SetLobbyJoinable(lobbyId, bLobbyJoinable: true);
			int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(lobbyId);
			_platformStatusService.UpdatePlayerGroup(lobbyId.ToString(), numLobbyMembers);
			_steamLobbySyncModel.SetSteamLobbyId(lobbyId.m_SteamID);
		}

		public void JoinLobby(CSteamID lobbyId)
		{
			SteamMatchmaking.JoinLobby(lobbyId);
		}

		public bool CheckLaunchCommandLineAndJoinLobby()
		{
			if (SteamLobbyJoinSessionState.HasConsumedLaunchLobbyJoin)
			{
				return false;
			}
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length - 1; i++)
			{
				if (!(commandLineArgs[i] != "+connect_lobby"))
				{
					if (!ulong.TryParse(commandLineArgs[i + 1], out var result))
					{
						break;
					}
					SteamLobbyJoinSessionState.MarkLaunchLobbyJoinConsumed();
					JoinLobby(new CSteamID(result));
					return true;
				}
			}
			return false;
		}

		public void CreateLobby(ELobbyType lobbyType, int maxSteamLobbyMembers)
		{
			SteamMatchmaking.CreateLobby(lobbyType, maxSteamLobbyMembers);
		}
	}
}
