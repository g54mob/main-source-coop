#define TRACE
#define DEBUG
using System;

namespace Fusion.Sockets.V2
{
	internal ref struct PacketQueue : IDisposable
	{
		private unsafe readonly Connection* connection;

		private unsafe readonly Channel* channel;

		private unsafe readonly PacketGroup* group;

		private unsafe Packet* first;

		private unsafe Packet* current;

		private readonly bool progress;

		public unsafe PacketQueue(Connection* connection, Channel* channel, nuint userToken, bool progress)
		{
			first = default(Packet*);
			current = default(Packet*);
			Assert.Check(channel->Reliable || (channel->SendQueue.IsEmpty && channel->ResendQueue.IsEmpty), "channel->Reliable || channel->SendQueue.IsEmpty && channel->ResendQueue.IsEmpty");
			group = PacketGroup.Alloc();
			group->State.UserToken = userToken;
			group->State.FragmentGroup = ++channel->SendGroup;
			this.channel = channel;
			this.connection = connection;
			this.progress = progress;
		}

		private unsafe Packet* NextPacket()
		{
			HostProfiler.Counters.FragmentsQueued?.Add(1);
			HostProfiler.Counters.BytesQueued?.Add(32);
			Packet* ptr = Packet.Pool.Get(&connection->Socket->SendPool);
			ptr->State.Group = group;
			ptr->State.Size = 32;
			Header* header = ptr->Header;
			header->Channel = channel->Id;
			header->FragGroup = group->State.FragmentGroup;
			header->FragIndex = (uint)group->State.FragmentCount;
			header->PacketTypeV2 = PacketTypeV2.NotifyData;
			group->State.FragmentCount++;
			InternalLogStreams.LogTraceNetwork?.Log($"created: {*header}");
			channel->SendQueue.AddLast(ptr);
			return ptr;
		}

		public unsafe void Add(ReadOnlySpan<byte> data)
		{
			if (data.IsEmpty)
			{
				return;
			}
			int mtuData = channel->MtuData;
			if (current == null)
			{
				current = (first = NextPacket());
			}
			while (!data.IsEmpty)
			{
				int num = current->State.Size - 32;
				Assert.Check(num >= 0 && num <= mtuData, "bytesUsed >= 0 && bytesUsed <= packetCapacity");
				if (num >= mtuData)
				{
					current = NextPacket();
					num = 0;
				}
				int num2 = Math.Min(data.Length, mtuData - num);
				ReadOnlySpan<byte> source = data.Slice(0, num2);
				Span<byte> destination = new Span<byte>(current->State.Buffer + 32 + num, num2);
				FusionUnsafe.Copy(destination, source);
				HostProfiler.Counters.BytesQueued?.Add(num2);
				data = data.Slice(num2);
				current->State.Size += num2;
			}
		}

		public unsafe void Dispose()
		{
			if (current != null)
			{
				PacketTypeV2* packetTypeV = &current->Header->PacketTypeV2;
				*packetTypeV |= PacketTypeV2.LastFrag_Bit;
				InternalLogStreams.LogTraceNetwork?.Log($"created-done: {*current->Header}");
			}
			if (group->State.FragmentCount > 1)
			{
				if (progress)
				{
					Assert.Always(first != null, "first != null");
					first->Header->FragCount = (uint)group->State.FragmentCount;
					first->Header->PacketTypeV2 |= PacketTypeV2.Progress_Bit;
				}
				group->State.FragmentsDelivered = FusionUnsafe.AllocAndClearArray<ulong>((group->State.FragmentCount + 63) / 64, 8, "Fusion\\Fusion.Sockets\\_V2\\PacketQueue.cs", 97);
			}
		}
	}
}
