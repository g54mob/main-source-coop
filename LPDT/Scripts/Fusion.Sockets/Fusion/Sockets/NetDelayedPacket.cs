#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Fusion.Analyzer;

namespace Fusion.Sockets
{
	[GenerateIntrusiveLinkedList("_node", FreeMethod = "FusionUnsafe.Free")]
	internal struct NetDelayedPacket
	{
		[DebuggerTypeProxy(typeof(DebuggerProxy))]
		internal struct List : IIntrusiveLinkedListPtr<NetDelayedPacket>, IIntrusiveLinkedList
		{
			private class DebuggerProxy
			{
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public unsafe readonly NetDelayedPacket*[] Items;

				public unsafe DebuggerProxy(List list)
				{
					Items = list.ToArray();
					base._002Ector();
				}
			}

			private int _count;

			public unsafe NetDelayedPacket* Head;

			public int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _count;
				}
			}

			public unsafe NetDelayedPacket* Tail
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (Head != null) ? Head->_prev : null;
				}
			}

			public unsafe bool IsEmpty => Head == null;

			public unsafe void AddFirst(NetDelayedPacket* item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item->_next = (item->_prev = item));
				}
				else
				{
					NetDelayedPacket* prev = Head->_prev;
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

			public unsafe void AddLast(NetDelayedPacket* item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item->_next = (item->_prev = item));
				}
				else
				{
					NetDelayedPacket* prev = Head->_prev;
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

			public unsafe bool TryRemoveFirst(out NetDelayedPacket* result)
			{
				if (Head == null)
				{
					result = default(NetDelayedPacket*);
					return false;
				}
				NetDelayedPacket* prev = Head->_prev;
				ValidateNodeSet(Head);
				ValidateNodeSet(prev);
				result = Head;
				if (prev == Head)
				{
					Assert.Check(prev == Head->_next, "tail == Head->_next");
					Head = default(NetDelayedPacket*);
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

			public unsafe bool TryPeekFirst(out NetDelayedPacket* result)
			{
				result = Head;
				return Head != null;
			}

			public unsafe NetDelayedPacket* RemoveFirst()
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

			public unsafe bool Remove(NetDelayedPacket* item)
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
					Head = default(NetDelayedPacket*);
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

			public unsafe bool IsInListSlow(NetDelayedPacket* item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				NetDelayedPacket* ptr = Head;
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
			public unsafe NetDelayedPacket* Next(NetDelayedPacket* item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item->_next != Head) ? item->_next : null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe NetDelayedPacket* Prev(NetDelayedPacket* item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item != Head) ? item->_prev : null;
			}

			public unsafe void FreeAll()
			{
				NetDelayedPacket* result;
				while (TryRemoveFirst(out result))
				{
					FusionUnsafe.Free(ref result);
				}
			}

			public unsafe NetDelayedPacket*[] ToArray()
			{
				NetDelayedPacket*[] array = new NetDelayedPacket*[_count];
				if (Head == null)
				{
					return new NetDelayedPacket*[0];
				}
				Assert.Check(_count > 0, "_count > 0");
				ValidateNodeSet(Head);
				NetDelayedPacket* ptr = Head;
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
					NetDelayedPacket* ptr = Head;
					do
					{
						array.SetValue(FusionUnsafe.Box(ptr), index++);
						ptr = ptr->_next;
					}
					while (ptr != Head);
				}
			}

			private unsafe void ValidateNodeSet(NetDelayedPacket* item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item->_next != null, "item->_next != null");
				Assert.Check(item->_prev != null, "item->_prev != null");
			}

			private unsafe void ValidateNodeClear(NetDelayedPacket* item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item->_next == null, "item->_next == null");
				Assert.Check(item->_prev == null, "item->_prev == null");
			}
		}

		private unsafe NetDelayedPacket* _next;

		private unsafe NetDelayedPacket* _prev;

		public double DeliveryTime;

		public NetAddress Address;

		public unsafe byte* Data;

		public int DataLength;

		public unsafe static NetDelayedPacket* Create(int dataLength)
		{
			int num = FusionUnsafe.MakeAligned(sizeof(NetDelayedPacket));
			int num2 = FusionUnsafe.MakeAligned(dataLength);
			byte* ptr = (byte*)FusionUnsafe.AllocAndClear(num + num2, 8, "Fusion\\Fusion.Sockets\\NetDelayedPacket.cs", 18);
			NetDelayedPacket* ptr2 = (NetDelayedPacket*)ptr;
			ptr2->Data = ptr + num;
			ptr2->DataLength = dataLength;
			return ptr2;
		}

		public unsafe static void Dispose(ref NetDelayedPacket* delayed)
		{
			FusionUnsafe.Free(ref delayed);
		}
	}
}
