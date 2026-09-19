#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Fusion.Analyzer;
using Fusion.Sockets;

namespace Fusion
{
	[GenerateIntrusiveLinkedList("_node", GenerateAsClass = true)]
	public class SimulationInput
	{
		public class Buffer
		{
			private readonly int _capacity;

			private readonly Dictionary<Tick, SimulationInput> _inputs;

			private readonly Dictionary<Tick, double> _insertTimes;

			private SimulationInputHeader _lastUsedInputHeader;

			public int Count => _inputs.Count;

			public int Capacity => _capacity;

			public bool IsFull => Count == Capacity;

			public Buffer(int capacity)
			{
				_capacity = capacity;
				_inputs = new Dictionary<Tick, SimulationInput>(new Tick.EqualityComparer());
				_insertTimes = new Dictionary<Tick, double>();
			}

			public void Clear()
			{
				_inputs.Clear();
				_insertTimes.Clear();
			}

			public int CopySortedTo(SimulationInput[] array)
			{
				Assert.Always(array.Length >= Count, "Array too small");
				Array.Clear(array, 0, array.Length);
				_inputs.Values.CopyTo(array, 0);
				ArraySpecialized.Sort(array, 0, _inputs.Count);
				for (int i = 0; i < _inputs.Count; i++)
				{
					Assert.Check(array[i] != null, "array[i] != null");
				}
				return _inputs.Count;
			}

			public bool Contains(Tick tick)
			{
				return _inputs.ContainsKey(tick);
			}

			public SimulationInput Get(Tick tick)
			{
				return _inputs[tick];
			}

			public bool TryGet(Tick tick, out SimulationInput input)
			{
				return _inputs.TryGetValue(tick, out input);
			}

			public bool TryGet(Tick tick, out SimulationInput input, out double? timeInserted)
			{
				if (_inputs.TryGetValue(tick, out input))
				{
					if (_insertTimes.TryGetValue(tick, out var value))
					{
						timeInserted = value;
					}
					else
					{
						timeInserted = null;
					}
					return true;
				}
				input = null;
				timeInserted = null;
				return false;
			}

			public bool Add(SimulationInput input, double? timeInserted = null)
			{
				Assert.Check(input != null, "input cannot be null");
				if (Contains(input.Header.Tick))
				{
					return false;
				}
				if (IsFull)
				{
					InternalLogStreams.LogWarn?.Log($"could not add {input.Player} input for {input.Header.Tick} to buffer because buffer was full");
					return false;
				}
				_inputs.Add(input.Header.Tick, input);
				if (timeInserted.HasValue)
				{
					_insertTimes.Add(input.Header.Tick, timeInserted.Value);
				}
				return true;
			}

			public bool Remove(Tick tick, out SimulationInput removed)
			{
				double? timeInserted;
				return Remove(tick, out removed, out timeInserted);
			}

			public bool Remove(Tick tick, out SimulationInput removed, out double? timeInserted)
			{
				if (_inputs.Remove(tick, out removed))
				{
					_lastUsedInputHeader.Tick = removed.Header.Tick;
					_lastUsedInputHeader.InterpFrom = removed.Header.InterpFrom;
					_lastUsedInputHeader.InterpTo = removed.Header.InterpTo;
					_lastUsedInputHeader.InterpAlpha = removed.Header.InterpAlpha;
					if (_insertTimes.Remove(tick, out var value))
					{
						timeInserted = value;
					}
					else
					{
						timeInserted = null;
					}
					return true;
				}
				timeInserted = null;
				return false;
			}

			public SimulationInputHeader GetLastUsedInputHeader()
			{
				return _lastUsedInputHeader;
			}
		}

		[DebuggerTypeProxy(typeof(DebuggerProxy))]
		internal class List : IIntrusiveLinkedList<SimulationInput>, IIntrusiveLinkedList
		{
			private class DebuggerProxy
			{
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public readonly SimulationInput[] Items;

				public DebuggerProxy(List list)
				{
					Items = list.ToArray();
					base._002Ector();
				}
			}

			private int _count;

			public SimulationInput Head;

			public int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _count;
				}
			}

			public SimulationInput Tail
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (Head != null) ? Head._node.Prev : null;
				}
			}

			public bool IsEmpty => Head == null;

			public void AddFirst(SimulationInput item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._node.Next = (item._node.Prev = item));
				}
				else
				{
					SimulationInput prev = Head._node.Prev;
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

			public void AddLast(SimulationInput item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._node.Next = (item._node.Prev = item));
				}
				else
				{
					SimulationInput prev = Head._node.Prev;
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

			public bool TryRemoveFirst(out SimulationInput result)
			{
				if (Head == null)
				{
					result = null;
					return false;
				}
				SimulationInput prev = Head._node.Prev;
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

			public bool TryPeekFirst(out SimulationInput result)
			{
				result = Head;
				return Head != null;
			}

			public SimulationInput RemoveFirst()
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

			public bool Remove(SimulationInput item)
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

			public bool IsInListSlow(SimulationInput item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				SimulationInput simulationInput = Head;
				do
				{
					if (simulationInput == item)
					{
						return true;
					}
					simulationInput = simulationInput._node.Next;
				}
				while (simulationInput != Head);
				return false;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public SimulationInput Next(SimulationInput item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item._node.Next != Head) ? item._node.Next : null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public SimulationInput Prev(SimulationInput item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item != Head) ? item._node.Prev : null;
			}

			public SimulationInput[] ToArray()
			{
				SimulationInput[] array = new SimulationInput[_count];
				if (Head == null)
				{
					return Array.Empty<SimulationInput>();
				}
				Assert.Check(_count > 0, "_count > 0");
				ValidateNodeSet(Head);
				SimulationInput simulationInput = Head;
				int num = 0;
				do
				{
					array[num++] = simulationInput;
					simulationInput = simulationInput._node.Next;
				}
				while (simulationInput != Head);
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
					SimulationInput simulationInput = Head;
					do
					{
						array.SetValue(simulationInput, index++);
						simulationInput = simulationInput._node.Next;
					}
					while (simulationInput != Head);
				}
			}

			private void ValidateNodeSet(SimulationInput item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._node.Next != null, "item._node.Next != null");
				Assert.Check(item._node.Prev != null, "item._node.Prev != null");
			}

			private void ValidateNodeClear(SimulationInput item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._node.Next == null, "item._node.Next == null");
				Assert.Check(item._node.Prev == null, "item._node.Prev == null");
			}
		}

		private int _sent;

		private bool _delayed;

		private bool _pooled;

		private PlayerRef _player;

		internal unsafe int* _ptr;

		private IntrusiveLinkedListNode<SimulationInput> _node;

		internal bool Delayed
		{
			get
			{
				return _delayed;
			}
			set
			{
				_delayed = value;
			}
		}

		public PlayerRef Player
		{
			get
			{
				Assert.Check(!_pooled, "_pooled == false");
				return _player;
			}
			set
			{
				Assert.Check(!_pooled, "_pooled == false");
				_player = value;
			}
		}

		public unsafe ref SimulationInputHeader Header
		{
			get
			{
				Assert.Check(!_pooled, "_pooled == false");
				return ref *(SimulationInputHeader*)_ptr;
			}
		}

		public unsafe int* Data
		{
			get
			{
				Assert.Check(!_pooled, "_pooled == false");
				return _ptr + 4;
			}
		}

		public int Sent
		{
			get
			{
				Assert.Check(!_pooled, "_pooled == false");
				return _sent;
			}
			set
			{
				Assert.Check(!_pooled, "_pooled == false");
				_sent = value;
			}
		}

		public unsafe void Clear(int wordCount)
		{
			Assert.Check(!_pooled, "_pooled == false");
			FusionUnsafe.Clear(_ptr, wordCount * 4);
		}

		public unsafe void CopyFrom(SimulationInput source, int wordCount)
		{
			Assert.Check(!_pooled, "_pooled == false");
			FusionUnsafe.Copy(_ptr, source._ptr, wordCount * 4);
		}

		internal unsafe void Serialize(SimulationInput previous, SimulationConfig config, WriteBuffer writer, ReadBuffer reader)
		{
			Assert.Check(!_pooled, "_pooled == false");
			Assert.Always(config.Topology == Topologies.ClientServer, "config.Topology == Topologies.ClientServer");
			int* ptr = _ptr;
			int* ptr2 = previous._ptr;
			if (writer != null)
			{
				for (int i = 0; i < config.InputTotalWordCount; i++)
				{
					if (ptr[i] != ptr2[i])
					{
						writer.IntVar(i);
						writer.LongVar(Maths.ZigZagEncode((long)ptr[i] - (long)ptr2[i]));
					}
				}
				writer.IntVar(16383);
			}
			else
			{
				FusionUnsafe.Copy(ptr, ptr2, config.InputTotalWordCount * 4);
				for (int num = reader.IntVar(); num != 16383; num = reader.IntVar())
				{
					ptr[num] += (int)Maths.ZigZagDecode(reader.LongVar());
				}
			}
		}

		internal unsafe void PoolInit(Simulation simulation, SimulationConfig config)
		{
			if (_ptr == null)
			{
				_ptr = simulation.NativeAllocInputData(config.InputTotalWordCount);
			}
			Assert.Check(_sent == 0, "_sent == 0");
			Assert.Check(_player == PlayerRef.None, "_player == PlayerRef.None");
			_pooled = false;
		}

		internal void PoolReset(Simulation simulation)
		{
			Clear(simulation.Config.InputTotalWordCount);
			_sent = 0;
			_player = default(PlayerRef);
			_pooled = true;
		}
	}
}
