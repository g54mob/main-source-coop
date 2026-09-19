using Cysharp.Threading.Tasks;
using Fusion;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface IStartSessionService
	{
		UniTask<CreateRoomResult> StartCreateRoomTask(SessionCreateData sessionData);

		UniTask<JoinRoomResult> StartJoinRoomTask(SessionCreateData sessionData, bool isReconnectAttempt = false, bool bypassInProgressGuard = false);

		UniTask<bool> StartJoinSessionLobbyTask(string lobbyCode = null);

		UniTask<bool> Shutdown(ShutdownReason shutdownReason);
	}
}
