using System;
using Cysharp.Threading.Tasks;
using Features.ProgressSavingModule.Scripts.Implementation;
using Fusion;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class StartSessionService : IStartSessionService
	{
		private IMultiplayerService _multiplayerService;

		private NetworkRunnerEventBus _eventBus;

		private MultiplayerModel _multiplayerModel;

		private IMultiplayerFactory _multiplayerFactory;

		private ISavingService _savingService;

		private ISessionRecoverySource _sessionRecoverySource;

		[Inject]
		public void InjectDependencies(IMultiplayerService multiplayerService, NetworkRunnerEventBus eventBus, MultiplayerModel multiplayerModel, IMultiplayerFactory multiplayerFactory, ISavingService savingService, ISessionRecoverySource sessionRecoverySource)
		{
			_multiplayerService = multiplayerService;
			_eventBus = eventBus;
			_multiplayerModel = multiplayerModel;
			_multiplayerFactory = multiplayerFactory;
			_savingService = savingService;
			_sessionRecoverySource = sessionRecoverySource;
		}

		public async UniTask<CreateRoomResult> StartCreateRoomTask(SessionCreateData sessionData)
		{
			_multiplayerModel.IsReconnecting = false;
			if (sessionData.JoinSource == JoinSource.Unknown)
			{
				sessionData.JoinSource = JoinSource.Host;
			}
			_multiplayerModel.LocalJoinSource = sessionData.JoinSource;
			CreateRoomResult taskResult = new CreateRoomResult(isSuccess: false);
			try
			{
				SavePlayerProfile();
				EnsureStartableNetworkRunner();
				StartGameResult startGameResult = await _multiplayerService.CreateRoom(_multiplayerModel.NetworkRunner, sessionData);
				if (startGameResult != null && startGameResult.Ok && _multiplayerModel.NetworkRunner != null)
				{
					PlayerSessionPrefs.Save(_multiplayerModel.NetworkRunner.SessionInfo.Name);
					_eventBus.Publish(new OnSuccessfullyStartGameEvent(startedAsHost: true, _multiplayerModel.NetworkRunner.SessionInfo.Name, _multiplayerModel.NetworkRunner.SessionInfo.Region));
				}
				_eventBus.Publish(new OnFailedStartGameEvent(startGameResult.ErrorMessage, startGameResult.ShutdownReason, startGameResult.StackTrace));
				if (startGameResult != null)
				{
					taskResult.IsSuccess = startGameResult.Ok;
					taskResult.ErrorMessage = startGameResult.ErrorMessage;
					taskResult.ShutdownReason = startGameResult.ShutdownReason;
				}
			}
			catch (Exception ex)
			{
				taskResult.ErrorMessage = ex.Message;
				taskResult.ShutdownReason = ShutdownReason.Error;
				Debug.LogException(ex);
				return taskResult;
			}
			return taskResult;
		}

		public async UniTask<JoinRoomResult> StartJoinRoomTask(SessionCreateData sessionData, bool isReconnectAttempt = false, bool bypassInProgressGuard = false)
		{
			if (!isReconnectAttempt)
			{
				_multiplayerModel.IsReconnecting = false;
			}
			if (isReconnectAttempt)
			{
				sessionData.JoinSource = JoinSource.Reconnect;
			}
			_multiplayerModel.LocalJoinSource = sessionData.JoinSource;
			JoinRoomResult taskResult = new JoinRoomResult(isSuccess: false);
			try
			{
				SavePlayerProfile();
				EnsureStartableNetworkRunner();
				StartGameResult startGameResult = await _multiplayerService.ConnectToRoom(_multiplayerModel.NetworkRunner, sessionData);
				if (startGameResult != null && startGameResult.Ok && _multiplayerModel.NetworkRunner != null)
				{
					string name = _multiplayerModel.NetworkRunner.SessionInfo.Name;
					bool flag = IsJoinedSessionInProgress(_multiplayerModel.NetworkRunner);
					string sessionName;
					bool flag2 = _sessionRecoverySource.TryGetReconnectSession(out sessionName) && sessionName == name;
					if (!bypassInProgressGuard && !isReconnectAttempt && flag && !flag2)
					{
						await _multiplayerService.Shutdown(_multiplayerModel.NetworkRunner, ShutdownReason.Ok);
						taskResult.IsSuccess = false;
						taskResult.FailureReason = JoinFailureReason.SessionInProgress;
						return taskResult;
					}
					PlayerSessionPrefs.Save(name);
					_eventBus.Publish(new OnSuccessfullyStartGameEvent(startedAsHost: false, name, _multiplayerModel.NetworkRunner.SessionInfo.Region));
				}
				else if (startGameResult != null)
				{
					_eventBus.Publish(new OnFailedStartGameEvent(startGameResult.ErrorMessage, startGameResult.ShutdownReason, startGameResult.StackTrace));
				}
				if (startGameResult != null)
				{
					taskResult.IsSuccess = startGameResult.Ok;
					taskResult.ErrorMessage = startGameResult.ErrorMessage;
					taskResult.ShutdownReason = startGameResult.ShutdownReason;
				}
			}
			catch (Exception ex)
			{
				taskResult.ErrorMessage = ex.Message;
				taskResult.ShutdownReason = ShutdownReason.Error;
				Debug.LogException(ex);
				return taskResult;
			}
			return taskResult;
		}

		private bool IsJoinedSessionInProgress(NetworkRunner runner)
		{
			if (runner == null || runner.SessionInfo.Properties == null)
			{
				return false;
			}
			try
			{
				return _multiplayerService.GetSessionProperty<bool>(runner, SessionPropertyType.IsSessionStarted);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async UniTask<bool> StartJoinSessionLobbyTask(string lobbyCode = null)
		{
			bool taskResult = false;
			try
			{
				EnsureStartableNetworkRunner();
				StartGameResult startGameResult = await _multiplayerService.JoinSessionLobby(_multiplayerModel.NetworkRunner, lobbyCode);
				if (startGameResult != null)
				{
					taskResult = startGameResult.Ok;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			return taskResult;
		}

		public async UniTask<bool> Shutdown(ShutdownReason shutdownReason)
		{
			try
			{
				if (_multiplayerModel.NetworkRunner == null)
				{
					_multiplayerModel.NetworkRunner = _multiplayerFactory.CreateNetworkRunner();
				}
				await _multiplayerService.Shutdown(_multiplayerModel.NetworkRunner, shutdownReason);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return false;
			}
			return true;
		}

		private void EnsureStartableNetworkRunner()
		{
			if (_multiplayerModel.NetworkRunner != null)
			{
				UnityEngine.Object.DestroyImmediate(_multiplayerModel.NetworkRunner.gameObject);
			}
			_multiplayerModel.NetworkRunner = _multiplayerFactory.CreateNetworkRunner();
		}

		private void SavePlayerProfile()
		{
			_savingService.SaveDataForGroup(SavingGroup.PlayerProfile);
		}
	}
}
