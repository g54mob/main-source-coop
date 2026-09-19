#define TRACE
#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Fusion.Analyzer;

namespace Fusion
{
	[GenerateIntrusiveLinkedList("_node")]
	internal class NetworkObjectConnectionData
	{
		[DebuggerTypeProxy(typeof(DebuggerProxy))]
		internal struct List : IIntrusiveLinkedList<NetworkObjectConnectionData>, IIntrusiveLinkedList
		{
			private class DebuggerProxy
			{
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public readonly NetworkObjectConnectionData[] Items;

				public DebuggerProxy(List list)
				{
					Items = list.ToArray();
					base._002Ector();
				}
			}

			private int _count;

			public NetworkObjectConnectionData Head;

			public int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _count;
				}
			}

			public NetworkObjectConnectionData Tail
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (Head != null) ? Head._node.Prev : null;
				}
			}

			public bool IsEmpty => Head == null;

			public void AddFirst(NetworkObjectConnectionData item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._node.Next = (item._node.Prev = item));
				}
				else
				{
					NetworkObjectConnectionData prev = Head._node.Prev;
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

			public void AddLast(NetworkObjectConnectionData item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._node.Next = (item._node.Prev = item));
				}
				else
				{
					NetworkObjectConnectionData prev = Head._node.Prev;
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

			public bool TryRemoveFirst(out NetworkObjectConnectionData result)
			{
				if (Head == null)
				{
					result = null;
					return false;
				}
				NetworkObjectConnectionData prev = Head._node.Prev;
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

			public bool TryPeekFirst(out NetworkObjectConnectionData result)
			{
				result = Head;
				return Head != null;
			}

			public NetworkObjectConnectionData RemoveFirst()
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

			public bool Remove(NetworkObjectConnectionData item)
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

			public bool IsInListSlow(NetworkObjectConnectionData item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				NetworkObjectConnectionData networkObjectConnectionData = Head;
				do
				{
					if (networkObjectConnectionData == item)
					{
						return true;
					}
					networkObjectConnectionData = networkObjectConnectionData._node.Next;
				}
				while (networkObjectConnectionData != Head);
				return false;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NetworkObjectConnectionData Next(NetworkObjectConnectionData item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item._node.Next != Head) ? item._node.Next : null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NetworkObjectConnectionData Prev(NetworkObjectConnectionData item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item != Head) ? item._node.Prev : null;
			}

			public NetworkObjectConnectionData[] ToArray()
			{
				NetworkObjectConnectionData[] array = new NetworkObjectConnectionData[_count];
				if (Head == null)
				{
					return Array.Empty<NetworkObjectConnectionData>();
				}
				Assert.Check(_count > 0, "_count > 0");
				ValidateNodeSet(Head);
				NetworkObjectConnectionData networkObjectConnectionData = Head;
				int num = 0;
				do
				{
					array[num++] = networkObjectConnectionData;
					networkObjectConnectionData = networkObjectConnectionData._node.Next;
				}
				while (networkObjectConnectionData != Head);
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
					NetworkObjectConnectionData networkObjectConnectionData = Head;
					do
					{
						array.SetValue(networkObjectConnectionData, index++);
						networkObjectConnectionData = networkObjectConnectionData._node.Next;
					}
					while (networkObjectConnectionData != Head);
				}
			}

			private void ValidateNodeSet(NetworkObjectConnectionData item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._node.Next != null, "item._node.Next != null");
				Assert.Check(item._node.Prev != null, "item._node.Prev != null");
			}

			private void ValidateNodeClear(NetworkObjectConnectionData item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._node.Next == null, "item._node.Next == null");
				Assert.Check(item._node.Prev == null, "item._node.Prev == null");
			}
		}

		private IntrusiveLinkedListNode<NetworkObjectConnectionData> _node;

		public NetworkId Id;

		public int PriorityLevel = 1;

		private Tick _tickAcknowledged;

		private Tick _tickSent;

		public NetworkObjectConnectionDataFlags Flags;

		public Tick TickMin;

		public ulong Filter = ulong.MaxValue;

		public NetworkObjectConnectionDataStatus Status { get; private set; }

		public Tick TickSent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _tickSent;
			}
		}

		public Tick TickAcknowledged
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _tickAcknowledged;
			}
		}

		public NetworkObjectHeaderPlayerDataFlags PlayerFlags
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private set;
		}

		public ChangeTick PlayerFlagsChangeTick
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private set;
		}

		public void SetPlayerFlag(NetworkObjectHeaderPlayerDataFlags flags, Simulation simulation)
		{
			PlayerFlags |= flags;
			PlayerFlagsChangeTick = ChangeTick.FromSimulationTick(simulation.Tick);
			TickMin = simulation.Tick;
		}

		public void ClearPlayerFlag(NetworkObjectHeaderPlayerDataFlags flags, Simulation simulation)
		{
			PlayerFlags &= ~flags;
			PlayerFlagsChangeTick = ChangeTick.FromSimulationTick(simulation.Tick);
			TickMin = simulation.Tick;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStatus(SimulationConnection sc, NetworkObjectConnectionDataStatus status, [CallerMemberName] string caller = null)
		{
			if (Status != status)
			{
				InternalLogStreams.LogTraceSendRecv?.Log(sc.Simulation, string.Format("[ConnectionStatus] {0} from {1}: {2}: {3} -> {4}", Simulation.StateReplicator.LogPrefix(sc.Simulation, Id), caller, sc.Simulation.IsClient ? "" : ((object)sc.Player), Status, status));
				Status = status;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTickAck(SimulationConnection sc, Tick tick, [CallerMemberName] string caller = null)
		{
			if (!(_tickAcknowledged == tick))
			{
				InternalLogStreams.LogTraceSendRecv?.Log(sc.Simulation, string.Format("[ConnectionTickAck] {0} from {1}: {2}: {3} -> {4}", Simulation.StateReplicator.LogPrefix(sc.Simulation, Id), caller, sc.Simulation.IsClient ? "" : ((object)sc.Player), _tickAcknowledged, tick));
				_tickAcknowledged = tick;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTickSent(SimulationConnection sc, Tick tick, string extraLogMessagePleaseDoNotAlloc = null, [CallerMemberName] string caller = null)
		{
			if (!(_tickSent == tick))
			{
				InternalLogStreams.LogTraceSendRecv?.Log(sc.Simulation, string.Format("[ConnectionTickSent] {0} from {1}:{2} {3}: {4} -> {5}", Simulation.StateReplicator.LogPrefix(sc.Simulation, Id), caller, extraLogMessagePleaseDoNotAlloc, sc.Simulation.IsClient ? "" : ((object)sc.Player), _tickSent, tick));
				_tickSent = tick;
			}
		}
	}
}
