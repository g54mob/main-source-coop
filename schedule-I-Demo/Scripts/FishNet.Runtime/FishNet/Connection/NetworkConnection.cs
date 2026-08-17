using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Broadcast;
using FishNet.Component.Observing;
using FishNet.Documenting;
using FishNet.Managing;
using FishNet.Managing.Logging;
using FishNet.Managing.Server;
using FishNet.Managing.Timing;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Transporting;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Connection
{
	public class NetworkConnection : IEquatable<NetworkConnection>
	{
		public class LevelOfDetailData : IResettable
		{
			public byte CurrentLevelOfDetail;

			public byte PreviousLevelOfDetail;

			internal void Update(byte lodLevel)
			{
				PreviousLevelOfDetail = CurrentLevelOfDetail;
				CurrentLevelOfDetail = lodLevel;
			}

			public void ResetState()
			{
				CurrentLevelOfDetail = 0;
				PreviousLevelOfDetail = 0;
			}

			public void InitializeState()
			{
			}
		}

		private List<PacketBundle> _toClientBundles = new List<PacketBundle>();

		private bool _serverDirtied;

		private bool _loadedStartScenesAsServer;

		private bool _loadedStartScenesAsClient;

		internal Queue<int> PredictedObjectIds = new Queue<int>();

		public int ClientId = -1;

		public HashSet<NetworkObject> Objects = new HashSet<NetworkObject>();

		public object CustomData;

		internal uint ServerConnectionTick;

		public EstimatedTick PacketTick;

		public EstimatedTick LocalTick;

		public const int UNSET_CLIENTID_VALUE = -1;

		public Dictionary<NetworkObject, LevelOfDetailData> LevelOfDetails = new Dictionary<NetworkObject, LevelOfDetailData>(new NetworkObjectIdComparer());

		internal int AllowedForcedLodUpdates;

		internal uint LastLevelOfDetailUpdate;

		internal int LevelOfDetailInfractions;

		internal GridEntry HashGridEntry = HashGrid.EmptyGridEntry;

		private HashGrid _hashGrid;

		private float _nextHashGridUpdateTime;

		private Vector2Int _hashGridPosition = HashGrid.UnsetGridPosition;

		private uint _lastPingTick;

		private uint _requiredPingTicks;

		private const byte EXCESSIVE_PING_LIMIT = 10;

		private MovingAverage _replicateQueueAverage;

		private uint _lastAverageQueueAddTick;

		public NetworkManager NetworkManager { get; private set; }

		public int TransportIndex { get; internal set; } = -1;

		public bool Authenticated { get; private set; }

		public bool IsActive
		{
			get
			{
				if (ClientId >= 0)
				{
					return !Disconnecting;
				}
				return false;
			}
		}

		public bool IsValid => ClientId >= 0;

		public NetworkObject FirstObject { get; private set; }

		public HashSet<Scene> Scenes { get; private set; } = new HashSet<Scene>();

		public bool Disconnecting { get; private set; }

		internal uint DisconnectingTick { get; private set; }

		[Obsolete("Use LocalTick instead.")]
		public uint Tick => LocalTick.Value(NetworkManager.TimeManager);

		public uint LocalReplicateTick { get; internal set; }

		public bool IsHost
		{
			get
			{
				if (!(NetworkManager == null))
				{
					if (NetworkManager.IsServer)
					{
						return this == NetworkManager.ClientManager.Connection;
					}
					return false;
				}
				return false;
			}
		}

		public bool IsLocalClient
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.ClientManager.Connection == this;
				}
				return false;
			}
		}

		public event Action<NetworkConnection, bool> OnLoadedStartScenes;

		public event Action<NetworkObject> OnObjectAdded;

		public event Action<NetworkObject> OnObjectRemoved;

		private void InitializeBuffer()
		{
			for (byte b = 0; b < 2; b++)
			{
				int lowestMTU = NetworkManager.TransportManager.GetLowestMTU(b);
				_toClientBundles.Add(new PacketBundle(NetworkManager, lowestMTU));
			}
		}

		public void Broadcast<T>(T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (!IsActive)
			{
				NetworkManager.LogError("Connection is not valid, cannot send broadcast.");
			}
			else
			{
				NetworkManager.ServerManager.Broadcast(this, message, requireAuthenticated, channel);
			}
		}

		internal void SendToClient(byte channel, ArraySegment<byte> segment, bool forceNewBuffer = false, DataOrderType orderType = DataOrderType.Default)
		{
			if (Disconnecting)
			{
				return;
			}
			if (!IsActive)
			{
				NetworkManager?.LogWarning($"Data cannot be sent to connection {ClientId} because it is not active.");
				return;
			}
			if (channel >= _toClientBundles.Count)
			{
				channel = 0;
			}
			_toClientBundles[channel].Write(segment, forceNewBuffer, orderType);
			ServerDirty();
		}

		internal bool GetPacketBundle(int channel, out PacketBundle packetBundle)
		{
			return PacketBundle.GetPacketBundle(channel, _toClientBundles, out packetBundle);
		}

		private void ServerDirty()
		{
			bool serverDirtied = _serverDirtied;
			_serverDirtied = true;
			if (!serverDirtied)
			{
				NetworkManager.TransportManager.ServerDirty(this);
			}
		}

		internal void ResetServerDirty()
		{
			_serverDirtied = false;
		}

		public bool LoadedStartScenes()
		{
			if (!_loadedStartScenesAsServer)
			{
				return _loadedStartScenesAsClient;
			}
			return true;
		}

		public bool LoadedStartScenes(bool asServer)
		{
			if (asServer)
			{
				return _loadedStartScenesAsServer;
			}
			return _loadedStartScenesAsClient;
		}

		public void SetFirstObject(NetworkObject nob)
		{
			if (!Objects.Contains(nob))
			{
				string value = $"FirstObject for {ClientId} cannot be set to {nob.name} as it's not within Objects for this connection.";
				if (NetworkManager == null)
				{
					NetworkManager.StaticLogError(value);
				}
				else
				{
					NetworkManager.LogError(value);
				}
			}
			else
			{
				FirstObject = nob;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is NetworkConnection networkConnection)
			{
				return networkConnection.ClientId == ClientId;
			}
			return false;
		}

		public bool Equals(NetworkConnection nc)
		{
			if ((object)nc == null)
			{
				return false;
			}
			if (ClientId == -1 || nc.ClientId == -1)
			{
				return false;
			}
			if ((object)this == nc)
			{
				return true;
			}
			return ClientId == nc.ClientId;
		}

		public override int GetHashCode()
		{
			return ClientId;
		}

		public static bool operator ==(NetworkConnection a, NetworkConnection b)
		{
			if ((object)a == null && (object)b == null)
			{
				return true;
			}
			if ((object)a == null && (object)b != null)
			{
				return false;
			}
			if (!(b == null))
			{
				return b.Equals(a);
			}
			return a.Equals(b);
		}

		public static bool operator !=(NetworkConnection a, NetworkConnection b)
		{
			return !(a == b);
		}

		[APIExclude]
		public NetworkConnection()
		{
		}

		[APIExclude]
		public NetworkConnection(NetworkManager manager, int clientId, int transportIndex, bool asServer)
		{
			Initialize(manager, clientId, transportIndex, asServer);
		}

		internal void Dispose()
		{
			Deinitialize();
		}

		public override string ToString()
		{
			int clientId = ClientId;
			string arg = ((NetworkManager != null) ? NetworkManager.TransportManager.Transport.GetConnectionAddress(clientId) : "Unset");
			return $"Id [{ClientId}] Address [{arg}]";
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Initialize(NetworkManager nm, int clientId, int transportIndex, bool asServer)
		{
			NetworkManager = nm;
			if (asServer)
			{
				ServerConnectionTick = nm.TimeManager.LocalTick;
			}
			TransportIndex = transportIndex;
			ClientId = clientId;
			PacketTick.Update(nm.TimeManager, 0u, EstimatedTick.OldTickOption.SetLastRemoteTick);
			Observers_Initialize(nm);
			Prediction_Initialize(nm, asServer);
			if (asServer)
			{
				InitializeBuffer();
				InitializePing();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Deinitialize()
		{
			MatchCondition.RemoveFromMatchesWithoutRebuild(this, NetworkManager);
			foreach (PacketBundle toClientBundle in _toClientBundles)
			{
				toClientBundle.Dispose();
			}
			_toClientBundles.Clear();
			ServerConnectionTick = 0u;
			PacketTick.Reset();
			TransportIndex = -1;
			ClientId = -1;
			ClearObjects();
			Authenticated = false;
			NetworkManager = null;
			_loadedStartScenesAsClient = false;
			_loadedStartScenesAsServer = false;
			SetDisconnecting(value: false);
			Scenes.Clear();
			PredictedObjectIds.Clear();
			ResetPingPong();
			ResetStates_Lod();
			AllowedForcedLodUpdates = 0;
			LastLevelOfDetailUpdate = 0u;
			LevelOfDetailInfractions = 0;
			Observers_Reset();
			Prediction_Reset();
		}

		internal void SetDisconnecting(bool value)
		{
			Disconnecting = value;
			if (Disconnecting)
			{
				DisconnectingTick = NetworkManager.TimeManager.LocalTick;
			}
		}

		public void Disconnect(bool immediately)
		{
			if (!IsValid)
			{
				NetworkManager.StaticLogWarning("Disconnect called on an invalid connection.");
				return;
			}
			if (Disconnecting)
			{
				NetworkManager.LogWarning($"ClientId {ClientId} is already disconnecting.");
				return;
			}
			SetDisconnecting(value: true);
			if (immediately)
			{
				NetworkManager.TransportManager.Transport.StopConnection(ClientId, immediately: true);
			}
			else
			{
				ServerDirty();
			}
		}

		internal bool SetLoadedStartScenes(bool asServer)
		{
			bool result = !(asServer ? _loadedStartScenesAsServer : _loadedStartScenesAsClient);
			if (asServer)
			{
				_loadedStartScenesAsServer = true;
			}
			else
			{
				_loadedStartScenesAsClient = true;
			}
			Action<NetworkConnection, bool> action = this.OnLoadedStartScenes;
			if (action != null)
			{
				action(this, asServer);
				return result;
			}
			return result;
		}

		internal void ConnectionAuthenticated()
		{
			Authenticated = true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void AddObject(NetworkObject nob)
		{
			if (IsValid)
			{
				Objects.Add(nob);
				if (Objects.Count == 1)
				{
					SetFirstObject();
				}
				this.OnObjectAdded?.Invoke(nob);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void RemoveObject(NetworkObject nob)
		{
			if (!IsValid)
			{
				ClearObjects();
				return;
			}
			Objects.Remove(nob);
			if (nob == FirstObject)
			{
				SetFirstObject();
			}
			this.OnObjectRemoved?.Invoke(nob);
		}

		private void ClearObjects()
		{
			Objects.Clear();
			FirstObject = null;
		}

		private void SetFirstObject()
		{
			if (Objects.Count == 0)
			{
				FirstObject = null;
				return;
			}
			using HashSet<NetworkObject>.Enumerator enumerator = Objects.GetEnumerator();
			if (enumerator.MoveNext())
			{
				NetworkObject current = enumerator.Current;
				FirstObject = current;
				Observers_FirstObjectChanged();
			}
		}

		internal bool AddToScene(Scene scene)
		{
			return Scenes.Add(scene);
		}

		internal bool RemoveFromScene(Scene scene)
		{
			return Scenes.Remove(scene);
		}

		internal bool IsLateForLevelOfDetail(uint expectedInterval)
		{
			if (IsLocalClient)
			{
				return false;
			}
			return PacketTick.RemoteTick - LastLevelOfDetailUpdate > expectedInterval;
		}

		private void ResetStates_Lod()
		{
			foreach (LevelOfDetailData value in LevelOfDetails.Values)
			{
				ResettableObjectCaches<LevelOfDetailData>.Store(value);
			}
			LevelOfDetails.Clear();
		}

		private void Observers_FirstObjectChanged()
		{
			UpdateHashGridPositions(force: true);
		}

		private void Observers_Initialize(NetworkManager nm)
		{
			nm.TryGetInstance<HashGrid>(out _hashGrid);
		}

		internal void UpdateHashGridPositions(bool force)
		{
			if (_hashGrid == null)
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			if (!force && unscaledTime < _nextHashGridUpdateTime)
			{
				return;
			}
			_nextHashGridUpdateTime = unscaledTime + 1f;
			if (FirstObject == null)
			{
				HashGridEntry = HashGrid.EmptyGridEntry;
				_hashGridPosition = HashGrid.UnsetGridPosition;
				return;
			}
			Vector2Int hashGridPosition = _hashGrid.GetHashGridPosition(FirstObject);
			if (hashGridPosition != _hashGridPosition)
			{
				_hashGridPosition = hashGridPosition;
				HashGridEntry = _hashGrid.GetGridEntry(hashGridPosition);
			}
		}

		private void Observers_Reset()
		{
			_hashGrid = null;
			_hashGridPosition = HashGrid.UnsetGridPosition;
			_nextHashGridUpdateTime = 0f;
		}

		private void InitializePing()
		{
			float num = (float)(int)NetworkManager.TimeManager.PingInterval * 0.85f;
			_requiredPingTicks = NetworkManager.TimeManager.TimeToTicks(num, TickRounding.RoundDown);
		}

		private void ResetPingPong()
		{
			_lastPingTick = 0u;
		}

		internal bool CanPingPong()
		{
			TimeManager timeManager = ((NetworkManager == null) ? InstanceFinder.TimeManager : NetworkManager.TimeManager);
			if (timeManager.LowFrameRate)
			{
				return false;
			}
			uint tick = timeManager.Tick;
			uint num = tick - _lastPingTick;
			_lastPingTick = tick;
			if (num < _requiredPingTicks)
			{
				return false;
			}
			return true;
		}

		internal void Prediction_Initialize(NetworkManager manager, bool asServer)
		{
			if (asServer)
			{
				int sampleSize = (int)Mathf.Max((float)(int)manager.TimeManager.TickRate * 0.25f, 3f);
				_replicateQueueAverage = new MovingAverage(sampleSize);
			}
		}

		internal void AddAverageQueueCount(ushort value, uint tick)
		{
			if (tick - _lastAverageQueueAddTick > _replicateQueueAverage.SampleSize)
			{
				_replicateQueueAverage.Reset();
			}
			_lastAverageQueueAddTick = tick;
			_replicateQueueAverage.ComputeAverage((int)value);
		}

		internal ushort GetAndResetAverageQueueCount()
		{
			if (_replicateQueueAverage == null)
			{
				return 0;
			}
			int num = (int)_replicateQueueAverage.Average;
			if (num < 0)
			{
				num = 0;
			}
			return (ushort)num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Prediction_Reset()
		{
			GetAndResetAverageQueueCount();
		}

		public string GetAddress()
		{
			if (!IsValid)
			{
				return string.Empty;
			}
			if (NetworkManager == null)
			{
				return string.Empty;
			}
			return NetworkManager.TransportManager.Transport.GetConnectionAddress(ClientId);
		}

		public void Kick(KickReason kickReason, LoggingType loggingType = LoggingType.Common, string log = "")
		{
			NetworkManager.ServerManager.Kick(this, kickReason, loggingType, log);
		}

		public void Kick(Reader reader, KickReason kickReason, LoggingType loggingType = LoggingType.Common, string log = "")
		{
			NetworkManager.ServerManager.Kick(this, reader, kickReason, loggingType, log);
		}
	}
}
