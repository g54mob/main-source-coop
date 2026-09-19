#define DEBUG
#define TRACE
using System;
using JetBrains.Annotations;

namespace Fusion.Sockets.V2
{
	internal struct Connection
	{
		public unsafe Peer* Socket;

		public NetAddress Address;

		public Channel Game;

		public Channel GameReliable;

		public Channel Streaming;

		public ushort RecvSequence;

		public ulong RecvMask;

		public double RecvTime;

		public ushort SendSequence;

		public double SendTime;

		public Packet.List SendWindow;

		public double NotifyRecvTime;

		public double NotifySendTime;

		public unsafe NetConnection* V1;

		public unsafe NetPeerGroup* V1Group;

		public double Rtt;

		public double RttSmooth;

		public readonly int SendWindowRemaining => Math.Max(0, 1024 - SendWindow.Count);

		public unsafe static void Free(ref Connection* connection)
		{
			if (connection != null)
			{
				connection->SendWindow.FreeAll();
				Free(ref connection->Game);
				Free(ref connection->GameReliable);
				Free(ref connection->Streaming);
				*connection = default(Connection);
				FusionUnsafe.Free(ref connection);
			}
		}

		public unsafe static Connection* Alloc()
		{
			Connection* ptr = FusionUnsafe.AllocAndClear<Connection>(8, "Fusion\\Fusion.Sockets\\_V2\\Connection.cs", 50);
			ptr->Rtt = 0.10000000149011612;
			ptr->RttSmooth = 0.10000000149011612;
			ptr->Game.Id = 1;
			ptr->Game.Flags = (ChannelFlags)0;
			ptr->GameReliable.Id = 3;
			ptr->GameReliable.Flags = ChannelFlags.Reliable | ChannelFlags.NotifyDelivered;
			ptr->Streaming.Id = 2;
			ptr->Streaming.Flags = ChannelFlags.Reliable;
			return ptr;
		}

		private static void Free(ref Channel channel)
		{
			channel.NotifyQueue.FreeAll();
			channel.SendQueue.FreeAll();
			channel.ResendQueue.FreeAll();
			channel.RecvQueue.FreeAll();
		}

		public unsafe static bool Recv(INetPeerGroupCallbacks callbacks, Connection* connection, ref Packet* packet)
		{
			connection->RecvTime = connection->Socket->Clock;
			InternalLogStreams.LogTraceNetwork?.Log($"recv: {packet->State.Size} bytes from {connection->Address} type: {*packet->Header}");
			Header* header = packet->Header;
			if (header->Is(PacketTypeV2.NotifyData))
			{
				return RecvNotifyData(callbacks, connection, ref packet);
			}
			if (header->Is(PacketTypeV2.NotifyAcks))
			{
				RecvNotifyAcks(connection, ref packet);
			}
			return true;
		}

		private unsafe static void RecvNotifyAcks(Connection* connection, ref Packet* packet)
		{
			if (packet->State.Size == 32)
			{
				int num = SequenceDistance(packet->Header->Sequence, connection->RecvSequence);
				if (Math.Abs(num) >= 1024)
				{
					InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyAcks: ignoring, seqDistance too high: {num} ({*packet})");
					Packet.Release(ref packet, "RecvNotifyAcks", 33);
					return;
				}
				if (num <= 0)
				{
					InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyAcks: ignoring, out of send window: {num} ({*packet})");
					Packet.Release(ref packet, "RecvNotifyAcks", 40);
					return;
				}
				InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyAcks: accepting ack ({*packet})");
				connection->NotifyRecvTime = connection->V1Group->_clock.ElapsedInSeconds;
				connection->RecvMask = Maths.SafeShiftLeft(connection->RecvMask, num);
				connection->RecvMask |= 1uL;
				connection->RecvSequence = packet->Header->Sequence;
				connection->RecvTime = connection->Socket->Clock;
				Ack(connection, *packet->Header);
			}
			else
			{
				InternalLogStreams.LogTraceNetwork?.Warn($"RecvNotifyData: not processing Acks because packet size does not match: {packet->State.Size}");
			}
			Packet.Release(ref packet, "RecvNotifyAcks", 62);
		}

		private unsafe static bool RecvNotifyData(INetPeerGroupCallbacks callbacks, Connection* connection, ref Packet* packet)
		{
			if (packet->State.Size < 32)
			{
				InternalLogStreams.LogTraceNetwork?.Warn($"RecvNotifyData: Invalid state size: {packet->State.Size}, ignoring");
				Packet.Release(ref packet, "RecvNotifyData", 68);
				return true;
			}
			if (packet->State.Size == 32)
			{
				RecvNotifyAcks(connection, ref packet);
				return true;
			}
			if (packet->Header->Channel == connection->Game.Id)
			{
				return RecvNotifyData(callbacks, connection, &connection->Game, ref packet);
			}
			if (packet->Header->Channel == connection->GameReliable.Id)
			{
				return RecvNotifyData(callbacks, connection, &connection->GameReliable, ref packet);
			}
			if (packet->Header->Channel == connection->Streaming.Id)
			{
				return RecvNotifyData(callbacks, connection, &connection->Streaming, ref packet);
			}
			Packet.Release(ref packet, "RecvNotifyData", 89);
			return true;
		}

		private unsafe static bool RecvNotifyData(INetPeerGroupCallbacks callbacks, Connection* connection, Channel* channel, ref Packet* packet)
		{
			int num = SequenceDistance(packet->Header->Sequence, connection->RecvSequence);
			if (Math.Abs(num) >= 1024)
			{
				InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyData [Channel:{channel->Id}]: ignoring packet, out of send window ({num}) ({*packet})");
				Packet.Release(ref packet, "RecvNotifyData", 99);
				return false;
			}
			if (num <= 0)
			{
				InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyData [Channel:{channel->Id}]: ignoring packet, out of send window but not massively ({num}) ({*packet})");
				Packet.Release(ref packet, "RecvNotifyData", 106);
				return true;
			}
			if (packet->Header->FragGroup + 1024 <= channel->RecvGroup)
			{
				InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyData [Channel:{channel->Id}]: ignoring packet, frag group is too far ahead, disconnect RecvGroup:{channel->RecvGroup} ({*packet})");
				Packet.Release(ref packet, "RecvNotifyData", 113);
				return false;
			}
			if (packet->Header->FragGroup <= channel->RecvGroup)
			{
				InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyData [Channel:{channel->Id}]: ignoring packet, old frag group, RecvGroup:{channel->RecvGroup} ({*packet})");
				Packet.Release(ref packet, "RecvNotifyData", 120);
				return true;
			}
			connection->NotifyRecvTime = connection->V1Group->_clock.ElapsedInSeconds;
			Header header = *packet->Header;
			connection->RecvMask = Maths.SafeShiftLeft(connection->RecvMask, num);
			connection->RecvMask |= 1uL;
			connection->RecvSequence = packet->Header->Sequence;
			connection->RecvTime = connection->Socket->Clock;
			Packet.List recvQueue = default(Packet.List);
			Packet* result;
			while (channel->RecvQueue.TryRemoveFirst(out result))
			{
				if (packet != null)
				{
					if (result->Header->FragGroup == packet->Header->FragGroup)
					{
						if (result->Header->FragIndex == packet->Header->FragIndex)
						{
							InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyData [Channel:{channel->Id}]: ignoring packet, is a duplicate duplicate ({*packet})");
							Packet.Release(ref packet, "RecvNotifyData", 144);
							packet = null;
						}
						else if (result->Header->FragIndex > packet->Header->FragIndex)
						{
							InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyData [Channel:{channel->Id}]: accepting packet, enqueued in an existing FragGroup ({*packet})");
							recvQueue.AddLast(packet);
							packet = null;
						}
					}
					else if (result->Header->FragGroup > packet->Header->FragGroup)
					{
						InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyData [Channel:{channel->Id}]: accepting packet, enqueued before existing FragGroup: ({*packet})");
						recvQueue.AddLast(packet);
						packet = null;
					}
				}
				recvQueue.AddLast(result);
			}
			if (packet != null)
			{
				InternalLogStreams.LogTraceNetwork?.Log($"RecvNotifyData [Channel:{channel->Id}]: accepting packet, enqueued as the last one ({*packet})");
				recvQueue.AddLast(packet);
				packet = null;
			}
			channel->RecvQueue = recvQueue;
			Ack(connection, header);
			if (!channel->Reliable)
			{
				while (DeliverNotify(callbacks, connection, channel))
				{
				}
			}
			return true;
		}

		private unsafe static Channel* GetChannel(Connection* connection, Packet* packet)
		{
			return GetChannel(connection, packet->Header->Channel);
		}

		private unsafe static Channel* GetChannel(Connection* connection, byte id)
		{
			if (id == connection->Game.Id)
			{
				return &connection->Game;
			}
			if (id == connection->Streaming.Id)
			{
				return &connection->Streaming;
			}
			if (id == connection->GameReliable.Id)
			{
				return &connection->GameReliable;
			}
			throw new InvalidOperationException(id.ToString());
		}

		private unsafe static void Ack(Connection* connection, Header header)
		{
			Packet* result;
			while (connection->SendWindow.TryPeekFirst(out result))
			{
				int num = SequenceDistance(result->Header->Sequence, header.AckSequence);
				if (num > 0)
				{
					break;
				}
				if (num == 0)
				{
					connection->Rtt = Math.Max(0.001, connection->Socket->Clock - result->State.SendTime);
					connection->V1->Rtt = connection->Rtt;
					if (connection->RttSmooth == 0.0)
					{
						connection->RttSmooth = connection->Rtt;
					}
					else
					{
						connection->RttSmooth = connection->RttSmooth * 0.9 + connection->Rtt * 0.1;
					}
				}
				num = -num;
				Packet* ptr = connection->SendWindow.RemoveFirst();
				Assert.Check(result == ptr, "packet == removed");
				Channel* channel = GetChannel(connection, result);
				if ((header.AckMask & Maths.SafeShiftLeft(1uL, num)) == 0)
				{
					HostProfiler.Counters.FragmentsLost?.Add(1);
					if (channel->Reliable)
					{
						ReliableLost(connection, result);
					}
					else
					{
						UnreliableLost(connection, ref result);
					}
				}
				else
				{
					HostProfiler.Counters.FragmentsDelivered?.Add(1);
					if (channel->Reliable)
					{
						ReliableDelivered(connection, result, channel->CheckFlag(ChannelFlags.NotifyDelivered));
					}
					else
					{
						UnreliableDelivered(connection, ref result);
					}
				}
			}
		}

		private unsafe static void ReliableLost(Connection* connection, Packet* packet)
		{
			GetChannel(connection, packet)->ResendQueue.AddLast(packet);
		}

		private unsafe static void ReliableDelivered(Connection* connection, Packet* packet, bool notifyDelivered)
		{
			if (notifyDelivered)
			{
				Assert.Check(!packet->State.Group->IsDelivered(packet->Header->FragIndex), "packet->State.Group->IsDelivered(packet->Header->FragIndex) == false");
				InternalLogStreams.LogTraceNetwork?.Log($"ReliableDelivered with notifyDelivered: {*packet}");
				if (packet->State.Group->SetDelivered(packet->Header->FragIndex))
				{
					QueueResult(GetChannel(connection, packet), packet->State.Group, delivered: true, lost: false);
				}
			}
			else
			{
				InternalLogStreams.LogTraceNetwork?.Log($"ReliableDelivered without notifyDelivered: {*packet}");
				if (packet->State.Group->SetDelivered(packet->Header->FragIndex))
				{
					PacketGroup.Free(ref packet->State.Group);
				}
			}
			Packet.Release(ref packet, "ReliableDelivered", 274);
		}

		public unsafe static void Update(INetPeerGroupCallbacks callbacks, Connection* connection)
		{
			UpdateNotify(callbacks, connection, &connection->Game);
			UpdateNotify(callbacks, connection, &connection->GameReliable);
			UpdateNotify(callbacks, connection, &connection->Streaming);
			while (DeliverNotify(callbacks, connection, &connection->Game))
			{
			}
			while (DeliverNotify(callbacks, connection, &connection->GameReliable))
			{
			}
			while (DeliverNotify(callbacks, connection, &connection->Streaming))
			{
			}
		}

		private unsafe static bool DeliverNotify(INetPeerGroupCallbacks callbacks, Connection* connection, Channel* channel)
		{
			if (channel->RecvQueue.IsEmpty)
			{
				return false;
			}
			int num = 0;
			uint num2 = (channel->Reliable ? (channel->RecvGroup + 1) : channel->RecvQueue.Head->Header->FragGroup);
			Packet* ptr = channel->RecvQueue.Head;
			callbacks.DeliverList.Clear();
			while (ptr != null)
			{
				Packet* ptr2 = ptr;
				ptr = channel->RecvQueue.Next(ptr);
				HostProfiler.Counters.BytesIn?.Add(ptr2->State.Size);
				HostProfiler.Counters.FragmentsIn?.Add(1);
				if (ptr2->Header->FragGroup == num2 && ptr2->Header->FragIndex == num)
				{
					callbacks.DeliverList.Add((IntPtr)ptr2);
					if (ptr2->Header->IsLastFrag)
					{
						channel->RecvGroup = ptr2->Header->FragGroup;
						Assert.Always(callbacks.ReadBuffer.IsEmpty, "callbacks.ReadBuffer.IsEmpty");
						using (callbacks.ReadBuffer)
						{
							Assert.Always(callbacks.ReadBuffer.Offset == 0, "callbacks.ReadBuffer.Offset == 0");
							Assert.Always(callbacks.ReadBuffer.Length == 0, "callbacks.ReadBuffer.Length == 0");
							for (int i = 0; i < callbacks.DeliverList.Count; i++)
							{
								Packet* packet = (Packet*)(void*)callbacks.DeliverList[i];
								Assert.Check(packet->State.Size <= 1136, "p->State.Size <= Config.PACKET_MTU_BYTES");
								callbacks.ReadBuffer.Fill(new Span<byte>(packet->State.Buffer + 32, packet->State.Size - 32));
								channel->RecvQueue.Remove(packet);
								Packet.Release(ref packet, "DeliverNotify", 334);
							}
							callbacks.DeliverList.Clear();
							try
							{
								if (channel->Id == connection->Game.Id)
								{
									callbacks.OnNotifyData(connection->V1, callbacks.ReadBuffer);
								}
								else if (channel->Id == connection->GameReliable.Id)
								{
									callbacks.OnNotifyData(connection->V1, callbacks.ReadBuffer);
								}
								else
								{
									callbacks.OnReliableData(connection->V1, callbacks.ReadBuffer);
								}
							}
							catch (Exception error)
							{
								InternalLogStreams.LogException?.Log(error);
							}
						}
					}
					num++;
					continue;
				}
				if (channel->Reliable)
				{
					if (callbacks.DeliverList.Count > 0)
					{
						Packet* ptr3 = (Packet*)(void*)callbacks.DeliverList[0];
						if (ptr3->Header->FragIndex == 0 && ptr3->Header->PacketTypeV2.Has(PacketTypeV2.Progress_Bit) && ptr3->Header->FragCount != 0)
						{
							Span<byte> span = new Span<byte>(ptr3->State.Buffer + 32, ptr3->State.Size - 32);
							try
							{
								callbacks.OnReliableDataProgress(connection->V1, span, callbacks.DeliverList.Count, (int)ptr3->Header->FragCount);
							}
							catch (Exception error2)
							{
								InternalLogStreams.LogError?.Log(error2);
							}
						}
					}
					return false;
				}
				InternalLogStreams.LogTraceNetwork?.Log($"Dropping packets due to index/group mismatch: {ptr2->Header->FragGroup} != {num2} || {ptr2->Header->FragIndex} != {num}");
				if (ptr2->Header->FragGroup <= num2)
				{
					callbacks.DeliverList.Add((IntPtr)ptr2);
				}
				for (int j = 0; j < callbacks.DeliverList.Count; j++)
				{
					Packet* packet2 = (Packet*)(void*)callbacks.DeliverList[j];
					channel->RecvQueue.Remove(packet2);
					Packet.Release(ref packet2, "DeliverNotify", 387);
				}
				callbacks.DeliverList.Clear();
				return true;
			}
			return false;
		}

		private unsafe static void UpdateNotify(INetPeerGroupCallbacks callbacks, Connection* connection, Channel* channel)
		{
			while (!channel->NotifyQueue.IsEmpty && channel->NotifyGroup + 1 == channel->NotifyQueue.Head->State.FragmentGroup)
			{
				PacketGroup* group = channel->NotifyQueue.RemoveFirst();
				channel->NotifyGroup = group->State.FragmentGroup;
				try
				{
					NetNotifyDataInfo info;
					if (group->State.WasDelivered)
					{
						NetConnection* v = connection->V1;
						info = new NetNotifyDataInfo(&group->State);
						callbacks.OnNotifyDataDelivered(v, in info);
					}
					else
					{
						Assert.Check(group->State.WasLost, "head->State.WasLost");
						NetConnection* v2 = connection->V1;
						info = new NetNotifyDataInfo(&group->State);
						callbacks.OnNotifyDataLost(v2, in info);
					}
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
				}
				PacketGroup.Free(ref group);
			}
		}

		private unsafe static void UnreliableLost(Connection* connection, ref Packet* packet)
		{
			InternalLogStreams.LogTraceNetwork?.Log($"XX Unreliable Lost {connection->Address} {*packet}");
			Channel* channel = GetChannel(connection, packet);
			if (packet->State.Group->State.FragmentCount > 1)
			{
				int num = 0;
				while (!channel->SendQueue.IsEmpty && channel->SendQueue.Head->State.Group == packet->State.Group)
				{
					Packet* packet2 = channel->SendQueue.RemoveFirst();
					Packet.Release(ref packet2, "UnreliableLost", 431);
					num++;
				}
				Packet* ptr = connection->SendWindow.Head;
				while (ptr != null)
				{
					Packet* packet3 = ptr;
					ptr = connection->SendWindow.Next(ptr);
					if (packet->State.Group == packet3->State.Group)
					{
						bool isLastFrag = packet3->Header->IsLastFrag;
						connection->SendWindow.Remove(packet3);
						Packet.Release(ref packet3, "UnreliableLost", 448);
						num++;
						if (isLastFrag)
						{
							break;
						}
					}
				}
				HostProfiler.Counters.FragmentsLost?.Add(num);
			}
			QueueResult(channel, packet->State.Group, delivered: false, lost: true);
			Packet.Release(ref packet, "UnreliableLost", 465);
		}

		private unsafe static void UnreliableDelivered(Connection* connection, ref Packet* packet)
		{
			Assert.Check(!packet->State.Group->IsDelivered(packet->Header->FragIndex), "packet->State.Group->IsDelivered(packet->Header->FragIndex) == false");
			if (packet->State.Group->SetDelivered(packet->Header->FragIndex))
			{
				QueueResult(GetChannel(connection, packet), packet->State.Group, delivered: true, lost: false);
			}
			Packet.Release(ref packet, "UnreliableDelivered", 476);
		}

		private static int SequenceDistance(ushort from, ushort to)
		{
			return (short)(from - to);
		}

		private unsafe static void QueueResult(Channel* channel, PacketGroup* group, bool delivered, bool lost)
		{
			InternalLogStreams.LogTraceNetwork?.Log($"Queue result: {*channel} {group->State} Delivered:{delivered} Lost:{lost}");
			group->State.WasLost = lost;
			group->State.WasDelivered = delivered;
			PacketGroup.List notifyQueue = default(PacketGroup.List);
			PacketGroup* result;
			while (channel->NotifyQueue.TryRemoveFirst(out result))
			{
				if (group != null && result->State.FragmentGroup > group->State.FragmentGroup)
				{
					notifyQueue.AddLast(group);
					group = null;
				}
				notifyQueue.AddLast(result);
			}
			if (group != null)
			{
				notifyQueue.AddLast(group);
				group = null;
			}
			channel->NotifyQueue = notifyQueue;
		}

		public unsafe static bool CanQueue(Connection* connection, Channel* channel)
		{
			return channel->SendQueue.IsEmpty && channel->ResendQueue.IsEmpty && connection->SendWindowRemaining > 0;
		}

		public unsafe static int Send(INetSocket socket, Connection* connection)
		{
			if (connection->V1->Status != NetConnectionStatus.Connected)
			{
				return 0;
			}
			int num = 0;
			num += Send(socket, connection, &connection->Game);
			num += Send(socket, connection, &connection->Game);
			num += Send(socket, connection, &connection->GameReliable);
			num += Send(socket, connection, &connection->Streaming);
			if (num == 0)
			{
				Packet* packet = Packet.Pool.Get(&connection->Socket->SendPool);
				packet->State.Size = 32;
				packet->Header->PacketTypeV2 = PacketTypeV2.NotifyAcks;
				packet->Header->AckSequence = connection->RecvSequence;
				packet->Header->AckMask = connection->RecvMask;
				SendPacket(socket, connection, packet);
				Packet.Release(ref packet, "Send", 36);
				return 1;
			}
			return num;
		}

		public unsafe static int Send(INetSocket socket, Connection* connection, Channel* channel)
		{
			if (connection->SendWindowRemaining == 0 || (channel->SendQueue.IsEmpty && channel->ResendQueue.IsEmpty))
			{
				return 0;
			}
			if (!channel->ResendQueue.IsEmpty)
			{
				SendPacket(socket, connection, channel->ResendQueue.RemoveFirst());
				return 1;
			}
			SendPacket(socket, connection, channel->SendQueue.RemoveFirst());
			return 1;
		}

		private unsafe static void SendPacket(INetSocket socket, Connection* connection, Packet* packet)
		{
			HostProfiler.Counters.FragmentsOut?.Add(1);
			HostProfiler.Counters.BytesOut?.Add(packet->State.Size);
			if (packet->Header->Is(PacketTypeV2.NotifyData) || packet->Header->Is(PacketTypeV2.NotifyAcks))
			{
				packet->Header->Sequence = ++connection->SendSequence;
				packet->Header->AckSequence = connection->RecvSequence;
				packet->Header->AckMask = connection->RecvMask;
				connection->NotifySendTime = connection->V1Group->_clock.ElapsedInSeconds;
			}
			packet->State.Address = connection->Address;
			packet->State.SendTime = (connection->SendTime = connection->Socket->Clock);
			if (packet->Header->Is(PacketTypeV2.NotifyData))
			{
				connection->SendWindow.AddLast(packet);
			}
			connection->V1->SendTime = connection->V1Group->_clock.ElapsedInSeconds;
			packet->Header->PacketType = NetPacketType.V2;
			InternalLogStreams.LogTraceNetwork?.Log($"send: {packet->State.Size} bytes to {connection->Address} type: {*packet->Header}");
			Peer.Send(socket, connection->Socket, packet);
		}

		[MustUseReturnValue]
		public unsafe static PacketQueue Queue(Connection* connection, Channel* channel, nuint userToken, bool progress)
		{
			return new PacketQueue(connection, channel, userToken, progress);
		}
	}
}
