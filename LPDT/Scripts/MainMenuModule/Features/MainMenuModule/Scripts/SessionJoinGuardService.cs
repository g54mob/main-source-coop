using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Features.MainMenuModule.Scripts
{
	public class SessionJoinGuardService : ISessionJoinGuardService
	{
		private readonly struct SessionProbeResult
		{
			public static readonly SessionProbeResult NotFound = new SessionProbeResult(found: false, null);

			public bool Found { get; }

			public SessionInfo SessionInfo { get; }

			public SessionProbeResult(bool found, SessionInfo sessionInfo)
			{
				Found = found;
				SessionInfo = sessionInfo;
			}
		}

		private sealed class SessionListValidationCallbacks : INetworkRunnerCallbacks, IPublicFacingInterface
		{
			private readonly string _sessionName;

			private readonly UniTaskCompletionSource<SessionProbeResult> _firstResult = new UniTaskCompletionSource<SessionProbeResult>();

			private SessionProbeResult _latest = SessionProbeResult.NotFound;

			private bool _hasLatest;

			private bool _terminated;

			public SessionListValidationCallbacks(string sessionName)
			{
				_sessionName = sessionName;
			}

			public async UniTask<SessionProbeResult> WaitForResultAsync(int timeoutMilliseconds)
			{
				try
				{
					return await _firstResult.Task.Timeout(TimeSpan.FromMilliseconds(timeoutMilliseconds));
				}
				catch (TimeoutException)
				{
					return SessionProbeResult.NotFound;
				}
				catch (OperationCanceledException)
				{
					return SessionProbeResult.NotFound;
				}
			}

			public async UniTask<SessionProbeResult> WaitForSettledResultAsync(Func<SessionProbeResult, bool> acceptable, int timeoutMilliseconds, SessionProbeResult fallback)
			{
				int elapsed = 0;
				SessionProbeResult latest = fallback;
				for (; elapsed < timeoutMilliseconds; elapsed += 150)
				{
					if (_terminated)
					{
						return SessionProbeResult.NotFound;
					}
					if (_hasLatest)
					{
						latest = _latest;
						if (acceptable(latest))
						{
							return latest;
						}
					}
					await UniTask.Delay(150, ignoreTimeScale: true);
				}
				if (_terminated)
				{
					return SessionProbeResult.NotFound;
				}
				return _hasLatest ? _latest : latest;
			}

			public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
			{
				int num = sessionList.FindIndex((SessionInfo session) => session.Name == _sessionName);
				SessionProbeResult result = (_latest = ((num < 0) ? SessionProbeResult.NotFound : new SessionProbeResult(found: true, sessionList[num])));
				_hasLatest = true;
				_firstResult.TrySetResult(result);
			}

			public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
			{
			}

			public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
			{
			}

			public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
			{
			}

			public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
			{
			}

			public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
			{
				_terminated = true;
				_firstResult.TrySetResult(SessionProbeResult.NotFound);
			}

			public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
			{
				_terminated = true;
				_firstResult.TrySetResult(SessionProbeResult.NotFound);
			}

			public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
			{
			}

			public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
			{
				_terminated = true;
				_firstResult.TrySetResult(SessionProbeResult.NotFound);
			}

			public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
			{
			}

			public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
			{
			}

			public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
			{
			}

			public void OnInput(NetworkRunner runner, NetworkInput input)
			{
			}

			public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
			{
			}

			public void OnConnectedToServer(NetworkRunner runner)
			{
			}

			public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
			{
			}

			public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
			{
			}

			public void OnSceneLoadDone(NetworkRunner runner)
			{
			}

			public void OnSceneLoadStart(NetworkRunner runner)
			{
			}
		}

		private const int SessionValidationTimeoutMilliseconds = 5000;

		private const int SessionSettleTimeoutMilliseconds = 4000;

		private readonly IMultiplayerService _multiplayerService;

		private readonly ISessionRecoverySource _sessionRecoverySource;

		public SessionJoinGuardService(IMultiplayerService multiplayerService, ISessionRecoverySource sessionRecoverySource)
		{
			_multiplayerService = multiplayerService;
			_sessionRecoverySource = sessionRecoverySource;
		}

		public async UniTask<bool> IsSessionValidAsync(string sessionName)
		{
			if (string.IsNullOrEmpty(sessionName))
			{
				return false;
			}
			SessionProbeResult sessionProbeResult = await ProbeSessionAsync(sessionName, SessionInProgressJoinPolicy.FastFail);
			if (!sessionProbeResult.Found)
			{
				return false;
			}
			return IsSessionStarted(sessionProbeResult.SessionInfo);
		}

		public async UniTask<bool> IsJoinBlockedAsync(string sessionName, SessionInProgressJoinPolicy policy = SessionInProgressJoinPolicy.FastFail)
		{
			if (string.IsNullOrEmpty(sessionName))
			{
				return false;
			}
			SessionProbeResult sessionProbeResult = await ProbeSessionAsync(sessionName, policy);
			if (!sessionProbeResult.Found)
			{
				return false;
			}
			if (!IsSessionStarted(sessionProbeResult.SessionInfo))
			{
				return false;
			}
			string sessionName2;
			return !_sessionRecoverySource.TryGetReconnectSession(out sessionName2) || !(sessionName2 == sessionName);
		}

		private bool IsSessionStarted(SessionInfo sessionInfo)
		{
			bool value;
			return _multiplayerService.TryGetSessionProperty<bool>(sessionInfo, SessionPropertyType.IsSessionStarted, out value) && value;
		}

		private async UniTask<SessionProbeResult> ProbeSessionAsync(string sessionName, SessionInProgressJoinPolicy policy)
		{
			NetworkRunner tempRunner = null;
			SessionListValidationCallbacks validationCallbacks = null;
			SessionProbeResult result;
			try
			{
				_ = 2;
				try
				{
					GameObject gameObject = new GameObject("SessionProbeRunner");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
					tempRunner = gameObject.AddComponent<NetworkRunner>();
					validationCallbacks = new SessionListValidationCallbacks(sessionName);
					tempRunner.AddCallbacks(validationCallbacks);
					StartGameResult startGameResult = await _multiplayerService.JoinSessionLobby(tempRunner);
					if (startGameResult == null || !startGameResult.Ok)
					{
						result = SessionProbeResult.NotFound;
					}
					else
					{
						SessionProbeResult sessionProbeResult = await validationCallbacks.WaitForResultAsync(5000);
						if (policy == SessionInProgressJoinPolicy.SettleThenBlock && sessionProbeResult.Found && IsSessionStarted(sessionProbeResult.SessionInfo))
						{
							sessionProbeResult = await validationCallbacks.WaitForSettledResultAsync((SessionProbeResult snapshot) => !snapshot.Found || !IsSessionStarted(snapshot.SessionInfo), 4000, sessionProbeResult);
						}
						result = sessionProbeResult;
					}
				}
				catch (Exception)
				{
					result = SessionProbeResult.NotFound;
				}
			}
			finally
			{
				if (tempRunner != null)
				{
					if (validationCallbacks != null)
					{
						tempRunner.RemoveCallbacks(validationCallbacks);
					}
					if (tempRunner.IsRunning)
					{
						await _multiplayerService.Shutdown(tempRunner, ShutdownReason.Ok);
					}
					UnityEngine.Object.Destroy(tempRunner.gameObject);
				}
			}
			return result;
		}
	}
}
