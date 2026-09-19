using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NetworkedModelCodegen.Scripts
{
	public sealed class NetworkedDictionary<TKey, TValue>
	{
		private static readonly Dictionary<TKey, TValue> _empty = new Dictionary<TKey, TValue>();

		private Dictionary<TKey, TValue> _items = new Dictionary<TKey, TValue>();

		private Func<IReadOnlyDictionary<TKey, TValue>, bool> _writer;

		public IReadOnlyDictionary<TKey, TValue> Items
		{
			get
			{
				if (_writer == null)
				{
					return _empty;
				}
				return _items;
			}
		}

		public int Count => Items.Count;

		public event Action Changed;

		public bool TryGetValue(TKey key, out TValue value)
		{
			return Items.TryGetValue(key, out value);
		}

		public bool ContainsKey(TKey key)
		{
			return Items.ContainsKey(key);
		}

		public void BindWriter(Func<IReadOnlyDictionary<TKey, TValue>, bool> writer)
		{
			_writer = writer;
		}

		public void ApplyFromNetwork(IReadOnlyDictionary<TKey, TValue> incoming)
		{
			if (ContentEquals(incoming))
			{
				return;
			}
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(incoming.Count);
			foreach (KeyValuePair<TKey, TValue> item in incoming)
			{
				dictionary[item.Key] = item.Value;
			}
			_items = dictionary;
			this.Changed?.Invoke();
		}

		public void Set(TKey key, TValue value)
		{
			if (!_items.TryGetValue(key, out var value2) || !EqualityComparer<TValue>.Default.Equals(value2, value))
			{
				Dictionary<TKey, TValue> candidate = new Dictionary<TKey, TValue>(_items) { [key] = value };
				Commit(candidate);
			}
		}

		public void Remove(TKey key)
		{
			if (_items.ContainsKey(key))
			{
				Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(_items);
				dictionary.Remove(key);
				Commit(dictionary);
			}
		}

		public void Clear()
		{
			if (_items.Count != 0)
			{
				Commit(new Dictionary<TKey, TValue>());
			}
		}

		public void ReplaceAll(IReadOnlyDictionary<TKey, TValue> next)
		{
			if (ContentEquals(next))
			{
				return;
			}
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(next.Count);
			foreach (KeyValuePair<TKey, TValue> item in next)
			{
				dictionary[item.Key] = item.Value;
			}
			Commit(dictionary);
		}

		private void Commit(Dictionary<TKey, TValue> candidate)
		{
			if (_writer == null)
			{
				Debug.LogError("[NetworkedModel] wrote a networked map whose shadow is not attached — out of scope, or before the owning scope finished attaching. The write was dropped. Open and await the owning scope before writing.");
			}
			else if (_writer(candidate))
			{
				_items = candidate;
				this.Changed?.Invoke();
			}
		}

		private bool ContentEquals(IReadOnlyDictionary<TKey, TValue> other)
		{
			if (_items.Count != other.Count)
			{
				return false;
			}
			foreach (KeyValuePair<TKey, TValue> item in _items)
			{
				if (!other.TryGetValue(item.Key, out var value) || !EqualityComparer<TValue>.Default.Equals(item.Value, value))
				{
					return false;
				}
			}
			return true;
		}
	}
}
