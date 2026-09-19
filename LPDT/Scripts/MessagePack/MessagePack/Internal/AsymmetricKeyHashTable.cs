using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace MessagePack.Internal
{
	internal sealed class AsymmetricKeyHashTable<TKey1, TKey2, TValue>
	{
		private class Entry
		{
			internal readonly TKey1 Key;

			internal readonly TValue Value;

			internal readonly int Hash;

			internal Entry? Next;

			internal int Count
			{
				get
				{
					int num = 1;
					Entry entry = this;
					while (entry.Next != null)
					{
						num = checked(num + 1);
						entry = entry.Next;
					}
					return num;
				}
			}

			internal Entry(TKey1 key, TValue value, int hash)
			{
				Key = key;
				Value = value;
				Hash = hash;
			}

			public override string ToString()
			{
				return "Count:" + Count;
			}
		}

		private Entry[] buckets;

		private int size;

		private readonly object writerLock = new object();

		private readonly float loadFactor;

		private readonly IAsymmetricEqualityComparer<TKey1, TKey2> comparer;

		public AsymmetricKeyHashTable(IAsymmetricEqualityComparer<TKey1, TKey2> comparer)
			: this(4, 0.72f, comparer)
		{
		}

		public AsymmetricKeyHashTable(int capacity, float loadFactor, IAsymmetricEqualityComparer<TKey1, TKey2> comparer)
		{
			int num = CalculateCapacity(capacity, loadFactor);
			buckets = new Entry[num];
			this.loadFactor = loadFactor;
			this.comparer = comparer;
		}

		public TValue AddOrGet(TKey1 key1, Func<TKey1, TValue> valueFactory)
		{
			TryAddInternal(key1, valueFactory, out var resultingValue);
			return resultingValue;
		}

		public bool TryAdd(TKey1 key, TValue value)
		{
			return TryAdd(key, (TKey1 _) => value);
		}

		public bool TryAdd(TKey1 key, Func<TKey1, TValue> valueFactory)
		{
			TValue resultingValue;
			return TryAddInternal(key, valueFactory, out resultingValue);
		}

		private bool TryAddInternal(TKey1 key, Func<TKey1, TValue> valueFactory, out TValue resultingValue)
		{
			checked
			{
				lock (writerLock)
				{
					int num = CalculateCapacity(size + 1, loadFactor);
					if (buckets.Length < num)
					{
						Entry[] value = new Entry[num];
						for (int i = 0; i < buckets.Length; i++)
						{
							for (Entry entry = buckets[i]; entry != null; entry = entry.Next)
							{
								Entry newEntryOrNull = new Entry(entry.Key, entry.Value, entry.Hash);
								AddToBuckets(value, key, newEntryOrNull, null, out resultingValue);
							}
						}
						bool num2 = AddToBuckets(value, key, null, valueFactory, out resultingValue);
						VolatileWrite(ref buckets, value);
						if (num2)
						{
							size++;
						}
						return num2;
					}
					bool num3 = AddToBuckets(buckets, key, null, valueFactory, out resultingValue);
					if (num3)
					{
						size++;
					}
					return num3;
				}
			}
		}

		private bool AddToBuckets(Entry[] buckets, TKey1 newKey, Entry? newEntryOrNull, Func<TKey1, TValue>? valueFactory, out TValue resultingValue)
		{
			int num = newEntryOrNull?.Hash ?? comparer.GetHashCode(newKey);
			checked
			{
				if (buckets[num & (buckets.Length - 1)] == null)
				{
					if (newEntryOrNull != null)
					{
						resultingValue = newEntryOrNull.Value;
						VolatileWrite(ref buckets[num & (buckets.Length - 1)], newEntryOrNull);
					}
					else
					{
						if (valueFactory == null)
						{
							throw new ArgumentNullException("valueFactory", "Either newEntryOrNull or valueFactory must be non-null.");
						}
						resultingValue = valueFactory(newKey);
						VolatileWrite(ref buckets[num & (buckets.Length - 1)], new Entry(newKey, resultingValue, num));
					}
				}
				else
				{
					Entry entry = buckets[num & (buckets.Length - 1)];
					while (true)
					{
						if (comparer.Equals(entry.Key, newKey))
						{
							resultingValue = entry.Value;
							return false;
						}
						if (entry.Next == null)
						{
							break;
						}
						entry = entry.Next;
					}
					if (newEntryOrNull != null)
					{
						resultingValue = newEntryOrNull.Value;
						VolatileWrite(ref entry.Next, newEntryOrNull);
					}
					else
					{
						if (valueFactory == null)
						{
							throw new ArgumentNullException("valueFactory", "Either newEntryOrNull or valueFactory must be non-null.");
						}
						resultingValue = valueFactory(newKey);
						VolatileWrite(ref entry.Next, new Entry(newKey, resultingValue, num));
					}
				}
				return true;
			}
		}

		public bool TryGetValue(TKey2 key, [MaybeNullWhen(false)] out TValue value)
		{
			Entry[] array = buckets;
			int hashCode = comparer.GetHashCode(key);
			Entry entry = array[hashCode & checked(array.Length - 1)];
			if (entry != null)
			{
				if (comparer.Equals(entry.Key, key))
				{
					value = entry.Value;
					return true;
				}
				for (Entry next = entry.Next; next != null; next = next.Next)
				{
					if (comparer.Equals(next.Key, key))
					{
						value = next.Value;
						return true;
					}
				}
			}
			value = default(TValue);
			return false;
		}

		private static int CalculateCapacity(int collectionSize, float loadFactor)
		{
			int num = checked((int)((float)collectionSize / loadFactor));
			int num2;
			for (num2 = 1; num2 < num; num2 <<= 1)
			{
			}
			if (num2 < 8)
			{
				return 8;
			}
			return num2;
		}

		private static void VolatileWrite<T>(ref T location, T value) where T : Entry?
		{
			Volatile.Write(ref location, value);
		}

		private static void VolatileWrite(ref Entry[] location, Entry[] value)
		{
			Volatile.Write(ref location, value);
		}
	}
}
