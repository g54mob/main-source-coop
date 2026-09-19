#define TRACE
#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Fusion.Analyzer;

namespace Fusion
{
	[GenerateIntrusiveLinkedList("_nodeAreaOfInterest")]
	[GenerateIntrusiveLinkedList("_nodeMigration")]
	public class NetworkObjectMeta
	{
		[DebuggerTypeProxy(typeof(DebuggerProxy))]
		internal struct AreaOfInterestList : IIntrusiveLinkedList<NetworkObjectMeta>, IIntrusiveLinkedList
		{
			private class DebuggerProxy
			{
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public readonly NetworkObjectMeta[] Items;

				public DebuggerProxy(AreaOfInterestList list)
				{
					Items = list.ToArray();
					base._002Ector();
				}
			}

			private int _count;

			public NetworkObjectMeta Head;

			public int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _count;
				}
			}

			public NetworkObjectMeta Tail
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (Head != null) ? Head._nodeAreaOfInterest.Prev : null;
				}
			}

			public bool IsEmpty => Head == null;

			public void AddFirst(NetworkObjectMeta item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._nodeAreaOfInterest.Next = (item._nodeAreaOfInterest.Prev = item));
				}
				else
				{
					NetworkObjectMeta prev = Head._nodeAreaOfInterest.Prev;
					ValidateNodeSet(prev);
					item._nodeAreaOfInterest.Next = Head;
					Head._nodeAreaOfInterest.Prev = item;
					prev._nodeAreaOfInterest.Next = item;
					item._nodeAreaOfInterest.Prev = prev;
					Head = item;
				}
				_count++;
				ValidateNodeSet(item);
				ValidateNodeSet(Head);
			}

			public void AddLast(NetworkObjectMeta item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._nodeAreaOfInterest.Next = (item._nodeAreaOfInterest.Prev = item));
				}
				else
				{
					NetworkObjectMeta prev = Head._nodeAreaOfInterest.Prev;
					ValidateNodeSet(prev);
					item._nodeAreaOfInterest.Next = Head;
					Head._nodeAreaOfInterest.Prev = item;
					prev._nodeAreaOfInterest.Next = item;
					item._nodeAreaOfInterest.Prev = prev;
				}
				_count++;
				ValidateNodeSet(item);
				ValidateNodeSet(Head);
			}

			public bool TryRemoveFirst(out NetworkObjectMeta result)
			{
				if (Head == null)
				{
					result = null;
					return false;
				}
				NetworkObjectMeta prev = Head._nodeAreaOfInterest.Prev;
				ValidateNodeSet(Head);
				ValidateNodeSet(prev);
				result = Head;
				if (prev == Head)
				{
					Assert.Check(prev == Head._nodeAreaOfInterest.Next, "tail == Head._nodeAreaOfInterest.Next");
					Head = null;
				}
				else
				{
					Head = Head._nodeAreaOfInterest.Next;
					prev._nodeAreaOfInterest.Next = Head;
					Head._nodeAreaOfInterest.Prev = prev;
				}
				result._nodeAreaOfInterest.Next = (result._nodeAreaOfInterest.Prev = null);
				_count--;
				ValidateNodeClear(result);
				return true;
			}

			public bool TryPeekFirst(out NetworkObjectMeta result)
			{
				result = Head;
				return Head != null;
			}

			public NetworkObjectMeta RemoveFirst()
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

			public bool Remove(NetworkObjectMeta item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				if (item._nodeAreaOfInterest.Prev == null || item._nodeAreaOfInterest.Next == null)
				{
					ValidateNodeClear(item);
					return false;
				}
				Assert.Check(IsInListSlow(item), "IsInListSlow(item)");
				if (_count == 1)
				{
					Assert.Check(Head == item, "Head == item");
					Assert.Check(Head == item._nodeAreaOfInterest.Prev, "Head == item._nodeAreaOfInterest.Prev");
					Assert.Check(Head == item._nodeAreaOfInterest.Next, "Head == item._nodeAreaOfInterest.Next");
					Head = null;
				}
				else
				{
					item._nodeAreaOfInterest.Prev._nodeAreaOfInterest.Next = item._nodeAreaOfInterest.Next;
					item._nodeAreaOfInterest.Next._nodeAreaOfInterest.Prev = item._nodeAreaOfInterest.Prev;
					if (item == Head)
					{
						Head = item._nodeAreaOfInterest.Next;
					}
				}
				item._nodeAreaOfInterest.Next = (item._nodeAreaOfInterest.Prev = null);
				_count--;
				return true;
			}

			public bool IsInListSlow(NetworkObjectMeta item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				NetworkObjectMeta networkObjectMeta = Head;
				do
				{
					if (networkObjectMeta == item)
					{
						return true;
					}
					networkObjectMeta = networkObjectMeta._nodeAreaOfInterest.Next;
				}
				while (networkObjectMeta != Head);
				return false;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NetworkObjectMeta Next(NetworkObjectMeta item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item._nodeAreaOfInterest.Next != Head) ? item._nodeAreaOfInterest.Next : null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NetworkObjectMeta Prev(NetworkObjectMeta item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item != Head) ? item._nodeAreaOfInterest.Prev : null;
			}

			public NetworkObjectMeta[] ToArray()
			{
				NetworkObjectMeta[] array = new NetworkObjectMeta[_count];
				if (Head == null)
				{
					return Array.Empty<NetworkObjectMeta>();
				}
				Assert.Check(_count > 0, "_count > 0");
				ValidateNodeSet(Head);
				NetworkObjectMeta networkObjectMeta = Head;
				int num = 0;
				do
				{
					array[num++] = networkObjectMeta;
					networkObjectMeta = networkObjectMeta._nodeAreaOfInterest.Next;
				}
				while (networkObjectMeta != Head);
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
					NetworkObjectMeta networkObjectMeta = Head;
					do
					{
						array.SetValue(networkObjectMeta, index++);
						networkObjectMeta = networkObjectMeta._nodeAreaOfInterest.Next;
					}
					while (networkObjectMeta != Head);
				}
			}

			private void ValidateNodeSet(NetworkObjectMeta item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._nodeAreaOfInterest.Next != null, "item._nodeAreaOfInterest.Next != null");
				Assert.Check(item._nodeAreaOfInterest.Prev != null, "item._nodeAreaOfInterest.Prev != null");
			}

			private void ValidateNodeClear(NetworkObjectMeta item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._nodeAreaOfInterest.Next == null, "item._nodeAreaOfInterest.Next == null");
				Assert.Check(item._nodeAreaOfInterest.Prev == null, "item._nodeAreaOfInterest.Prev == null");
			}
		}

		[DebuggerTypeProxy(typeof(DebuggerProxy))]
		internal struct MigrationList : IIntrusiveLinkedList<NetworkObjectMeta>, IIntrusiveLinkedList
		{
			private class DebuggerProxy
			{
				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public readonly NetworkObjectMeta[] Items;

				public DebuggerProxy(MigrationList list)
				{
					Items = list.ToArray();
					base._002Ector();
				}
			}

			private int _count;

			public NetworkObjectMeta Head;

			public int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return _count;
				}
			}

			public NetworkObjectMeta Tail
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (Head != null) ? Head._nodeMigration.Prev : null;
				}
			}

			public bool IsEmpty => Head == null;

			public void AddFirst(NetworkObjectMeta item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._nodeMigration.Next = (item._nodeMigration.Prev = item));
				}
				else
				{
					NetworkObjectMeta prev = Head._nodeMigration.Prev;
					ValidateNodeSet(prev);
					item._nodeMigration.Next = Head;
					Head._nodeMigration.Prev = item;
					prev._nodeMigration.Next = item;
					item._nodeMigration.Prev = prev;
					Head = item;
				}
				_count++;
				ValidateNodeSet(item);
				ValidateNodeSet(Head);
			}

			public void AddLast(NetworkObjectMeta item)
			{
				ValidateNodeClear(item);
				if (Head == null)
				{
					Head = (item._nodeMigration.Next = (item._nodeMigration.Prev = item));
				}
				else
				{
					NetworkObjectMeta prev = Head._nodeMigration.Prev;
					ValidateNodeSet(prev);
					item._nodeMigration.Next = Head;
					Head._nodeMigration.Prev = item;
					prev._nodeMigration.Next = item;
					item._nodeMigration.Prev = prev;
				}
				_count++;
				ValidateNodeSet(item);
				ValidateNodeSet(Head);
			}

			public bool TryRemoveFirst(out NetworkObjectMeta result)
			{
				if (Head == null)
				{
					result = null;
					return false;
				}
				NetworkObjectMeta prev = Head._nodeMigration.Prev;
				ValidateNodeSet(Head);
				ValidateNodeSet(prev);
				result = Head;
				if (prev == Head)
				{
					Assert.Check(prev == Head._nodeMigration.Next, "tail == Head._nodeMigration.Next");
					Head = null;
				}
				else
				{
					Head = Head._nodeMigration.Next;
					prev._nodeMigration.Next = Head;
					Head._nodeMigration.Prev = prev;
				}
				result._nodeMigration.Next = (result._nodeMigration.Prev = null);
				_count--;
				ValidateNodeClear(result);
				return true;
			}

			public bool TryPeekFirst(out NetworkObjectMeta result)
			{
				result = Head;
				return Head != null;
			}

			public NetworkObjectMeta RemoveFirst()
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

			public bool Remove(NetworkObjectMeta item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				if (item._nodeMigration.Prev == null || item._nodeMigration.Next == null)
				{
					ValidateNodeClear(item);
					return false;
				}
				Assert.Check(IsInListSlow(item), "IsInListSlow(item)");
				if (_count == 1)
				{
					Assert.Check(Head == item, "Head == item");
					Assert.Check(Head == item._nodeMigration.Prev, "Head == item._nodeMigration.Prev");
					Assert.Check(Head == item._nodeMigration.Next, "Head == item._nodeMigration.Next");
					Head = null;
				}
				else
				{
					item._nodeMigration.Prev._nodeMigration.Next = item._nodeMigration.Next;
					item._nodeMigration.Next._nodeMigration.Prev = item._nodeMigration.Prev;
					if (item == Head)
					{
						Head = item._nodeMigration.Next;
					}
				}
				item._nodeMigration.Next = (item._nodeMigration.Prev = null);
				_count--;
				return true;
			}

			public bool IsInListSlow(NetworkObjectMeta item)
			{
				if (item == null || Head == null)
				{
					return false;
				}
				NetworkObjectMeta networkObjectMeta = Head;
				do
				{
					if (networkObjectMeta == item)
					{
						return true;
					}
					networkObjectMeta = networkObjectMeta._nodeMigration.Next;
				}
				while (networkObjectMeta != Head);
				return false;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NetworkObjectMeta Next(NetworkObjectMeta item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item._nodeMigration.Next != Head) ? item._nodeMigration.Next : null;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NetworkObjectMeta Prev(NetworkObjectMeta item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return (item != Head) ? item._nodeMigration.Prev : null;
			}

			public NetworkObjectMeta[] ToArray()
			{
				NetworkObjectMeta[] array = new NetworkObjectMeta[_count];
				if (Head == null)
				{
					return Array.Empty<NetworkObjectMeta>();
				}
				Assert.Check(_count > 0, "_count > 0");
				ValidateNodeSet(Head);
				NetworkObjectMeta networkObjectMeta = Head;
				int num = 0;
				do
				{
					array[num++] = networkObjectMeta;
					networkObjectMeta = networkObjectMeta._nodeMigration.Next;
				}
				while (networkObjectMeta != Head);
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
					NetworkObjectMeta networkObjectMeta = Head;
					do
					{
						array.SetValue(networkObjectMeta, index++);
						networkObjectMeta = networkObjectMeta._nodeMigration.Next;
					}
					while (networkObjectMeta != Head);
				}
			}

			private void ValidateNodeSet(NetworkObjectMeta item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._nodeMigration.Next != null, "item._nodeMigration.Next != null");
				Assert.Check(item._nodeMigration.Prev != null, "item._nodeMigration.Prev != null");
			}

			private void ValidateNodeClear(NetworkObjectMeta item)
			{
				Assert.Check(item != null, "item != null");
				Assert.Check(item._nodeMigration.Next == null, "item._nodeMigration.Next == null");
				Assert.Check(item._nodeMigration.Prev == null, "item._nodeMigration.Prev == null");
			}
		}

		private Simulation _simulation;

		private unsafe ChangeTick* _changes;

		private NetworkObjectHeaderSnapshot _shadow;

		private NetworkObjectHeaderSnapshot _renderOrReceived;

		private NetworkObjectHeaderSnapshot _previous;

		private NetworkObjectHeaderSnapshot _migration;

		private NetworkObjectHeaderSnapshotList _snapshots;

		private NetworkObjectHeaderSnapshot[] _snapshotsByIndex;

		private Tick? _snapshotsByIndexLatest;

		private Timeline _timeline;

		internal InterpolationParams LocalInterpolationParams;

		internal InterpolationParams RemoteInterpolationParams;

		internal double LastGlobalRemoteTime;

		private IntrusiveLinkedListNode<NetworkObjectMeta> _nodeAreaOfInterest;

		private IntrusiveLinkedListNode<NetworkObjectMeta> _nodeMigration;

		internal Tick? InitialTick;

		internal Tick ScannedTick;

		internal ChangeTick ChangedTick;

		internal int AreaOfInterestCell;

		internal NetworkObjectMetaFlags LocalFlags;

		internal NetworkObjectHeaderPlayerDataFlags PlayerFlags;

		private unsafe int* _ptr;

		internal NetworkObject Instance;

		internal short WordCount;

		internal short BehaviourCount;

		internal NetworkObjectTypeDescriptor TypeDescriptor;

		internal const int PRIORITY_IDLE = -32768;

		internal const int PRIORITY_LEVEL_PLAYER = 0;

		internal const int PRIORITY_LEVEL_HIGH = 1;

		internal const int PRIORITY_LEVEL_MED = 2;

		internal const int PRIORITY_LEVEL_LOW = 3;

		internal const int PRIORITY_LEVEL_LOWEST = 4;

		internal const int PRIORITY_LEVEL_COUNT = 5;

		internal bool IsInAreaOfInterestList => _nodeAreaOfInterest != default(IntrusiveLinkedListNode<NetworkObjectMeta>);

		internal bool IsInMigrationList => _nodeMigration != default(IntrusiveLinkedListNode<NetworkObjectMeta>);

		internal Timeline Timeline
		{
			get
			{
				Assert.Check(_simulation.HasRuntimeConfig, "_simulation.HasRuntimeConfig");
				if (_timeline == null)
				{
					int capacity = ((!Flags.Has(NetworkObjectHeaderFlags.EnableInterpolation)) ? 1 : _simulation.TickRate);
					_timeline = new Timeline(capacity);
				}
				return _timeline;
			}
		}

		internal unsafe ref NetworkObjectHeader Header
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref *(NetworkObjectHeader*)_ptr;
			}
		}

		internal bool HasNetworkedState
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (Flags.Has(NetworkObjectHeaderFlags.HasMainNetworkTRSP))
				{
					return true;
				}
				return WordCount > 34 + BehaviourCount;
			}
		}

		internal unsafe bool IsValid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _ptr != null;
			}
		}

		internal bool HasMainTRSP
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return IsValid && Flags.Has(NetworkObjectHeaderFlags.HasMainNetworkTRSP) && WordCount >= 34;
			}
		}

		internal unsafe ref NetworkTRSPData MainTRSPData
		{
			get
			{
				Assert.Check(HasMainTRSP, "HasMainTRSP");
				return ref *(NetworkTRSPData*)(_ptr + 20);
			}
		}

		internal unsafe Span<int> Raw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Span<int>(_ptr, WordCount);
			}
		}

		internal unsafe int* RawPtr
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _ptr;
			}
		}

		internal Span<int> Data
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				Span<int> raw = Raw;
				return raw.Slice(20, raw.Length - 20);
			}
		}

		internal Span<int> BehaviourChangedTickArray
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				Span<int> raw = Raw;
				int num = WordCount - BehaviourCount;
				return raw.Slice(num, raw.Length - num);
			}
		}

		internal bool HasSnapshots
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _snapshots.Latest != null;
			}
		}

		internal NetworkObjectHeaderSnapshotRef SnapshotLatest
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new NetworkObjectHeaderSnapshotRef(_snapshots.Latest);
			}
		}

		internal bool IsStruct
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Flags.Has(NetworkObjectHeaderFlags.Struct);
			}
		}

		internal bool IsObject
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Flags.HasNot(NetworkObjectHeaderFlags.Struct);
			}
		}

		public NetworkObjectTypeId Type
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Header.Type;
			}
		}

		public NetworkId Id
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Header.Id;
			}
		}

		public NetworkId NestingRoot
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Header.NestingRoot;
			}
		}

		public NetworkObjectNestingKey NestingKey
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Header.NestingKey;
			}
		}

		public NetworkObjectHeaderFlags Flags
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Header.Flags;
			}
		}

		internal PlayerRef StateAuthority
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Header.StateAuthority;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[Obsolete("Don't call the setter on this, call Simulation.SetStateAuthority() instead")]
			set
			{
				Header.StateAuthority = value;
			}
		}

		public ref PlayerRef InputAuthority
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref Header.InputAuthority;
			}
		}

		internal NetworkObjectHeaderSnapshotRef Shadow
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _shadow ?? (_shadow = GetFirstShadowSnapshot());
			}
		}

		internal NetworkObjectHeaderSnapshotRef Render
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _renderOrReceived ?? (_renderOrReceived = GetSnapshot(copyState: false));
			}
		}

		internal NetworkObjectHeaderSnapshotRef Previous
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _previous ?? (_previous = GetSnapshot(copyState: true));
			}
		}

		internal NetworkObjectHeaderSnapshotRef Migration
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _migration ?? (_migration = GetSnapshot(copyState: false));
			}
		}

		internal unsafe ChangeTick* ChangesRaw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (_changes == null)
				{
					_changes = _simulation.NativeAllocObjectChanges(WordCount);
				}
				return _changes;
			}
		}

		internal unsafe Span<ChangeTick> Changes
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Span<ChangeTick>(ChangesRaw, WordCount);
			}
		}

		public override string ToString()
		{
			return $"[meta = id:{Id}, wordCount:{WordCount}]";
		}

		internal void SetChangedTick(ChangeTick tick, [CallerMemberName] string member = null)
		{
			InternalLogStreams.LogTraceSendRecv?.Log(_simulation, $"[ChangedTick] {Simulation.StateReplicator.LogPrefix(_simulation, Id)} from {member}: {ChangedTick} -> {tick}");
			ChangedTick = tick;
		}

		internal Span<int> GetBehaviourChangedTickArray(NetworkObjectHeaderSnapshotRef snapshot)
		{
			Span<int> raw = snapshot.Raw;
			int num = WordCount - BehaviourCount;
			return raw.Slice(num, raw.Length - num);
		}

		internal Tick GetMaxBehaviourChangedTick()
		{
			Tick tick = 0;
			Span<int> behaviourChangedTickArray = BehaviourChangedTickArray;
			for (int i = 0; i < behaviourChangedTickArray.Length; i++)
			{
				Tick tick2 = behaviourChangedTickArray[i];
				tick = Math.Max(tick, tick2);
			}
			return tick;
		}

		internal Tick GetMaxBehaviourChangedTick(NetworkObjectHeaderSnapshotRef snapshot)
		{
			Tick tick = 0;
			Span<int> behaviourChangedTickArray = GetBehaviourChangedTickArray(snapshot);
			for (int i = 0; i < behaviourChangedTickArray.Length; i++)
			{
				Tick tick2 = behaviourChangedTickArray[i];
				tick = Math.Max(tick, tick2);
			}
			return tick;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe ref T GetStructData<T>() where T : unmanaged
		{
			return ref *(T*)(_ptr + 20 + 14);
		}

		private NetworkObjectHeaderSnapshot GetFirstShadowSnapshot()
		{
			NetworkObjectHeaderSnapshot snapshot = GetSnapshot(copyState: false);
			NetworkObjectHeaderSnapshotRef networkObjectHeaderSnapshotRef = new NetworkObjectHeaderSnapshotRef(snapshot);
			networkObjectHeaderSnapshotRef.Header.StateAuthority = PlayerRef.Invalid;
			networkObjectHeaderSnapshotRef.Header.Id = Id;
			return snapshot;
		}

		internal unsafe void PoolInit(Simulation simulation, in NetworkObjectHeader header)
		{
			Assert.Check(_ptr == null, "_ptr == null");
			Assert.Check(BehaviourUtils.IsNull(Instance), "BehaviourUtils.IsNull(Instance)");
			Assert.Always(header.WordCount >= 20, "header.WordCount >= NetworkObjectHeader.WORDS");
			Assert.Check(_shadow == null, "_shadow == null");
			Assert.Check(_renderOrReceived == null, "_renderOrReceived == null");
			Assert.Check(_previous == null, "_previous == null");
			Assert.Check(_migration == null, "_migration == null");
			Assert.Check(_changes == null, "_changes == null");
			_simulation = simulation;
			WordCount = header.WordCount;
			BehaviourCount = header.BehaviourCount;
			_ptr = simulation.NativeAllocObjectData(header.WordCount);
			*(NetworkObjectHeader*)_ptr = header;
		}

		internal unsafe void PoolReset(Simulation simulation)
		{
			Assert.Check(!IsInAreaOfInterestList, "Still in AOI list {0} {1}", Id, _nodeAreaOfInterest);
			Assert.Check(!IsInMigrationList, "Still in Migration list {0} {1}", Id, _nodeMigration);
			Header = default(NetworkObjectHeader);
			simulation.NativeFreeObjectData(ref _ptr, WordCount);
			if ((bool)Instance)
			{
				UnlinkInstance(Instance);
			}
			Instance = null;
			PlayerFlags = (NetworkObjectHeaderPlayerDataFlags)0;
			AreaOfInterestCell = 0;
			LocalFlags = NetworkObjectMetaFlags.None;
			InitialTick = null;
			ScannedTick = default(Tick);
			ChangedTick = default(ChangeTick);
			TypeDescriptor = null;
			_simulation.Release(ref _shadow);
			_simulation.Release(ref _renderOrReceived);
			_simulation.Release(ref _previous);
			_simulation.Release(ref _migration);
			simulation.NativeFreeObjectChanges(ref _changes, WordCount);
			if (_snapshotsByIndex != null)
			{
				Array.Clear(_snapshotsByIndex, 0, _snapshotsByIndex.Length);
			}
			while (_snapshots.Count > 0)
			{
				NetworkObjectHeaderSnapshot obj = _snapshots.RemoveLatest();
				_simulation.Release(ref obj);
			}
			_snapshots = default(NetworkObjectHeaderSnapshotList);
			_timeline?.Clear();
		}

		internal void ResetMigrationSnapshot()
		{
			_simulation.Release(ref _migration);
		}

		private NetworkObjectHeaderSnapshot GetSnapshot(bool copyState)
		{
			NetworkObjectHeaderSnapshot networkObjectHeaderSnapshot = _simulation.AcquireNetworkObjectHeaderSnapshot(WordCount);
			if (copyState)
			{
				networkObjectHeaderSnapshot.CopyFrom(this);
			}
			return networkObjectHeaderSnapshot;
		}

		internal NetworkObjectHeaderSnapshotRef NextSnapshot(Tick tick, bool copyInitialState = true)
		{
			if (_snapshots.Count == 0)
			{
				_snapshots.AddFirst(GetSnapshot(copyInitialState));
			}
			else if (_simulation.HasRuntimeConfig && _snapshots.Count >= _simulation.TickRate)
			{
				NetworkObjectHeaderSnapshot networkObjectHeaderSnapshot = _snapshots.RemoveOldest();
				networkObjectHeaderSnapshot.CopyFrom(_snapshots.Latest);
				_snapshots.AddFirst(networkObjectHeaderSnapshot);
			}
			else
			{
				_snapshots.AddFirst(_snapshots.Latest.Clone(_simulation));
			}
			_snapshots.Latest.Tick = tick;
			if (_snapshotsByIndex == null)
			{
				_snapshotsByIndex = new NetworkObjectHeaderSnapshot[64];
			}
			Array.Clear(_snapshotsByIndex, 0, _snapshotsByIndex.Length);
			_snapshotsByIndexLatest = _snapshots.Latest.Tick;
			NetworkObjectHeaderSnapshot networkObjectHeaderSnapshot2 = _snapshots.Latest;
			while (networkObjectHeaderSnapshot2 != null)
			{
				int num = (int)_snapshotsByIndexLatest.Value - (int)networkObjectHeaderSnapshot2.Tick;
				if (num >= _snapshotsByIndex.Length)
				{
					break;
				}
				if (num < 0)
				{
					NetworkObjectHeaderSnapshot obj = networkObjectHeaderSnapshot2;
					networkObjectHeaderSnapshot2 = networkObjectHeaderSnapshot2.Next;
					_snapshots.Remove(obj);
					_simulation.Release(ref obj);
				}
				else
				{
					_snapshotsByIndex[num] = networkObjectHeaderSnapshot2;
					networkObjectHeaderSnapshot2 = networkObjectHeaderSnapshot2.Next;
				}
			}
			return new NetworkObjectHeaderSnapshotRef(_snapshots.Latest);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void AddLatestSnapshotToTimeline()
		{
			if (_simulation.IsClient && _simulation.HasRuntimeConfig)
			{
				AddLatestSnapshotToTimelineFast(_simulation.TickDeltaDouble);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void AddLatestSnapshotToTimelineFast(double tickDelta)
		{
			if (HasSnapshots)
			{
				Tick tick = SnapshotLatest.Tick;
				Tick maxBehaviourChangedTick = GetMaxBehaviourChangedTick(SnapshotLatest);
				Timeline.AddPoint(new TimelinePoint(tick, maxBehaviourChangedTick, tickDelta), tickDelta);
			}
		}

		internal NetworkObjectHeaderSnapshot FindSnapshot(Tick tick)
		{
			if (_snapshotsByIndexLatest.HasValue)
			{
				Assert.Check<NetworkObjectHeaderSnapshot>(_snapshotsByIndex, "_snapshotsByIndex");
				int num = (int)_snapshotsByIndexLatest.Value - (int)tick;
				if ((uint)num < (uint)_snapshotsByIndex.Length)
				{
					return _snapshotsByIndex[num];
				}
			}
			for (NetworkObjectHeaderSnapshot networkObjectHeaderSnapshot = _snapshots.Latest; networkObjectHeaderSnapshot != null; networkObjectHeaderSnapshot = networkObjectHeaderSnapshot.Next)
			{
				if (networkObjectHeaderSnapshot.Tick == tick)
				{
					return networkObjectHeaderSnapshot;
				}
			}
			return null;
		}

		internal bool TryFindSnapshot(Tick tick, out NetworkObjectHeaderSnapshot snapshot)
		{
			return (snapshot = FindSnapshot(tick)) != null;
		}

		internal unsafe int* GetBehaviourPtr(NetworkBehaviour behaviour)
		{
			Assert.Always(behaviour != null && behaviour.WordOffset < WordCount, "behaviour != null && behaviour.WordOffset < WordCount");
			return (_ptr != null) ? (_ptr + behaviour.WordOffset) : null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int EncodePriorityLevel(int level)
		{
			return Maths.Clamp(level, 0, 4) + 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int DecodePriorityLevel(int level)
		{
			return level - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsIdle(int level)
		{
			return level < -1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsActive(int level)
		{
			return level >= 1 && level <= 5;
		}

		internal unsafe void LinkInstance(NetworkObject instance)
		{
			Assert.Check(BehaviourUtils.IsNotNull(instance), "IsNotNull(instance)");
			Assert.Check(BehaviourUtils.IsNull(Instance), "IsNull(Instance)");
			Assert.Check(instance.Ptr == null, "instance.Ptr == null");
			Assert.Check(_ptr != null, "_ptr != null");
			instance.Ptr = _ptr;
			instance.Meta = this;
			Instance = instance;
		}

		internal unsafe void UnlinkInstance(NetworkObject instance)
		{
			Assert.Check((object)Instance == instance, "ReferenceEquals(Instance, instance)");
			Assert.Check(this == instance.Meta, "ReferenceEquals(this, instance.Meta)");
			Instance = null;
			instance.Ptr = default(int*);
			instance.Meta = null;
		}
	}
}
