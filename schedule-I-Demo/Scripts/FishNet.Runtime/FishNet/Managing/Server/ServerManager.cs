using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet.Authenticating;
using FishNet.Broadcast;
using FishNet.Broadcast.Helping;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Managing.Predicting;
using FishNet.Managing.Timing;
using FishNet.Managing.Transporting;
using FishNet.Managing.Utility;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Serializing.Helping;
using FishNet.Transporting;
using FishNet.Transporting.Multipass;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Managing.Server
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Manager/ServerManager")]
	public sealed class ServerManager : MonoBehaviour
	{
		private delegate void ClientBroadcastDelegate(NetworkConnection connection, PooledReader reader);

		private readonly Dictionary<ushort, HashSet<ClientBroadcastDelegate>> _broadcastHandlers = new Dictionary<ushort, HashSet<ClientBroadcastDelegate>>();

		private Dictionary<ushort, HashSet<(int, ClientBroadcastDelegate)>> _handlerTargets = new Dictionary<ushort, HashSet<(int, ClientBroadcastDelegate)>>();

		private HashSet<NetworkConnection> _connectionsWithoutExclusions = new HashSet<NetworkConnection>();

		[HideInInspector]
		public Dictionary<int, NetworkConnection> Clients = new Dictionary<int, NetworkConnection>();

		[Tooltip("Authenticator for this ServerManager. May be null if not using authentication.")]
		[SerializeField]
		private Authenticator _authenticator;

		[Tooltip("What platforms to enable remote client timeout.")]
		[SerializeField]
		private RemoteTimeoutType _remoteClientTimeout = RemoteTimeoutType.Development;

		[Tooltip("How long in seconds a client must go without sending any packets before getting disconnected. This is independent of any transport settings.")]
		[Range(1f, 1500f)]
		[SerializeField]
		private ushort _remoteClientTimeoutDuration = 60;

		[Tooltip("Default send rate for SyncTypes. A value of 0f will send changed values every tick.")]
		[Range(0f, 60f)]
		[SerializeField]
		private float _syncTypeRate = 0.1f;

		[Tooltip("How to pack object spawns.")]
		[SerializeField]
		internal TransformPackingData SpawnPacking = new TransformPackingData
		{
			Position = AutoPackType.Unpacked,
			Rotation = AutoPackType.PackedLess,
			Scale = AutoPackType.PackedLess
		};

		[Tooltip("True to automatically set the frame rate when the client connects.")]
		[SerializeField]
		private bool _changeFrameRate = true;

		[Tooltip("Maximum frame rate the server may run at. When as host this value runs at whichever is higher between client and server.")]
		[Range(1f, 500f)]
		[SerializeField]
		private ushort _frameRate = 500;

		[Tooltip("True to share the Ids of clients and the objects they own with other clients. No sensitive information is shared.")]
		[SerializeField]
		private bool _shareIds = true;

		[Tooltip("True to automatically start the server connection when running as headless.")]
		[SerializeField]
		private bool _startOnHeadless = true;

		[Tooltip("True to kick clients which send data larger than the MTU.")]
		[SerializeField]
		private bool _limitClientMTU = true;

		private int _nextClientTimeoutCheckIndex;

		private float _nextTimeoutCheckTime;

		private SplitReader _splitReader = new SplitReader();

		public const ushort MAXIMUM_REMOTE_CLIENT_TIMEOUT_DURATION = 1500;

		private uint _cachedLevelOfDetailInterval;

		private bool _cachedUseLod;

		internal Dictionary<ushort, RpcLink> RpcLinks = new Dictionary<ushort, RpcLink>();

		private Queue<ushort> _availableRpcLinkIndexes = new Queue<ushort>();

		public bool Started { get; private set; }

		public ServerObjects Objects { get; private set; }

		[HideInInspector]
		public NetworkManager NetworkManager { get; private set; }

		[Obsolete("Use GetAuthenticator and SetAuthenticator.")]
		public Authenticator Authenticator
		{
			get
			{
				return GetAuthenticator();
			}
			set
			{
				SetAuthenticator(value);
			}
		}

		internal ushort FrameRate
		{
			get
			{
				if (!_changeFrameRate)
				{
					return 0;
				}
				return _frameRate;
			}
		}

		internal bool ShareIds => _shareIds;

		internal bool LimitClientMTU => _limitClientMTU;

		public event Action<ServerConnectionStateArgs> OnServerConnectionState;

		public event Action<NetworkConnection, bool> OnAuthenticationResult;

		public event Action<NetworkConnection, RemoteConnectionStateArgs> OnRemoteConnectionState;

		public event Action<NetworkConnection, int, KickReason> OnClientKick;

		public void RegisterBroadcast<T>(Action<NetworkConnection, T> handler, bool requireAuthentication = true) where T : struct, IBroadcast
		{
			if (handler == null)
			{
				NetworkManager.LogError("Broadcast cannot be registered because handler is null. This may occur when trying to register to objects which require initialization, such as events.");
				return;
			}
			ushort key = BroadcastHelper.GetKey<T>();
			if (!_broadcastHandlers.TryGetValueIL2CPP(key, out var value))
			{
				value = new HashSet<ClientBroadcastDelegate>();
				_broadcastHandlers.Add(key, value);
			}
			ClientBroadcastDelegate clientBroadcastDelegate = CreateBroadcastDelegate(handler, requireAuthentication);
			value.Add(clientBroadcastDelegate);
			int hashCode = handler.GetHashCode();
			if (!_handlerTargets.TryGetValueIL2CPP(key, out var value2))
			{
				value2 = new HashSet<(int, ClientBroadcastDelegate)>();
				_handlerTargets.Add(key, value2);
			}
			value2.Add((hashCode, clientBroadcastDelegate));
		}

		public void UnregisterBroadcast<T>(Action<NetworkConnection, T> handler) where T : struct, IBroadcast
		{
			ushort key = BroadcastHelper.GetKey<T>();
			if (!_broadcastHandlers.TryGetValueIL2CPP(key, out var value))
			{
				return;
			}
			if (_handlerTargets.TryGetValueIL2CPP(key, out var value2))
			{
				int hashCode = handler.GetHashCode();
				ClientBroadcastDelegate clientBroadcastDelegate = null;
				foreach (var (num, clientBroadcastDelegate2) in value2)
				{
					if (num == hashCode)
					{
						clientBroadcastDelegate = clientBroadcastDelegate2;
						value2.Remove((num, clientBroadcastDelegate2));
						break;
					}
				}
				if (value2.Count == 0)
				{
					_handlerTargets.Remove(key);
				}
				if (clientBroadcastDelegate != null)
				{
					value.Remove(clientBroadcastDelegate);
				}
			}
			if (value.Count == 0)
			{
				_broadcastHandlers.Remove(key);
			}
		}

		private ClientBroadcastDelegate CreateBroadcastDelegate<T>(Action<NetworkConnection, T> handler, bool requireAuthentication)
		{
			return LogicContainer;
			void LogicContainer(NetworkConnection connection, PooledReader reader)
			{
				if (requireAuthentication && !connection.Authenticated)
				{
					connection.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"ConnectionId {connection.ClientId} sent broadcast {typeof(T).Name} which requires authentication, but client was not authenticated. Client has been disconnected.");
				}
				else
				{
					T arg = reader.Read<T>();
					handler?.Invoke(connection, arg);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ParseBroadcast(PooledReader reader, NetworkConnection conn, Channel channel)
		{
			ushort key = reader.ReadUInt16();
			int packetLength = Packets.GetPacketLength(12, reader, channel);
			if (_broadcastHandlers.TryGetValueIL2CPP(key, out var value))
			{
				int position = reader.Position;
				bool flag = false;
				bool flag2 = false;
				foreach (ClientBroadcastDelegate item in value)
				{
					if (item.Target == null)
					{
						NetworkManager.LogWarning("A Broadcast handler target is null. This can occur when a script is destroyed but does not unregister from a Broadcast.");
						flag = true;
					}
					else
					{
						reader.Position = position;
						item(conn, reader);
						flag2 = true;
					}
				}
				if (flag)
				{
					List<ClientBroadcastDelegate> list = value.ToList();
					value.Clear();
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Target != null)
						{
							value.Add(list[i]);
						}
					}
				}
				if (!flag2)
				{
					reader.Skip(packetLength);
				}
			}
			else
			{
				reader.Skip(packetLength);
			}
		}

		public void Broadcast<T>(NetworkConnection connection, T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!Started)
			{
				NetworkManager.LogWarning("Cannot send broadcast to client because server is not active.");
				return;
			}
			if (requireAuthenticated && !connection.Authenticated)
			{
				NetworkManager.LogWarning("Cannot send broadcast to client because they are not authenticated.");
				return;
			}
			PooledWriter pooledWriter = WriterPool.Retrieve();
			Broadcasts.WriteBroadcast(NetworkManager, pooledWriter, message, ref channel);
			ArraySegment<byte> arraySegment = pooledWriter.GetArraySegment();
			NetworkManager.TransportManager.SendToClient((byte)channel, arraySegment, connection);
			pooledWriter.Store();
		}

		public void Broadcast<T>(HashSet<NetworkConnection> connections, T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!Started)
			{
				NetworkManager.LogWarning("Cannot send broadcast to client because server is not active.");
				return;
			}
			bool flag = false;
			PooledWriter pooledWriter = WriterPool.Retrieve();
			Broadcasts.WriteBroadcast(NetworkManager, pooledWriter, message, ref channel);
			ArraySegment<byte> arraySegment = pooledWriter.GetArraySegment();
			foreach (NetworkConnection connection in connections)
			{
				if (requireAuthenticated && !connection.Authenticated)
				{
					flag = true;
				}
				else
				{
					NetworkManager.TransportManager.SendToClient((byte)channel, arraySegment, connection);
				}
			}
			pooledWriter.Store();
			if (flag)
			{
				NetworkManager.LogWarning("One or more broadcast did not send to a client because they were not authenticated.");
			}
		}

		public void BroadcastExcept<T>(HashSet<NetworkConnection> connections, NetworkConnection excludedConnection, T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!Started)
			{
				NetworkManager.LogWarning("Cannot send broadcast to client because server is not active.");
				return;
			}
			if (excludedConnection == null || !excludedConnection.IsValid)
			{
				Broadcast(connections, message, requireAuthenticated, channel);
				return;
			}
			connections.Remove(excludedConnection);
			Broadcast(connections, message, requireAuthenticated, channel);
		}

		public void BroadcastExcept<T>(HashSet<NetworkConnection> connections, HashSet<NetworkConnection> excludedConnections, T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!Started)
			{
				NetworkManager.LogWarning("Cannot send broadcast to client because server is not active.");
				return;
			}
			if (excludedConnections == null || excludedConnections.Count == 0)
			{
				Broadcast(connections, message, requireAuthenticated, channel);
				return;
			}
			foreach (NetworkConnection excludedConnection in excludedConnections)
			{
				connections.Remove(excludedConnection);
			}
			Broadcast(connections, message, requireAuthenticated, channel);
		}

		public void BroadcastExcept<T>(NetworkConnection excludedConnection, T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!Started)
			{
				NetworkManager.LogWarning("Cannot send broadcast to client because server is not active.");
				return;
			}
			if (excludedConnection == null || !excludedConnection.IsValid)
			{
				Broadcast(message, requireAuthenticated, channel);
				return;
			}
			_connectionsWithoutExclusions.Clear();
			foreach (NetworkConnection value in Clients.Values)
			{
				_connectionsWithoutExclusions.Add(value);
			}
			_connectionsWithoutExclusions.Remove(excludedConnection);
			Broadcast(_connectionsWithoutExclusions, message, requireAuthenticated, channel);
		}

		public void BroadcastExcept<T>(HashSet<NetworkConnection> excludedConnections, T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!Started)
			{
				NetworkManager.LogWarning("Cannot send broadcast to client because server is not active.");
				return;
			}
			if (excludedConnections == null || excludedConnections.Count == 0)
			{
				Broadcast(message, requireAuthenticated, channel);
				return;
			}
			_connectionsWithoutExclusions.Clear();
			foreach (NetworkConnection value in Clients.Values)
			{
				_connectionsWithoutExclusions.Add(value);
			}
			foreach (NetworkConnection excludedConnection in excludedConnections)
			{
				_connectionsWithoutExclusions.Remove(excludedConnection);
			}
			Broadcast(_connectionsWithoutExclusions, message, requireAuthenticated, channel);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Broadcast<T>(NetworkObject networkObject, T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (networkObject == null)
			{
				NetworkManager.LogWarning("Cannot send broadcast because networkObject is null.");
			}
			else
			{
				Broadcast(networkObject.Observers, message, requireAuthenticated, channel);
			}
		}

		public void Broadcast<T>(T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!Started)
			{
				NetworkManager.LogWarning("Cannot send broadcast to client because server is not active.");
				return;
			}
			bool flag = false;
			PooledWriter pooledWriter = WriterPool.Retrieve();
			Broadcasts.WriteBroadcast(NetworkManager, pooledWriter, message, ref channel);
			ArraySegment<byte> arraySegment = pooledWriter.GetArraySegment();
			foreach (NetworkConnection value in Clients.Values)
			{
				if (requireAuthenticated && !value.Authenticated)
				{
					flag = true;
				}
				else
				{
					NetworkManager.TransportManager.SendToClient((byte)channel, arraySegment, value);
				}
			}
			pooledWriter.Store();
			if (flag)
			{
				NetworkManager.LogWarning("One or more broadcast did not send to a client because they were not authenticated.");
			}
		}

		public Authenticator GetAuthenticator()
		{
			return _authenticator;
		}

		public void SetAuthenticator(Authenticator value)
		{
			_authenticator = value;
			InitializeAuthenticator();
		}

		public void SetRemoteClientTimeout(RemoteTimeoutType timeoutType, ushort duration)
		{
			_remoteClientTimeout = timeoutType;
			duration = (ushort)Mathf.Clamp(duration, 1, 1500);
			_remoteClientTimeoutDuration = duration;
		}

		internal float GetSynctypeRate()
		{
			return _syncTypeRate;
		}

		public bool GetStartOnHeadless()
		{
			return _startOnHeadless;
		}

		public void SetStartOnHeadless(bool value)
		{
			_startOnHeadless = value;
		}

		private void OnDestroy()
		{
			Objects?.SubscribeToSceneLoaded(subscribe: false);
		}

		internal void InitializeOnce_Internal(NetworkManager manager)
		{
			NetworkManager = manager;
			Objects = new ServerObjects(manager);
			Objects.SubscribeToSceneLoaded(subscribe: true);
			InitializeRpcLinks();
			SubscribeToTransport(subscribe: false);
			SubscribeToTransport(subscribe: true);
			NetworkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
			NetworkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
			NetworkManager.TimeManager.OnPostTick += TimeManager_OnPostTick;
			if (_authenticator == null)
			{
				_authenticator = GetComponent<Authenticator>();
			}
			if (_authenticator != null)
			{
				InitializeAuthenticator();
			}
			_cachedLevelOfDetailInterval = NetworkManager.ClientManager.LevelOfDetailInterval;
			_cachedUseLod = NetworkManager.ObserverManager.GetEnableNetworkLod();
		}

		private void InitializeAuthenticator()
		{
			Authenticator authenticator = GetAuthenticator();
			if (!(authenticator == null) && !authenticator.Initialized && !(NetworkManager == null))
			{
				authenticator.InitializeOnce(NetworkManager);
				authenticator.OnAuthenticationResult += _authenticator_OnAuthenticationResult;
			}
		}

		internal void StartForHeadless()
		{
			GetStartOnHeadless();
		}

		public bool StopConnection(bool sendDisconnectMessage)
		{
			if (sendDisconnectMessage)
			{
				SendDisconnectMessages(Clients.Values.ToList(), iterate: true);
			}
			return NetworkManager.TransportManager.Transport.StopConnection(server: true);
		}

		internal void SendDisconnectMessages(int[] connectionIds)
		{
			List<NetworkConnection> list = new List<NetworkConnection>();
			foreach (int key in connectionIds)
			{
				if (Clients.TryGetValueIL2CPP(key, out var value))
				{
					list.Add(value);
				}
			}
			if (list.Count > 0)
			{
				SendDisconnectMessages(list, iterate: false);
			}
		}

		private void SendDisconnectMessages(List<NetworkConnection> conns, bool iterate)
		{
			PooledWriter pooledWriter = WriterPool.Retrieve();
			pooledWriter.WritePacketId(PacketId.Disconnect);
			ArraySegment<byte> arraySegment = pooledWriter.GetArraySegment();
			foreach (NetworkConnection conn in conns)
			{
				conn.SendToClient(0, arraySegment);
			}
			pooledWriter.Store();
			if (iterate)
			{
				NetworkManager.TransportManager.IterateOutgoing(toServer: true);
			}
		}

		public bool StartConnection()
		{
			return NetworkManager.TransportManager.Transport.StartConnection(server: true);
		}

		public bool StartConnection(ushort port)
		{
			Transport transport = NetworkManager.TransportManager.Transport;
			transport.SetPort(port);
			return transport.StartConnection(server: true);
		}

		private void CheckClientTimeout()
		{
			if (_remoteClientTimeout == RemoteTimeoutType.Disabled || NetworkManager.SceneManager.IsIteratingQueue(2f))
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime < _nextTimeoutCheckTime)
			{
				return;
			}
			_nextTimeoutCheckTime = unscaledTime + 0.2f;
			int count = Clients.Count;
			if (count == 0)
			{
				return;
			}
			if (_nextClientTimeoutCheckIndex >= count)
			{
				_nextClientTimeoutCheckIndex = 0;
			}
			uint num = NetworkManager.TimeManager.TimeToTicks((int)_remoteClientTimeoutDuration, TickRounding.RoundUp);
			int num2 = Mathf.CeilToInt(10f);
			int num3 = Mathf.Max(count / num2, 1);
			uint localTick = NetworkManager.TimeManager.LocalTick;
			int num4 = 0;
			foreach (NetworkConnection value in Clients.Values)
			{
				if (num4 >= _nextClientTimeoutCheckIndex)
				{
					uint num5 = value.PacketTick.LocalTick;
					if (num5 == 0)
					{
						num5 = value.ServerConnectionTick;
					}
					if (localTick - num5 >= num)
					{
						value.Kick(KickReason.UnexpectedProblem, LoggingType.Common, value.ToString() + " has timed out. You can modify this feature on the ServerManager component.");
					}
					if (--num3 <= 0)
					{
						break;
					}
				}
				num4++;
			}
		}

		private void TimeManager_OnPostTick()
		{
			CheckClientTimeout();
		}

		private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
		{
			if (obj.ConnectionState != LocalConnectionState.Started)
			{
				Objects.DestroyPending();
			}
		}

		private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
		{
			if (!asServer)
			{
				return;
			}
			Objects.RebuildObservers(conn);
			if (!conn.IsLocalClient)
			{
				return;
			}
			foreach (NetworkObject value in Objects.Spawned.Values)
			{
				if (!value.Observers.Contains(conn))
				{
					value.SetRenderersVisible(visible: false);
				}
			}
		}

		private void SubscribeToTransport(bool subscribe)
		{
			if (!(NetworkManager == null) && !(NetworkManager.TransportManager == null) && !(NetworkManager.TransportManager.Transport == null))
			{
				if (subscribe)
				{
					NetworkManager.TransportManager.Transport.OnServerReceivedData += Transport_OnServerReceivedData;
					NetworkManager.TransportManager.Transport.OnServerConnectionState += Transport_OnServerConnectionState;
					NetworkManager.TransportManager.Transport.OnRemoteConnectionState += Transport_OnRemoteConnectionState;
				}
				else
				{
					NetworkManager.TransportManager.Transport.OnServerReceivedData -= Transport_OnServerReceivedData;
					NetworkManager.TransportManager.Transport.OnServerConnectionState -= Transport_OnServerConnectionState;
					NetworkManager.TransportManager.Transport.OnRemoteConnectionState -= Transport_OnRemoteConnectionState;
				}
			}
		}

		private void _authenticator_OnAuthenticationResult(NetworkConnection conn, bool authenticated)
		{
			if (!authenticated)
			{
				conn.Disconnect(immediately: false);
			}
			else
			{
				ClientAuthenticated(conn);
			}
		}

		private void Transport_OnServerConnectionState(ServerConnectionStateArgs args)
		{
			Started = AnyServerStarted();
			NetworkManager.ClientManager.Objects.OnServerConnectionState(args);
			if (!Started)
			{
				MatchCondition.StoreCollections(NetworkManager);
				Objects.DespawnWithoutSynchronization(asServer: true);
			}
			Objects.OnServerConnectionState(args);
			LocalConnectionState connectionState = args.ConnectionState;
			if (NetworkManager.CanLog(LoggingType.Common))
			{
				Transport transport = NetworkManager.TransportManager.GetTransport(args.TransportIndex);
				string text = ((transport == null) ? "Unknown" : transport.GetType().Name);
				string text2 = string.Empty;
				if (connectionState == LocalConnectionState.Starting)
				{
					text2 = $" Listening on port {transport.GetPort()}.";
				}
				Debug.Log("Local server is " + connectionState.ToString().ToLower() + " for " + text + "." + text2);
			}
			NetworkManager.UpdateFramerate();
			this.OnServerConnectionState?.Invoke(args);
		}

		private void Transport_OnRemoteConnectionState(RemoteConnectionStateArgs args)
		{
			int connectionId = args.ConnectionId;
			int num = 32767;
			NetworkConnection value;
			if (connectionId < 0 || connectionId > num)
			{
				Kick(args.ConnectionId, KickReason.UnexpectedProblem, LoggingType.Error, $"The transport you are using supplied an invalid connection Id of {connectionId}. Connection Id values must range between 0 and {num}. The client has been disconnected.");
			}
			else if (args.ConnectionState == RemoteConnectionState.Started)
			{
				NetworkManager.Log($"Remote connection started for Id {connectionId}.");
				NetworkConnection networkConnection = new NetworkConnection(NetworkManager, connectionId, args.TransportIndex, asServer: true);
				Clients.Add(args.ConnectionId, networkConnection);
				this.OnRemoteConnectionState?.Invoke(networkConnection, args);
				if (networkConnection.IsValid)
				{
					Authenticator authenticator = GetAuthenticator();
					if (authenticator != null && !NetworkManager.TransportManager.IsLocalTransport(connectionId))
					{
						authenticator.OnRemoteConnection(networkConnection);
					}
					else
					{
						ClientAuthenticated(networkConnection);
					}
				}
			}
			else if (args.ConnectionState == RemoteConnectionState.Stopped && Clients.TryGetValueIL2CPP(connectionId, out value))
			{
				value.SetDisconnecting(value: true);
				this.OnRemoteConnectionState?.Invoke(value, args);
				Clients.Remove(connectionId);
				Objects.ClientDisconnected(value);
				BroadcastClientConnectionChange(connected: false, value);
				Queue<int> predictedObjectIds = value.PredictedObjectIds;
				while (predictedObjectIds.Count > 0)
				{
					Objects.CacheObjectId(predictedObjectIds.Dequeue());
				}
				value.Dispose();
				NetworkManager.Log($"Remote connection stopped for Id {connectionId}.");
			}
		}

		private void SendAuthenticated(NetworkConnection conn)
		{
			PooledWriter pooledWriter = WriterPool.Retrieve();
			pooledWriter.WritePacketId(PacketId.Authenticated);
			pooledWriter.WriteNetworkConnection(conn);
			PredictionManager predictionManager = NetworkManager.PredictionManager;
			if (predictionManager.GetAllowPredictedSpawning())
			{
				int num = Mathf.Min(Objects.GetObjectIdCache().Count, predictionManager.GetReservedObjectIds());
				pooledWriter.WriteByte((byte)num);
				for (int i = 0; i < num; i++)
				{
					ushort num2 = (ushort)Objects.GetNextNetworkObjectId(errorCheck: false);
					pooledWriter.WriteNetworkObjectId(num2);
					conn.PredictedObjectIds.Enqueue(num2);
				}
			}
			NetworkManager.TransportManager.SendToClient(0, pooledWriter.GetArraySegment(), conn);
			pooledWriter.Store();
		}

		private void Transport_OnServerReceivedData(ServerReceivedDataArgs args)
		{
			ParseReceived(args);
		}

		private void ParseReceived(ServerReceivedDataArgs args)
		{
			if (args.ConnectionId < 0)
			{
				return;
			}
			ArraySegment<byte> data = args.Data;
			NetworkManager.StatisticsManager.NetworkTraffic.LocalServerReceivedData((ulong)data.Count);
			if (data.Count <= 4)
			{
				return;
			}
			int mTU = NetworkManager.TransportManager.GetMTU(args.TransportIndex, (byte)args.Channel);
			if (data.Count > mTU && !NetworkManager.TransportManager.IsLocalTransport(args.ConnectionId))
			{
				ExceededMTUKick();
				return;
			}
			bool hasIntermediateLayer = NetworkManager.TransportManager.HasIntermediateLayer;
			PacketId packetId = PacketId.Unset;
			PooledReader pooledReader = null;
			try
			{
				Reader.DataSource source = Reader.DataSource.Client;
				pooledReader = ReaderPool.Retrieve(data, NetworkManager, source);
				uint num = pooledReader.ReadTickUnpacked();
				NetworkManager.TimeManager.SetLastPacketTick(num);
				if (pooledReader.PeekPacketId() == PacketId.Split)
				{
					pooledReader.ReadPacketId();
					_splitReader.GetHeader(pooledReader, out var expectedMessages);
					_splitReader.Write(NetworkManager.TimeManager.LastPacketTick, pooledReader, expectedMessages);
					ArraySegment<byte> fullMessage = _splitReader.GetFullMessage();
					if (fullMessage.Count == 0)
					{
						return;
					}
					if (hasIntermediateLayer)
					{
						pooledReader.Initialize(NetworkManager.TransportManager.ProcessIntermediateIncoming(fullMessage, fromServer: false), NetworkManager, source);
					}
					else
					{
						pooledReader.Initialize(fullMessage, NetworkManager, source);
					}
				}
				else if (hasIntermediateLayer)
				{
					ArraySegment<byte> segment = NetworkManager.TransportManager.ProcessIntermediateIncoming(pooledReader.GetRemainingData(), fromServer: false);
					pooledReader.Initialize(segment, NetworkManager, source);
				}
				while (pooledReader.Remaining > 0)
				{
					packetId = pooledReader.ReadPacketId();
					if (!Clients.TryGetValueIL2CPP(args.ConnectionId, out var value))
					{
						Kick(args.ConnectionId, KickReason.UnexpectedProblem, LoggingType.Error, $"ConnectionId {args.ConnectionId} not found within Clients. Connection will be kicked immediately.");
						break;
					}
					value.PacketTick.Update(NetworkManager.TimeManager, num, EstimatedTick.OldTickOption.SetLastRemoteTick);
					if (!value.Authenticated && packetId != PacketId.Broadcast)
					{
						value.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"ConnectionId {value.ClientId} sent a Broadcast without being authenticated. Connection will be kicked immediately.");
						break;
					}
					if (_cachedUseLod && value.IsLateForLevelOfDetail(_cachedLevelOfDetailInterval * 60))
					{
						value.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"ConnectionId {value.ClientId} has gone too long without sending a level of detail update. Connection will be kicked immediately.");
						break;
					}
					switch (packetId)
					{
					case PacketId.Replicate:
						Objects.ParseReplicateRpc(pooledReader, value, args.Channel);
						break;
					case PacketId.ServerRpc:
						Objects.ParseServerRpc(pooledReader, value, args.Channel);
						break;
					case PacketId.ObjectSpawn:
						if (!NetworkManager.PredictionManager.GetAllowPredictedSpawning())
						{
							value.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"ConnectionId {value.ClientId} sent a predicted spawn while predicted spawning is not enabled. Connection will be kicked immediately.");
							return;
						}
						Objects.ReadPredictedSpawn(pooledReader, value);
						break;
					case PacketId.ObjectDespawn:
						if (!NetworkManager.PredictionManager.GetAllowPredictedSpawning())
						{
							value.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"ConnectionId {value.ClientId} sent a predicted spawn while predicted spawning is not enabled. Connection will be kicked immediately.");
							return;
						}
						Objects.ReadPredictedDespawn(pooledReader, value);
						break;
					case PacketId.NetworkLODUpdate:
						ParseNetworkLODUpdate(pooledReader, value);
						break;
					case PacketId.Broadcast:
						ParseBroadcast(pooledReader, value, args.Channel);
						break;
					case PacketId.PingPong:
						ParsePingPong(pooledReader, value);
						break;
					default:
						NetworkManager.LogError($"Server received an unhandled PacketId of {(ushort)packetId} on channel {args.Channel} from connectionId {args.ConnectionId}. Connection will be kicked immediately.");
						NetworkManager.TransportManager.Transport.StopConnection(args.ConnectionId, immediately: true);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Kick(args.ConnectionId, KickReason.MalformedData, LoggingType.Error, $"Server encountered an error while parsing data for packetId {packetId} from connectionId {args.ConnectionId}. Connection will be kicked immediately. Message: {ex.Message}.");
			}
			finally
			{
				pooledReader?.Store();
			}
			void ExceededMTUKick()
			{
				Kick(args.ConnectionId, KickReason.ExploitExcessiveData, LoggingType.Common, $"ConnectionId {args.ConnectionId} sent a message larger than allowed amount. Connection will be kicked immediately.");
			}
		}

		private void ParsePingPong(PooledReader reader, NetworkConnection conn)
		{
			uint clientTick = reader.ReadTickUnpacked();
			if (conn.CanPingPong())
			{
				NetworkManager.TimeManager.SendPong(conn, clientTick);
			}
		}

		private void ClientAuthenticated(NetworkConnection connection)
		{
			connection.ConnectionAuthenticated();
			BroadcastClientConnectionChange(connected: true, connection);
			SendAuthenticated(connection);
			this.OnAuthenticationResult?.Invoke(connection, arg2: true);
			NetworkManager.SceneManager.OnClientAuthenticated(connection);
		}

		private void BroadcastClientConnectionChange(bool connected, NetworkConnection conn)
		{
			if (ShareIds)
			{
				ClientConnectionChangeBroadcast message = new ClientConnectionChangeBroadcast
				{
					Connected = connected,
					Id = conn.ClientId
				};
				foreach (NetworkConnection value in Clients.Values)
				{
					if (value.Authenticated)
					{
						Broadcast(value, message);
					}
				}
				if (!connected)
				{
					return;
				}
				List<int> list = CollectionCaches<int>.RetrieveList();
				foreach (int key in Clients.Keys)
				{
					list.Add(key);
				}
				ConnectedClientsBroadcast message2 = new ConnectedClientsBroadcast
				{
					Values = list
				};
				conn.Broadcast(message2);
				CollectionCaches<int>.Store(list);
			}
			else if (connected)
			{
				ClientConnectionChangeBroadcast message3 = new ClientConnectionChangeBroadcast
				{
					Connected = connected,
					Id = conn.ClientId
				};
				Broadcast(conn, message3);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ParseNetworkLODUpdate(PooledReader reader, NetworkConnection conn)
		{
		}

		public bool OneServerStarted()
		{
			int num = 0;
			TransportManager transportManager = NetworkManager.TransportManager;
			if (transportManager.Transport is Multipass multipass)
			{
				foreach (Transport transport in multipass.Transports)
				{
					if (transport.GetConnectionState(server: true) == LocalConnectionState.Started)
					{
						num++;
					}
				}
			}
			else if (transportManager.Transport.GetConnectionState(server: true) == LocalConnectionState.Started)
			{
				num = 1;
			}
			return num == 1;
		}

		public bool AnyServerStarted(int? excludedIndex = null)
		{
			TransportManager transportManager = NetworkManager.TransportManager;
			if (transportManager.Transport is Multipass multipass)
			{
				Transport transport = ((!excludedIndex.HasValue) ? null : multipass.GetTransport(excludedIndex.Value));
				foreach (Transport transport2 in multipass.Transports)
				{
					if (!(transport2 == transport) && transport2.GetConnectionState(server: true) == LocalConnectionState.Started)
					{
						return true;
					}
				}
				return false;
			}
			return transportManager.Transport.GetConnectionState(server: true) == LocalConnectionState.Started;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Spawn(GameObject go, NetworkConnection ownerConnection = null, Scene scene = default(Scene))
		{
			if (go == null)
			{
				NetworkManager.LogWarning("GameObject cannot be spawned because it is null.");
				return;
			}
			NetworkObject component = go.GetComponent<NetworkObject>();
			Spawn(component, ownerConnection, scene);
		}

		public void Spawn(NetworkObject nob, NetworkConnection ownerConnection = null, Scene scene = default(Scene))
		{
			Objects.Spawn(nob, ownerConnection, scene);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Despawn(GameObject go, DespawnType? despawnType = null)
		{
			if (go == null)
			{
				NetworkManager.LogWarning("GameObject cannot be despawned because it is null.");
				return;
			}
			NetworkObject component = go.GetComponent<NetworkObject>();
			Despawn(component, despawnType);
		}

		public void Despawn(NetworkObject networkObject, DespawnType? despawnType = null)
		{
			DespawnType despawnType2 = ((!despawnType.HasValue) ? networkObject.GetDefaultDespawnType() : despawnType.Value);
			Objects.Despawn(networkObject, despawnType2, asServer: true);
		}

		public void Kick(NetworkConnection conn, KickReason kickReason, LoggingType loggingType = LoggingType.Common, string log = "")
		{
			if (conn.IsValid)
			{
				this.OnClientKick?.Invoke(conn, conn.ClientId, kickReason);
				if (conn.IsActive)
				{
					conn.Disconnect(immediately: true);
				}
				if (!string.IsNullOrEmpty(log))
				{
					NetworkManager.Log(loggingType, log);
				}
			}
		}

		public void Kick(int clientId, KickReason kickReason, LoggingType loggingType = LoggingType.Common, string log = "")
		{
			this.OnClientKick?.Invoke(null, clientId, kickReason);
			NetworkManager.TransportManager.Transport.StopConnection(clientId, immediately: true);
			if (!string.IsNullOrEmpty(log))
			{
				NetworkManager.Log(loggingType, log);
			}
		}

		public void Kick(NetworkConnection conn, Reader reader, KickReason kickReason, LoggingType loggingType = LoggingType.Common, string log = "")
		{
			reader.Clear();
			Kick(conn, kickReason, loggingType, log);
		}

		private void InitializeRpcLinks()
		{
			ushort startingRpcLinkIndex = NetworkManager.StartingRpcLinkIndex;
			for (ushort num = ushort.MaxValue; num >= startingRpcLinkIndex; num--)
			{
				_availableRpcLinkIndexes.Enqueue(num);
			}
		}

		internal bool GetRpcLink(out ushort value)
		{
			if (_availableRpcLinkIndexes.Count > 0)
			{
				value = _availableRpcLinkIndexes.Dequeue();
				return true;
			}
			value = 0;
			return false;
		}

		internal void SetRpcLink(ushort linkIndex, RpcLink data)
		{
			RpcLinks[linkIndex] = data;
		}

		internal void StoreRpcLinks(Dictionary<uint, RpcLinkType> links)
		{
			foreach (RpcLinkType value in links.Values)
			{
				_availableRpcLinkIndexes.Enqueue(value.LinkIndex);
			}
		}
	}
}
