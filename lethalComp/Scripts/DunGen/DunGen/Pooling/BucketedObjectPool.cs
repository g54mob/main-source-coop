using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunGen.Pooling
{
	public sealed class BucketedObjectPool<TKey, TObject> where TObject : class
	{
		private readonly Dictionary<TKey, List<TObject>> buckets = new Dictionary<TKey, List<TObject>>();

		private readonly Func<TKey, TObject> objectFactory;

		private readonly Action<TObject> takeAction;

		private readonly Action<TObject> returnAction;

		private readonly int initialCapacity;

		private readonly Dictionary<TObject, TKey> owningBuckets = new Dictionary<TObject, TKey>();

		public BucketedObjectPool(Func<TKey, TObject> objectFactory, Action<TObject> takeAction = null, Action<TObject> returnAction = null, int initialCapacity = 0)
		{
			this.objectFactory = objectFactory;
			this.takeAction = takeAction;
			this.returnAction = returnAction;
			this.initialCapacity = initialCapacity;
		}

		public void Clear()
		{
			buckets.Clear();
			owningBuckets.Clear();
		}

		public TObject TakeObject(TKey key)
		{
			TryTakeObject(key, out var obj);
			return obj;
		}

		public bool TryTakeObject(TKey key, out TObject obj)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!buckets.TryGetValue(key, out var value))
			{
				value = InitialiseBucket(key);
			}
			if (value.Count > 0)
			{
				obj = value[value.Count - 1];
				value.RemoveAt(value.Count - 1);
				takeAction?.Invoke(obj);
				return true;
			}
			TObject val = objectFactory(key);
			owningBuckets[val] = key;
			obj = val;
			return false;
		}

		public bool ReturnObject(TObject obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!owningBuckets.TryGetValue(obj, out var value))
			{
				return false;
			}
			returnAction?.Invoke(obj);
			buckets[value].Add(obj);
			return true;
		}

		public bool InsertObject(TKey key, TObject obj)
		{
			if (key == null || obj == null)
			{
				return false;
			}
			if (owningBuckets.TryGetValue(obj, out var _))
			{
				Debug.LogError("Tried to 'Insert' an object into the pool that already belongs to it, use ReturnObject() instead");
				return false;
			}
			if (!buckets.TryGetValue(key, out var value2))
			{
				value2 = InitialiseBucket(key);
			}
			returnAction?.Invoke(obj);
			buckets[key].Add(obj);
			owningBuckets[obj] = key;
			return true;
		}

		private List<TObject> InitialiseBucket(TKey key)
		{
			List<TObject> list = new List<TObject>(initialCapacity);
			buckets[key] = list;
			for (int i = 0; i < initialCapacity; i++)
			{
				TObject key2 = objectFactory(key);
				owningBuckets[key2] = key;
			}
			return list;
		}

		public void DumpPoolInfo(Func<TKey, string> getBucketName = null)
		{
			foreach (KeyValuePair<TKey, List<TObject>> bucket in buckets)
			{
				string arg = getBucketName?.Invoke(bucket.Key) ?? bucket.Key.ToString();
				Debug.Log($"Bucket: {arg}, Count: {bucket.Value.Count}");
			}
		}
	}
}
