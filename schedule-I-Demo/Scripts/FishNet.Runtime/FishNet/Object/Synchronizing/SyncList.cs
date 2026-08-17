using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Documenting;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;

namespace FishNet.Object.Synchronizing
{
	public class SyncList<T> : SyncBase, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyList<T>, IReadOnlyCollection<T>
	{
		private struct CachedOnChange
		{
			internal readonly SyncListOperation Operation;

			internal readonly int Index;

			internal readonly T Previous;

			internal readonly T Next;

			public CachedOnChange(SyncListOperation operation, int index, T previous, T next)
			{
				Operation = operation;
				Index = index;
				Previous = previous;
				Next = next;
			}
		}

		private struct ChangeData
		{
			internal readonly SyncListOperation Operation;

			internal readonly int Index;

			internal readonly T Item;

			public ChangeData(SyncListOperation operation, int index, T item)
			{
				Operation = operation;
				Index = index;
				Item = item;
			}
		}

		[APIExclude]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private readonly SyncList<T> _list;

			private int _index;

			public T Current { get; private set; }

			object IEnumerator.Current => Current;

			public Enumerator(SyncList<T> list)
			{
				_list = list;
				_index = -1;
				Current = default(T);
			}

			public bool MoveNext()
			{
				_index++;
				if (_index >= _list.Count)
				{
					return false;
				}
				Current = _list[_index];
				return true;
			}

			public void Reset()
			{
				_index = -1;
			}

			public void Dispose()
			{
			}
		}

		[APIExclude]
		public delegate void SyncListChanged(SyncListOperation op, int index, T oldItem, T newItem, bool asServer);

		public readonly IList<T> Collection;

		public readonly IList<T> ClientHostCollection = new List<T>();

		private IList<T> _initialValues = new List<T>();

		private readonly IEqualityComparer<T> _comparer;

		private readonly List<ChangeData> _changed = new List<ChangeData>();

		private readonly List<CachedOnChange> _serverOnChanges = new List<CachedOnChange>();

		private readonly List<CachedOnChange> _clientOnChanges = new List<CachedOnChange>();

		private bool _valuesChanged;

		private bool _sendAll;

		[APIExclude]
		public bool IsReadOnly => false;

		public int Count => Collection.Count;

		public T this[int i]
		{
			get
			{
				return Collection[i];
			}
			set
			{
				Set(i, value, asServer: true, force: true);
			}
		}

		public event SyncListChanged OnChange;

		[APIExclude]
		public SyncList()
			: this((IList<T>)new List<T>(), (IEqualityComparer<T>)EqualityComparer<T>.Default)
		{
		}

		[APIExclude]
		public SyncList(IEqualityComparer<T> comparer)
			: this((IList<T>)new List<T>(), (comparer == null) ? EqualityComparer<T>.Default : comparer)
		{
		}

		[APIExclude]
		public SyncList(IList<T> collection, IEqualityComparer<T> comparer = null)
		{
			IEqualityComparer<T> comparer2;
			if (comparer != null)
			{
				comparer2 = comparer;
			}
			else
			{
				IEqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
				comparer2 = equalityComparer;
			}
			_comparer = comparer2;
			Collection = collection;
			foreach (T item in collection)
			{
				ClientHostCollection.Add(item);
			}
		}

		protected override void Registered()
		{
			base.Registered();
			foreach (T item in Collection)
			{
				_initialValues.Add(item);
			}
		}

		public List<T> GetCollection(bool asServer)
		{
			return ((!asServer && NetworkManager.IsServer) ? ClientHostCollection : Collection) as List<T>;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddOperation(SyncListOperation operation, int index, T prev, T next)
		{
			if (!base.IsRegistered)
			{
				return;
			}
			bool flag = !base.IsNetworkInitialized || NetworkBehaviour.IsServer;
			if (flag)
			{
				_valuesChanged = true;
				if (Dirty())
				{
					ChangeData item = new ChangeData(operation, index, next);
					_changed.Add(item);
				}
			}
			InvokeOnChange(operation, index, prev, next, flag);
		}

		public override void OnStartCallback(bool asServer)
		{
			base.OnStartCallback(asServer);
			List<CachedOnChange> list = (asServer ? _serverOnChanges : _clientOnChanges);
			if (this.OnChange != null)
			{
				foreach (CachedOnChange item in list)
				{
					this.OnChange(item.Operation, item.Index, item.Previous, item.Next, asServer);
				}
			}
			list.Clear();
		}

		public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			if (_sendAll)
			{
				_sendAll = false;
				_changed.Clear();
				WriteFull(writer);
				return;
			}
			base.WriteDelta(writer, resetSyncTick);
			writer.WriteBoolean(value: false);
			writer.WriteInt32(_changed.Count);
			for (int i = 0; i < _changed.Count; i++)
			{
				ChangeData changeData = _changed[i];
				writer.WriteByte((byte)changeData.Operation);
				if (changeData.Operation == SyncListOperation.Add)
				{
					writer.Write(changeData.Item);
				}
				else if (changeData.Operation == SyncListOperation.RemoveAt)
				{
					writer.WriteInt32(changeData.Index);
				}
				else if (changeData.Operation == SyncListOperation.Insert || changeData.Operation == SyncListOperation.Set)
				{
					writer.WriteInt32(changeData.Index);
					writer.Write(changeData.Item);
				}
			}
			_changed.Clear();
		}

		public override void WriteFull(PooledWriter writer)
		{
			if (_valuesChanged)
			{
				base.WriteHeader(writer, resetSyncTick: false);
				writer.WriteBoolean(value: true);
				writer.WriteInt32(Collection.Count);
				for (int i = 0; i < Collection.Count; i++)
				{
					writer.WriteByte(0);
					writer.Write(Collection[i]);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		public override void Read(PooledReader reader, bool asServer)
		{
			int num;
			int num2;
			if (!asServer)
			{
				num = (NetworkManager.IsServer ? 1 : 0);
				if (num != 0)
				{
					num2 = ((!base.OnStartServerCalled) ? 1 : 0);
					goto IL_0020;
				}
			}
			else
			{
				num = 0;
			}
			num2 = 0;
			goto IL_0020;
			IL_0020:
			bool flag = (byte)num2 != 0;
			if (flag)
			{
				NetworkManager.LogWarning("SyncType " + GetType().Name + " received a Read but was deinitialized on the server. Client callback values may be incorrect. This is a ClientHost limitation.");
			}
			IList<T> list = ((num != 0) ? ClientHostCollection : Collection);
			if (reader.ReadBoolean())
			{
				list.Clear();
			}
			int num3 = reader.ReadInt32();
			for (int i = 0; i < num3; i++)
			{
				SyncListOperation syncListOperation = (SyncListOperation)reader.ReadByte();
				int index = -1;
				T prev = default(T);
				T val = default(T);
				switch (syncListOperation)
				{
				case SyncListOperation.Add:
					val = reader.Read<T>();
					if (!flag)
					{
						index = list.Count;
						list.Add(val);
					}
					break;
				case SyncListOperation.Clear:
					if (!flag)
					{
						list.Clear();
					}
					break;
				case SyncListOperation.Insert:
					index = reader.ReadInt32();
					val = reader.Read<T>();
					if (!flag)
					{
						list.Insert(index, val);
					}
					break;
				case SyncListOperation.RemoveAt:
					index = reader.ReadInt32();
					if (!flag)
					{
						prev = list[index];
						list.RemoveAt(index);
					}
					break;
				case SyncListOperation.Set:
					index = reader.ReadInt32();
					val = reader.Read<T>();
					if (!flag)
					{
						prev = list[index];
						list[index] = val;
					}
					break;
				}
				InvokeOnChange(syncListOperation, index, prev, val, asServer: false);
			}
			if (num3 > 0)
			{
				InvokeOnChange(SyncListOperation.Complete, -1, default(T), default(T), asServer: false);
			}
		}

		private void InvokeOnChange(SyncListOperation operation, int index, T prev, T next, bool asServer)
		{
			if (asServer)
			{
				if (NetworkBehaviour.OnStartServerCalled)
				{
					this.OnChange?.Invoke(operation, index, prev, next, asServer);
				}
				else
				{
					_serverOnChanges.Add(new CachedOnChange(operation, index, prev, next));
				}
			}
			else if (NetworkBehaviour.OnStartClientCalled)
			{
				this.OnChange?.Invoke(operation, index, prev, next, asServer);
			}
			else
			{
				_clientOnChanges.Add(new CachedOnChange(operation, index, prev, next));
			}
		}

		public override void ResetState()
		{
			base.ResetState();
			_sendAll = false;
			_changed.Clear();
			ClientHostCollection.Clear();
			Collection.Clear();
			foreach (T initialValue in _initialValues)
			{
				Collection.Add(initialValue);
				ClientHostCollection.Add(initialValue);
			}
		}

		public void Add(T item)
		{
			Add(item, asServer: true);
		}

		private void Add(T item, bool asServer)
		{
			if (!CanNetworkSetValues())
			{
				return;
			}
			Collection.Add(item);
			if (asServer)
			{
				if (NetworkManager == null)
				{
					ClientHostCollection.Add(item);
				}
				AddOperation(SyncListOperation.Add, Collection.Count - 1, default(T), item);
			}
		}

		public void AddRange(IEnumerable<T> range)
		{
			foreach (T item in range)
			{
				Add(item, asServer: true);
			}
		}

		public void Clear()
		{
			Clear(asServer: true);
		}

		private void Clear(bool asServer)
		{
			if (!CanNetworkSetValues())
			{
				return;
			}
			Collection.Clear();
			if (asServer)
			{
				if (NetworkManager == null)
				{
					ClientHostCollection.Clear();
				}
				AddOperation(SyncListOperation.Clear, -1, default(T), default(T));
			}
		}

		public bool Contains(T item)
		{
			return IndexOf(item) >= 0;
		}

		public void CopyTo(T[] array, int index)
		{
			Collection.CopyTo(array, index);
		}

		public int IndexOf(T item)
		{
			for (int i = 0; i < Collection.Count; i++)
			{
				if (_comparer.Equals(item, Collection[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public int FindIndex(Predicate<T> match)
		{
			for (int i = 0; i < Collection.Count; i++)
			{
				if (match(Collection[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public T Find(Predicate<T> match)
		{
			int num = FindIndex(match);
			if (num == -1)
			{
				return default(T);
			}
			return Collection[num];
		}

		public List<T> FindAll(Predicate<T> match)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < Collection.Count; i++)
			{
				if (match(Collection[i]))
				{
					list.Add(Collection[i]);
				}
			}
			return list;
		}

		public void Insert(int index, T item)
		{
			Insert(index, item, asServer: true);
		}

		private void Insert(int index, T item, bool asServer)
		{
			if (!CanNetworkSetValues())
			{
				return;
			}
			Collection.Insert(index, item);
			if (asServer)
			{
				if (NetworkManager == null)
				{
					ClientHostCollection.Insert(index, item);
				}
				AddOperation(SyncListOperation.Insert, index, default(T), item);
			}
		}

		public void InsertRange(int index, IEnumerable<T> range)
		{
			foreach (T item in range)
			{
				Insert(index, item);
				index++;
			}
		}

		public bool Remove(T item)
		{
			int num = IndexOf(item);
			bool num2 = num >= 0;
			if (num2)
			{
				RemoveAt(num);
			}
			return num2;
		}

		public void RemoveAt(int index)
		{
			RemoveAt(index, asServer: true);
		}

		private void RemoveAt(int index, bool asServer)
		{
			if (!CanNetworkSetValues())
			{
				return;
			}
			T prev = Collection[index];
			Collection.RemoveAt(index);
			if (asServer)
			{
				if (NetworkManager == null)
				{
					ClientHostCollection.RemoveAt(index);
				}
				AddOperation(SyncListOperation.RemoveAt, index, prev, default(T));
			}
		}

		public int RemoveAll(Predicate<T> match)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < Collection.Count; i++)
			{
				if (match(Collection[i]))
				{
					list.Add(Collection[i]);
				}
			}
			foreach (T item in list)
			{
				Remove(item);
			}
			return list.Count;
		}

		public void DirtyAll()
		{
			if (base.IsRegistered)
			{
				if (NetworkManager != null && !NetworkBehaviour.IsServer)
				{
					NetworkManager.LogWarning("Cannot complete operation as server when server is not active.");
				}
				else if (Dirty())
				{
					_sendAll = true;
				}
			}
		}

		public void Dirty(T obj)
		{
			int num = Collection.IndexOf(obj);
			if (num != -1)
			{
				Dirty(num);
			}
			else
			{
				NetworkManager.LogError("Could not find object within SyncList, dirty will not be set.");
			}
		}

		public void Dirty(int index)
		{
			if (CanNetworkSetValues())
			{
				T val = Collection[index];
				if (true)
				{
					AddOperation(SyncListOperation.Set, index, val, val);
				}
			}
		}

		public void Set(int index, T value, bool force = true)
		{
			Set(index, value, asServer: true, force);
		}

		private void Set(int index, T value, bool asServer, bool force)
		{
			if (!CanNetworkSetValues() || (!force && !_comparer.Equals(Collection[index], value)))
			{
				return;
			}
			T prev = Collection[index];
			Collection[index] = value;
			if (asServer)
			{
				if (NetworkManager == null)
				{
					ClientHostCollection[index] = value;
				}
				AddOperation(SyncListOperation.Set, index, prev, value);
			}
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		[APIExclude]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		[APIExclude]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}
	}
}
