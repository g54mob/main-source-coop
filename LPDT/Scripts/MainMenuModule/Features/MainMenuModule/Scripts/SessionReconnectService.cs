using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Fusion;

namespace Features.MainMenuModule.Scripts
{
	public class SessionReconnectService : ISessionReconnectService
	{
		private readonly IStartSessionService _startSessionService;

		private readonly ISessionJoinGuardService _sessionJoinGuardService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ILoadingScreenService _loadingScreenService;

		public SessionReconnectService(IStartSessionService startSessionService, ISessionJoinGuardService sessionJoinGuardService, MultiplayerModel multiplayerModel, ILoadingScreenService loadingScreenService)
		{
			_startSessionService = startSessionService;
			_sessionJoinGuardService = sessionJoinGuardService;
			_multiplayerModel = multiplayerModel;
			_loadingScreenService = loadingScreenService;
		}

		public UniTask<bool> IsSessionValidAsync(string sessionName)
		{
			return _sessionJoinGuardService.IsSessionValidAsync(sessionName);
		}

		public UniTask<bool> IsJoinBlockedAsync(string sessionName, SessionInProgressJoinPolicy policy = SessionInProgressJoinPolicy.FastFail)
		{
			return _sessionJoinGuardService.IsJoinBlockedAsync(sessionName, policy);
		}

		public async UniTask<bool> TryReconnectAsync(string sessionName)
		{
			_multiplayerModel.IsReconnecting = true;
			_multiplayerModel.IsOperationInProgress = true;
			await _loadingScreenService.ShowAsync(LoadingScreenShowType.ShowUntilPlayersLoading);
			bool result = default(bool);
			int num;
			try
			{
				JoinRoomResult obj = await _startSessionService.StartJoinRoomTask(new SessionCreateData
				{
					RoomCode = sessionName,
					JoinSource = JoinSource.Reconnect
				}, isReconnectAttempt: true);
				_multiplayerModel.IsOperationInProgress = false;
				if (obj.IsSuccess)
				{
					result = true;
					return result;
				}
				await HandleFailedReconnectAsync();
				result = false;
				return result;
			}
			catch
			{
				num = 1;
			}
			if (num != 1)
			{
				return result;
			}
			_multiplayerModel.IsOperationInProgress = false;
			await HandleFailedReconnectAsync();
			return false;
		}

		private async UniTask HandleFailedReconnectAsync()
		{
			_multiplayerModel.IsReconnecting = false;
			if (_multiplayerModel.NetworkRunner != null && _multiplayerModel.NetworkRunner.IsRunning)
			{
				await _startSessionService.Shutdown(ShutdownReason.Ok);
			}
			await _loadingScreenService.FadeOutAsync();
		}
	}
}
