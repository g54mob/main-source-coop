using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using UnityEngine;

namespace Features.MainMenuModule.Scripts
{
	public class QuickJoinService : IQuickJoinService
	{
		private readonly struct RankedSession
		{
			public SessionInfo Session { get; }

			public int Ping { get; }

			public RankedSession(SessionInfo session, int ping)
			{
				Session = session;
				Ping = ping;
			}
		}

		private sealed class SessionListBrowseCallbacks : INetworkRunnerCallbacks, IPublicFacingInterface
		{
			private readonly UniTaskCompletionSource<IReadOnlyList<SessionInfo>> _completionSource = new UniTaskCompletionSource<IReadOnlyList<SessionInfo>>();

			public async UniTask<IReadOnlyList<SessionInfo>> WaitForResultAsync(int timeoutMilliseconds, CancellationToken cancellationToken)
			{
				try
				{
					return await _completionSource.Task.AttachExternalCancellation(cancellationToken).Timeout(TimeSpan.FromMilliseconds(timeoutMilliseconds));
				}
				catch (TimeoutException)
				{
					return Array.Empty<SessionInfo>();
				}
				catch (OperationCanceledException)
				{
					return Array.Empty<SessionInfo>();
				}
			}

			public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
			{
				_completionSource.TrySetResult(sessionList);
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
				_completionSource.TrySetResult(Array.Empty<SessionInfo>());
			}

			public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
			{
				_completionSource.TrySetResult(Array.Empty<SessionInfo>());
			}

			public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
			{
			}

			public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
			{
				_completionSource.TrySetResult(Array.Empty<SessionInfo>());
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

		private const int SessionListTimeoutMilliseconds = 3500;

		private const int MaxRegionsToBrowse = 3;

		private readonly IMultiplayerService _multiplayerService;

		private readonly IStartSessionService _startSessionService;

		private readonly RegionsPingModel _regionsPingModel;

		public QuickJoinService(IMultiplayerService multiplayerService, IStartSessionService startSessionService, RegionsPingModel regionsPingModel)
		{
			_multiplayerService = multiplayerService;
			_startSessionService = startSessionService;
			_regionsPingModel = regionsPingModel;
		}

		public async UniTask<QuickJoinResult> FindOpenSessionAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			List<RankedSession> candidates = new List<RankedSession>();
			List<RegionInfo> regionsToBrowse = GetRegionsToBrowse();
			if (regionsToBrowse.Count == 0)
			{
				IReadOnlyList<SessionInfo> sessions = await BrowseSessionsAsync(cancellationToken);
				if (cancellationToken.IsCancellationRequested)
				{
					return QuickJoinResult.NotFound;
				}
				CollectJoinableSessions(sessions, candidates);
			}
			else
			{
				string originalRegion = PhotonAppSettings.Global.AppSettings.FixedRegion;
				try
				{
					foreach (RegionInfo region in regionsToBrowse)
					{
						if (!cancellationToken.IsCancellationRequested)
						{
							PhotonAppSettings.Global.AppSettings.FixedRegion = region.RegionCode;
							CollectJoinableSessions(await BrowseSessionsAsync(cancellationToken), candidates, region.RegionPing);
							continue;
						}
						break;
					}
				}
				finally
				{
					PhotonAppSettings.Global.AppSettings.FixedRegion = originalRegion;
				}
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return QuickJoinResult.NotFound;
			}
			return SelectBest(candidates);
		}

		public UniTask<JoinRoomResult> JoinSessionAsync(string sessionName, string region)
		{
			if (!string.IsNullOrEmpty(region))
			{
				PhotonAppSettings.Global.AppSettings.FixedRegion = region;
			}
			return _startSessionService.StartJoinRoomTask(new SessionCreateData
			{
				RoomCode = sessionName,
				JoinSource = JoinSource.Matchmaking
			});
		}

		private List<RegionInfo> GetRegionsToBrowse()
		{
			List<RegionInfo> list = new List<RegionInfo>(_regionsPingModel.Regions);
			list.Sort((RegionInfo left, RegionInfo right) => left.RegionPing.CompareTo(right.RegionPing));
			if (list.Count > 3)
			{
				list.RemoveRange(3, list.Count - 3);
			}
			return list;
		}

		private void CollectJoinableSessions(IReadOnlyList<SessionInfo> sessions, List<RankedSession> candidates, int regionPing = int.MinValue)
		{
			foreach (SessionInfo session in sessions)
			{
				if (IsJoinableOpenSession(session))
				{
					int ping = ((regionPing == int.MinValue) ? GetRegionPing(session.Region) : regionPing);
					candidates.Add(new RankedSession(session, ping));
				}
			}
		}

		private QuickJoinResult SelectBest(List<RankedSession> candidates)
		{
			RankedSession rankedSession = default(RankedSession);
			bool flag = false;
			foreach (RankedSession candidate in candidates)
			{
				if (!flag || candidate.Ping < rankedSession.Ping || (candidate.Ping == rankedSession.Ping && candidate.Session.PlayerCount > rankedSession.Session.PlayerCount))
				{
					rankedSession = candidate;
					flag = true;
				}
			}
			if (!flag)
			{
				return QuickJoinResult.NotFound;
			}
			_multiplayerService.TryGetSessionProperty<string>(rankedSession.Session, SessionPropertyType.HostName, out var value);
			int regionPing = ((rankedSession.Ping == int.MaxValue) ? (-1) : rankedSession.Ping);
			return new QuickJoinResult(found: true, rankedSession.Session.Name, value, rankedSession.Session.PlayerCount, rankedSession.Session.MaxPlayers, rankedSession.Session.Region, regionPing);
		}

		private int GetRegionPing(string region)
		{
			if (string.IsNullOrEmpty(region))
			{
				return int.MaxValue;
			}
			foreach (RegionInfo region2 in _regionsPingModel.Regions)
			{
				if (region2.RegionCode == region)
				{
					return region2.RegionPing;
				}
			}
			return int.MaxValue;
		}

		private bool IsJoinableOpenSession(SessionInfo session)
		{
			if (!session.IsValid || !session.IsVisible || !session.IsOpen)
			{
				return false;
			}
			if (session.PlayerCount >= session.MaxPlayers)
			{
				return false;
			}
			if (!_multiplayerService.TryGetSessionProperty<bool>(session, SessionPropertyType.IsPublic, out var value) || !value)
			{
				return false;
			}
			if (!_multiplayerService.TryGetSessionProperty<bool>(session, SessionPropertyType.IsSessionStarted, out var value2) || value2)
			{
				return false;
			}
			if (!_multiplayerService.TryGetSessionProperty<string>(session, SessionPropertyType.Version, out var value3) || value3 != Application.version)
			{
				return false;
			}
			return true;
		}

		private async UniTask<IReadOnlyList<SessionInfo>> BrowseSessionsAsync(CancellationToken cancellationToken)
		{
			NetworkRunner tempRunner = null;
			SessionListBrowseCallbacks browseCallbacks = null;
			IReadOnlyList<SessionInfo> result;
			try
			{
				_ = 1;
				try
				{
					GameObject gameObject = new GameObject("QuickJoinBrowseRunner");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
					tempRunner = gameObject.AddComponent<NetworkRunner>();
					browseCallbacks = new SessionListBrowseCallbacks();
					tempRunner.AddCallbacks(browseCallbacks);
					StartGameResult startGameResult = await _multiplayerService.JoinSessionLobby(tempRunner);
					result = ((startGameResult != null && startGameResult.Ok) ? (await browseCallbacks.WaitForResultAsync(3500, cancellationToken)) : Array.Empty<SessionInfo>());
				}
				catch (Exception)
				{
					result = Array.Empty<SessionInfo>();
				}
			}
			finally
			{
				if (tempRunner != null)
				{
					if (browseCallbacks != null)
					{
						tempRunner.RemoveCallbacks(browseCallbacks);
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
