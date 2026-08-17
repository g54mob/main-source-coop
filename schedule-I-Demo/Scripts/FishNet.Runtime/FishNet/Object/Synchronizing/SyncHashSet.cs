using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Documenting;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;

namespace FishNet.Object.Synchronizing
{
	public class SyncHashSet<T> : SyncBase, ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		private struct CachedOnChange
		{
			internal readonly SyncHashSetOperation Operation;

			internal readonly T Item;

			public CachedOnChange(SyncHashSetOperation operation, T item)
			{
				Operation = operation;
				Item = item;
			}
		}

		private struct ChangeData
		{
			internal readonly SyncHashSetOperation Operation;

			internal readonly T Item;

			public ChangeData(SyncHashSetOperation operation, T item)
			{
				Operation = operation;
				Item = item;
			}
		}

		[APIExclude]
		public delegate void SyncHashSetChanged(SyncHashSetOperation op, T item, bool asServer);

		public readonly ISet<T> Collection;

		public readonly ISet<T> ClientHostCollection = new HashSet<T>();

		private static List<T> _cache = new List<T>();

		private ISet<T> _initialValues = new HashSet<T>();

		private readonly IEqualityComparer<T> _comparer;

		private readonly List<ChangeData> _changed = new List<ChangeData>();

		private readonly List<CachedOnChange> _serverOnChanges = new List<CachedOnChange>();

		private readonly List<CachedOnChange> _clientOnChanges = new List<CachedOnChange>();

		private bool _valuesChanged;

		private bool _sendAll;

		[APIExclude]
		public bool IsReadOnly => false;

		public int Count => Collection.Count;

		public event SyncHashSetChanged OnChange;

		[APIExclude]
		public SyncHashSet()
			: this((ISet<T>)new HashSet<T>(), (IEqualityComparer<T>)EqualityComparer<T>.Default)
		{
		}

		[APIExclude]
		public SyncHashSet(IEqualityComparer<T> comparer)
			: this((ISet<T>)new HashSet<T>(), (comparer == null) ? EqualityComparer<T>.Default : comparer)
		{
		}

		[APIExclude]
		public SyncHashSet(ISet<T> collection, IEqualityComparer<T> comparer = null)
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

		public HashSet<T> GetCollection(bool asServer)
		{
			return ((!asServer && NetworkManager.IsServer) ? ClientHostCollection : Collection) as HashSet<T>;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddOperation(SyncHashSetOperation operation, T item)
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
					ChangeData item2 = new ChangeData(operation, item);
					_changed.Add(item2);
				}
			}
			InvokeOnChange(operation, item, flag);
		}

		public override void OnStartCallback(bool asServer)
		{
			base.OnStartCallback(asServer);
			List<CachedOnChange> list = (asServer ? _serverOnChanges : _clientOnChanges);
			if (this.OnChange != null)
			{
				foreach (CachedOnChange item in list)
				{
					this.OnChange(item.Operation, item.Item, asServer);
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
				if (changeData.Operation == SyncHashSetOperation.Add || changeData.Operation == SyncHashSetOperation.Remove || changeData.Operation == SyncHashSetOperation.Update)
				{
					writer.Write(changeData.Item);
				}
			}
			_changed.Clear();
		}

		public override void WriteFull(PooledWriter writer)
		{
			if (!_valuesChanged)
			{
				return;
			}
			base.WriteHeader(writer, resetSyncTick: false);
			writer.WriteBoolean(value: true);
			int count = Collection.Count;
			writer.WriteInt32(count);
			foreach (T item in Collection)
			{
				writer.WriteByte(0);
				writer.Write(item);
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
			ISet<T> set = ((num != 0) ? ClientHostCollection : Collection);
			if (reader.ReadBoolean())
			{
				set.Clear();
			}
			int num3 = reader.ReadInt32();
			for (int i = 0; i < num3; i++)
			{
				SyncHashSetOperation syncHashSetOperation = (SyncHashSetOperation)reader.ReadByte();
				T item = default(T);
				switch (syncHashSetOperation)
				{
				case SyncHashSetOperation.Add:
					item = reader.Read<T>();
					if (!flag)
					{
						set.Add(item);
					}
					break;
				case SyncHashSetOperation.Clear:
					if (!flag)
					{
						set.Clear();
					}
					break;
				case SyncHashSetOperation.Remove:
					item = reader.Read<T>();
					if (!flag)
					{
						set.Remove(item);
					}
					break;
				case SyncHashSetOperation.Update:
					item = reader.Read<T>();
					if (!flag)
					{
						set.Remove(item);
						set.Add(item);
					}
					break;
				}
				InvokeOnChange(syncHashSetOperation, item, asServer: false);
			}
			if (num3 > 0)
			{
				InvokeOnChange(SyncHashSetOperation.Complete, default(T), asServer: false);
			}
		}

		private void InvokeOnChange(SyncHashSetOperation operation, T item, bool asServer)
		{
			if (asServer)
			{
				if (NetworkBehaviour.OnStartServerCalled)
				{
					this.OnChange?.Invoke(operation, item, asServer);
				}
				else
				{
					_serverOnChanges.Add(new CachedOnChange(operation, item));
				}
			}
			else if (NetworkBehaviour.OnStartClientCalled)
			{
				this.OnChange?.Invoke(operation, item, asServer);
			}
			else
			{
				_clientOnChanges.Add(new CachedOnChange(operation, item));
			}
		}

		public override void ResetState()
		{
			base.ResetState();
			_sendAll = false;
			_changed.Clear();
			Collection.Clear();
			ClientHostCollection.Clear();
			foreach (T initialValue in _initialValues)
			{
				Collection.Add(initialValue);
				ClientHostCollection.Add(initialValue);
			}
		}

		public bool Add(T item)
		{
			return Add(item, asServer: true);
		}

		private bool Add(T item, bool asServer)
		{
			if (!CanNetworkSetValues())
			{
				return false;
			}
			bool num = Collection.Add(item);
			if (num && asServer)
			{
				if (NetworkManager == null)
				{
					ClientHostCollection.Add(item);
				}
				AddOperation(SyncHashSetOperation.Add, item);
			}
			return num;
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
				AddOperation(SyncHashSetOperation.Clear, default(T));
			}
		}

		public bool Contains(T item)
		{
			return Collection.Contains(item);
		}

		public bool Remove(T item)
		{
			return Remove(item, asServer: true);
		}

		private bool Remove(T item, bool asServer)
		{
			if (!CanNetworkSetValues())
			{
				return false;
			}
			bool num = Collection.Remove(item);
			if (num && asServer)
			{
				if (NetworkManager == null)
				{
					ClientHostCollection.Remove(item);
				}
				AddOperation(SyncHashSetOperation.Remove, item);
			}
			return num;
		}

		public void DirtyAll()
		{
			if (base.IsRegistered && CanNetworkSetValues() && Dirty())
			{
				_sendAll = true;
			}
		}

		public void Dirty(T obj)
		{
			if (!base.IsRegistered || !CanNetworkSetValues())
			{
				return;
			}
			foreach (T item in Collection)
			{
				if (item.Equals(obj))
				{
					AddOperation(SyncHashSetOperation.Update, obj);
					return;
				}
			}
			NetworkManager.LogError("Could not find object within SyncHashSet, dirty will not be set.");
		}

		public IEnumerator GetEnumerator()
		{
			return Collection.GetEnumerator();
		}

		[APIExclude]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Collection.GetEnumerator();
		}

		[APIExclude]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Collection.GetEnumerator();
		}

		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == Collection)
			{
				Clear();
				return;
			}
			foreach (T item in other)
			{
				Remove(item);
			}
		}

		public void IntersectWith(IEnumerable<T> other)
		{
			ISet<T> other2 = ((!(other is ISet<T> set)) ? new HashSet<T>(other) : set);
			IntersectWith(other2);
		}

		private void IntersectWith(ISet<T> other)
		{
			Intersect(Collection);
			if (NetworkManager == null)
			{
				Intersect(ClientHostCollection);
			}
			_cache.Clear();
			void Intersect(ISet<T> collection)
			{
				_cache.AddRange(collection);
				int count = _cache.Count;
				for (int i = 0; i < count; i++)
				{
					T item = _cache[i];
					if (!other.Contains(item))
					{
						Remove(item);
					}
				}
			}
		}

		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return Collection.IsProperSubsetOf(other);
		}

		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return Collection.IsProperSupersetOf(other);
		}

		public bool IsSubsetOf(IEnumerable<T> other)
		{
			return Collection.IsSubsetOf(other);
		}

		public bool IsSupersetOf(IEnumerable<T> other)
		{
			return Collection.IsSupersetOf(other);
		}

		public bool Overlaps(IEnumerable<T> other)
		{
			return Collection.Overlaps(other);
		}

		public bool SetEquals(IEnumerable<T> other)
		{
			return Collection.SetEquals(other);
		}

		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other == Collection)
			{
				Clear();
				return;
			}
			foreach (T item in other)
			{
				Remove(item);
			}
		}

		public void UnionWith(IEnumerable<T> other)
		{
			if (other == Collection)
			{
				return;
			}
			foreach (T item in other)
			{
				Add(item);
			}
		}

		void ICollection<T>.Add(T item)
		{
			Add(item, asServer: true);
		}

		public void CopyTo(T[] array, int index)
		{
			Collection.CopyTo(array, index);
			if (NetworkManager == null)
			{
				ClientHostCollection.CopyTo(array, index);
			}
		}
	}
}
