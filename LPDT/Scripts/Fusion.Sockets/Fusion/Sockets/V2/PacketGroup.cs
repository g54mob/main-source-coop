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
	internal struct PacketGroup
	{
		public struct StateType
		{
			public nuint UserToken;

			public bool WasLost;

			public bool WasDelivered;

			public uint FragmentGroup;

			public int FragmentCount;

			public unsafe ulong* FragmentsDelivered;

			public int FragmentsDeliveredCount;

			public override readonly string ToString()
			{
				return string.Format("[StateType: {0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}, {10}: {11}]", "UserToken", UserToken, "WasLost", WasLost, "WasDelivered", WasDelivered, "FragmentGroup", FragmentGroup, "FragmentCount", FragmentCount, "FragmentsDeliveredCount", FragmentsDeliveredCount);
			}
		}

		[DebuggerTypeProxy(typeof(DebuggerProxy))]
		internal struct List : IIntrusiveLinkedListPtr<PacketGroup>, IIntrusiveLinkedList
		{
			private class DebuggerProxy
			{
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public unsafe readonly PacketGroup*[] Items;

				public unsafe DebuggerProxy(List list)
				{
					Items = list.ToArray();
					base._002Ector();
				}
			}

			private int _count;

			public unsafe PacketGroup* Head;

			public int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _count;
				}
			}

			public unsafe PacketGroup* Tail
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (Head != null) ? Head->_prev : null;
				}
			}

			public unsafe bool IsEmpty => Head == null;

			public unsafe void AddFirst(PacketGroup* item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item->_next = (item->_prev = item));
				}
				else
				{
					PacketGroup* prev = Head->_prev;
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

			public unsafe void AddLast(PacketGroup* item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item->_next = (item->_prev = item));
				}
				else
				{
					PacketGroup* prev = Head->_prev;
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

			public unsafe bool TryRemoveFirst(out PacketGroup* result)
			{
				if (Head == null)
				{
					result = default(PacketGroup*);
					return false;
				}
				PacketGroup* prev = Head->_prev;
				ValidateNodeSet(Head);
				ValidateNodeSet(prev);
				result = Head;
				if (prev == Head)
				{
					Assert.Check(prev == Head->_next, "tail == Head->_next");
					Head = default(PacketGroup*);
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

			public unsafe bool TryPeekFirst(out PacketGroup* result)
			{
				result = Head;
				return Head != null;
			}

			public unsafe PacketGroup* RemoveFirst()
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

			public unsafe bool Remove(PacketGroup* item)
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
					Head = default(PacketGroup*);
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

			public unsafe bool IsInListSlow(PacketGroup* item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				PacketGroup* ptr = Head;
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
			public unsafe PacketGroup* Next(PacketGroup* item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item->_next != Head) ? item->_next : null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe PacketGroup* Prev(PacketGroup* item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item != Head) ? item->_prev : null;
			}

			public unsafe void FreeAll()
			{
				PacketGroup* result;
				while (TryRemoveFirst(out result))
				{
					Free(ref result);
				}
			}

			public unsafe PacketGroup*[] ToArray()
			{
				PacketGroup*[] array = new PacketGroup*[_count];
				if (Head == null)
				{
					return new PacketGroup*[0];
				}
				Assert.Check(_count > 0, "_count > 0");
				ValidateNodeSet(Head);
				PacketGroup* ptr = Head;
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
					PacketGroup* ptr = Head;
					do
					{
						array.SetValue(FusionUnsafe.Box(ptr), index++);
						ptr = ptr->_next;
					}
					while (ptr != Head);
				}
			}

			private unsafe void ValidateNodeSet(PacketGroup* item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item->_next != null, "item->_next != null");
				Assert.Check(item->_prev != null, "item->_prev != null");
			}

			private unsafe void ValidateNodeClear(PacketGroup* item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item->_next == null, "item->_next == null");
				Assert.Check(item->_prev == null, "item->_prev == null");
			}
		}

		[FieldOffset(0)]
		private unsafe PacketGroup* _next;

		[FieldOffset(8)]
		private unsafe PacketGroup* _prev;

		[FieldOffset(16)]
		public StateType State;

		public unsafe static PacketGroup* Alloc()
		{
			return FusionUnsafe.AllocAndClear<PacketGroup>(8, "Fusion\\Fusion.Sockets\\_V2\\PacketGroup.cs", 34);
		}

		public unsafe static void Free(ref PacketGroup* group)
		{
			if (group != null)
			{
				FusionUnsafe.Free(ref group->State.FragmentsDelivered);
				*group = default(PacketGroup);
				FusionUnsafe.Free(ref group);
			}
		}

		public unsafe bool IsDelivered(uint fragment)
		{
			if (State.FragmentCount == 1)
			{
				Assert.Check(fragment == 0, "fragment == 0");
				Assert.Check(State.FragmentsDelivered == null, "State.FragmentsDelivered == null");
				return State.FragmentsDeliveredCount == 1;
			}
			Assert.Check(State.FragmentsDelivered != null, "State.FragmentsDelivered != null");
			Assert.Check(fragment < State.FragmentCount, "fragment < State.FragmentCount");
			return (State.FragmentsDelivered[fragment / 64] & (ulong)(1L << (int)(fragment % 64))) != 0;
		}

		public unsafe bool SetDelivered(uint fragment)
		{
			if (State.FragmentCount == 1)
			{
				Assert.Check(fragment == 0, "fragment == 0");
				Assert.Check(State.FragmentsDelivered == null, "State.FragmentsDelivered == null");
				Assert.Check(State.FragmentsDeliveredCount == 0, "State.FragmentsDeliveredCount == 0");
				State.FragmentsDeliveredCount = 1;
			}
			else
			{
				Assert.Check(State.FragmentsDelivered != null, "State.FragmentsDelivered != null");
				Assert.Check(fragment < State.FragmentCount, "fragment < State.FragmentCount");
				ulong num = (ulong)(1L << (int)(fragment % 64));
				if ((State.FragmentsDelivered[fragment / 64] & num) == 0)
				{
					State.FragmentsDeliveredCount++;
					State.FragmentsDelivered[fragment / 64] |= num;
				}
				else
				{
					InternalLogStreams.LogTraceNetwork?.Warn($"Packet with fragment {fragment} already delivered, should not be called again");
				}
			}
			Assert.Check(State.FragmentsDeliveredCount <= State.FragmentCount, "State.FragmentsDeliveredCount <= State.FragmentCount");
			return State.FragmentsDeliveredCount == State.FragmentCount;
		}
	}
}
