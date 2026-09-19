using System;
using Steamworks;

namespace Features.SteamInviteModule.Scripts
{
	public interface ISteamInviteService
	{
		event Action OnSteamInitialized;

		void InviteToCurrentLobby();

		void OpenCurrentLobby();

		void CloseCurrentLobby();

		void LeaveCurrentLobby();

		void OpenFriendsOverlay();

		void CreateSteamLobby(CSteamID lobbyId, string sessionCode, string sessionRegion);

		void JoinLobby(CSteamID lobbyId);

		bool CheckLaunchCommandLineAndJoinLobby();

		void CreateLobby(ELobbyType lobbyType, int maxSteamLobbyMembers);

		bool IsLocalPlayerLobbyOwner();

		void RefreshConnectRichPresence();

		void WriteLobbyData(string sessionCode, string sessionRegion);
	}
}
