using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Documenting;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;
using GameKit.Utilities;
using JetBrains.Annotations;

namespace FishNet.Object.Synchronizing
{
	public class SyncIDictionary<TKey, TValue> : SyncBase, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		private struct CachedOnChange
		{
			internal readonly SyncDictionaryOperation Operation;

			internal readonly TKey Key;

			internal readonly TValue Value;

			public CachedOnChange(SyncDictionaryOperation operation, TKey key, TValue value)
			{
				Operation = operation;
				Key = key;
				Value = value;
			}
		}

		private struct ChangeData
		{
			internal readonly SyncDictionaryOperation Operation;

			internal readonly TKey Key;

			internal readonly TValue Value;

			public ChangeData(SyncDictionaryOperation operation, TKey key, TValue value)
			{
				Operation = operation;
				Key = key;
				Value = value;
			}
		}

		[APIExclude]
		public delegate void SyncDictionaryChanged(SyncDictionaryOperation op, TKey key, TValue value, bool asServer);

		public readonly IDictionary<TKey, TValue> Collection;

		public readonly IDictionary<TKey, TValue> ClientHostCollection = new Dictionary<TKey, TValue>();

		private IDictionary<TKey, TValue> _initialValues = new Dictionary<TKey, TValue>();

		private readonly List<ChangeData> _changed = new List<ChangeData>();

		private readonly List<CachedOnChange> _serverOnChanges = new List<CachedOnChange>();

		private readonly List<CachedOnChange> _clientOnChanges = new List<CachedOnChange>();

		private bool _valuesChanged;

		private bool _sendAll;

		[APIExclude]
		public bool IsReadOnly => false;

		public int Count => Collection.Count;

		public ICollection<TKey> Keys => Collection.Keys;

		[APIExclude]
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Collection.Keys;

		public ICollection<TValue> Values => Collection.Values;

		[APIExclude]
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Collection.Values;

		public TValue this[TKey key]
		{
			get
			{
				return Collection[key];
			}
			set
			{
				if (CanNetworkSetValues())
				{
					Collection[key] = value;
					AddOperation(SyncDictionaryOperation.Set, key, value);
				}
			}
		}

		public event SyncDictionaryChanged OnChange;

		[APIExclude]
		public SyncIDictionary(IDictionary<TKey, TValue> objects)
		{
			Collection = objects;
			foreach (KeyValuePair<TKey, TValue> @object in objects)
			{
				ClientHostCollection[@object.Key] = @object.Value;
			}
		}

		public Dictionary<TKey, TValue> GetCollection(bool asServer)
		{
			return ((!asServer && NetworkManager.IsServer) ? ClientHostCollection : Collection) as Dictionary<TKey, TValue>;
		}

		protected override void Registered()
		{
			base.Registered();
			foreach (KeyValuePair<TKey, TValue> item in Collection)
			{
				_initialValues[item.Key] = item.Value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		private void AddOperation(SyncDictionaryOperation operation, TKey key, TValue value)
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
					ChangeData item = new ChangeData(operation, key, value);
					_changed.Add(item);
				}
			}
			InvokeOnChange(operation, key, value, flag);
		}

		public override void OnStartCallback(bool asServer)
		{
			base.OnStartCallback(asServer);
			List<CachedOnChange> list = (asServer ? _serverOnChanges : _clientOnChanges);
			if (this.OnChange != null)
			{
				foreach (CachedOnChange item in list)
				{
					this.OnChange(item.Operation, item.Key, item.Value, asServer);
				}
			}
			list.Clear();
		}

		[APIExclude]
		public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			base.WriteDelta(writer, resetSyncTick);
			if (_sendAll)
			{
				_sendAll = false;
				_changed.Clear();
				WriteFull(writer);
				return;
			}
			writer.WriteBoolean(value: false);
			writer.WriteInt32(_changed.Count);
			for (int i = 0; i < _changed.Count; i++)
			{
				ChangeData changeData = _changed[i];
				writer.WriteByte((byte)changeData.Operation);
				if (changeData.Operation == SyncDictionaryOperation.Add || changeData.Operation == SyncDictionaryOperation.Set)
				{
					writer.Write(changeData.Key);
					writer.Write(changeData.Value);
				}
				else if (changeData.Operation == SyncDictionaryOperation.Remove)
				{
					writer.Write(changeData.Key);
				}
			}
			_changed.Clear();
		}

		[APIExclude]
		public override void WriteFull(PooledWriter writer)
		{
			if (!_valuesChanged)
			{
				return;
			}
			base.WriteHeader(writer, resetSyncTick: false);
			writer.WriteBoolean(value: true);
			writer.WriteInt32(Collection.Count);
			foreach (KeyValuePair<TKey, TValue> item in Collection)
			{
				writer.WriteByte(0);
				writer.Write(item.Key);
				writer.Write(item.Value);
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
				num = (NetworkBehaviour.IsServer ? 1 : 0);
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
			IDictionary<TKey, TValue> dictionary = ((num != 0) ? ClientHostCollection : Collection);
			if (reader.ReadBoolean())
			{
				dictionary.Clear();
			}
			int num3 = reader.ReadInt32();
			for (int i = 0; i < num3; i++)
			{
				SyncDictionaryOperation syncDictionaryOperation = (SyncDictionaryOperation)reader.ReadByte();
				TKey key = default(TKey);
				TValue value = default(TValue);
				switch (syncDictionaryOperation)
				{
				case SyncDictionaryOperation.Add:
				case SyncDictionaryOperation.Set:
					key = reader.Read<TKey>();
					value = reader.Read<TValue>();
					if (!flag)
					{
						dictionary[key] = value;
					}
					break;
				case SyncDictionaryOperation.Clear:
					if (!flag)
					{
						dictionary.Clear();
					}
					break;
				case SyncDictionaryOperation.Remove:
					key = reader.Read<TKey>();
					if (!flag)
					{
						dictionary.Remove(key);
					}
					break;
				}
				InvokeOnChange(syncDictionaryOperation, key, value, asServer: false);
			}
			if (num3 > 0)
			{
				InvokeOnChange(SyncDictionaryOperation.Complete, default(TKey), default(TValue), asServer: false);
			}
		}

		private void InvokeOnChange(SyncDictionaryOperation operation, TKey key, TValue value, bool asServer)
		{
			if (asServer)
			{
				if (NetworkBehaviour.OnStartServerCalled)
				{
					this.OnChange?.Invoke(operation, key, value, asServer);
				}
				else
				{
					_serverOnChanges.Add(new CachedOnChange(operation, key, value));
				}
			}
			else if (NetworkBehaviour.OnStartClientCalled)
			{
				this.OnChange?.Invoke(operation, key, value, asServer);
			}
			else
			{
				_clientOnChanges.Add(new CachedOnChange(operation, key, value));
			}
		}

		[APIExclude]
		public override void ResetState()
		{
			base.ResetState();
			_sendAll = false;
			_changed.Clear();
			Collection.Clear();
			ClientHostCollection.Clear();
			_valuesChanged = false;
			foreach (KeyValuePair<TKey, TValue> initialValue in _initialValues)
			{
				Collection[initialValue.Key] = initialValue.Value;
				ClientHostCollection[initialValue.Key] = initialValue.Value;
			}
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public void Add(TKey key, TValue value)
		{
			Add(key, value, asServer: true);
		}

		private void Add(TKey key, TValue value, bool asServer)
		{
			if (CanNetworkSetValues())
			{
				Collection.Add(key, value);
				if (asServer)
				{
					AddOperation(SyncDictionaryOperation.Add, key, value);
				}
			}
		}

		public void Clear()
		{
			Clear(asServer: true);
		}

		private void Clear(bool asServer)
		{
			if (CanNetworkSetValues())
			{
				Collection.Clear();
				if (asServer)
				{
					AddOperation(SyncDictionaryOperation.Clear, default(TKey), default(TValue));
				}
			}
		}

		public bool ContainsKey(TKey key)
		{
			return Collection.ContainsKey(key);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (TryGetValue(item.Key, out var value))
			{
				return EqualityComparer<TValue>.Default.Equals(value, item.Value);
			}
			return false;
		}

		public void CopyTo([NotNull] KeyValuePair<TKey, TValue>[] array, int offset)
		{
			if (offset <= -1 || offset >= array.Length)
			{
				NetworkManager.LogError("Index is out of range.");
				return;
			}
			if (array.Length - offset < Count)
			{
				NetworkManager.LogError($"Array is not large enough to copy data. Array is of length {array.Length}, index is {offset}, and number of values to be copied is {Count.ToString()}.");
				return;
			}
			int num = offset;
			foreach (KeyValuePair<TKey, TValue> item in Collection)
			{
				array[num] = item;
				num++;
			}
		}

		public bool Remove(TKey key)
		{
			if (!CanNetworkSetValues())
			{
				return false;
			}
			if (Collection.Remove(key))
			{
				AddOperation(SyncDictionaryOperation.Remove, key, default(TValue));
				return true;
			}
			return false;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return Remove(item.Key);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryGetValue(TKey key, out TValue value)
		{
			return Collection.TryGetValueIL2CPP(key, out value);
		}

		public void DirtyAll()
		{
			if (base.IsRegistered && CanNetworkSetValues() && Dirty())
			{
				_sendAll = true;
			}
		}

		public void Dirty(TKey key)
		{
			if (base.IsRegistered && CanNetworkSetValues() && Collection.TryGetValueIL2CPP(key, out var value))
			{
				AddOperation(SyncDictionaryOperation.Set, key, value);
			}
		}

		public bool Dirty(TValue value, EqualityComparer<TValue> comparer = null)
		{
			if (!base.IsRegistered)
			{
				return false;
			}
			if (!CanNetworkSetValues())
			{
				return false;
			}
			if (comparer == null)
			{
				comparer = EqualityComparer<TValue>.Default;
			}
			foreach (KeyValuePair<TKey, TValue> item in Collection)
			{
				if (comparer.Equals(item.Value, value))
				{
					AddOperation(SyncDictionaryOperation.Set, item.Key, value);
					return true;
				}
			}
			return false;
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return Collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Collection.GetEnumerator();
		}
	}
}
