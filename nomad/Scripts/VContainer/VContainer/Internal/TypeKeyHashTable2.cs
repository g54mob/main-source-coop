using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
	internal sealed class TypeKeyHashTable2<TValue>
	{
		private struct Bucket
		{
			public const uint DistOne = 256u;

			public const uint FingerPrintMask = 255u;

			public uint DistAndFingerPrint;

			public int EntryIndex;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint DistAndFingerPrintFromHash(int hash)
			{
				return (uint)(0x100 | (hash & 0xFF));
			}
		}

		private readonly Bucket[] buckets;

		private readonly KeyValuePair<Type, TValue>[] entries;

		private readonly int indexFor;

		private int insertedEntryLength;

		public TypeKeyHashTable2(KeyValuePair<Type, TValue>[] values, float loadFactor = 0.75f)
		{
			int num = (int)((float)values.Length / loadFactor);
			int num2;
			for (num2 = 1; num2 < num; num2 <<= 1)
			{
			}
			buckets = new Bucket[num2];
			entries = new KeyValuePair<Type, TValue>[values.Length];
			indexFor = buckets.Length - 1;
			int num3 = 0;
			foreach (KeyValuePair<Type, TValue> entry in values)
			{
				Insert(entry, num3++);
			}
		}

		public bool TryGet(Type key, out TValue value)
		{
			int hashCode = RuntimeHelpers.GetHashCode(key);
			uint num = Bucket.DistAndFingerPrintFromHash(hashCode);
			int num2 = hashCode & indexFor;
			Bucket bucket = buckets[num2];
			while (true)
			{
				if (num == bucket.DistAndFingerPrint)
				{
					KeyValuePair<Type, TValue> keyValuePair = entries[bucket.EntryIndex];
					if (key == keyValuePair.Key)
					{
						value = keyValuePair.Value;
						return true;
					}
				}
				else if (num > bucket.DistAndFingerPrint)
				{
					break;
				}
				num += 256;
				num2 = NextBucketIndex(num2);
				bucket = buckets[num2];
			}
			value = default(TValue);
			return false;
		}

		private void Insert(KeyValuePair<Type, TValue> entry, int entryIndex)
		{
			int hashCode = RuntimeHelpers.GetHashCode(entry.Key);
			uint num = Bucket.DistAndFingerPrintFromHash(hashCode);
			int num2;
			for (num2 = hashCode & indexFor; num <= buckets[num2].DistAndFingerPrint; num += 256)
			{
				if (num == buckets[num2].DistAndFingerPrint && entry.Key == entries[buckets[num2].EntryIndex].Key)
				{
					throw new InvalidOperationException($"The key already exists: {entry.Key}");
				}
				num2 = NextBucketIndex(num2);
			}
			entries[entryIndex] = entry;
			SetBucketAt(num2, new Bucket
			{
				DistAndFingerPrint = num,
				EntryIndex = entryIndex
			});
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetBucketAt(int i, Bucket bucket)
		{
			while (buckets[i].DistAndFingerPrint != 0)
			{
				ref Bucket reference = ref buckets[i];
				Bucket bucket2 = bucket;
				Bucket bucket3 = buckets[i];
				reference = bucket2;
				bucket = bucket3;
				bucket.DistAndFingerPrint += 256u;
				i = NextBucketIndex(i);
			}
			buckets[i] = bucket;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int NextBucketIndex(int i)
		{
			if (i + 1 < buckets.Length)
			{
				return i + 1;
			}
			return 0;
		}
	}
}
