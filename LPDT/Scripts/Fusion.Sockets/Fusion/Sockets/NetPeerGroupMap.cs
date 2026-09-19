#define TRACE
#define DEBUG
namespace Fusion.Sockets
{
	internal struct NetPeerGroupMap
	{
		public enum EntryState
		{
			None = 0,
			Free = 1,
			Used = 2
		}

		public struct Entry
		{
			public unsafe Entry* Next;

			public ulong Hash;

			public EntryState State;

			public NetAddress Address;

			public short Group;
		}

		public unsafe Entry** Buckets;

		public unsafe Entry* Entries;

		public unsafe Entry* FreeHead;

		public ulong UsedCount;

		public ulong FreeCount;

		public ulong CapacityUsable;

		public ulong CapacityAllocated;

		public readonly ulong Count => UsedCount - FreeCount;

		public readonly bool Full => UsedCount == CapacityAllocated;

		public unsafe static void Dispose(ref NetPeerGroupMap* map)
		{
			FusionUnsafe.Free(ref map);
		}

		public unsafe static NetPeerGroupMap* Allocate(int capacity)
		{
			Assert.Check(capacity >= 0, "capacity >= 0");
			int nextPrime = Primes.GetNextPrime(capacity * 2);
			int num = FusionUnsafe.MakeAligned(sizeof(NetPeerGroupMap));
			int num2 = FusionUnsafe.MakeAligned(sizeof(Entry*) * nextPrime);
			int num3 = FusionUnsafe.MakeAligned(sizeof(Entry) * nextPrime);
			byte* ptr = (byte*)FusionUnsafe.AllocAndClear(num + num2 + num3, 8, "Fusion\\Fusion.Sockets\\NetPeerGroupMap.cs", 43);
			NetPeerGroupMap* ptr2 = (NetPeerGroupMap*)ptr;
			ptr2->Buckets = (Entry**)(ptr + num);
			ptr2->Entries = (Entry*)(ptr + num + num2);
			ptr2->UsedCount = 0uL;
			ptr2->FreeCount = 0uL;
			ptr2->CapacityUsable = (ulong)capacity;
			ptr2->CapacityAllocated = (ulong)nextPrime;
			for (int i = 0; i < capacity; i++)
			{
				ptr2->Entries[i].Group = -1;
			}
			return ptr2;
		}

		public unsafe int Remove(NetAddress address)
		{
			ulong num = NetAddress.Hash64(address);
			ulong num2 = num % CapacityAllocated;
			Entry* ptr = Buckets[num2];
			Entry* ptr2 = default(Entry*);
			while (ptr != null)
			{
				if (ptr->Hash == num && ptr->Address.Block0 == address.Block0 && ptr->Address.Block1 == address.Block1 && ptr->Address.Block2 == address.Block2)
				{
					if (ptr2 == null)
					{
						Buckets[num2] = ptr->Next;
					}
					else
					{
						ptr2->Next = ptr->Next;
					}
					Assert.Check(ptr->State == EntryState.Used, "bucketHead->State == EntryState.Used");
					short result = ptr->Group;
					*ptr = default(Entry);
					ptr->Group = -1;
					ptr->Next = FreeHead;
					ptr->State = EntryState.Free;
					FreeHead = ptr;
					FreeCount++;
					return result;
				}
				ptr2 = ptr;
				ptr = ptr->Next;
			}
			return -1;
		}

		public unsafe bool Insert(NetAddress address, short group)
		{
			Assert.Check(Find(address) == -1, "Find(address) == -1");
			ulong num = NetAddress.Hash64(address);
			ulong num2 = num % CapacityAllocated;
			Entry* ptr;
			if (FreeHead != null)
			{
				Assert.Check(FreeCount != 0, "FreeCount > 0");
				ptr = FreeHead;
				FreeHead = ptr->Next;
				FreeCount--;
				Assert.Check(ptr->State == EntryState.Free, "entry->State == EntryState.Free");
			}
			else
			{
				if (UsedCount == CapacityUsable)
				{
					InternalLogStreams.LogTraceNetwork?.Log("NetPeerGroupMap is full");
					return false;
				}
				ptr = Entries + UsedCount++;
				Assert.Check(ptr->Group == -1, "entry->Group == -1");
				Assert.Check(ptr->State == EntryState.None, "entry->State == EntryState.None");
				Assert.Check(ptr->Next == null, "entry->Next == null");
			}
			InternalLogStreams.LogTraceNetwork?.Log($"{address.ToString()} mapped to group {group}");
			ptr->Hash = num;
			ptr->Group = group;
			ptr->Address = address;
			ptr->State = EntryState.Used;
			ptr->Next = Buckets[num2];
			Buckets[num2] = ptr;
			return true;
		}

		public unsafe short Find(NetAddress address)
		{
			ulong num = NetAddress.Hash64(address);
			ulong num2 = num % CapacityAllocated;
			for (Entry* ptr = Buckets[num2]; ptr != null; ptr = ptr->Next)
			{
				if (ptr->Hash == num && ptr->Address.Block0 == address.Block0 && ptr->Address.Block1 == address.Block1 && ptr->Address.Block2 == address.Block2)
				{
					return ptr->Group;
				}
			}
			return -1;
		}
	}
}
