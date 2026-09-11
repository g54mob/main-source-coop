using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Zorro.Core
{
	public class UnmanagedNativeBookkeeper<T> : IDisposable, INativeBookkeeper where T : unmanaged
	{
		protected List<BookkeepingRecord> m_bookKeepingRecords;

		protected BidirectionalDictionary<T, int> m_bookkeepingIndexer;

		public NativeList<T> Keys;

		protected int recordsCount;

		public int Count => recordsCount;

		public UnmanagedNativeBookkeeper(int defaultCapacity)
		{
			m_bookkeepingIndexer = new BidirectionalDictionary<T, int>(defaultCapacity);
			m_bookKeepingRecords = new List<BookkeepingRecord>(8);
			Keys = new NativeList<T>(defaultCapacity, Allocator.Persistent);
		}

		public void RegisterRecord(BookkeepingRecord record)
		{
			m_bookKeepingRecords.Add(record);
		}

		public void Dispose()
		{
			foreach (BookkeepingRecord bookKeepingRecord in m_bookKeepingRecords)
			{
				bookKeepingRecord.Dispose();
			}
			Keys.Dispose();
		}

		public virtual int Add(T newEntry)
		{
			int result = recordsCount;
			m_bookkeepingIndexer.Add(newEntry, recordsCount);
			Keys.Add(in newEntry);
			recordsCount++;
			return result;
		}

		public T GetKeyFromIndex(int index)
		{
			return Keys[index];
		}

		public virtual BookkeperRemovalInfo Remove(T entry)
		{
			int index = GetIndex(entry);
			int num = recordsCount - 1;
			foreach (BookkeepingRecord bookKeepingRecord in m_bookKeepingRecords)
			{
				bookKeepingRecord.RemoveAtSwapBack(index);
			}
			Keys.RemoveAtSwapBack(index);
			T key = m_bookkeepingIndexer.RemoveFromValue(num);
			if (index != num)
			{
				m_bookkeepingIndexer.RemoveFromKey(entry);
				m_bookkeepingIndexer.Add(key, index);
			}
			recordsCount--;
			return new BookkeperRemovalInfo(index, num);
		}

		public int GetIndex(T entry)
		{
			return m_bookkeepingIndexer.GetFromKey(entry);
		}

		public T GetFromIndex(int i)
		{
			return Keys[i];
		}

		public bool HasKey(T key)
		{
			return m_bookkeepingIndexer.ContainsKey(key);
		}
	}
}
