#define TRACE
#define DEBUG
using System;
using System.Threading;
using Fusion.Sockets.V2;

namespace Fusion.Sockets
{
	public struct NetPeerGroup
	{
		private const double RELIABLE_SEND_INTERVAL = 0.05;

		private unsafe NetPeer* _peer;

		private short _group;

		internal Timer _clock;

		private NetConfig _config;

		private uint _counter;

		private IntPtr _sendHead;

		private IntPtr _recvHead;

		private NetBitBufferStack _recvStack;

		private unsafe NetBitBufferBlock* _sendBlock;

		private unsafe NetConnectionMap* _connectionsMap;

		internal double ReliableSendInterval;

		public readonly double Time => _clock.ElapsedInSeconds;

		public readonly int Group => _group;

		public unsafe readonly int ConnectionCount => _connectionsMap->Count;

		internal unsafe static void Dispose(NetPeerGroup* g, INetPeerGroupCallbacks callbacks)
		{
			if (g != null)
			{
				NetConnectionMap.Dispose(ref g->_connectionsMap, callbacks);
				NetBitBufferBlock.Dispose(ref g->_sendBlock);
				NetBitBufferStack.Dispose(ref g->_recvStack);
			}
		}

		public unsafe static bool TryGetConnectionByIndex(NetPeerGroup* g, int index, out NetConnection* connection)
		{
			return g->_connectionsMap->TryFindByIndex(index, out connection);
		}

		public unsafe static NetConnectionMap.Iterator ConnectionIterator(NetPeerGroup* g)
		{
			return new NetConnectionMap.Iterator(g->_connectionsMap);
		}

		public unsafe static void Connect(NetPeerGroup* g, NetAddress address, byte[] token, byte[] uniqueId = null)
		{
			NetConnection* ptr = AllocateConnection(g, address, token, uniqueId);
			if (ptr == null)
			{
				InternalLogStreams.LogError?.Log("No free connection slots");
				return;
			}
			ChangeConnectionStatus(g, null, ptr, NetConnectionStatus.Connecting);
			SendCommandConnect(g, null, ptr);
		}

		public unsafe static void Connect(NetPeerGroup* g, string ip, ushort port, byte[] token, byte[] uniqueId = null)
		{
			Connect(g, NetAddress.CreateFromIpPort(ip, port), token, uniqueId);
		}

		public unsafe static void Disconnect(NetPeerGroup* g, NetConnection* c, byte[] token)
		{
			if (g != null && (c->Status == NetConnectionStatus.Connected || c->Status == NetConnectionStatus.Connecting))
			{
				SendCommand(g, c, NetCommandDisconnect.Create(NetDisconnectReason.Requested, token));
				DisconnectInternal(g, c, NetDisconnectReason.Requested);
			}
		}

		internal unsafe static void DisconnectInternal(NetPeerGroup* g, NetConnection* c, NetDisconnectReason reason = NetDisconnectReason.ByRemote, byte[] token = null)
		{
			if (g == null || (c->Status != NetConnectionStatus.Connected && c->Status != NetConnectionStatus.Connecting))
			{
				return;
			}
			c->StateDisconnected = default(NetConnection.StateDisconnectedData);
			c->StateDisconnected.Reason = reason;
			if (token != null)
			{
				int num = (c->DisconnectTokenLength = Math.Min(128, token.Length));
				c->DisconnectToken = FusionUnsafe.AllocAndClearArray<byte>(num, 8, "Fusion\\Fusion.Sockets\\NetPeerGroup.cs", 144);
				for (int i = 0; i < num; i++)
				{
					c->DisconnectToken[i] = token[i];
				}
			}
			else
			{
				c->DisconnectToken = null;
				c->DisconnectTokenLength = 0;
			}
			ChangeConnectionStatus(g, null, c, NetConnectionStatus.Disconnected);
		}

		public unsafe static void RequestRejoin(NetPeerGroup* g, NetConnection* c, string sessionId)
		{
			if (g != null && c->Status == NetConnectionStatus.Connected)
			{
				SendCommand(g, c, NetCommandRejoin.Create(sessionId));
			}
		}

		internal unsafe static void RejoinInternal(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, string sessionId)
		{
			if (g != null && c->Status == NetConnectionStatus.Connected)
			{
				cb.OnRejoinRequest(sessionId);
			}
		}

		public unsafe static int Update(NetPeerGroup* g, INetPeerGroupCallbacks cb)
		{
			if (g == null)
			{
				return 0;
			}
			int num = 0;
			num += Receive(g, cb);
			Assert.Check(g->_recvStack.Count == 0, "g->_recvStack.Count == 0");
			UpdateConnections(g, cb);
			IntPtr intPtr;
			do
			{
				intPtr = Volatile.Read(ref g->_recvHead);
			}
			while (Interlocked.CompareExchange(ref g->_recvHead, IntPtr.Zero, intPtr) != intPtr);
			if (intPtr != IntPtr.Zero)
			{
				g->_recvStack.PushFromHead((NetBitBuffer*)(void*)intPtr);
				num += Receive(g, cb);
			}
			return num;
		}

		internal unsafe static void Initialize(short groupIndex, NetPeerGroup* g, NetPeer* p, NetConfig config)
		{
			*g = default(NetPeerGroup);
			g->_config = config;
			g->_peer = p;
			g->_group = groupIndex;
			g->_clock = Timer.StartNew();
			g->_sendBlock = NetBitBufferBlock.Create(config.PacketSize);
			g->_recvStack = NetBitBufferStack.Create(1024);
			g->_connectionsMap = NetConnectionMap.Allocate(g->_config.ConnectionsPerGroup, groupIndex, &g->_config);
			g->ReliableSendInterval = 0.05;
		}

		internal unsafe static IntPtr PopSendHead(NetPeerGroup* g)
		{
			IntPtr intPtr;
			do
			{
				intPtr = Volatile.Read(ref g->_sendHead);
			}
			while (Interlocked.CompareExchange(ref g->_sendHead, IntPtr.Zero, intPtr) != intPtr);
			return intPtr;
		}

		internal unsafe static void PushOnRecvHead(NetPeerGroup* g, NetBitBuffer* b)
		{
			IntPtr intPtr;
			do
			{
				intPtr = Volatile.Read(ref g->_recvHead);
				b->Next = (NetBitBuffer*)(void*)intPtr;
			}
			while (Interlocked.CompareExchange(ref g->_recvHead, (IntPtr)b, intPtr) != intPtr);
		}

		private unsafe static void UpdateConnections(NetPeerGroup* g, INetPeerGroupCallbacks cb)
		{
			int countUsed = g->_connectionsMap->CountUsed;
			NetConnection* connectionsBuffer = g->_connectionsMap->ConnectionsBuffer;
			for (int i = 0; i < countUsed; i++)
			{
				NetConnection* ptr = connectionsBuffer + i;
				if (ptr->MapState != NetConnectionMap.EntryState.Used)
				{
					continue;
				}
				switch (ptr->Status)
				{
				case NetConnectionStatus.Connecting:
					UpdateConnecting(g, cb, ptr);
					break;
				case NetConnectionStatus.Connected:
					UpdateConnected(g, cb, ptr);
					if (ptr->Status == NetConnectionStatus.Connected)
					{
						Connection.Update(cb, ptr->V2);
					}
					break;
				case NetConnectionStatus.Disconnected:
					UpdateDisconnected(g, cb, ptr);
					break;
				case NetConnectionStatus.Shutdown:
					UpdateShutdown(g, cb, ptr);
					break;
				}
			}
		}

		public unsafe static void SendReliable(NetPeerGroup* g, NetConnection* c, ReadOnlySpan<byte> data, bool progress = false)
		{
			using PacketQueue packetQueue = Connection.Queue(c->V2, &c->V2->Streaming, 0u, progress);
			packetQueue.Add(data);
		}

		public unsafe static void SendReliable(NetPeerGroup* g, NetConnection* c, ReadOnlySpan<byte> data1, ReadOnlySpan<byte> data2, bool progress = false)
		{
			using PacketQueue packetQueue = Connection.Queue(c->V2, &c->V2->Streaming, 0u, progress);
			packetQueue.Add(data1);
			packetQueue.Add(data2);
		}

		public unsafe static void ChangeConnectionAddressDuringConnecting(NetPeerGroup* g, NetConnection* c, NetAddress newAddress)
		{
			Assert.Check(c->Status == NetConnectionStatus.Connecting, "c->Status == NetConnectionStatus.Connecting");
			InternalLogStreams.LogTraceNetwork?.Log($"Changing address for connection ({c->LocalId}:{(IntPtr)c}) from {c->Address} to {newAddress} during connecting phase");
			NetAddress address = c->Address;
			g->_connectionsMap->Remap(address, newAddress);
			Assert.Check(c->Address.Equals(newAddress), "c->Address.Equals(newAddress)");
			NetPeer.RemapAddress(g->_peer, address, newAddress);
		}

		private unsafe static void SendCommandConnect(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c)
		{
			Assert.Check(c->Status == NetConnectionStatus.Connecting, "c->Status == NetConnectionStatus.Connecting");
			if (c->StateConnecting.Attempts == g->_config.ConnectAttempts)
			{
				Assert.Check(cb != null, "cb != null");
				NetAddress address = c->Address;
				ChangeConnectionStatus(g, cb, c, NetConnectionStatus.Shutdown);
				cb.OnConnectionFailed(address, NetConnectFailedReason.Timeout);
			}
			else
			{
				cb?.OnConnectionAttempt(c, c->StateConnecting.Attempts, g->_config.ConnectAttempts);
				c->StateConnecting.Attempts++;
				c->StateConnecting.AttemptTimeout = g->_clock.ElapsedInSeconds + g->_config.ConnectInterval;
				SendCommand(g, c, NetCommandConnect.Create(c->LocalId, c->ConnectionToken, c->ConnectionTokenLength, c->UniqueId));
				InternalLogStreams.LogDebug?.Log($"Connection Attempt: {*c} [{c->StateConnecting.Attempts}/{g->_config.ConnectAttempts}]");
			}
		}

		private unsafe static void UpdateConnecting(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c)
		{
			if (c->StateConnecting.AttemptTimeout < g->_clock.ElapsedInSeconds)
			{
				SendCommandConnect(g, cb, c);
			}
		}

		private unsafe static void UpdateConnected(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c)
		{
			if (c->RecvTime + g->_config.ConnectionTimeout < g->_clock.ElapsedInSeconds)
			{
				DisconnectInternal(g, c, NetDisconnectReason.Timeout);
			}
		}

		private unsafe static void UpdateDisconnected(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c)
		{
			if (c->StateDisconnected.SentDisconnectCommand == 0)
			{
				c->StateDisconnected.SentDisconnectCommand = (SendCommand(g, c, NetCommandDisconnect.Create(c->StateDisconnected.Reason, c->DisconnectToken, c->DisconnectTokenLength)) ? 1 : 0);
			}
			if (c->StateDisconnected.CallbackInvoked == 0)
			{
				c->StateDisconnected.CallbackInvoked = 1;
				cb.OnDisconnected(c, c->StateDisconnected.Reason);
			}
			if (c->StateDisconnected.SentDisconnectCommand == 1 && c->StateDisconnected.CallbackInvoked == 1)
			{
				ChangeConnectionStatus(g, cb, c, NetConnectionStatus.Shutdown);
			}
		}

		private unsafe static void UpdateShutdown(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c)
		{
			if (c->StateShutdown.Unmapped == 1)
			{
				if (c->StateShutdown.Timeout < g->_clock.ElapsedInSeconds)
				{
					ReleaseConnection(g, cb, c);
				}
			}
			else if (c->StateShutdown.Unmapped == 0)
			{
				QueueAddressUnmap(g, c);
			}
		}

		private unsafe static void SendUnconnected(NetPeerGroup* g, NetBitBuffer* b)
		{
			IntPtr sendHead;
			do
			{
				sendHead = g->_sendHead;
				b->Next = (NetBitBuffer*)(void*)sendHead;
			}
			while (Interlocked.CompareExchange(ref g->_sendHead, (IntPtr)b, sendHead) != sendHead);
		}

		private unsafe static bool GetConnectionSendBuffer(NetPeerGroup* g, NetConnection* c, out NetBitBuffer* b)
		{
			if (g->_sendBlock->TryAcquire(out b))
			{
				b->Group = g->_group;
				b->Address = c->Address;
				return true;
			}
			return false;
		}

		public unsafe static bool SendUnconnectedData(NetPeerGroup* g, NetAddress address, void* data, int dataLength)
		{
			if (g->_sendBlock->TryAcquire(out var ptr))
			{
				*(sbyte*)ptr->Data = 5;
				ptr->Group = 0;
				ptr->OffsetBits = 8;
				ptr->Address = address;
				ptr->WriteBytesAligned(data, dataLength);
				SendUnconnected(g, ptr);
				return true;
			}
			return false;
		}

		public unsafe static bool GetUnreliableDataBuffer(NetPeerGroup* g, NetConnection* c, out NetBitBuffer* b)
		{
			if (c->Status == NetConnectionStatus.Connected && GetConnectionSendBuffer(g, c, out b))
			{
				*(NetUnreliableHeader*)b->Data = NetUnreliableHeader.Create();
				b->OffsetBits = 8;
				return true;
			}
			b = null;
			return false;
		}

		public unsafe static bool SendUnreliableDataBuffer(NetPeerGroup* g, NetConnection* c, NetBitBuffer* b)
		{
			Assert.Check(b->PacketType == NetPacketType.UnreliableData, "b->PacketType == NetPacketType.UnreliableData");
			if (c->Status != NetConnectionStatus.Connected)
			{
				NetBitBuffer.Release(b);
				return false;
			}
			Send(g, c, b);
			return true;
		}

		public unsafe static bool GetNotifyDataBuffer(NetPeerGroup* g, NetConnection* c, out NetBitBuffer* b)
		{
			if (c->Status == NetConnectionStatus.Connected && c->V2->SendWindowRemaining > 0 && GetConnectionSendBuffer(g, c, out b))
			{
				return true;
			}
			b = null;
			return false;
		}

		private unsafe static int Receive(NetPeerGroup* g, INetPeerGroupCallbacks cb)
		{
			NetBitBuffer* ptr = null;
			int num = 0;
			while (g->_recvStack.TryPop(&ptr))
			{
				InternalLogStreams.LogTraceNetwork?.Log($"Receive: {ptr->LengthBytes}B");
				num++;
				try
				{
					NetConnection* ptr2 = g->_connectionsMap->Find(ptr->Address);
					if (ptr2 == null)
					{
						HandlePacketUnconnected(g, cb, ptr);
					}
					else
					{
						HandlePacket(g, cb, ptr2, ptr);
					}
				}
				finally
				{
					NetBitBuffer.Release(ptr);
				}
			}
			return num;
		}

		private unsafe static void HandlePacketUnconnected(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetBitBuffer* b)
		{
			if (b->PacketType == NetPacketType.Command)
			{
				InternalLogStreams.LogDebug?.Log($"Handle Packet Unconnected from {b->Address}");
				NetCommandHeader* data = (NetCommandHeader*)b->Data;
				if (data->Command != NetCommands.Connect)
				{
					return;
				}
				NetCommandConnect data2 = *(NetCommandConnect*)b->Data;
				byte[] tokenDataAsArray = NetCommandConnect.GetTokenDataAsArray(data2);
				byte[] uniqueIdAsArray = NetCommandConnect.GetUniqueIdAsArray(data2);
				switch (cb.OnConnectionRequest(b->Address, tokenDataAsArray, uniqueIdAsArray))
				{
				case OnConnectionRequestReply.Ok:
				{
					NetConnection* ptr = AllocateConnection(g, b->Address, tokenDataAsArray, uniqueIdAsArray);
					if (ptr != null)
					{
						HandlePacketCommand(g, cb, ptr, b);
					}
					break;
				}
				case OnConnectionRequestReply.Refuse:
					if (!SendCommandUnconnected(g, b->Address, NetCommandRefused.Create(NetConnectFailedReason.ServerRefused)))
					{
						InternalLogStreams.LogDebug?.Error("Sending Refused Connection Failed");
					}
					break;
				case OnConnectionRequestReply.Waiting:
					break;
				}
			}
			else if (b->PacketType == NetPacketType.Unconnected)
			{
				cb.OnUnconnectedData(b);
			}
		}

		public unsafe static double GetConnectionIdleTime(NetPeerGroup* g, NetConnection* c)
		{
			return g->Time - c->RecvTime;
		}

		private unsafe static void HandlePacket(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetBitBuffer* b)
		{
			c->RecvTime = g->_clock.ElapsedInSeconds;
			switch (b->PacketType)
			{
			case NetPacketType.Command:
				if (c->Status == NetConnectionStatus.Connecting || c->Status == NetConnectionStatus.Connected)
				{
					HandlePacketCommand(g, cb, c, b);
				}
				break;
			case NetPacketType.UnreliableData:
				if (c->Status == NetConnectionStatus.Connected)
				{
					HandlePacketUnreliableData(g, cb, c, b);
				}
				break;
			case NetPacketType.V2:
				if (c->Status == NetConnectionStatus.Connected)
				{
					HandleV2Packet(g, cb, c, b);
				}
				break;
			case NetPacketType.Unconnected:
				break;
			default:
				InternalLogStreams.LogError?.Log($"Invalid Packet Type {b->PacketType}");
				break;
			}
		}

		private unsafe static void HandleV2Packet(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetBitBuffer* b)
		{
			if (c->Status == NetConnectionStatus.Connected)
			{
				Assert.Check(b->LengthBytes <= 1136, "{0} <= V2.Config.PACKET_MTU_BYTES", b->LengthBytes);
				Packet* packet = Packet.Pool.Get(&g->_peer->V2->RecvPool);
				packet->State.Size = b->LengthBytes;
				packet->State.Address = b->Address;
				FusionUnsafe.Copy(packet->State.Buffer, b->Data, packet->State.Size);
				if (!Connection.Recv(cb, c->V2, ref packet))
				{
					DisconnectInternal(g, c, NetDisconnectReason.ProtocolError);
				}
			}
		}

		private unsafe static void HandlePacketUnreliableData(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetBitBuffer* b)
		{
			b->OffsetBits = 8;
			cb.OnUnreliableData(c, b);
		}

		private unsafe static void HandlePacketCommand(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetBitBuffer* b)
		{
			NetCommandHeader* data = (NetCommandHeader*)b->Data;
			InternalLogStreams.LogTraceNetwork?.Log($"command {c->Address} <= {data->Command}");
			switch (data->Command)
			{
			case NetCommands.Connect:
				HandleCommandConnect(g, cb, c, *(NetCommandConnect*)b->Data);
				break;
			case NetCommands.Refused:
				HandleCommandRefused(g, cb, c, *(NetCommandRefused*)b->Data);
				break;
			case NetCommands.Accepted:
				HandleCommandAccepted(g, cb, c, *(NetCommandAccepted*)b->Data);
				break;
			case NetCommands.Disconnect:
				HandleCommandDisconnect(g, cb, c, *(NetCommandDisconnect*)b->Data);
				break;
			case NetCommands.Rejoin:
				HandleCommandRejoin(g, cb, c, *(NetCommandRejoin*)b->Data);
				break;
			default:
				InternalLogStreams.LogError?.Log($"Invalid Command Type {data->Command}");
				break;
			}
		}

		private unsafe static void HandleCommandRefused(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetCommandRefused cmd)
		{
			Assert.Check(c->Status == NetConnectionStatus.Connecting, "c->Status == NetConnectionStatus.Connecting");
			try
			{
				cb.OnConnectionFailed(c->Address, cmd.Reason);
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(error);
			}
			ChangeConnectionStatus(g, cb, c, NetConnectionStatus.Shutdown);
		}

		private unsafe static void HandleCommandRejoin(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetCommandRejoin cmd)
		{
			if (c->Status != NetConnectionStatus.Connected)
			{
				InternalLogStreams.LogError?.Log(string.Format("received {0} with connection status {1}", "NetCommandDisconnect", c->Status));
			}
			else
			{
				RejoinInternal(g, cb, c, NetCommandRejoin.GetSessionId(cmd));
			}
		}

		private unsafe static void HandleCommandDisconnect(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetCommandDisconnect cmd)
		{
			if (c->Status != NetConnectionStatus.Connected)
			{
				InternalLogStreams.LogError?.Log(string.Format("received {0} with connection status {1}", "NetCommandDisconnect", c->Status));
			}
			else
			{
				DisconnectInternal(g, c, cmd.Reason);
			}
		}

		private unsafe static void HandleCommandConnect(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetCommandConnect cmd)
		{
			switch (c->Status)
			{
			case NetConnectionStatus.Created:
				c->RemoteId = cmd.ConnectionId;
				c->Counter = ++g->_counter;
				ChangeConnectionStatus(g, cb, c, NetConnectionStatus.Connected);
				SendCommand(g, c, NetCommandAccepted.Create(c->LocalId, c->RemoteId, c->Counter));
				cb.OnConnected(c);
				break;
			case NetConnectionStatus.Connected:
				SendCommand(g, c, NetCommandAccepted.Create(c->LocalId, c->RemoteId, c->Counter));
				break;
			default:
				InternalLogStreams.LogError?.Log(string.Format("received {0} with connection status {1}", "NetCommandConnect", c->Status));
				break;
			}
		}

		private unsafe static void Send(NetPeerGroup* g, NetConnection* c, NetBitBuffer* b)
		{
			Assert.Check(!c->Address.Equals(default(NetAddress)), "c->Address.Equals(default) == false");
			c->SendTime = g->Time;
			IntPtr sendHead;
			do
			{
				sendHead = g->_sendHead;
				b->Next = (NetBitBuffer*)(void*)sendHead;
			}
			while (Interlocked.CompareExchange(ref g->_sendHead, (IntPtr)b, sendHead) != sendHead);
		}

		private unsafe static void HandleCommandAccepted(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetCommandAccepted cmd)
		{
			switch (c->Status)
			{
			case NetConnectionStatus.Connected:
				c->RemoteId = cmd.AcceptedLocalId;
				c->Counter = cmd.Counter;
				break;
			case NetConnectionStatus.Connecting:
				Assert.Check(c->LocalId.Equals(cmd.AcceptedRemoteId), "c->LocalId.Equals(cmd.AcceptedRemoteId)");
				c->RemoteId = cmd.AcceptedLocalId;
				c->Counter = cmd.Counter;
				ChangeConnectionStatus(g, cb, c, NetConnectionStatus.Connected);
				cb.OnConnected(c);
				break;
			default:
				InternalLogStreams.LogTraceNetwork?.Error(string.Format("received {0} with connection status {1}", "NetCommandAccepted", c->Status));
				break;
			}
		}

		private unsafe static bool SendCommand<T>(NetPeerGroup* g, NetConnection* c, T cmd) where T : unmanaged
		{
			if (GetConnectionSendBuffer(g, c, out var b))
			{
				*(T*)b->Data = cmd;
				b->OffsetBits = Maths.SizeOfBits<T>();
				Send(g, c, b);
				InternalLogStreams.LogTraceNetwork?.Log($"command {c->Address} => {((NetCommandHeader*)b->Data)->Command}");
				return true;
			}
			return false;
		}

		private unsafe static bool SendCommandUnconnected<T>(NetPeerGroup* g, NetAddress address, T cmd) where T : unmanaged
		{
			if (g->_sendBlock->TryAcquire(out var ptr))
			{
				*(T*)ptr->Data = cmd;
				ptr->Group = g->_group;
				ptr->Address = address;
				ptr->OffsetBits = Maths.SizeOfBits<T>();
				SendUnconnected(g, ptr);
				InternalLogStreams.LogTraceNetwork?.Log($"command {address} => {((NetCommandHeader*)ptr->Data)->Command}");
				return true;
			}
			return false;
		}

		private unsafe static void QueueAddressUnmap(NetPeerGroup* g, NetConnection* c)
		{
			Assert.Check(c->Status == NetConnectionStatus.Shutdown, "c->Status                 == NetConnectionStatus.Shutdown");
			Assert.Check(c->StateShutdown.Unmapped == 0, "c->StateShutdown.Unmapped == FALSE");
			if (c->StateShutdown.Unmapped == 0 && GetConnectionSendBuffer(g, c, out var b))
			{
				InternalLogStreams.LogTraceNetwork?.Log($"Sending Unmap For: {c->Address}");
				b->Group = -1;
				b->OffsetBits = 0;
				Send(g, c, b);
				c->StateShutdown.Unmapped = 1;
			}
		}

		private unsafe static void ChangeConnectionStatus(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c, NetConnectionStatus status)
		{
			if (c->Status != status)
			{
				InternalLogStreams.LogDebug?.Log($"XX {c->Address} status changed from {c->Status} to {status}");
				c->Status = status;
				if (status == NetConnectionStatus.Shutdown)
				{
					c->StateShutdown.Unmapped = 0;
					c->StateShutdown.Timeout = g->_clock.ElapsedInSeconds + g->_config.ConnectionShutdownTime;
					QueueAddressUnmap(g, c);
				}
			}
		}

		private unsafe static void ReleaseConnection(NetPeerGroup* g, INetPeerGroupCallbacks cb, NetConnection* c)
		{
			Assert.Check(g->_connectionsMap->Find(c->Address) == c, "g->_connectionsMap->Find(c->Address) == c");
			g->_connectionsMap->Remove(c->Address);
		}

		private unsafe static NetConnection* AllocateConnection(NetPeerGroup* g, NetAddress address, byte[] token, byte[] uniqueId)
		{
			Assert.Check(uniqueId != null, "UniqueId is required");
			NetConnection* ptr = g->_connectionsMap->Insert(address, uniqueId);
			if (ptr == null)
			{
				return null;
			}
			Assert.Check(ptr->RecvTime == 0.0, "c->RecvTime == 0");
			Assert.Check(ptr->SendTime == 0.0, "c->SendTime == 0");
			Assert.Check(ptr->Rtt == 0.0, "c->Rtt == 0");
			ptr->RecvTime = g->_clock.ElapsedInSeconds;
			ptr->SendTime = ptr->RecvTime;
			ptr->Rtt = g->_config.ConnectionDefaultRtt;
			ptr->Status = NetConnectionStatus.Created;
			ptr->V2->Socket = g->_peer->V2;
			ptr->V2->Address = address;
			ptr->V2->V1 = ptr;
			ptr->V2->V1Group = g;
			if (token != null)
			{
				ptr->ConnectionTokenLength = NetCommandConnect.ClampTokenLength(token.Length);
				ptr->ConnectionToken = (byte*)FusionUnsafe.AllocAndClear(token.Length, 8, "Fusion\\Fusion.Sockets\\NetPeerGroup.cs", 970);
				fixed (byte* source = token)
				{
					FusionUnsafe.Copy(ptr->ConnectionToken, source, ptr->ConnectionTokenLength);
				}
			}
			return ptr;
		}
	}
}
