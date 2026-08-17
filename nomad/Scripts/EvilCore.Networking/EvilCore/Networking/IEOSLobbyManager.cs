using System;
using System.Collections.Generic;

namespace EvilCore.Networking
{
	public interface IEOSLobbyManager
	{
		string CurrentLobbyId { get; }

		bool IsInLobby { get; }

		bool IsOwner { get; }

		bool PlayerInLobby { get; }

		string JoinCode { get; }

		bool WasKicked { get; }

		string LocalProductUserId { get; }

		int PendingWorldSeed { get; }

		IReadOnlyDictionary<string, string> Members { get; }

		event Action OnLobbyCreated;

		event Action OnLobbyJoined;

		event Action OnLobbyLeft;

		event Action OnLobbyClosed;

		event Action<string, string> OnMemberJoined;

		event Action<string> OnMemberLeft;

		void ConsumeKickedFlag();

		void SetPendingWorldSeed(int seed);

		void CreateGameWithLobby();

		void CreateGameWithLobby(LobbyCreateOptions options);

		void JoinLobbyById(string lobbyId);

		void LeaveLobby();

		void SearchLobbies(Action<List<LobbySearchResult>> callback = null, int maxResults = 50);

		void SearchLobbyByCode(string code, Action<LobbySearchResult?> callback);

		void KickMember(string memberId);

		void BanMember(string memberId);

		bool IsBanned(string memberId);

		bool ValidatePassword(LobbySearchResult lobby, string enteredPassword);

		bool IsVersionCompatible(LobbySearchResult lobby);
	}
}
