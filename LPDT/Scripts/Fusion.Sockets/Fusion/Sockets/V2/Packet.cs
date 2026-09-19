#define TRACE
#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fusion.Analyzer;

namespace Fusion.Sockets.V2
{
	[StructLayout(LayoutKind.Explicit)]
	[GenerateIntrusiveLinkedList("_node", FreeMethod = "Free")]
	internal struct Packet
	{
		public struct StateType
		{
			public double SendTime;

			public NetAddress Address;

			public int Size;

			public unsafe byte* Buffer;

			public unsafe Pool* Pool;

			public unsafe PacketGroup* Group;

			public unsafe override readonly string ToString()
			{
				return string.Format("[StateType: {0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}]", "SendTime", SendTime, "Address", Address, "Size", Size, "Group", (Group != null) ? ((object)Group->State) : "nope");
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Pool
		{
			public unsafe static void FreeAll(Pool* pool)
			{
			}

			public unsafe static Packet* Get(Pool* pool)
			{
				Packet* ptr = Alloc();
				ptr->State.Pool = pool;
				return ptr;
			}

			public unsafe static void Return(Pool* pool, ref Packet* packet)
			{
				Assert.Check(pool == packet->State.Pool, "pool == packet->State.Pool");
				Assert.Check(packet->_prev == null, "packet->_prev == null");
				Assert.Check(packet->_next == null, "packet->_next == null");
				packet->State.Size = 0;
				packet->State.Group = default(PacketGroup*);
				packet->State.Address = default(NetAddress);
				packet->State.SendTime = 0.0;
				Free(ref packet);
			}
		}

		[DebuggerTypeProxy(typeof(DebuggerProxy))]
		internal struct List : IIntrusiveLinkedListPtr<Packet>, IIntrusiveLinkedList
		{
			private class DebuggerProxy
			{
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public unsafe readonly Packet*[] Items;

				public unsafe DebuggerProxy(List list)
				{
					Items = list.ToArray();
					base._002Ector();
				}
			}

			private int _count;

			public unsafe Packet* Head;

			public int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _count;
				}
			}

			public unsafe Packet* Tail
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (Head != null) ? Head->_prev : null;
				}
			}

			public unsafe bool IsEmpty => Head == null;

			public unsafe void AddFirst(Packet* item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item->_next = (item->_prev = item));
				}
				else
				{
					Packet* prev = Head->_prev;
					ValidateNodeSet(prev);
					item->_next = Head;
					Head->_prev = item;
					prev->_next = item;
					item->_prev = prev;
					Head = item;
				}
				_count++;
				ValidateNodeSet(item);
				ValidateNodeSet(Head);
			}

			public unsafe void AddLast(Packet* item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item->_next = (item->_prev = item));
				}
				else
				{
					Packet* prev = Head->_prev;
					ValidateNodeSet(prev);
					item->_next = Head;
					Head->_prev = item;
					prev->_next = item;
					item->_prev = prev;
				}
				_count++;
				ValidateNodeSet(item);
				ValidateNodeSet(Head);
			}

			public unsafe bool TryRemoveFirst(out Packet* result)
			{
				if (Head == null)
				{
					result = default(Packet*);
					return false;
				}
				Packet* prev = Head->_prev;
				ValidateNodeSet(Head);
				ValidateNodeSet(prev);
				result = Head;
				if (prev == Head)
				{
					Assert.Check(prev == Head->_next, "tail == Head->_next");
					Head = default(Packet*);
				}
				else
				{
					Head = Head->_next;
					prev->_next = Head;
					Head->_prev = prev;
				}
				result->_next = (result->_prev = null);
				_count--;
				ValidateNodeClear(result);
				return true;
			}

			public unsafe bool TryPeekFirst(out Packet* result)
			{
				result = Head;
				return Head != null;
			}

			public unsafe Packet* RemoveFirst()
			{
				Assert.Check(_count > 0, "_count > 0");
				Assert.Check(Head != null, "Head != null");
				Assert.Check(IsInListSlow(Head), "IsInListSlow(Head)");
				if (!TryRemoveFirst(out var result))
				{
					Assert.AlwaysFail("Failed to remove first");
				}
				return result;
			}

			public unsafe bool Remove(Packet* item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				if (item->_prev == null || item->_next == null)
				{
					ValidateNodeClear(item);
					return false;
				}
				Assert.Check(IsInListSlow(item), "IsInListSlow(item)");
				if (_count == 1)
				{
					Assert.Check(Head == item, "Head == item");
					Assert.Check(Head == item->_prev, "Head == item->_prev");
					Assert.Check(Head == item->_next, "Head == item->_next");
					Head = default(Packet*);
				}
				else
				{
					item->_prev->_next = item->_next;
					item->_next->_prev = item->_prev;
					if (item == Head)
					{
						Head = item->_next;
					}
				}
				item->_next = (item->_prev = null);
				_count--;
				return true;
			}

			public unsafe bool IsInListSlow(Packet* item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				Packet* ptr = Head;
				do
				{
					if (ptr == item)
					{
						return true;
					}
					ptr = ptr->_next;
				}
				while (ptr != Head);
				return false;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe Packet* Next(Packet* item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item->_next != Head) ? item->_next : null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe Packet* Prev(Packet* item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item != Head) ? item->_prev : null;
			}

			public unsafe void FreeAll()
			{
				Packet* result;
				while (TryRemoveFirst(out result))
				{
					Free(ref result);
				}
			}

			public unsafe Packet*[] ToArray()
			{
				Packet*[] array = new Packet*[_count];
				if (Head == null)
				{
					return new Packet*[0];
				}
				Assert.Check(_count > 0, "_count > 0");
				ValidateNodeSet(Head);
				Packet* ptr = Head;
				int num = 0;
				do
				{
					array[num++] = ptr;
					ptr = ptr->_next;
				}
				while (ptr != Head);
				return array;
			}

			unsafe void IIntrusiveLinkedList.CopyTo(Array array, int index)
			{
				if (array.Length - index < _count)
				{
					throw new ArgumentException();
				}
				if (Head != null)
				{
					Assert.Check(_count > 0, "_count > 0");
					ValidateNodeSet(Head);
					Packet* ptr = Head;
					do
					{
						array.SetValue(FusionUnsafe.Box(ptr), index++);
						ptr = ptr->_next;
					}
					while (ptr != Head);
				}
			}

			private unsafe void ValidateNodeSet(Packet* item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item->_next != null, "item->_next != null");
				Assert.Check(item->_prev != null, "item->_prev != null");
			}

			private unsafe void ValidateNodeClear(Packet* item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item->_next == null, "item->_next == null");
				Assert.Check(item->_prev == null, "item->_prev == null");
			}
		}

		[FieldOffset(0)]
		private unsafe Packet* _next;

		[FieldOffset(8)]
		private unsafe Packet* _prev;

		[FieldOffset(16)]
		public StateType State;

		public unsafe readonly Header* Header
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (Header*)State.Buffer;
			}
		}

		public unsafe static void Release(ref Packet* packet, [CallerMemberName] string callerMember = null, [CallerLineNumber] int lineNumber = 0)
		{
			InternalLogStreams.LogTraceNetwork?.Log(string.Format("Releasing packet {0} {1} from {2}:{3}", (packet->Header != null) ? packet->Header->ToString() : "nope", packet->State, callerMember, lineNumber));
			Pool.Return(packet->State.Pool, ref packet);
		}

		public unsafe static Packet* Alloc()
		{
			Packet* ptr = FusionUnsafe.AllocAndClear<Packet>(8, "Fusion\\Fusion.Sockets\\_V2\\Packet.cs", 46);
			ptr->State.Buffer = (byte*)FusionUnsafe.AllocAndClear(1136, 8, "Fusion\\Fusion.Sockets\\_V2\\Packet.cs", 47);
			return ptr;
		}

		public unsafe override readonly string ToString()
		{
			return $"[Packet Header:{*Header}, State:{State}]";
		}

		public unsafe static void Free(ref Packet* packet)
		{
			if (packet != null)
			{
				Assert.Check(packet->_prev == null, "packet->_prev == null");
				Assert.Check(packet->_next == null, "packet->_next == null");
				FusionUnsafe.Free(ref packet->State.Buffer);
				*packet = default(Packet);
				FusionUnsafe.Free(ref packet);
			}
		}
	}
}
