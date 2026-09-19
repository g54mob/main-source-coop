#define TRACE
#define DEBUG
using System;
using System.Runtime.CompilerServices;
using Fusion.Sockets.V2;

namespace Fusion.Sockets
{
	public struct NetPeer
	{
		public const int DEFAULT_HEADERS = 144;

		public const int MAX_MTU_BYTES_TOTAL = 1280;

		public const int MAX_MTU_BYTES_PAYLOAD = 1136;

		public const int MAX_MTU_BITS_PAYLOAD = 9088;

		public const int MAX_PACKET_BYTES_PAYLOAD = 44880;

		public const int MAX_PACKET_BYTES_TOTAL = 51200;

		internal const int FRAG_MAX_COUNT = 40;

		internal const byte FRAG_END_BIT = 128;

		private const int STATE_RUNNING = 0;

		private const int STATE_SHUTDOWN = 2;

		private volatile int _state;

		private NetConfig _config;

		private Timer _recvTimer;

		private unsafe byte* _fragmentBuffer;

		internal NetSocket _socket;

		private NetAddress _address;

		private NetBitBufferStack _sendStack;

		private unsafe NetPeerGroup* _groups;

		private unsafe NetPeerGroupMap* _groupsMap;

		private unsafe int* _groupsAssigned;

		private unsafe NetCommandRefused* _refusedCommand;

		private unsafe NetBitBuffer* _recv;

		private unsafe NetBitBufferBlock* _recvBlock;

		internal unsafe Peer* V2;

		private Timer _delayedClock;

		private NetDelayedPacket.List _delayedPackets;

		public readonly NetAddress Address => _address;

		public readonly NetConfig Config => _config;

		public readonly int GroupCount => _config.ConnectionGroups;

		public readonly bool IsShutdown => _state == 2;

		public unsafe static NetConfig* GetConfigPointer(NetPeer* p)
		{
			if (p->_state == 2)
			{
				return null;
			}
			return &p->_config;
		}

		public unsafe static NetPeerGroup* GetGroup(NetPeer* p, int index)
		{
			if (p->_state == 2)
			{
				return null;
			}
			Assert.Check((uint)index < (uint)p->_config.ConnectionGroups, "(uint)index < (uint)p->_config.ConnectionGroups");
			return p->_groups + index;
		}

		public unsafe static void Update(NetPeer* p, INetSocket socket, Random rng)
		{
			bool flag = false;
			Update(p, socket, &flag, rng);
		}

		public unsafe static void Update(NetPeer* p, INetSocket socket, bool* work, Random rng)
		{
			if (p->_state != 2)
			{
				if (p->_state != 0)
				{
					InternalLogStreams.LogError?.Log("Can't call Update on NetPeer which is running or has been running on a thread");
					return;
				}
				RecvInternal(p, socket, work, rng);
				SendInternal(p, socket, work);
			}
		}

		public unsafe static void Recv(NetPeer* p, INetSocket socket, Random rng)
		{
			bool flag = false;
			Recv(p, socket, &flag, rng);
		}

		public unsafe static void Recv(NetPeer* p, INetSocket socket, bool* work, Random rng)
		{
			if (p->_state != 2)
			{
				if (p->_state != 0)
				{
					InternalLogStreams.LogError?.Log("Can't call Update on NetPeer which is running or has been running on a thread");
				}
				else
				{
					RecvInternal(p, socket, work, rng);
				}
			}
		}

		public unsafe static void RemapAddress(NetPeer* p, NetAddress oldAddress, NetAddress newAddress)
		{
			int num = p->_groupsMap->Remove(oldAddress);
			Assert.Check(num >= 0, "removed >= 0");
			p->_groupsMap->Insert(newAddress, 0);
		}

		public unsafe static int Send(NetPeer* p, INetSocket socket)
		{
			if (p->_state == 2)
			{
				return 0;
			}
			bool flag = false;
			return Send(p, socket, &flag);
		}

		public unsafe static int Send(NetPeer* p, INetSocket socket, bool* work)
		{
			if (p->_state == 2)
			{
				return 0;
			}
			if (p->_state != 0)
			{
				InternalLogStreams.LogError?.Log("Can't call Update on NetPeer which is running or has been running on a thread");
				return 0;
			}
			return SendInternal(p, socket, work);
		}

		public unsafe static NetPeer* Initialize(NetConfig config, INetSocket socket)
		{
			NetPeer* ptr = FusionUnsafe.AllocAndClear<NetPeer>(8, "Fusion\\Fusion.Sockets\\NetPeer.cs", 262);
			Initialize(ptr, config, socket);
			return ptr;
		}

		public unsafe static void Initialize(NetPeer* p, NetConfig config, INetSocket socket)
		{
			config.MaxConnections = Maths.Clamp(config.MaxConnections, 1, 2048);
			socket.Initialize(config);
			p->_config = config;
			p->_state = 0;
			p->_recvTimer = default(Timer);
			p->_fragmentBuffer = (byte*)FusionUnsafe.AllocAndClear(1280, 8, "Fusion\\Fusion.Sockets\\NetPeer.cs", 281);
			p->_refusedCommand = FusionUnsafe.AllocAndClear<NetCommandRefused>(8, "Fusion\\Fusion.Sockets\\NetPeer.cs", 284);
			p->_delayedClock = Timer.StartNew();
			p->_delayedPackets = default(NetDelayedPacket.List);
			p->_sendStack = NetBitBufferStack.Create(2048);
			p->_recvBlock = NetBitBufferBlock.Create(config.PacketSize);
			p->_socket = socket.Create(config);
			p->_groupsMap = NetPeerGroupMap.Allocate(config.MaxConnections);
			p->_groups = FusionUnsafe.AllocAndClearArray<NetPeerGroup>(config.ConnectionGroups, 8, "Fusion\\Fusion.Sockets\\NetPeer.cs", 297);
			p->_groupsAssigned = FusionUnsafe.AllocAndClearArray<int>(config.ConnectionGroups, 8, "Fusion\\Fusion.Sockets\\NetPeer.cs", 298);
			for (short num = 0; num < config.ConnectionGroups; num++)
			{
				NetPeerGroup.Initialize(num, p->_groups + num, p, config);
			}
			p->_address = socket.Bind(p->_socket, p->_config);
			p->V2 = Peer.Alloc();
			p->V2->Socket = p->_socket;
			InternalLogStreams.LogTraceNetwork?.Log($"socket bound to {p->_address}");
		}

		public unsafe static void Destroy(NetPeer* p, INetSocket socket, INetPeerGroupCallbacks callbacks)
		{
			if (p->_state == 0)
			{
				p->_state = 2;
				DestroySocket(p, socket, callbacks);
			}
		}

		private unsafe static void DestroySocket(NetPeer* p, INetSocket socket, INetPeerGroupCallbacks callbacks)
		{
			if (p != null && p->V2 != null)
			{
				Peer.Free(p->V2);
				p->V2 = null;
			}
			if (p != null && p->_socket.IsCreated)
			{
				NetBitBufferStack.Dispose(ref p->_sendStack);
				p->_delayedPackets.FreeAll();
				for (int i = 0; i < p->GroupCount; i++)
				{
					NetPeerGroup.Dispose(p->_groups + i, callbacks);
				}
				NetBitBuffer.ReleaseRef(ref p->_recv);
				NetBitBufferBlock.Dispose(ref p->_recvBlock);
				NetPeerGroupMap.Dispose(ref p->_groupsMap);
				FusionUnsafe.Free(ref p->_groupsAssigned);
				FusionUnsafe.Free(ref p->_refusedCommand);
				FusionUnsafe.Free(ref p->_fragmentBuffer);
				FusionUnsafe.Free(ref p->_groups);
				socket.Destroy(p->_socket);
				p->_socket = default(NetSocket);
				FusionUnsafe.Free(ref p);
			}
		}

		private unsafe static short FindGroupWithLeastAssignedAddresses(NetPeer* p)
		{
			short result = -1;
			int num = p->_config.ConnectionsPerGroup;
			for (short num2 = 0; num2 < p->_config.ConnectionGroups; num2++)
			{
				if (p->_groupsAssigned[num2] < num)
				{
					result = num2;
					num = p->_groupsAssigned[num2];
				}
			}
			return result;
		}

		private unsafe static void RecvInternal(NetPeer* p, INetSocket socket, bool* work, Random rng)
		{
			p->_recvTimer.Restart();
			RecvDelayed(p, socket, work, rng);
			if (RecvExpired(p))
			{
				return;
			}
			int num;
			while (RecvBufferAvailable(p) && (num = socket.Receive(p->_socket, &p->_recv->Address, (byte*)p->_recv->Data, p->_config.PacketSize)) > 0)
			{
				InternalLogStreams.LogTraceNetwork?.Log($"recv {p->_recv->Address} <= {num} bytes");
				*work = true;
				p->_recv->LengthBytes = num;
				RecvBufferPushToGroup(p, socket, rng);
				if (RecvExpired(p))
				{
					break;
				}
			}
		}

		private unsafe static void RecvBufferPushToGroup(NetPeer* p, INetSocket socket, Random rng)
		{
			Assert.Check(p->_recv != null, "p->_recv                          != null");
			Assert.Check(!p->_recv->Address.Equals(default(NetAddress)), "p->_recv->Address.Equals(default) == false");
			short num = p->_groupsMap->Find(p->_recv->Address);
			if (num == -1)
			{
				NetCommandHeader data = *(NetCommandHeader*)p->_recv->Data;
				if (data.PacketType != NetPacketType.Command || data.Command != NetCommands.Connect)
				{
					return;
				}
				num = FindGroupWithLeastAssignedAddresses(p);
				if (num == -1)
				{
					*p->_refusedCommand = NetCommandRefused.Create(NetConnectFailedReason.ServerFull);
					socket.Send(p->_socket, &p->_recv->Address, (byte*)p->_refusedCommand, 3);
					return;
				}
				Assert.Check(p->_groupsAssigned[num] >= 0 && p->_groupsAssigned[num] < p->_config.ConnectionsPerGroup, "p->_groupsAssigned[group] >= 0 && p->_groupsAssigned[group] < p->_config.ConnectionsPerGroup");
				if (!p->_groupsMap->Insert(p->_recv->Address, num))
				{
					return;
				}
				p->_groupsAssigned[num]++;
			}
			Assert.Check(num >= 0 && num <= p->_config.ConnectionGroups, "group >= 0 && group <= p->_config.ConnectionGroups");
			NetPeerGroup.PushOnRecvHead(p->_groups + num, p->_recv);
			p->_recv = null;
		}

		private unsafe static void RecvDelayed(NetPeer* p, INetSocket socket, bool* work, Random rng)
		{
			while (!p->_delayedPackets.IsEmpty && p->_delayedPackets.Head->DeliveryTime < p->_delayedClock.ElapsedInSeconds && RecvBufferAvailable(p) && !RecvExpired(p))
			{
				*work = true;
				NetDelayedPacket* ptr = p->_delayedPackets.RemoveFirst();
				FusionUnsafe.Copy(p->_recv->Data, ptr->Data, ptr->DataLength);
				p->_recv->Address = ptr->Address;
				p->_recv->LengthBytes = ptr->DataLength;
				RecvBufferPushToGroup(p, socket, rng);
				FusionUnsafe.Free(ref ptr);
			}
		}

		private unsafe static int SendInternal(NetPeer* p, INetSocket socket, bool* work)
		{
			int num = SendFromStack(p, socket, work);
			Assert.Check(p->_sendStack.Count == 0, "p->_sendStack.Count == 0");
			for (int i = 0; i < p->_config.ConnectionGroups; i++)
			{
				IntPtr intPtr = NetPeerGroup.PopSendHead(p->_groups + i);
				if (!(intPtr == IntPtr.Zero))
				{
					*work = true;
					p->_sendStack.PushFromHead((NetBitBuffer*)(void*)intPtr);
				}
			}
			return num + SendFromStack(p, socket, work);
		}

		private unsafe static int SendFromStack(NetPeer* p, INetSocket socket, bool* work)
		{
			NetBitBuffer* ptr = null;
			int num = 0;
			while (p->_sendStack.TryPop(&ptr))
			{
				*work = true;
				Assert.Check(!ptr->Address.Equals(default(NetAddress)), "b->Address.Equals(default) == false");
				if (ptr->PacketType == NetPacketType.Command)
				{
					NetCommandHeader* data = (NetCommandHeader*)ptr->Data;
					if (data->Command == NetCommands.Connect)
					{
						short num2 = p->_groupsMap->Find(ptr->Address);
						if (num2 == -1)
						{
							if (!p->_groupsMap->Insert(ptr->Address, ptr->Group))
							{
								NetBitBuffer.Release(ptr);
								continue;
							}
							p->_groupsAssigned[ptr->Group]++;
						}
					}
				}
				if (ptr->PacketType != NetPacketType.Unconnected)
				{
					Assert.Check((uint)p->_groupsMap->Find(ptr->Address) < (uint)p->_config.ConnectionGroups, "(uint)p->_groupsMap->Find(b->Address) < (uint)p->_config.ConnectionGroups");
				}
				if (ptr->Group == -1)
				{
					Assert.Check(ptr->OffsetBits == 0, "b->OffsetBits == 0");
					int num3 = p->_groupsMap->Remove(ptr->Address);
					socket.DeleteEncryptionKey(ptr->Address);
					InternalLogStreams.LogTraceNetwork?.Log($"{ptr->Address} unmapped from {num3}");
					Assert.Check((uint)num3 < (uint)p->_config.ConnectionGroups, "(uint)group < (uint)p->_config.ConnectionGroups");
					p->_groupsAssigned[num3]--;
					Assert.Check(p->_groupsAssigned[num3] >= 0, "p->_groupsAssigned[group] >= 0");
					NetBitBuffer.Release(ptr);
					continue;
				}
				int bufferLength = Maths.BytesRequiredForBits(ptr->OffsetBits);
				socket.Send(p->_socket, &ptr->Address, (byte*)ptr->Data, bufferLength);
				num++;
				if (ptr->PacketType == NetPacketType.Command)
				{
					NetCommandHeader* data2 = (NetCommandHeader*)ptr->Data;
					if (data2->Command == NetCommands.Refused && p->_groupsMap->Find(ptr->Address) != -1)
					{
						int num4 = p->_groupsMap->Remove(ptr->Address);
						InternalLogStreams.LogTraceNetwork?.Log($"{ptr->Address} unmapped from {num4} because it was refused.");
						Assert.Check((uint)num4 < (uint)p->_config.ConnectionGroups, "(uint)group < (uint)p->_config.ConnectionGroups");
						p->_groupsAssigned[num4]--;
						Assert.Check(p->_groupsAssigned[num4] >= 0, "p->_groupsAssigned[group] >= 0");
					}
				}
				NetBitBuffer.Release(ptr);
			}
			Assert.Check(p->_sendStack.Count == 0, "p->_sendStack.Count == 0");
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool RecvBufferAvailable(NetPeer* p)
		{
			if (p->_recv == null)
			{
				p->_recv = p->_recvBlock->TryAcquire();
			}
			if (p->_recv != null)
			{
				p->_recv->Address = default(NetAddress);
			}
			return p->_recv != null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static bool RecvExpired(NetPeer* p)
		{
			return p->_recvTimer.ElapsedInMilliseconds > p->_config.OperationExpireTime;
		}
	}
}
