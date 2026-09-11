using System;
using System.Collections.Generic;

namespace Zorro.Core
{
	public class NativeBookkeeper<T> : IDisposable, INativeBookkeeper
	{
		protected List<BookkeepingRecord> m_bookKeepingRecords;

		protected BidirectionalDictionary<T, int> m_bookkeepingIndexer;

		protected PerformantList<T> m_keys;

		protected int recordsCount;

		public int Count => recordsCount;

		public NativeBookkeeper(int defaultCapacity)
		{
			m_bookkeepingIndexer = new BidirectionalDictionary<T, int>(defaultCapacity);
			m_bookKeepingRecords = new List<BookkeepingRecord>(8);
			m_keys = new PerformantList<T>(defaultCapacity);
		}

		public PerformantList<T> GetKeyList()
		{
			return m_keys;
		}

		public void RegisterRecord(BookkeepingRecord record)
		{
			m_bookKeepingRecords.Add(record);
		}

		public virtual void Dispose()
		{
			foreach (BookkeepingRecord bookKeepingRecord in m_bookKeepingRecords)
			{
				bookKeepingRecord.Dispose();
			}
		}

		public virtual int Add(T newEntry)
		{
			int result = recordsCount;
			m_bookkeepingIndexer.Add(newEntry, recordsCount);
			m_keys.Add(newEntry);
			recordsCount++;
			return result;
		}

		public T GetKeyFromIndex(int index)
		{
			return m_keys[index];
		}

		public virtual BookkeperRemovalInfo Remove(T entry)
		{
			int index = GetIndex(entry);
			int num = recordsCount - 1;
			foreach (BookkeepingRecord bookKeepingRecord in m_bookKeepingRecords)
			{
				bookKeepingRecord.RemoveAtSwapBack(index);
			}
			m_keys.RemoveAtSwapBack(index);
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
			return m_keys[i];
		}

		public bool Contains(T mesh)
		{
			return m_bookkeepingIndexer.ContainsKey(mesh);
		}
	}
}
