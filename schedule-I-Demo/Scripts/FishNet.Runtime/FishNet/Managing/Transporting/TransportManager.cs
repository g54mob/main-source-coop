using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Managing.Timing;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Transporting;
using FishNet.Transporting.Multipass;
using FishNet.Transporting.Tugboat;
using UnityEngine;

namespace FishNet.Managing.Transporting
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Manager/TransportManager")]
	public sealed class TransportManager : MonoBehaviour
	{
		private struct DisconnectingClient
		{
			public uint Tick;

			public NetworkConnection Connection;

			public DisconnectingClient(uint tick, NetworkConnection connection)
			{
				Tick = tick;
				Connection = connection;
			}
		}

		[Tooltip("The current Transport being used.")]
		public Transport Transport;

		[Tooltip("Layer used to modify data before it is sent or received.")]
		[SerializeField]
		private IntermediateLayer _intermediateLayer;

		[Tooltip("Latency simulation settings.")]
		[SerializeField]
		private LatencySimulator _latencySimulator = new LatencySimulator();

		private List<NetworkConnection> _dirtyToClients = new List<NetworkConnection>();

		private List<PacketBundle> _toServerBundles = new List<PacketBundle>();

		private NetworkManager _networkManager;

		private List<DisconnectingClient> _disconnectingClients = new List<DisconnectingClient>();

		private int[] _lowestMtu;

		private HashSet<NetworkConnection> _networkConnectionHashSet = new HashSet<NetworkConnection>();

		public const byte PACKET_ID_BYTES = 2;

		public const byte OBJECT_ID_BYTES = 2;

		public const byte COMPONENT_INDEX_BYTES = 1;

		public const byte TICK_BYTES = 4;

		private const byte SPLIT_COUNT_BYTES = 4;

		public const byte SPLIT_INDICATOR_SIZE = 6;

		public const byte CHANNEL_COUNT = 2;

		public bool HasIntermediateLayer => _intermediateLayer != null;

		public LatencySimulator LatencySimulator
		{
			get
			{
				if (_latencySimulator == null)
				{
					_latencySimulator = new LatencySimulator();
				}
				return _latencySimulator;
			}
		}

		internal event Action OnIterateOutgoingStart;

		internal event Action OnIterateOutgoingEnd;

		internal event Action<bool> OnIterateIncomingStart;

		internal event Action<bool> OnIterateIncomingEnd;

		internal void InitializeOnce_Internal(NetworkManager manager)
		{
			_networkManager = manager;
			if (Transport == null && !base.gameObject.TryGetComponent<Transport>(out Transport))
			{
				Transport = base.gameObject.AddComponent<Tugboat>();
			}
			Transport.Initialize(_networkManager, 0);
			_lowestMtu = new int[2];
			for (byte b = 0; b < 2; b++)
			{
				_lowestMtu[b] = GetLowestMTU(b);
			}
			InitializeToServerBundles();
			if (_intermediateLayer != null)
			{
				_intermediateLayer.InitializeOnce(this);
			}
		}

		internal void ServerDirty(NetworkConnection conn)
		{
			_dirtyToClients.Add(conn);
		}

		private void InitializeToServerBundles()
		{
			for (byte b = 0; b < 2; b++)
			{
				int lowestMTU = GetLowestMTU(b);
				_toServerBundles.Add(new PacketBundle(_networkManager, lowestMTU));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetLowestMTU(byte channel)
		{
			if (_lowestMtu[channel] > 0)
			{
				return _lowestMtu[channel];
			}
			if (Transport is Multipass multipass)
			{
				int? num = null;
				foreach (Transport transport in multipass.Transports)
				{
					int mTU = transport.GetMTU(channel);
					if (!num.HasValue || mTU < num.Value)
					{
						num = mTU;
					}
				}
				if (!num.HasValue)
				{
					return -1;
				}
				int num2 = num.Value;
				if (num2 >= 0)
				{
					num2--;
				}
				return num2;
			}
			return GetMTU(channel);
		}

		public int GetMTU(byte channel)
		{
			int num = Transport.GetMTU(channel);
			if (num >= 0)
			{
				num--;
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetMTU(int transportIndex, byte channel)
		{
			if (Transport is Multipass multipass)
			{
				int num = multipass.GetMTU(channel, transportIndex);
				if (num >= 0)
				{
					num--;
				}
				return num;
			}
			if (transportIndex == 0)
			{
				return GetMTU(channel);
			}
			_networkManager.LogWarning("MTU cannot be returned with transportIndex because " + typeof(Multipass).Name + " is not in use.");
			return -1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetMTU<T>(byte channel) where T : Transport
		{
			Transport transport = GetTransport<T>();
			if (transport != null)
			{
				int num = transport.GetMTU(channel);
				if (num >= 0)
				{
					num--;
				}
				return num;
			}
			return -1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ArraySegment<byte> ProcessIntermediateIncoming(ArraySegment<byte> src, bool fromServer)
		{
			return _intermediateLayer.HandleIncoming(src, fromServer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ArraySegment<byte> ProcessIntermediateOutgoing(ArraySegment<byte> src, bool toServer)
		{
			return _intermediateLayer.HandleOutgoing(src, toServer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void SendToClient(byte channelId, ArraySegment<byte> segment, NetworkConnection connection, bool splitLargeMessages = true, DataOrderType orderType = DataOrderType.Default)
		{
			if (HasIntermediateLayer)
			{
				segment = ProcessIntermediateOutgoing(segment, toServer: false);
			}
			SetSplitValues(channelId, segment, splitLargeMessages, out var requiredSplitMessages, out var maxSplitMessageSize);
			SendToClient(channelId, segment, connection, requiredSplitMessages, maxSplitMessageSize, orderType);
		}

		private void SendToClient(byte channelId, ArraySegment<byte> segment, NetworkConnection connection, int requiredSplitMessages, int maxSplitMessageSize, DataOrderType orderType = DataOrderType.Default)
		{
			if (!(connection == null))
			{
				if (requiredSplitMessages > 1)
				{
					SendSplitData(connection, ref segment, requiredSplitMessages, maxSplitMessageSize, orderType);
				}
				else
				{
					connection.SendToClient(channelId, segment, forceNewBuffer: false, orderType);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void SendToClients(byte channelId, ArraySegment<byte> segment, HashSet<NetworkConnection> observers, NetworkConnection excludedConnection = null, bool splitLargeMessages = true, DataOrderType orderType = DataOrderType.Default)
		{
			_networkConnectionHashSet.Clear();
			_networkConnectionHashSet.Add(excludedConnection);
			SendToClients(channelId, segment, observers, _networkConnectionHashSet, splitLargeMessages, orderType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void SendToClients(byte channelId, ArraySegment<byte> segment, HashSet<NetworkConnection> observers, HashSet<NetworkConnection> excludedConnections = null, bool splitLargeMessages = true, DataOrderType orderType = DataOrderType.Default)
		{
			if (HasIntermediateLayer)
			{
				segment = ProcessIntermediateOutgoing(segment, toServer: false);
			}
			SetSplitValues(channelId, segment, splitLargeMessages, out var requiredSplitMessages, out var maxSplitMessageSize);
			SendToClients(channelId, segment, observers, excludedConnections, requiredSplitMessages, maxSplitMessageSize, orderType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SendToClients(byte channelId, ArraySegment<byte> segment, HashSet<NetworkConnection> observers, HashSet<NetworkConnection> excludedConnections, int requiredSplitMessages, int maxSplitMessageSize, DataOrderType orderType = DataOrderType.Default)
		{
			if (excludedConnections == null || excludedConnections.Count == 0)
			{
				foreach (NetworkConnection observer in observers)
				{
					SendToClient(channelId, segment, observer, requiredSplitMessages, maxSplitMessageSize, orderType);
				}
				return;
			}
			foreach (NetworkConnection observer2 in observers)
			{
				if (!excludedConnections.Contains(observer2))
				{
					SendToClient(channelId, segment, observer2, requiredSplitMessages, maxSplitMessageSize, orderType);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void SendToClients(byte channelId, ArraySegment<byte> segment, bool splitLargeMessages = true)
		{
			if (HasIntermediateLayer)
			{
				segment = ProcessIntermediateOutgoing(segment, toServer: false);
			}
			SetSplitValues(channelId, segment, splitLargeMessages, out var requiredSplitMessages, out var maxSplitMessageSize);
			SendToClients_Internal(channelId, segment, requiredSplitMessages, maxSplitMessageSize);
		}

		private void SendToClients_Internal(byte channelId, ArraySegment<byte> segment, int requiredSplitMessages, int maxSplitMessageSize)
		{
			foreach (NetworkConnection value in _networkManager.ServerManager.Clients.Values)
			{
				SendToClient(channelId, segment, value, requiredSplitMessages, maxSplitMessageSize);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void SendToServer(byte channelId, ArraySegment<byte> segment, bool splitLargeMessages = true, DataOrderType orderType = DataOrderType.Default)
		{
			if (HasIntermediateLayer)
			{
				segment = ProcessIntermediateOutgoing(segment, toServer: true);
			}
			SetSplitValues(channelId, segment, splitLargeMessages, out var requiredSplitMessages, out var maxSplitMessageSize);
			SendToServer(channelId, segment, requiredSplitMessages, maxSplitMessageSize, orderType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SendToServer(byte channelId, ArraySegment<byte> segment, int requiredSplitMessages, int maxSplitMessageSize, DataOrderType orderType)
		{
			if (channelId >= _toServerBundles.Count)
			{
				channelId = 0;
			}
			if (requiredSplitMessages > 1)
			{
				SendSplitData(null, ref segment, requiredSplitMessages, maxSplitMessageSize, orderType);
			}
			else
			{
				_toServerBundles[channelId].Write(segment, forceNewBuffer: false, orderType);
			}
		}

		private void SetSplitValues(byte channelId, ArraySegment<byte> segment, bool split, out int requiredSplitMessages, out int maxSplitMessageSize)
		{
			if (!split)
			{
				requiredSplitMessages = 0;
				maxSplitMessageSize = 0;
			}
			else
			{
				SplitRequired(channelId, segment.Count, out requiredSplitMessages, out maxSplitMessageSize);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void CheckSetReliableChannel(int dataLength, ref Channel channel)
		{
			if (channel != Channel.Reliable && GetRequiredMessageCount((byte)channel, dataLength, out var _) > 1)
			{
				channel = Channel.Reliable;
			}
		}

		private int GetRequiredMessageCount(byte channelId, int segmentSize, out int maxMessageSize)
		{
			maxMessageSize = GetLowestMTU(channelId) - 10;
			return Mathf.CeilToInt((float)segmentSize / (float)maxMessageSize);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool SplitRequired(byte channelId, int segmentSize, out int requiredMessages, out int maxMessageSize)
		{
			requiredMessages = GetRequiredMessageCount(channelId, segmentSize, out maxMessageSize);
			bool num = requiredMessages > 1;
			if (num && channelId != 0)
			{
				_networkManager.LogError($"A message of length {segmentSize} requires the reliable channel but was sent on channel {(Channel)channelId}. Please file this stack trace as a bug report.");
			}
			return num;
		}

		private void SendSplitData(NetworkConnection conn, ref ArraySegment<byte> segment, int requiredMessages, int maxMessageSize, DataOrderType orderType)
		{
			if (requiredMessages <= 1)
			{
				_networkManager.LogError($"SendSplitData was called with {requiredMessages} required messages. This method should only be called if messages must be split into 2 pieces or more.");
				return;
			}
			byte b = 0;
			PooledWriter pooledWriter = WriterPool.Retrieve();
			pooledWriter.WritePacketId(PacketId.Split);
			pooledWriter.WriteInt32(requiredMessages);
			ArraySegment<byte> arraySegment = pooledWriter.GetArraySegment();
			int i = 0;
			bool flag = true;
			int num2;
			for (; i < segment.Count; i += num2)
			{
				int num = 0;
				if (flag)
				{
					num = arraySegment.Count;
					flag = false;
				}
				num2 = Mathf.Min(segment.Count - i - num, maxMessageSize);
				ArraySegment<byte> segment2 = new ArraySegment<byte>(segment.Array, segment.Offset + i, num2);
				if (conn != null)
				{
					conn.SendToClient(b, arraySegment, forceNewBuffer: true);
					conn.SendToClient(b, segment2);
				}
				else
				{
					_toServerBundles[b].Write(arraySegment, forceNewBuffer: true, orderType);
					_toServerBundles[b].Write(segment2, forceNewBuffer: false, orderType);
				}
			}
			pooledWriter.Store();
		}

		internal void IterateIncoming(bool server)
		{
			this.OnIterateIncomingStart?.Invoke(server);
			Transport.IterateIncoming(server);
			this.OnIterateIncomingEnd?.Invoke(server);
		}

		internal void IterateOutgoing(bool toServer)
		{
			this.OnIterateOutgoingStart?.Invoke();
			int num = 2;
			ulong sentBytes = 0uL;
			byte channel2;
			if (!toServer)
			{
				TimeManager timeManager = _networkManager.TimeManager;
				uint localTick = timeManager.LocalTick;
				_networkManager.ServerManager.Objects.WriteDirtySyncTypes();
				int count = _dirtyToClients.Count;
				for (int i = 0; i < count; i++)
				{
					NetworkConnection conn = _dirtyToClients[i];
					if (conn == null || !conn.IsValid)
					{
						continue;
					}
					byte channel;
					for (channel = 0; channel < num; channel++)
					{
						if (conn.GetPacketBundle(channel, out var packetBundle))
						{
							ProcessPacketBundle(packetBundle);
							ProcessPacketBundle(packetBundle.GetSendLastBundle(), isLast: true);
						}
					}
					if (conn.Disconnecting)
					{
						uint val = timeManager.TimeToTicks(0.1, TickRounding.RoundUp);
						val = Math.Max(val, 2u);
						_disconnectingClients.Add(new DisconnectingClient(val + localTick, conn));
					}
					conn.ResetServerDirty();
					void ProcessPacketBundle(PacketBundle ppb, bool isLast = false)
					{
						for (int k = 0; k < ppb.WrittenBuffers; k++)
						{
							if (ppb.GetBuffer(k, out var bb))
							{
								ArraySegment<byte> segment = new ArraySegment<byte>(bb.Data, 0, bb.Length);
								Transport.SendToClient(channel, segment, conn.ClientId);
								sentBytes += (ulong)segment.Count;
							}
						}
						ppb.Reset(resetSendLast: false);
					}
				}
				for (int j = 0; j < _disconnectingClients.Count; j++)
				{
					DisconnectingClient disconnectingClient = _disconnectingClients[j];
					if (localTick >= disconnectingClient.Tick)
					{
						_networkManager.TransportManager.Transport.StopConnection(disconnectingClient.Connection.ClientId, immediately: true);
						_disconnectingClients.RemoveAt(j);
						j--;
					}
				}
				_networkManager.StatisticsManager.NetworkTraffic.LocalServerSentData(sentBytes);
				if (count == _dirtyToClients.Count)
				{
					_dirtyToClients.Clear();
				}
				else if (count > 0)
				{
					_dirtyToClients.RemoveRange(0, count);
				}
			}
			else
			{
				for (channel2 = 0; channel2 < num; channel2++)
				{
					if (PacketBundle.GetPacketBundle(channel2, _toServerBundles, out var mtuBuffer))
					{
						ProcessPacketBundle2(mtuBuffer);
						ProcessPacketBundle2(mtuBuffer.GetSendLastBundle());
					}
				}
				_networkManager.StatisticsManager.NetworkTraffic.LocalClientSentData(sentBytes);
			}
			Transport.IterateOutgoing(toServer);
			this.OnIterateOutgoingEnd?.Invoke();
			void ProcessPacketBundle2(PacketBundle ppb)
			{
				for (int k = 0; k < ppb.WrittenBuffers; k++)
				{
					if (ppb.GetBuffer(k, out var bb))
					{
						ArraySegment<byte> segment = new ArraySegment<byte>(bb.Data, 0, bb.Length);
						Transport.SendToServer(channel2, segment);
						sentBytes += (ulong)segment.Count;
					}
				}
				ppb.Reset(resetSendLast: false);
			}
		}

		public bool IsLocalTransport(int connectionId)
		{
			if (!(Transport == null))
			{
				return Transport.IsLocalTransport(connectionId);
			}
			return false;
		}

		public Transport GetTransport(int index)
		{
			if (Transport is Multipass multipass)
			{
				return multipass.GetTransport(index);
			}
			return Transport;
		}

		public T GetTransport<T>() where T : Transport
		{
			if (Transport is Multipass multipass)
			{
				if (typeof(T) == typeof(Multipass))
				{
					return (T)(Transport)multipass;
				}
				return multipass.GetTransport<T>();
			}
			if (Transport.GetType() == typeof(T))
			{
				return (T)Transport;
			}
			return null;
		}
	}
}
