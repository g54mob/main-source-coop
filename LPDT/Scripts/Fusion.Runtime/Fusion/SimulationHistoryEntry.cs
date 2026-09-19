#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Fusion.Analyzer;

namespace Fusion
{
	[GenerateIntrusiveLinkedList("_node", GenerateAsClass = true)]
	internal class SimulationHistoryEntry
	{
		[DebuggerTypeProxy(typeof(DebuggerProxy))]
		internal class List : IIntrusiveLinkedList<SimulationHistoryEntry>, IIntrusiveLinkedList
		{
			private class DebuggerProxy
			{
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public readonly SimulationHistoryEntry[] Items;

				public DebuggerProxy(List list)
				{
					Items = list.ToArray();
					base._002Ector();
				}
			}

			private int _count;

			public SimulationHistoryEntry Head;

			public int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _count;
				}
			}

			public SimulationHistoryEntry Tail
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (Head != null) ? Head._node.Prev : null;
				}
			}

			public bool IsEmpty => Head == null;

			public void AddFirst(SimulationHistoryEntry item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._node.Next = (item._node.Prev = item));
				}
				else
				{
					SimulationHistoryEntry prev = Head._node.Prev;
					ValidateNodeSet(prev);
					item._node.Next = Head;
					Head._node.Prev = item;
					prev._node.Next = item;
					item._node.Prev = prev;
					Head = item;
				}
				_count++;
				ValidateNodeSet(item);
				ValidateNodeSet(Head);
			}

			public void AddLast(SimulationHistoryEntry item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._node.Next = (item._node.Prev = item));
				}
				else
				{
					SimulationHistoryEntry prev = Head._node.Prev;
					ValidateNodeSet(prev);
					item._node.Next = Head;
					Head._node.Prev = item;
					prev._node.Next = item;
					item._node.Prev = prev;
				}
				_count++;
				ValidateNodeSet(item);
				ValidateNodeSet(Head);
			}

			public bool TryRemoveFirst(out SimulationHistoryEntry result)
			{
				if (Head == null)
				{
					result = null;
					return false;
				}
				SimulationHistoryEntry prev = Head._node.Prev;
				ValidateNodeSet(Head);
				ValidateNodeSet(prev);
				result = Head;
				if (prev == Head)
				{
					Assert.Check(prev == Head._node.Next, "tail == Head._node.Next");
					Head = null;
				}
				else
				{
					Head = Head._node.Next;
					prev._node.Next = Head;
					Head._node.Prev = prev;
				}
				result._node.Next = (result._node.Prev = null);
				_count--;
				ValidateNodeClear(result);
				return true;
			}

			public bool TryPeekFirst(out SimulationHistoryEntry result)
			{
				result = Head;
				return Head != null;
			}

			public SimulationHistoryEntry RemoveFirst()
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

			public bool Remove(SimulationHistoryEntry item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				if (item._node.Prev == null || item._node.Next == null)
				{
					ValidateNodeClear(item);
					return false;
				}
				Assert.Check(IsInListSlow(item), "IsInListSlow(item)");
				if (_count == 1)
				{
					Assert.Check(Head == item, "Head == item");
					Assert.Check(Head == item._node.Prev, "Head == item._node.Prev");
					Assert.Check(Head == item._node.Next, "Head == item._node.Next");
					Head = null;
				}
				else
				{
					item._node.Prev._node.Next = item._node.Next;
					item._node.Next._node.Prev = item._node.Prev;
					if (item == Head)
					{
						Head = item._node.Next;
					}
				}
				item._node.Next = (item._node.Prev = null);
				_count--;
				return true;
			}

			public bool IsInListSlow(SimulationHistoryEntry item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				SimulationHistoryEntry simulationHistoryEntry = Head;
				do
				{
					if (simulationHistoryEntry == item)
					{
						return true;
					}
					simulationHistoryEntry = simulationHistoryEntry._node.Next;
				}
				while (simulationHistoryEntry != Head);
				return false;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public SimulationHistoryEntry Next(SimulationHistoryEntry item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item._node.Next != Head) ? item._node.Next : null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public SimulationHistoryEntry Prev(SimulationHistoryEntry item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item != Head) ? item._node.Prev : null;
			}

			public SimulationHistoryEntry[] ToArray()
			{
				SimulationHistoryEntry[] array = new SimulationHistoryEntry[_count];
				if (Head == null)
				{
					return Array.Empty<SimulationHistoryEntry>();
				}
				Assert.Check(_count > 0, "_count > 0");
				ValidateNodeSet(Head);
				SimulationHistoryEntry simulationHistoryEntry = Head;
				int num = 0;
				do
				{
					array[num++] = simulationHistoryEntry;
					simulationHistoryEntry = simulationHistoryEntry._node.Next;
				}
				while (simulationHistoryEntry != Head);
				return array;
			}

			void IIntrusiveLinkedList.CopyTo(Array array, int index)
			{
				if (array.Length - index < _count)
				{
					throw new ArgumentException();
				}
				if (Head != null)
				{
					Assert.Check(_count > 0, "_count > 0");
					ValidateNodeSet(Head);
					SimulationHistoryEntry simulationHistoryEntry = Head;
					do
					{
						array.SetValue(simulationHistoryEntry, index++);
						simulationHistoryEntry = simulationHistoryEntry._node.Next;
					}
					while (simulationHistoryEntry != Head);
				}
			}

			private void ValidateNodeSet(SimulationHistoryEntry item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._node.Next != null, "item._node.Next != null");
				Assert.Check(item._node.Prev != null, "item._node.Prev != null");
			}

			private void ValidateNodeClear(SimulationHistoryEntry item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._node.Next == null, "item._node.Next == null");
				Assert.Check(item._node.Prev == null, "item._node.Prev == null");
			}
		}

		private IntrusiveLinkedListNode<SimulationHistoryEntry> _node;

		public Tick Tick;

		public double Time;
	}
}
