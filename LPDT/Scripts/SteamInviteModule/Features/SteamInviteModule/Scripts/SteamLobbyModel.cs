using Steamworks;

namespace Features.SteamInviteModule.Scripts
{
	public class SteamLobbyModel
	{
		public CSteamID? CurrentLobbyId { get; private set; }

		public bool HasActiveLobby => CurrentLobbyId?.IsValid() ?? false;

		public void SetCurrentLobby(CSteamID lobbyId)
		{
			CurrentLobbyId = lobbyId;
		}

		public void Clear()
		{
			CurrentLobbyId = null;
		}
	}
}
