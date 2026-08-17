using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Broadcast;
using FishNet.Broadcast.Helping;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Managing.Server;
using FishNet.Managing.Timing;
using FishNet.Managing.Transporting;
using FishNet.Managing.Utility;
using FishNet.Serializing;
using FishNet.Serializing.Helping;
using FishNet.Transporting;
using FishNet.Transporting.Multipass;
using GameKit.Utilities;
using UnityEngine;

namespace FishNet.Managing.Client
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Manager/ClientManager")]
	public sealed class ClientManager : MonoBehaviour
	{
		private delegate void ServerBroadcastDelegate(PooledReader reader);

		private readonly Dictionary<ushort, HashSet<ServerBroadcastDelegate>> _broadcastHandlers = new Dictionary<ushort, HashSet<ServerBroadcastDelegate>>();

		private Dictionary<ushort, HashSet<(int, ServerBroadcastDelegate)>> _handlerTargets = new Dictionary<ushort, HashSet<(int, ServerBroadcastDelegate)>>();

		public NetworkConnection Connection = NetworkManager.EmptyConnection;

		public Dictionary<int, NetworkConnection> Clients = new Dictionary<int, NetworkConnection>();

		[Tooltip("What platforms to enable remote server timeout.")]
		[SerializeField]
		private RemoteTimeoutType _remoteServerTimeout = RemoteTimeoutType.Development;

		[Tooltip("How long in seconds server must go without sending any packets before the local client disconnects. This is independent of any transport settings.")]
		[Range(1f, 1500f)]
		[SerializeField]
		private ushort _remoteServerTimeoutDuration = 60;

		[Tooltip("True to automatically set the frame rate when the client connects.")]
		[SerializeField]
		private bool _changeFrameRate = true;

		[Tooltip("Maximum frame rate the client may run at. When as host this value runs at whichever is higher between client and server.")]
		[Range(1f, 500f)]
		[SerializeField]
		private ushort _frameRate = 500;

		private float _lastPacketTime;

		private SplitReader _splitReader = new SplitReader();

		private List<Vector3> _objectsPositionsCache = new List<Vector3>();

		private int _nextLodNobIndex;

		public bool Started { get; private set; }

		public ClientObjects Objects { get; private set; }

		[HideInInspector]
		public NetworkManager NetworkManager { get; private set; }

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

		public uint LevelOfDetailInterval => NetworkManager.TimeManager.TimeToTicks(0.5, TickRounding.RoundUp);

		public uint LastPacketLocalTick { get; private set; }

		public event Action OnAuthenticated;

		public event Action OnClientTimeOut;

		public event Action<ClientConnectionStateArgs> OnClientConnectionState;

		public event Action<RemoteConnectionStateArgs> OnRemoteConnectionState;

		public event Action<ConnectedClientsArgs> OnConnectedClients;

		public void RegisterBroadcast<T>(Action<T> handler) where T : struct, IBroadcast
		{
			ushort stableHashU = typeof(T).FullName.GetStableHashU16();
			if (!_broadcastHandlers.TryGetValueIL2CPP(stableHashU, out var value))
			{
				value = new HashSet<ServerBroadcastDelegate>();
				_broadcastHandlers.Add(stableHashU, value);
			}
			ServerBroadcastDelegate serverBroadcastDelegate = CreateBroadcastDelegate(handler);
			value.Add(serverBroadcastDelegate);
			int hashCode = handler.GetHashCode();
			if (!_handlerTargets.TryGetValueIL2CPP(stableHashU, out var value2))
			{
				value2 = new HashSet<(int, ServerBroadcastDelegate)>();
				_handlerTargets.Add(stableHashU, value2);
			}
			value2.Add((hashCode, serverBroadcastDelegate));
		}

		public void UnregisterBroadcast<T>(Action<T> handler) where T : struct, IBroadcast
		{
			ushort key = BroadcastHelper.GetKey<T>();
			if (!_broadcastHandlers.TryGetValueIL2CPP(key, out var value))
			{
				return;
			}
			if (_handlerTargets.TryGetValueIL2CPP(key, out var value2))
			{
				int hashCode = handler.GetHashCode();
				ServerBroadcastDelegate serverBroadcastDelegate = null;
				foreach (var (num, serverBroadcastDelegate2) in value2)
				{
					if (num == hashCode)
					{
						serverBroadcastDelegate = serverBroadcastDelegate2;
						value2.Remove((num, serverBroadcastDelegate2));
						break;
					}
				}
				if (value2.Count == 0)
				{
					_handlerTargets.Remove(key);
				}
				if (serverBroadcastDelegate != null)
				{
					value.Remove(serverBroadcastDelegate);
				}
			}
			if (value.Count == 0)
			{
				_broadcastHandlers.Remove(key);
			}
		}

		private ServerBroadcastDelegate CreateBroadcastDelegate<T>(Action<T> handler)
		{
			return LogicContainer;
			void LogicContainer(PooledReader reader)
			{
				T obj = reader.Read<T>();
				handler?.Invoke(obj);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ParseBroadcast(PooledReader reader, Channel channel)
		{
			ushort key = reader.ReadUInt16();
			int packetLength = Packets.GetPacketLength(12, reader, channel);
			if (_broadcastHandlers.TryGetValueIL2CPP(key, out var value))
			{
				int position = reader.Position;
				{
					foreach (ServerBroadcastDelegate item in value)
					{
						reader.Position = position;
						item(reader);
					}
					return;
				}
			}
			reader.Skip(packetLength);
		}

		public void Broadcast<T>(T message, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!Started)
			{
				NetworkManager.LogWarning("Cannot send broadcast to server because client is not active.");
				return;
			}
			PooledWriter pooledWriter = WriterPool.Retrieve();
			Broadcasts.WriteBroadcast(NetworkManager, pooledWriter, message, ref channel);
			ArraySegment<byte> arraySegment = pooledWriter.GetArraySegment();
			NetworkManager.TransportManager.SendToServer((byte)channel, arraySegment);
			pooledWriter.Store();
		}

		public void SetRemoteServerTimeout(RemoteTimeoutType timeoutType, ushort duration)
		{
			_remoteServerTimeout = timeoutType;
			duration = (ushort)Mathf.Clamp(duration, 1, 1500);
			_remoteServerTimeoutDuration = duration;
		}

		private void UpdateLastPacketDatas()
		{
			_lastPacketTime = Time.unscaledTime;
			LastPacketLocalTick = NetworkManager.TimeManager.LocalTick;
		}

		private void OnDestroy()
		{
			Objects?.SubscribeToSceneLoaded(subscribe: false);
		}

		internal void InitializeOnce_Internal(NetworkManager manager)
		{
			NetworkManager = manager;
			Objects = new ClientObjects(manager);
			Objects.SubscribeToSceneLoaded(subscribe: true);
			SubscribeToEvents(subscribe: false);
			SubscribeToEvents(subscribe: true);
			RegisterBroadcast<ClientConnectionChangeBroadcast>(OnClientConnectionBroadcast);
			RegisterBroadcast<ConnectedClientsBroadcast>(OnConnectedClientsBroadcast);
		}

		private void OnClientConnectionBroadcast(ClientConnectionChangeBroadcast args)
		{
			RemoteConnectionStateArgs obj = new RemoteConnectionStateArgs(args.Connected ? RemoteConnectionState.Started : RemoteConnectionState.Stopped, args.Id, -1);
			if (args.Connected)
			{
				Clients[args.Id] = new NetworkConnection(NetworkManager, args.Id, -1, asServer: false);
				this.OnRemoteConnectionState?.Invoke(obj);
				return;
			}
			this.OnRemoteConnectionState?.Invoke(obj);
			if (Clients.TryGetValue(args.Id, out var value))
			{
				value.Dispose();
				Clients.Remove(args.Id);
			}
		}

		private void OnConnectedClientsBroadcast(ConnectedClientsBroadcast args)
		{
			NetworkManager.ClearClientsCollection(Clients);
			List<int> list = args.Values;
			if (list == null)
			{
				list = new List<int>();
			}
			else
			{
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					int num = list[i];
					Clients[num] = new NetworkConnection(NetworkManager, num, -1, asServer: false);
				}
			}
			this.OnConnectedClients?.Invoke(new ConnectedClientsArgs(list));
		}

		private void SubscribeToEvents(bool subscribe)
		{
			if (!(NetworkManager == null) && !(NetworkManager.TransportManager == null) && !(NetworkManager.TransportManager.Transport == null))
			{
				if (subscribe)
				{
					NetworkManager.TransportManager.OnIterateIncomingEnd += TransportManager_OnIterateIncomingEnd;
					NetworkManager.TransportManager.Transport.OnClientReceivedData += Transport_OnClientReceivedData;
					NetworkManager.TransportManager.Transport.OnClientConnectionState += Transport_OnClientConnectionState;
					NetworkManager.TimeManager.OnPostTick += TimeManager_OnPostTick;
				}
				else
				{
					NetworkManager.TransportManager.OnIterateIncomingEnd -= TransportManager_OnIterateIncomingEnd;
					NetworkManager.TransportManager.Transport.OnClientReceivedData -= Transport_OnClientReceivedData;
					NetworkManager.TransportManager.Transport.OnClientConnectionState -= Transport_OnClientConnectionState;
					NetworkManager.TimeManager.OnPostTick -= TimeManager_OnPostTick;
				}
			}
		}

		public int GetTransportIndex()
		{
			if (NetworkManager.TransportManager.Transport is Multipass multipass)
			{
				return multipass.ClientTransport.Index;
			}
			return 0;
		}

		public bool StopConnection()
		{
			return NetworkManager.TransportManager.Transport.StopConnection(server: false);
		}

		public bool StartConnection()
		{
			return NetworkManager.TransportManager.Transport.StartConnection(server: false);
		}

		public bool StartConnection(string address)
		{
			return StartConnection(address, NetworkManager.TransportManager.Transport.GetPort());
		}

		public bool StartConnection(string address, ushort port)
		{
			NetworkManager.TransportManager.Transport.SetClientAddress(address);
			NetworkManager.TransportManager.Transport.SetPort(port);
			return StartConnection();
		}

		private void Transport_OnClientConnectionState(ClientConnectionStateArgs args)
		{
			LocalConnectionState connectionState = args.ConnectionState;
			Started = connectionState == LocalConnectionState.Started;
			Objects.OnClientConnectionState(args);
			if (!Started)
			{
				Connection = NetworkManager.EmptyConnection;
				NetworkManager.ClearClientsCollection(Clients);
			}
			else
			{
				UpdateLastPacketDatas();
			}
			if (NetworkManager.CanLog(LoggingType.Common))
			{
				Transport transport = NetworkManager.TransportManager.GetTransport(args.TransportIndex);
				string text = ((transport == null) ? "Unknown" : transport.GetType().Name);
				string text2 = string.Empty;
				if (connectionState == LocalConnectionState.Starting)
				{
					text2 = $" Server IP is {transport.GetClientAddress()}, port is {transport.GetPort()}.";
				}
				Debug.Log("Local client is " + connectionState.ToString().ToLower() + " for " + text + "." + text2);
			}
			NetworkManager.UpdateFramerate();
			this.OnClientConnectionState?.Invoke(args);
		}

		private void Transport_OnClientReceivedData(ClientReceivedDataArgs args)
		{
			ParseReceived(args);
		}

		private void TransportManager_OnIterateIncomingEnd(bool server)
		{
			if (Started && !server)
			{
				Objects.IterateObjectCache();
			}
		}

		private void ParseReceived(ClientReceivedDataArgs args)
		{
			UpdateLastPacketDatas();
			ArraySegment<byte> data = args.Data;
			NetworkManager.StatisticsManager.NetworkTraffic.LocalClientReceivedData((ulong)data.Count);
			if (data.Count > 4)
			{
				PooledReader pooledReader = ReaderPool.Retrieve(data, NetworkManager, Reader.DataSource.Server);
				NetworkManager.TimeManager.SetLastPacketTick(pooledReader.ReadTickUnpacked());
				ParseReader(pooledReader, args.Channel);
				ReaderPool.Store(pooledReader);
			}
		}

		internal void ParseReader(PooledReader reader, Channel channel, bool print = false)
		{
			bool hasIntermediateLayer = NetworkManager.TransportManager.HasIntermediateLayer;
			PacketId packetId = PacketId.Unset;
			try
			{
				Reader.DataSource source = Reader.DataSource.Server;
				if (reader.PeekPacketId() == PacketId.Split)
				{
					reader.ReadPacketId();
					_splitReader.GetHeader(reader, out var expectedMessages);
					_splitReader.Write(NetworkManager.TimeManager.LastPacketTick, reader, expectedMessages);
					ArraySegment<byte> fullMessage = _splitReader.GetFullMessage();
					if (fullMessage.Count == 0)
					{
						return;
					}
					if (hasIntermediateLayer)
					{
						reader.Initialize(NetworkManager.TransportManager.ProcessIntermediateIncoming(fullMessage, fromServer: true), NetworkManager, source);
					}
					else
					{
						reader.Initialize(fullMessage, NetworkManager, source);
					}
				}
				else if (hasIntermediateLayer)
				{
					ArraySegment<byte> segment = NetworkManager.TransportManager.ProcessIntermediateIncoming(reader.GetRemainingData(), fromServer: false);
					reader.Initialize(segment, NetworkManager, source);
				}
				while (reader.Remaining > 0)
				{
					packetId = reader.ReadPacketId();
					if (packetId == PacketId.ObjectSpawn || packetId == PacketId.ObjectDespawn)
					{
						switch (packetId)
						{
						case PacketId.ObjectSpawn:
							Objects.CacheSpawn(reader);
							break;
						case PacketId.ObjectDespawn:
							Objects.CacheDespawn(reader);
							break;
						}
						continue;
					}
					Objects.IterateObjectCache();
					if ((int)packetId >= (int)NetworkManager.StartingRpcLinkIndex)
					{
						Objects.ParseRpcLink(reader, (ushort)packetId, channel);
						continue;
					}
					switch (packetId)
					{
					case PacketId.Replicate:
						Objects.ParseReplicateRpc(reader, null, channel);
						break;
					case PacketId.Reconcile:
						Objects.ParseReconcileRpc(reader, channel);
						break;
					case PacketId.ObserversRpc:
						Objects.ParseObserversRpc(reader, channel);
						break;
					case PacketId.TargetRpc:
						Objects.ParseTargetRpc(reader, channel);
						break;
					case PacketId.Broadcast:
						ParseBroadcast(reader, channel);
						break;
					case PacketId.PingPong:
						ParsePingPong(reader);
						break;
					case PacketId.SyncVar:
						Objects.ParseSyncType(reader, isSyncObject: false, channel);
						break;
					case PacketId.SyncObject:
						Objects.ParseSyncType(reader, isSyncObject: true, channel);
						break;
					case PacketId.PredictedSpawnResult:
						Objects.ParsePredictedSpawnResult(reader);
						break;
					case PacketId.TimingUpdate:
						NetworkManager.TimeManager.ParseTimingUpdate(reader);
						break;
					case PacketId.OwnershipChange:
						Objects.ParseOwnershipChange(reader);
						break;
					case PacketId.Authenticated:
						ParseAuthenticated(reader);
						break;
					case PacketId.Disconnect:
						reader.Clear();
						StopConnection();
						break;
					default:
						NetworkManager.LogError($"Client received an unhandled PacketId of {(ushort)packetId} on channel {channel}. Remaining data has been purged.");
						return;
					}
				}
				Objects.IterateObjectCache();
			}
			catch (Exception ex)
			{
				if (NetworkManager.CanLog(LoggingType.Error))
				{
					Debug.LogError($"Client encountered an error while parsing data for packetId {packetId}. Message: {ex.Message}.");
				}
			}
		}

		private void ParsePingPong(PooledReader reader)
		{
			uint clientTick = reader.ReadTickUnpacked();
			NetworkManager.TimeManager.ModifyPing(clientTick);
		}

		private void ParseAuthenticated(PooledReader reader)
		{
			NetworkManager networkManager = NetworkManager;
			int num = reader.ReadNetworkConnectionId();
			NetworkConnection value;
			if (!networkManager.IsServer)
			{
				Clients.TryGetValueIL2CPP(num, out Connection);
				if (Connection == null)
				{
					NetworkManager.LogWarning("Client connection could not be found while parsing authenticated status. This usually occurs when the client is receiving a packet immediately before losing connection.");
					Connection = new NetworkConnection(networkManager, num, GetTransportIndex(), asServer: false);
				}
			}
			else if (networkManager.ServerManager.Clients.TryGetValueIL2CPP(num, out value))
			{
				Connection = value;
			}
			else
			{
				networkManager.LogError($"Unable to lookup LocalConnection for {num} as host.");
				Connection = new NetworkConnection(networkManager, num, GetTransportIndex(), asServer: false);
			}
			if (NetworkManager.PredictionManager.GetAllowPredictedSpawning())
			{
				byte b = reader.ReadByte();
				Queue<int> predictedObjectIds = Connection.PredictedObjectIds;
				for (int i = 0; i < b; i++)
				{
					predictedObjectIds.Enqueue(reader.ReadNetworkObjectId());
				}
			}
			if (!networkManager.IsServer)
			{
				networkManager.TimeManager.Tick = networkManager.TimeManager.LastPacketTick;
			}
			Connection.ConnectionAuthenticated();
			this.OnAuthenticated?.Invoke();
			Objects.RegisterAndDespawnSceneObjects();
		}

		private void TimeManager_OnPostTick()
		{
			CheckServerTimeout();
		}

		private void CheckServerTimeout()
		{
			if (Started && !NetworkManager.IsServer && _remoteServerTimeout != RemoteTimeoutType.Disabled && !NetworkManager.SceneManager.IsIteratingQueue(2f) && Time.unscaledTime - _lastPacketTime > (float)(int)_remoteServerTimeoutDuration)
			{
				this.OnClientTimeOut?.Invoke();
				NetworkManager.Log("Server has timed out. You can modify this feature on the ClientManager component.");
				StopConnection();
			}
		}

		internal void TrySendLodUpdate(uint localTick, bool forceFullUpdate)
		{
		}
	}
}
