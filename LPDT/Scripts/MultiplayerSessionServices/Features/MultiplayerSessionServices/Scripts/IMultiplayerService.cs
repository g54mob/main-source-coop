using Cysharp.Threading.Tasks;
using Fusion;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface IMultiplayerService
	{
		UniTask<StartGameResult> CreateRoom(NetworkRunner runner, SessionCreateData sessionData);

		UniTask<StartGameResult> ConnectToRoom(NetworkRunner runner, SessionCreateData sessionData);

		UniTask<StartGameResult> JoinSessionLobby(NetworkRunner runner, string lobbyCode = null);

		UniTask Shutdown(NetworkRunner runner, ShutdownReason shutdownReason);

		bool TryGetPlayerRefById(int playerId, out PlayerRef playerRef);

		void KickFromRoom(NetworkRunner runner, int playerId);

		void SetSessionPublic(NetworkRunner runner, bool isPublic);

		void MarkSessionStarted(NetworkRunner runner);

		void RefreshSessionVisibility(NetworkRunner runner, bool? isPublicOverride = null, bool? isSessionStartedOverride = null);

		void OverrideSessionProperty<TProperty>(NetworkRunner runner, SessionPropertyType propertyType, TProperty propertyValue);

		TProperty GetSessionProperty<TProperty>(NetworkRunner runner, SessionPropertyType propertyType);

		bool TryGetSessionProperty<TProperty>(SessionInfo sessionInfo, SessionPropertyType propertyType, out TProperty value);
	}
}
