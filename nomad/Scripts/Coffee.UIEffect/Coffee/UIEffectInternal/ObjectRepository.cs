using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIEffectInternal
{
	internal class ObjectRepository<T> where T : UnityEngine.Object
	{
		private class Entry
		{
			public Hash128 hash;

			public int reference;

			public T storedObject;

			public void Release(Action<T> onRelease)
			{
				reference = 0;
				if ((bool)storedObject)
				{
					onRelease?.Invoke(storedObject);
				}
				storedObject = null;
			}

			public override string ToString()
			{
				return $"h{(uint)hash.GetHashCode()} (refs#{reference}), {storedObject}";
			}
		}

		private readonly Dictionary<Hash128, Entry> _cache = new Dictionary<Hash128, Entry>(8);

		private readonly Dictionary<int, Hash128> _objectKey = new Dictionary<int, Hash128>(8);

		private readonly string _name;

		private readonly Action<T> _onRelease;

		private readonly Stack<Entry> _pool = new Stack<Entry>(8);

		public int count => _cache.Count;

		public ObjectRepository(Action<T> onRelease = null)
		{
			_name = typeof(T).Name + "Repository";
			if (onRelease == null)
			{
				_onRelease = delegate(T x)
				{
					UnityEngine.Object.Destroy(x);
				};
			}
			else
			{
				_onRelease = onRelease;
			}
			for (int num = 0; num < 8; num++)
			{
				_pool.Push(new Entry());
			}
		}

		public void Clear()
		{
			foreach (KeyValuePair<Hash128, Entry> item in _cache)
			{
				Entry value = item.Value;
				if (value != null)
				{
					value.Release(_onRelease);
					_pool.Push(value);
				}
			}
			_cache.Clear();
			_objectKey.Clear();
		}

		public bool Valid(Hash128 hash, T obj)
		{
			if (_cache.TryGetValue(hash, out var value))
			{
				return value.storedObject == obj;
			}
			return false;
		}

		public void Get(Hash128 hash, ref T obj, Func<T> onCreate)
		{
			if (!GetFromCache(hash, ref obj))
			{
				Add(hash, ref obj, onCreate());
			}
		}

		public void Get<TS>(Hash128 hash, ref T obj, Func<TS, T> onCreate, TS source)
		{
			if (!GetFromCache(hash, ref obj))
			{
				Add(hash, ref obj, onCreate(source));
			}
		}

		private bool GetFromCache(Hash128 hash, ref T obj)
		{
			if (_cache.TryGetValue(hash, out var value))
			{
				if (!value.storedObject)
				{
					Release(ref value.storedObject);
					return false;
				}
				if (value.storedObject != obj)
				{
					Release(ref obj);
					value.reference++;
					obj = value.storedObject;
				}
				return true;
			}
			return false;
		}

		private void Add(Hash128 hash, ref T obj, T newObject)
		{
			if (!newObject)
			{
				Release(ref obj);
				obj = newObject;
				return;
			}
			Entry entry = ((0 < _pool.Count) ? _pool.Pop() : new Entry());
			entry.storedObject = newObject;
			entry.hash = hash;
			entry.reference = 1;
			_cache[hash] = entry;
			_objectKey[newObject.GetInstanceID()] = hash;
			Release(ref obj);
			obj = newObject;
		}

		public void Release(ref T obj)
		{
			if ((object)obj == null)
			{
				return;
			}
			int instanceID = obj.GetInstanceID();
			if (_objectKey.TryGetValue(instanceID, out var value) && _cache.TryGetValue(value, out var value2))
			{
				value2.reference--;
				if (value2.reference <= 0 || !value2.storedObject)
				{
					Remove(value2);
				}
			}
			obj = null;
		}

		private void Remove(Entry entry)
		{
			if (entry != null)
			{
				_cache.Remove(entry.hash);
				_objectKey.Remove(entry.storedObject.GetInstanceID());
				_pool.Push(entry);
				entry.reference = 0;
				entry.Release(_onRelease);
			}
		}
	}
}
