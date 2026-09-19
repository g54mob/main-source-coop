#define TRACE
#define FUSION_UNITY
#define DEBUG
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Fusion.Async;
using Fusion.Encryption;
using Fusion.Matchmaking;
using Fusion.Matchmaking.Extensions;
using Fusion.Photon.Realtime;
using Fusion.Protocol;
using Fusion.Sockets;
using Fusion.Sockets.Stun;
using Photon.Client;
using Photon.Realtime;

namespace Fusion
{
	internal class CloudServices : IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks, IInRoomCallbacks, IDisposable
	{
		private static class ErrorMessages
		{
			public const string StartBeforeJoin = "Received Start Message, but never a Join Confirmation. Shutdown.";

			public const string RunnerFailInit = "Runner failed to Initialize. Shutdown.";

			public const string JoinTimeout = "Join Confirmation timeout. Shutdown.";
		}

		private readonly CloudServicesMetadata _metadata;

		private readonly NetworkRunner _runner;

		private CloudCommunicator _communicator;

		private readonly Dictionary<string, SessionInfo> _cachedSessionList = new Dictionary<string, SessionInfo>();

		private AsyncOperationHandler<Join> _joinAsyncHandler;

		private byte[] _dummyData;

		private CancellationTokenSource _dummyTrafficCts;

		private CancellationTokenSource _dummyTrafficLinkCts;

		public bool IsCloudReady => (_communicator?.Client)?.IsConnectedAndReady ?? false;

		public string UserId => IsCloudReady ? _communicator.Client.UserId : null;

		public bool IsInRoom => IsCloudReady && _communicator.Client.InRoom;

		public bool IsInLobby => IsCloudReady && _communicator.Client.InLobby;

		public JoinProcessStage CurrentJoinStage => _metadata.CurrentJoinStage;

		public ProtocolMessageVersion CurrentProtocolMessageVersion => _metadata.CurrentProtocolMessageVersion;

		public int SessionSlots => IsInRoom ? _communicator.Client.CurrentRoom.MaxPlayers : (-1);

		public bool IsMasterClient => IsInRoom && _communicator.Client.LocalPlayer.IsMasterClient;

		public int MasterClientActorId
		{
			get
			{
				if (!IsInRoom)
				{
					return 0;
				}
				return _communicator.Client.CurrentRoom.MasterClientId;
			}
		}

		public (double average, double last) RoundTripTime
		{
			get
			{
				(float, float) tuple = (IsInRoom ? ((float)_communicator.Client.RealtimePeer.Stats.RoundtripTime / 1000f, (float)_communicator.Client.RealtimePeer.Stats.LastRoundtripTime / 1000f) : (0f, 0f));
				return (average: tuple.Item1, last: tuple.Item2);
			}
		}

		public AuthenticationValues AuthenticationValues => IsCloudReady ? _communicator.Client.AuthValues : null;

		public ICommunicator Communicator => _communicator;

		public string CachedRegionSummary => _communicator.Client.SummaryToCache;

		public bool IsNATPunchthroughEnabled { get; internal set; } = true;

		public bool IsEncryptionEnabled => IsCloudReady && _communicator.Client.IsEncryptionEnabled();

		public string CustomSTUNServer { get; internal set; } = null;

		public NATType NATType => (_metadata?.LocalReflexiveInfo != null) ? _metadata.LocalReflexiveInfo.NatType : NATType.Invalid;

		public PlayerRef LocalPlayerRef => PlayerRef.FromIndex((_metadata?.PlayerRef).GetValueOrDefault());

		private bool IsServerOrMasterClient => _runner != null && (_runner.IsServer || _runner.IsSharedModeMasterClient);

		public void OnConnected()
		{
		}

		public void OnConnectedToMaster()
		{
		}

		public void OnCustomAuthenticationFailed(string debugMessage)
		{
			OperationFailHandler(32755, debugMessage);
		}

		public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
			_runner?.InvokeCustomAuthenticationResponse(data);
		}

		public void OnDisconnected(DisconnectCause cause)
		{
			ShutdownReason shutdownReason = DisconnectCauseExt.ConvertToShutdownReason(cause);
			if ((int)NetworkRunner.CloudConnectionLostCurrentMode >= 1 && HandlePhotonCloudDisconnect(shutdownReason))
			{
				InternalLogStreams.LogDebug?.Log("Photon Cloud Disconnect handled internally");
				return;
			}
			InternalLogStreams.LogDebug?.Log(_runner, $"Disconnected from Photon Cloud: {cause}/{shutdownReason}");
			string text = null;
			if (_metadata.LastDisconnectMsg != null)
			{
				shutdownReason = DisconnectReasonExt.ConvertToShutdownReason(_metadata.LastDisconnectMsg.DisconnectReason);
				text = _metadata.LastDisconnectMsg.CustomData;
				InternalLogStreams.LogDebug?.Warn(_runner, string.Format("Fusion Disconnect: {0}={1}, Message={2}", "DisconnectReason", _metadata.LastDisconnectMsg.DisconnectReason, text));
			}
			StartGameException exception = new StartGameException(shutdownReason, text);
			_joinAsyncHandler?.SetException(exception);
			if (_runner.StartGameOperation != null)
			{
				_runner.StartGameOperation.SetException(exception);
			}
			else
			{
				_runner.Shutdown(destroyGameObject: true, shutdownReason, forceShutdownProcedure: true);
			}
		}

		public void OnRegionListReceived(RegionHandler regionHandler)
		{
			string text = string.Join(", ", from region in regionHandler.EnabledRegions.AsEnumerable()
				select region.Code + "[" + region.HostAndPort + "]");
			InternalLogStreams.LogDebug?.Log("OnRegionListReceived: EnabledRegions=" + text);
		}

		public void OnCreatedRoom()
		{
			InternalLogStreams.LogDebug?.Log(_runner, "Created Session: " + _communicator.Client.CurrentRoom.Name);
		}

		public void OnJoinedRoom()
		{
			InternalLogStreams.LogDebug?.Log(_runner, "Joined Session: " + _communicator.Client.CurrentRoom.Name);
			_metadata.RejoinMetadata.Reset();
			_runner.LobbyInfo.Reset();
		}

		public void OnLeftRoom()
		{
			InternalLogStreams.LogDebug?.Log(_runner, "Left Session");
			_runner.LobbyInfo.Reset();
		}

		public void OnFriendListUpdate(List<FriendInfo> friendList)
		{
		}

		public void OnCreateRoomFailed(short returnCode, string message)
		{
			OperationFailHandler(returnCode, message);
		}

		public void OnJoinRandomFailed(short returnCode, string message)
		{
			OperationFailHandler(returnCode, message);
		}

		public void OnJoinRoomFailed(short returnCode, string message)
		{
			OperationFailHandler(returnCode, message);
		}

		public void OnJoinedLobby()
		{
			_runner.LobbyInfo.IsValid = true;
			_runner.LobbyInfo.Name = _communicator.Client.CurrentLobby.Name;
			_runner.LobbyInfo.Region = _communicator.Client.CurrentRegion.Replace("/*", "");
			InternalLogStreams.LogDebug?.Log(_runner, "Joined Lobby: " + _runner.LobbyInfo.Name + ", Region=" + _runner.LobbyInfo.Region);
		}

		public void OnLeftLobby()
		{
			_runner.LobbyInfo.Reset();
			InternalLogStreams.LogDebug?.Log(_runner, "Left Lobby");
		}

		public void OnRoomListUpdate(List<RoomInfo> roomList)
		{
			OnRoomListChanged(roomList);
		}

		public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
		{
		}

		private void OperationFailHandler(short returnCode, string message)
		{
			InternalLogStreams.LogDebug?.Warn(_runner, $"Photon Cloud Operation failed [{returnCode}]: '{message}'");
			ShutdownReason shutdownReason = ErrorCodeExt.ConvertToShutdownReason(returnCode);
			if (_runner.StartGameOperation != null)
			{
				_runner.StartGameOperation.SetException(new StartGameException(shutdownReason, message));
			}
			else if ((int)NetworkRunner.CloudConnectionLostCurrentMode >= 1 && HandlePhotonCloudDisconnect(shutdownReason))
			{
				InternalLogStreams.LogDebug?.Log("Photon Cloud Operation handled internally");
			}
			else
			{
				_runner.Shutdown(destroyGameObject: true, shutdownReason, forceShutdownProcedure: true);
			}
		}

		public void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
		{
		}

		public void OnMasterClientSwitched(Player newMasterClient)
		{
			OnRoomChanged();
		}

		public void OnPlayerEnteredRoom(Player newPlayer)
		{
			OnRoomChanged();
		}

		public void OnPlayerLeftRoom(Player otherPlayer)
		{
			OnRoomChanged();
		}

		public void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
		{
			OnRoomChanged();
		}

		public CloudServices(NetworkRunner runner, CloudCommunicator communicator = null, RealtimeClient client = null, FusionAppSettings appSettings = null)
		{
			_runner = runner;
			TaskManager.Setup();
			_communicator = communicator ?? new CloudCommunicator(client, appSettings);
			_communicator.Client.AddCallbackTarget(this);
			_communicator.Client.AddressRewriter = runner.CloudAddressRewriter;
			_communicator.WasExtracted = false;
			_communicator.RegisterPackageCallback<Join>(HandleJoinMessage);
			_communicator.RegisterPackageCallback<Start>(HandleStartMessage);
			_communicator.RegisterPackageCallback<Disconnect>(HandleDisconnectMessage);
			_communicator.RegisterPackageCallback<ReflexiveInfo>(HandleReflexiveInfoMessage);
			_communicator.RegisterPackageCallback<NetworkConfigSync>(HandleNetworkConfigMessage);
			_communicator.RegisterPackageCallback<HostMigration>(HandleHostMigrationMessage);
			_communicator.RegisterPackageCallback<Snapshot>(HandleSnapshotMessage);
			_communicator.RegisterPackageCallback<PlayerRefMapping>(HandlePlayerRefMapping);
			_communicator.RegisterPackageCallback<DummyTrafficSync>(HandleDummyTrafficSync);
			_metadata = new CloudServicesMetadata();
		}

		public CloudCommunicator ExtractCommunicator()
		{
			_communicator.Client.RemoveCallbackTarget(this);
			_communicator.Reset();
			_communicator.WasExtracted = true;
			return _communicator;
		}

		public void Update()
		{
			CloudCommunicator communicator = _communicator;
			if (communicator != null && !communicator.WasExtracted)
			{
				_communicator.Service();
			}
		}

		public async Task ConnectToCloud(AppSettings appSettings, AuthenticationValues authentication = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (_communicator.Client.IsConnectedAndReady && _communicator.Client.Server == ServerConnection.MasterServer)
			{
				InternalLogStreams.LogDebug?.Log(_runner, "Skip connecting to Photon Cloud. Already connected and ready.");
				return;
			}
			if (appSettings == null)
			{
				throw new InvalidOperationException("Photon Application Settings not found.");
			}
			_communicator.Client.AuthValues = authentication;
			if (_communicator.Client.AuthValues != null)
			{
				InternalLogStreams.LogDebug?.Log(_runner, $"Connecting using Authentication: {_communicator.Client.AuthValues}");
			}
			InternalLogStreams.LogTraceAutomatedRun?.Log(_runner, $"Connecting to Photon Cloud using {appSettings}");
			InternalLogStreams.LogDebug?.Log(_runner, $"Connecting to Photon Cloud using {appSettings}");
			_metadata.RejoinMetadata.AppSettings = appSettings.SetupAppSettings();
			await _communicator.Client.ConnectUsingSettingsAsync(_metadata.RejoinMetadata.AppSettings, BuildAsyncConfig(cancellationToken));
			InternalLogStreams.LogDebug?.Log(_runner, "Connected to Photon Cloud.");
		}

		public Task<short> JoinSessionLobby(SessionLobby sessionLobby, string lobbyID = null, LobbyType lobbyType = LobbyType.Default, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!IsCloudReady)
			{
				return Task.FromException<short>(new InvalidOperationException("Fusion Relay Client is not ready. Make sure the call ConnectToCloud before start with StartGame"));
			}
			TypedLobby lobby;
			switch (sessionLobby)
			{
			case SessionLobby.ClientServer:
				lobby = CloudServicesMetadata.LobbyClientServer;
				break;
			case SessionLobby.Shared:
				lobby = CloudServicesMetadata.LobbyShared;
				break;
			case SessionLobby.Custom:
				if (string.IsNullOrEmpty(lobbyID?.Trim()))
				{
					return Task.FromException<short>(new InvalidOperationException("Invalid Lobby Name: Empty or Null"));
				}
				lobby = new TypedLobby(lobbyID.Trim(), lobbyType);
				break;
			default:
				return Task.FromException<short>(new InvalidOperationException("Invalid Lobby Type"));
			}
			return _communicator.Client.JoinLobbyAsync(lobby, throwOnError: true, BuildAsyncConfig(cancellationToken));
		}

		public Task<short> EnterRoom(StartGameArgs args)
		{
			if (!IsCloudReady)
			{
				return Task.FromException<short>(new InvalidOperationException("Fusion Relay Client is not ready. Make sure the call ConnectToCloud before start with StartGame"));
			}
			if (IsInRoom)
			{
				return Task.FromResult((short)0);
			}
			_metadata.RejoinMetadata.StartGameArgs = args;
			bool flag = args.GameMode == GameMode.Client || args.GameMode == GameMode.Host || args.GameMode == GameMode.Server || args.GameMode == GameMode.AutoHostOrClient;
			if (flag)
			{
				_communicator.Client.RealtimePeer.DisconnectTimeout = 5000;
			}
			bool flag2 = args.GameMode == GameMode.Host || args.GameMode == GameMode.Server;
			if (!args.EnableClientSessionCreation.HasValue)
			{
				if (args.GameMode == GameMode.Shared || args.GameMode == GameMode.AutoHostOrClient)
				{
					args.EnableClientSessionCreation = true;
				}
				if (args.GameMode == GameMode.Client)
				{
					args.EnableClientSessionCreation = false;
				}
			}
			bool flag3 = (args.GameMode == GameMode.Shared || args.GameMode == GameMode.AutoHostOrClient || args.GameMode == GameMode.Client) && args.EnableClientSessionCreation.HasValue && args.EnableClientSessionCreation.Value;
			bool flag4 = args.GameMode == GameMode.Server;
			TypedLobby typedLobby = (_communicator.Client.InLobby ? _communicator.Client.CurrentLobby : (string.IsNullOrEmpty(args.CustomLobbyName?.Trim()) ? ((args.GameMode == GameMode.Shared) ? CloudServicesMetadata.LobbyShared : CloudServicesMetadata.LobbyClientServer) : new TypedLobby(args.CustomLobbyName.Trim(), LobbyType.Default)));
			string text = args.SessionName?.Trim();
			bool flag5 = string.IsNullOrEmpty(text);
			if (flag5)
			{
				text = (args.SessionNameGenerator?.Invoke())?.Trim();
				if (string.IsNullOrEmpty(text))
				{
					text = Guid.NewGuid().ToString();
				}
			}
			int maxPlayers = (args.PlayerCount ?? args.Config?.Simulation?.PlayerCount ?? NetworkProjectConfig.Global.Simulation.PlayerCount) + (flag4 ? 1 : 0);
			(EnterRoomArgs, JoinRandomRoomArgs) tuple = _communicator.Client.BuildRoomArgs(typedLobby, text, maxPlayers, args.SessionProperties, args.IsOpen ?? true, args.IsVisible ?? true, 0, flag, args.MatchmakingMode.GetValueOrDefault());
			if (flag5)
			{
				InternalLogStreams.LogDebug?.Log(_runner, "Joining: [Random Session], Lobby=" + tuple.Item1.Lobby.Name + ", Region=" + _communicator.Client.CurrentRegion?.Replace("/*", ""));
			}
			else
			{
				InternalLogStreams.LogDebug?.Log(_runner, "Joining Session: " + tuple.Item1.RoomName + ", Lobby=" + tuple.Item1.Lobby.Name + ", Region=" + _communicator.Client.CurrentRegion?.Replace("/*", ""));
			}
			AsyncConfig config = BuildAsyncConfig(args.StartGameCancellationToken);
			if (flag2)
			{
				return _communicator.Client.CreateAndJoinRoomAsync(tuple.Item1, throwOnError: true, config);
			}
			if (flag5)
			{
				return flag3 ? _communicator.Client.JoinRandomOrCreateRoomAsync(tuple.Item2, tuple.Item1, throwOnError: true, config) : _communicator.Client.JoinRandomRoomAsync(tuple.Item2, throwOnError: true, config);
			}
			return flag3 ? _communicator.Client.JoinOrCreateRoomAsync(tuple.Item1, throwOnError: true, config) : _communicator.Client.JoinRoomAsync(tuple.Item1, throwOnError: true, config);
		}

		public async Task DisconnectFromCloud()
		{
			if (_communicator != null)
			{
				if (_communicator.WasExtracted)
				{
					return;
				}
				if (_communicator.Client.InRoom)
				{
					InternalLogStreams.LogDebug?.Log(_runner, "Leaving Session.");
					await _communicator.Client.LeaveRoomAsync();
				}
				if (_communicator.Client.IsConnectedAndReady)
				{
					InternalLogStreams.LogDebug?.Log(_runner, "Disconnecting from Photon Cloud.");
					await _communicator.Client.DisconnectAsync();
				}
			}
			InternalLogStreams.LogDebug?.Log(_runner, "Disconnected from Photon Cloud.");
		}

		public string GetActorUserID(int actorID)
		{
			if (IsInRoom && _communicator.Client.CurrentRoom.Players.TryGetValue(actorID, out var value))
			{
				return value.UserId;
			}
			return null;
		}

		public bool TryGetActorIdByUniqueId(long uniqueId, out int actorId)
		{
			if (_metadata.UniqueIdToReflexiveInfoTable.TryGetValue(uniqueId, out var value))
			{
				actorId = value.ActorNr;
				return true;
			}
			actorId = -1;
			return false;
		}

		internal void OnInternalConnectionAttempt(int attempt, int totalConnectionAttempts, out bool shouldChange, out NetAddress newAddress)
		{
			shouldChange = false;
			newAddress = default(NetAddress);
			if (_runner.GameMode != GameMode.Client)
			{
				return;
			}
			switch (_metadata.CurrentPunchStage)
			{
			case NATPunchStage.None:
				Assert.AlwaysFail($"CloudServices should not be in Stage {_metadata.CurrentPunchStage}");
				break;
			case NATPunchStage.Local:
				if (attempt > 2)
				{
					shouldChange = true;
					newAddress = _metadata.RemoteReflexiveInfo.PublicAddr;
					_metadata.CurrentPunchStage = NATPunchStage.Public;
				}
				break;
			case NATPunchStage.Public:
				if ((float)attempt >= (float)totalConnectionAttempts * 2f / 3f)
				{
					shouldChange = true;
					newAddress = NetAddress.FromActorId(_metadata.RemoteReflexiveInfo.ActorNr);
					_metadata.CurrentPunchStage = NATPunchStage.Relay;
				}
				break;
			case NATPunchStage.Relay:
				break;
			}
		}

		private void Connect(NATPunchStage punchStage, NetAddress endPoint)
		{
			InternalLogStreams.LogDebug?.Log(_runner, $"Connecting to {endPoint}");
			_metadata.CurrentPunchStage = punchStage;
			_runner.Connect(endPoint, _metadata.RunnerInitializeArgs.ConnectionToken, _metadata.UniqueId);
		}

		public void Dispose()
		{
			_communicator?.Dispose();
			_communicator = null;
		}

		internal void OnRoomChanged()
		{
			if (IsInRoom && _runner.SessionInfo != null)
			{
				UpdateSessionInfo(_runner.SessionInfo, _communicator.Client.CurrentRoom, _communicator.Client.CurrentRegion);
				InternalLogStreams.LogDebug?.Log(_runner, $"SessionInfo Update: {_runner.SessionInfo}");
			}
		}

		internal bool UpdateRoomProperties(Dictionary<string, SessionProperty> customProperties)
		{
			return IsServerOrMasterClient && IsInRoom && _communicator.Client.UpdateRoomProperties(customProperties);
		}

		internal bool UpdateRoomIsOpen(bool status)
		{
			return IsServerOrMasterClient && IsInRoom && _communicator.Client.UpdateRoomIsOpen(status);
		}

		internal bool UpdateRoomIsVisible(bool status)
		{
			return IsServerOrMasterClient && IsInRoom && _communicator.Client.UpdateRoomIsVisible(status);
		}

		private void OnRoomListChanged(List<RoomInfo> roomList)
		{
			foreach (RoomInfo room in roomList)
			{
				if (room.RemovedFromList)
				{
					_cachedSessionList.Remove(room.Name);
					continue;
				}
				if (!_cachedSessionList.ContainsKey(room.Name))
				{
					_cachedSessionList[room.Name] = new SessionInfo();
				}
				UpdateSessionInfo(_cachedSessionList[room.Name], room, _communicator.Client.CurrentRegion);
			}
			_runner.InvokeSessionListUpdated(new List<SessionInfo>(_cachedSessionList.Values));
		}

		internal async Task SendJoin(CancellationToken externalCancellationToken = default(CancellationToken), long uniqueId = 0L, int playerRefOverride = 0, int playerRefCountOverride = 0, byte[] encryptionKeyOverride = null)
		{
			_metadata.CurrentJoinStage = JoinProcessStage.Idle;
			PeerMode peerMode;
			PluginGameMode joinMode;
			switch (_runner.GameMode)
			{
			case GameMode.Shared:
				peerMode = PeerMode.Client;
				joinMode = PluginGameMode.Shared;
				break;
			case GameMode.Server:
			case GameMode.Host:
				peerMode = PeerMode.Server;
				joinMode = PluginGameMode.ClientServer;
				break;
			case GameMode.Client:
				peerMode = PeerMode.Client;
				joinMode = PluginGameMode.ClientServer;
				break;
			default:
				throw new InvalidOperationException($"Invalid Game Mode {_runner.GameMode}");
			}
			JoinMessageType joinType = ((playerRefOverride == 0) ? JoinMessageType.Request : JoinMessageType.Rejoin);
			Join joinRequest = new Join(joinType, joinMode, peerMode, (joinType == JoinMessageType.Rejoin) ? playerRefOverride : 0, JoinRequests.None, playerCounterRef: (joinType == JoinMessageType.Rejoin) ? playerRefCountOverride : 0, encryptionKey: (joinType == JoinMessageType.Rejoin) ? encryptionKeyOverride : null, uniqueID: (uniqueId != 0L) ? BitConverter.GetBytes(uniqueId) : null);
			if (!IsNATPunchthroughEnabled)
			{
				joinRequest.JoinRequests |= JoinRequests.DisableNATPunch;
			}
			_communicator.SendMessage(0, joinRequest);
			_metadata.CurrentJoinStage = JoinProcessStage.Joining;
			Join joinResponse;
			try
			{
				_joinAsyncHandler = new AsyncOperationHandler<Join>(externalCancellationToken, 30f, "Join Confirmation timeout. Shutdown.");
				joinResponse = await _joinAsyncHandler.Task;
			}
			catch
			{
				_metadata.CurrentJoinStage = JoinProcessStage.Fail;
				throw;
			}
			finally
			{
				_joinAsyncHandler = null;
			}
			if (joinResponse == null)
			{
				throw new InvalidOperationException("Join Response was null.");
			}
			Assert.Check(joinResponse.Type == JoinMessageType.Confirmation, "Invalid Join Message, it should be a Confirmation");
			if (joinResponse.EncryptionKey != null)
			{
				_metadata.EncryptionToken = new EncryptionToken
				{
					Key = joinResponse.EncryptionKey,
					KeyEncrypted = joinResponse.EncryptionKeySecret
				};
				InternalLogStreams.LogTraceEncryption?.Log($"Received Encryption Token: {_metadata.EncryptionToken}");
			}
			if (joinResponse.JoinRequests.Has(JoinRequests.NetworkConfig))
			{
				SendNetworkSyncMessage(NetworkRunner.SetupNetworkProjectConfig(_metadata.RunnerInitializeArgs));
			}
			if (joinResponse.JoinRequests.Has(JoinRequests.ReflexiveInfo))
			{
				_metadata.ScheduledRequests.Set(ScheduledRequests.ReflexiveInfo);
			}
			_metadata.CurrentProtocolMessageVersion = joinResponse.ProtocolVersion;
			_metadata.UniqueId = joinResponse.UniqueId;
			_metadata.PlayerRef = joinResponse.PlayerRef;
			_metadata.CurrentJoinStage = JoinProcessStage.Done;
		}

		private void SendNetworkSyncMessage(NetworkProjectConfig projectConfig)
		{
			string text = NetworkProjectConfig.SerializeMinimal(projectConfig);
			InternalLogStreams.LogDebug?.Log(_runner, "Sending serialized NetworkProjectConfig:\n" + text);
			NetworkConfigSync message = new NetworkConfigSync(SyncType.Response, text, _metadata.CurrentProtocolMessageVersion);
			_communicator.SendMessage(0, message);
		}

		private void SendReflexiveInfo(StunResult stunResult)
		{
			ReflexiveInfo message = new ReflexiveInfo(_communicator.CommunicatorID, stunResult.PublicEndPoint, stunResult.PrivateEndPoint, stunResult.NatType, null, _metadata.CurrentProtocolMessageVersion);
			_communicator.SendMessage(0, message);
		}

		public void SendChangeMasterClient(int newCandidate)
		{
			ChangeMasterClient message = new ChangeMasterClient(newCandidate, _metadata.CurrentProtocolMessageVersion);
			_communicator.SendMessage(0, message);
		}

		public void SendStateSnapshot(byte[] data, int snapshotSize, int tick, uint lastId)
		{
			try
			{
				Snapshot snapshot = new Snapshot(tick, lastId, SnapshotType.Data, snapshotSize, data, _metadata.CurrentProtocolMessageVersion);
				Assert.Check(snapshot.IsValid, "snapshot.IsValid");
				_communicator.SendMessage(0, snapshot);
			}
			catch (Exception message)
			{
				InternalLogStreams.LogDebug?.Error(message);
			}
		}

		public void SendClearSnapshot()
		{
			try
			{
				Snapshot snapshot = new Snapshot(0, 0u, SnapshotType.Clear, 0, null, _metadata.CurrentProtocolMessageVersion);
				Assert.Check(snapshot.IsValid, "snapshot.IsValid");
				_communicator.SendMessage(0, snapshot);
			}
			catch (Exception message)
			{
				InternalLogStreams.LogDebug?.Error(message);
			}
		}

		private void HandleJoinMessage(int sender, Join join)
		{
			Assert.Check(sender == 0, "Invalid Sender of Join Confirmation {0}", sender);
			_joinAsyncHandler?.SetResult(join);
		}

		private async void HandleStartMessage(int sender, Start start)
		{
			Assert.Check(sender == 0, "Invalid Sender of Start Message: {0}", sender);
			if (!(await ConfirmJoin()))
			{
				InternalLogStreams.LogDebug?.Warn(_runner, "Received Start Message, but never a Join Confirmation. Shutdown.");
				_runner.StartGameOperation?.SetException(new StartGameException(ShutdownReason.Error, "Received Start Message, but never a Join Confirmation. Shutdown."));
				return;
			}
			try
			{
				if (!_runner.IsRunning)
				{
					if (_metadata.RunnerInitializeArgs.SimulationMode == SimulationModes.Client)
					{
						NetworkRunnerInitializeArgs initArgs = _metadata.RunnerInitializeArgs;
						initArgs.PlayerCount = SessionSlots;
						if (start.StartRequests.Has(StartRequests.CustomPlugin))
						{
							initArgs.Flags |= NetworkRunnerFlags.CustomPlugin;
						}
						_metadata.RunnerInitializeArgs = initArgs;
					}
					if (!(await _runner.Initialize(_metadata.RunnerInitializeArgs)))
					{
						InternalLogStreams.LogDebug?.Warn(_runner, "Runner failed to Initialize. Shutdown.");
						_runner.StartGameOperation?.SetException(new StartGameException(ShutdownReason.Error, "Runner failed to Initialize. Shutdown."));
						return;
					}
				}
				else
				{
					_runner.StartGameOperation?.SetResult(ShutdownReason.Ok);
				}
				switch (_runner.GameMode)
				{
				case GameMode.Shared:
					if (start.StartRequests.Has(StartRequests.ConnectToShared))
					{
						Connect(NATPunchStage.Relay, NetAddress.FromActorId(0));
					}
					break;
				case GameMode.Server:
				case GameMode.Host:
				case GameMode.Client:
					if (_metadata.ScheduledRequests.IsSet(ScheduledRequests.ReflexiveInfo))
					{
						CloudServicesMetadata metadata = _metadata;
						StunResult localReflexiveInfo = metadata.LocalReflexiveInfo;
						StunResult stunResult = localReflexiveInfo;
						if (stunResult == null)
						{
							CloudServicesMetadata cloudServicesMetadata = metadata;
							StunResult localReflexiveInfo2;
							StunResult stunResult2 = (localReflexiveInfo2 = await QueryReflexiveInfo().ConfigureAwait(continueOnCapturedContext: false));
							cloudServicesMetadata.LocalReflexiveInfo = localReflexiveInfo2;
							stunResult = stunResult2;
						}
						_ = stunResult;
						SendReflexiveInfo(_metadata.LocalReflexiveInfo);
						_metadata.ScheduledRequests.Clear(ScheduledRequests.ReflexiveInfo);
					}
					if (_runner.IsClient && _metadata.RemoteReflexiveInfo == null)
					{
						_metadata.RemoteReflexiveInfo = new ReflexiveInfo(start.RemoteServerID, NetAddress.AnyIPv4Addr, NetAddress.AnyIPv4Addr, NATType.UdpBlocked);
						IsNATPunchthroughEnabled &= start.StartRequests.Has(StartRequests.WaitForReflexiveInfo);
					}
					_runner.SetupEncryption(_metadata.EncryptionToken);
					break;
				}
				InternalLogStreams.LogDebug?.Log(_runner, "Fusion Simulation Startup Done.");
			}
			catch (Exception exception)
			{
				_runner.StartGameOperation?.SetException(exception);
			}
		}

		private void HandleDisconnectMessage(int sender, Disconnect disconnect)
		{
			Assert.Check(sender == 0, "Invalid Sender of Disconnect Message: {0}", sender);
			_metadata.LastDisconnectMsg = disconnect;
		}

		private void HandleNetworkConfigMessage(int sender, NetworkConfigSync configSync)
		{
		}

		private async void HandleReflexiveInfoMessage(int sender, ReflexiveInfo reflexiveInfo)
		{
			Assert.Check(sender == 0, "Invalid Sender of Reflexive Info Message: {0}", sender);
			if (!(await ConfirmJoin()))
			{
				InternalLogStreams.LogDebug?.Warn(_runner, "Received ReflexiveInfo Message, but never a Join Confirmation. Ignore.");
				return;
			}
			switch (_runner.GameMode)
			{
			case GameMode.Client:
				if (_metadata.RemoteReflexiveInfo == null)
				{
					await TaskManager.Run(async delegate(CancellationToken token)
					{
						int timeout = 10;
						while (_metadata.RemoteReflexiveInfo == null && timeout > 0)
						{
							timeout--;
							await TaskManager.Delay(10, token);
						}
					}, _runner.OperationsCancellationToken);
				}
				if (_metadata.RemoteReflexiveInfo != null && _metadata.RemoteReflexiveInfo.ActorNr == reflexiveInfo.ActorNr)
				{
					if (!_runner.IsConnectedToServer)
					{
						_metadata.RemoteReflexiveInfo = reflexiveInfo;
						if (IsNATPunchthroughEnabled && _metadata.RemoteReflexiveInfo.PrivateAddr.IsValid && CheckSubnet(_metadata.RemoteReflexiveInfo.PrivateAddr))
						{
							Connect(NATPunchStage.Local, _metadata.RemoteReflexiveInfo.PrivateAddr);
						}
						else if (IsNATPunchthroughEnabled && _metadata.RemoteReflexiveInfo.PublicAddr.IsValid && _metadata.RemoteReflexiveInfo.NatType.IsValid())
						{
							Connect(NATPunchStage.Public, _metadata.RemoteReflexiveInfo.PublicAddr);
						}
						else
						{
							Connect(NATPunchStage.Relay, NetAddress.FromActorId(_metadata.RemoteReflexiveInfo.ActorNr));
						}
					}
				}
				else
				{
					_runner.StartGameOperation?.SetException(new StartGameException(ShutdownReason.Error, $"Received Reflexive Info from an unexpected Actor or it is invalid: expected {_metadata.RemoteReflexiveInfo?.ActorNr} vs received {reflexiveInfo}"));
				}
				break;
			case GameMode.Server:
			case GameMode.Host:
				if (reflexiveInfo.UniqueId != null && reflexiveInfo.UniqueId.Length == 8)
				{
					long uniqueId = BitConverter.ToInt64(reflexiveInfo.UniqueId, 0);
					if (uniqueId != 0)
					{
						_metadata.UniqueIdToReflexiveInfoTable[uniqueId] = reflexiveInfo;
					}
					else
					{
						InternalLogStreams.LogWarn?.Log($"Received Invalid UniqueId from Actor {reflexiveInfo.ActorNr}");
					}
				}
				Run_ReversePing(reflexiveInfo.PrivateAddr);
				if (IsNATPunchthroughEnabled)
				{
					Run_ReversePing(reflexiveInfo.PublicAddr);
				}
				break;
			}
		}

		private void HandleHostMigrationMessage(int sender, HostMigration hostMigration)
		{
			Assert.Check(sender == 0, "Invalid Sender of HostMigration: {0}", sender);
			_runner.SetupHostMigration(hostMigration);
			if (hostMigration.PeerMode == PeerMode.Client)
			{
				_runner.StartHostMigration();
			}
		}

		private void HandleSnapshotMessage(int sender, Snapshot snapshot)
		{
			Assert.Check(sender == 0, "Invalid Sender of Snapshot: {0}", sender);
			switch (snapshot.SnapshotType)
			{
			case SnapshotType.Data:
				_runner.StartHostMigration(snapshot);
				break;
			case SnapshotType.Confirmation:
				if (_runner.LastSnapshotTick != snapshot.Tick)
				{
					InternalLogStreams.LogDebug?.Warn($"Expecting Snapshot: {_runner.LastSnapshotTick}");
				}
				if (_runner.LastConfirmedSnapshotTick < snapshot.Tick)
				{
					Interlocked.Exchange(ref _runner.LastConfirmedSnapshotTick, snapshot.Tick);
					InternalLogStreams.LogDebug?.Log($"Host Snapshot for Tick {_runner.LastConfirmedSnapshotTick} confirmed.");
				}
				break;
			}
		}

		private void HandleDummyTrafficSync(int sender, DummyTrafficSync dummyTrafficSync)
		{
			Assert.Check(sender == 0, "Invalid Sender of DummyTrafficSync: {0}", sender);
			SetupDummyTraffic(dummyTrafficSync);
		}

		private async Task<bool> ConfirmJoin()
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();
			while (timer.ElapsedMilliseconds <= 30000)
			{
				switch (_metadata.CurrentJoinStage)
				{
				case JoinProcessStage.Idle:
					InternalLogStreams.LogError?.Log("Received a Protocol Message without sending Join Message.");
					return false;
				case JoinProcessStage.Joining:
					await TaskManager.Delay(10);
					break;
				case JoinProcessStage.Done:
					return true;
				case JoinProcessStage.Fail:
					return false;
				}
			}
			InternalLogStreams.LogError?.Log("Confirm Join Timeout.");
			return false;
		}

		private void HandlePlayerRefMapping(int sender, PlayerRefMapping msg)
		{
			_runner._simulation.RegisterUniqueIdPlayerMapping(msg.ActorId, msg.UniqueId, PlayerRef.FromIndex(msg.PlayerRef));
		}

		internal void StartBackgroundCloudServices()
		{
			if (_runner.Config.HostMigration.EnableAutoUpdate)
			{
				TaskManager.Service((Func<Task<bool>>)Service_HostMigrationSnapshot, _runner.OperationsCancellationToken, 1000 * _runner.Config.HostMigration.UpdateDelay, "HostMigration");
			}
		}

		private async Task<bool> Service_HostMigrationSnapshot()
		{
			if (!_runner.IsRunning)
			{
				return true;
			}
			if (!_runner.IsServer || _runner.GameMode != GameMode.Host)
			{
				return false;
			}
			try
			{
				await _runner.SendHostMigrationSnapshot();
			}
			catch
			{
			}
			return true;
		}

		private void SetupDummyTraffic(DummyTrafficSync dummyTrafficSyncMessage)
		{
			if (dummyTrafficSyncMessage == null || !dummyTrafficSyncMessage.IsValid)
			{
				InternalLogStreams.LogTraceDummyTraffic?.Warn(_runner, "Invalid Dummy Traffic Message, ignore.");
				return;
			}
			if (_dummyTrafficCts != null)
			{
				_dummyTrafficCts.Cancel();
				_dummyTrafficCts.Dispose();
			}
			_dummyTrafficCts = new CancellationTokenSource();
			_dummyTrafficLinkCts = CancellationTokenSource.CreateLinkedTokenSource(_dummyTrafficCts.Token, _runner.OperationsCancellationToken);
			if (_dummyData == null || _dummyData.Length != dummyTrafficSyncMessage.Size)
			{
				_dummyData = new byte[dummyTrafficSyncMessage.Size];
				new Random().NextBytes(_dummyData);
			}
			TaskManager.Service(delegate
			{
				if (_runner.IsRunning && _runner.Topology != Topologies.ClientServer)
				{
					return Task.FromResult(result: false);
				}
				SendDummyTraffic(_dummyData);
				return Task.FromResult(result: true);
			}, _dummyTrafficLinkCts.Token, dummyTrafficSyncMessage.SendInterval, "DummyTraffic");
			unsafe void SendDummyTraffic(byte[] buffer)
			{
				if (_runner.IsRunning && _communicator.Client.IsConnectedAndReady)
				{
					fixed (byte* buffer2 = buffer)
					{
						_communicator.SendPackage(102, _communicator.CommunicatorID, reliable: false, buffer2, buffer.Length);
						InternalLogStreams.LogTraceDummyTraffic?.Log($"Sent to {_communicator.CommunicatorID} with {_dummyData.Length} bytes.");
					}
				}
			}
		}

		private unsafe async Task<StunResult> QueryReflexiveInfo()
		{
			if (!IsNATPunchthroughEnabled || _runner?._simulation == null)
			{
				return StunResult.Invalid;
			}
			if (_runner._simulation._netPeer != null)
			{
				NetAddress boundLocalAddress = _runner._simulation._netPeer->Address;
				return await StunClient.QueryReflexiveInfo(boundLocalAddress, SendAnyData, _metadata.RunnerInitializeArgs.PublicAddress, CustomSTUNServer, _runner.Config.PeerMode == NetworkProjectConfig.PeerModes.Multiple, KeepRunning);
			}
			return StunResult.Invalid;
			bool KeepRunning()
			{
				return !_runner.IsShutdown;
			}
			unsafe bool SendAnyData(byte[] requestBytes, NetAddress target)
			{
				if (!target.IsValid)
				{
					return false;
				}
				if (_runner?._simulation == null)
				{
					return false;
				}
				try
				{
					NetPeer* netPeer = _runner._simulation._netPeer;
					INetSocket netSocket = _runner._simulation._netSocket;
					if (netPeer != null && netSocket != null)
					{
						fixed (byte* buffer = requestBytes)
						{
							return netSocket.Send(netPeer->_socket, &target, buffer, requestBytes.Length) > 0;
						}
					}
					return false;
				}
				catch (Exception arg)
				{
					InternalLogStreams.LogDebug?.Warn($"Error while sending STUN Message: {arg}");
				}
				return false;
			}
		}

		public void UpdateInitializeArgs(NetworkRunnerInitializeArgs newArgs)
		{
			_metadata.RunnerInitializeArgs = newArgs;
		}

		private bool CheckSubnet(NetAddress remotePrivateEndPoint)
		{
			return _metadata.LocalReflexiveInfo != null && (remotePrivateEndPoint.IsIPv6 || NetAddress.SubnetMask.IsSameSubNet(_metadata.LocalReflexiveInfo.PrivateEndPoint, remotePrivateEndPoint));
		}

		private void UpdateSessionInfo(SessionInfo sessionInfo, RoomInfo roomInfo, string region)
		{
			if (roomInfo is Room room)
			{
				sessionInfo.Name = room.Name;
				sessionInfo._isOpen = room.IsOpen;
				sessionInfo._isVisible = room.IsVisible;
				sessionInfo.MaxPlayers = room.MaxPlayers;
				sessionInfo.PlayerCount = room.PlayerCount;
			}
			else
			{
				sessionInfo.Name = roomInfo.Name;
				sessionInfo._isOpen = roomInfo.IsOpen;
				sessionInfo._isVisible = roomInfo.IsVisible;
				sessionInfo.MaxPlayers = roomInfo.MaxPlayers;
				sessionInfo.PlayerCount = roomInfo.PlayerCount;
			}
			sessionInfo.Region = region;
			sessionInfo.Properties = new ReadOnlyDictionary<string, SessionProperty>(roomInfo.GetCustomProperties());
			sessionInfo._isValid = true;
		}

		private void Run_ReversePing(NetAddress remoteAddr)
		{
			if (!remoteAddr.IsValid)
			{
				return;
			}
			TaskManager.Run(async delegate(CancellationToken token)
			{
				InternalLogStreams.LogDebug?.Log(_runner, $"Reverse NAT Punch: {remoteAddr}");
				for (int i = 0; i < 10; i++)
				{
					token.ThrowIfCancellationRequested();
					if (!SendPing(remoteAddr))
					{
						break;
					}
					await TaskManager.Delay(100, token);
				}
			}, _runner.OperationsCancellationToken);
			unsafe bool SendPing(NetAddress netAddress)
			{
				return _runner?.Simulation?.NetworkSendPing(netAddress, null, 0) == true;
			}
		}

		private bool HandlePhotonCloudDisconnect(ShutdownReason shutdownReason)
		{
			bool flag = shutdownReason == ShutdownReason.Ok;
			if (flag && _metadata.RejoinMetadata.TryingMoveSessionToNewRoom)
			{
				return true;
			}
			if (!flag && _runner.StartGameOperation == null && shutdownReason != ShutdownReason.DisconnectedByPluginLogic && (_runner.IsServer || (_runner.IsClient && _runner.CurrentConnectionType == ConnectionType.Direct)) && _runner.GameMode >= GameMode.Server)
			{
				_metadata.RejoinMetadata.TryingToReconnect = _metadata.RejoinMetadata.RejoinAttempts > 0 && shutdownReason == ShutdownReason.PhotonCloudTimeout && _communicator.Client.ReconnectAndRejoin();
				if (_metadata.RejoinMetadata.TryingToReconnect)
				{
					_metadata.RejoinMetadata.RejoinAttempts--;
					InternalLogStreams.LogDebug?.Log(_runner, $"Attempting to reconnect to Photon Cloud ({_metadata.RejoinMetadata.RejoinAttempts} attempts left). Previous disconnect: {shutdownReason}");
				}
				else
				{
					if ((int)NetworkRunner.CloudConnectionLostCurrentMode < 2 || !_runner.IsServer)
					{
						return false;
					}
					TriggerMoveSessionToNewRoom();
				}
				try
				{
					NetworkRunner.CloudConnectionLost?.Invoke(_runner, shutdownReason, _metadata.RejoinMetadata.TryingToReconnect);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
				}
				return true;
			}
			return false;
		}

		internal void TriggerMoveSessionToNewRoom(string sessionId = null)
		{
			if (string.IsNullOrEmpty(sessionId))
			{
				if (_runner.IsClient)
				{
					throw new ArgumentException("Session ID must be valid to trigger Full Rejoin on Client peers");
				}
				if (_runner.IsServer)
				{
					string text = "[bkp-" + Guid.NewGuid().ToString().Substring(0, 6) + "]";
					sessionId = _metadata.RejoinMetadata.StartGameArgs.SessionNameGenerator?.Invoke().Trim();
					if (string.IsNullOrEmpty(sessionId))
					{
						sessionId = Regex.Replace(_runner.SessionInfo.Name.Trim(), "\\[bkp-.*\\]$", "");
					}
					sessionId = sessionId.Substring(0, Math.Min(sessionId.Length, 200 - text.Length));
					sessionId += text;
				}
			}
			Assert.Check(!string.IsNullOrEmpty(sessionId), "string.IsNullOrEmpty(sessionId) == false");
			TaskManager.Run((CancellationToken token) => MoveSessionToNewRoom(token, sessionId), _runner.OperationsCancellationToken);
		}

		private async Task MoveSessionToNewRoom(CancellationToken token, string sessionId)
		{
			Assert.Check(!token.IsCancellationRequested, "token must not be cancelled yet");
			InternalLogStreams.LogDebug?.Info("MoveSessionToNewRoom: START (" + sessionId + ")");
			_metadata.RejoinMetadata.TryingMoveSessionToNewRoom = true;
			if (IsInRoom)
			{
				InternalLogStreams.LogDebug?.Info("MoveSessionToNewRoom: leaving current Session");
				await _communicator.Client.LeaveRoomAsync();
			}
			if (_runner.IsServer)
			{
				_metadata.UniqueIdToReflexiveInfoTable.Clear();
				await RejoinSession(_runner.IsPlayer ? GameMode.Host : GameMode.Server, token, sessionId);
				_runner.RequestRejoinAllPlayers(sessionId);
				await WaitRejoin(token);
				_runner.SessionInfo.IsVisible = true;
			}
			if (_runner.IsClient)
			{
				await RejoinSession(GameMode.Client, token, sessionId);
			}
			InternalLogStreams.LogDebug?.Info("MoveSessionToNewRoom: END");
			async Task RejoinSession(GameMode gameMode, CancellationToken cancellationToken, string targetSession)
			{
				try
				{
					RejoinMetadata rejoinMetadata = _metadata.RejoinMetadata;
					rejoinMetadata.StartGameArgs.StartGameCancellationToken = cancellationToken;
					rejoinMetadata.StartGameArgs.SessionName = targetSession;
					rejoinMetadata.StartGameArgs.IsVisible = false;
					rejoinMetadata.StartGameArgs.GameMode = gameMode;
					rejoinMetadata.AppSettings.FixedRegion = _runner.SessionInfo.Region;
					_runner.StartGameOperation = new AsyncOperationHandler<ShutdownReason>(cancellationToken, 120f, "Full Rejoin Timed out");
					InternalLogStreams.LogDebug?.Info("MoveSessionToNewRoom: " + string.Format("{0}={1}, ", "AppSettings", rejoinMetadata.AppSettings) + string.Format("{0}={1}", "AuthValues", _communicator.Client.AuthValues));
					await ConnectToCloud(rejoinMetadata.AppSettings, _communicator.Client.AuthValues, cancellationToken);
					short enterRoomResult = await EnterRoom(rejoinMetadata.StartGameArgs);
					InternalLogStreams.LogDebug?.Log(string.Format("{0}: Enter Room: {1}", "MoveSessionToNewRoom", ErrorCodeExt.ConvertToShutdownReason(enterRoomResult)));
					OnRoomChanged();
					int playerRefCountOverride = 0;
					if (_runner.IsServer)
					{
						if (_metadata.PlayerRef > playerRefCountOverride)
						{
							playerRefCountOverride = _metadata.PlayerRef;
						}
						foreach (PlayerRef @ref in _runner.ActivePlayers)
						{
							if (@ref.AsIndex > playerRefCountOverride)
							{
								playerRefCountOverride = @ref.AsIndex;
							}
						}
						InternalLogStreams.LogDebug?.Log(string.Format("{0}: Server with Max PlayerRef={1}", "MoveSessionToNewRoom", playerRefCountOverride));
					}
					await SendJoin(cancellationToken, BitConverter.ToInt64(_metadata.UniqueId), _metadata.PlayerRef, playerRefCountOverride, _runner.IsServer ? _metadata.EncryptionToken.Key : null);
					ShutdownReason result = await _runner.StartGameOperation.Task;
					InternalLogStreams.LogDebug?.Log(string.Format("{0}: Operation Done: {1}", "MoveSessionToNewRoom", result));
				}
				catch (Exception ex)
				{
					Exception e = ex;
					InternalLogStreams.LogDebug?.Error(e);
				}
				finally
				{
					_runner.StartGameOperation = null;
				}
			}
			async Task WaitRejoin(CancellationToken cancellationToken)
			{
				InternalLogStreams.LogDebug?.Log("MoveSessionToNewRoom: WaitRejoin");
				Stopwatch watch = Stopwatch.StartNew();
				Queue<PlayerRef> playerRefs = new Queue<PlayerRef>(_runner.ActivePlayers);
				while (watch.ElapsedMilliseconds < 10000 && playerRefs.Count > 0 && !cancellationToken.IsCancellationRequested)
				{
					PlayerRef playerRef = playerRefs.Dequeue();
					int? playerActorId;
					int? actor = (playerActorId = _runner.GetPlayerActorId(playerRef));
					playerActorId = playerActorId;
					if (playerActorId.HasValue)
					{
						InternalLogStreams.LogDebug?.Info(string.Format("{0}: Player {1} found with Actor {2}", "MoveSessionToNewRoom", playerRef, actor));
					}
					else
					{
						playerRefs.Enqueue(playerRef);
						await TaskManager.Delay(50, cancellationToken);
					}
				}
				if (playerRefs.Count > 0)
				{
					InternalLogStreams.LogDebug?.Info(string.Format("{0}: {1} Players didn't re-join in time", "MoveSessionToNewRoom", playerRefs.Count));
				}
				InternalLogStreams.LogDebug?.Log("MoveSessionToNewRoom: WaitRejoin END");
			}
		}

		private AsyncConfig BuildAsyncConfig(CancellationToken externalCancellationToken = default(CancellationToken))
		{
			AsyncConfig asyncConfig = AsyncConfig.CreateUnityAsyncConfig();
			asyncConfig.OperationTimeoutSec = 45f;
			if (externalCancellationToken != CancellationToken.None)
			{
				asyncConfig.CancellationToken = CancellationTokenSource.CreateLinkedTokenSource(asyncConfig.CancellationToken, externalCancellationToken).Token;
			}
			return asyncConfig;
		}
	}
}
