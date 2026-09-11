using System;

namespace Zorro.Core
{
	public class MultiDataManagedRecord<T> : BookkeepingRecord
	{
		public int DataPerEntry;

		private PerformantList<T> m_dataList;

		public PerformantList<T> List => m_dataList;

		public T this[int index] => m_dataList[index];

		public MultiDataManagedRecord(int dataPerEntry, int defaultCapacity)
		{
			DataPerEntry = dataPerEntry;
			m_dataList = new PerformantList<T>(dataPerEntry * defaultCapacity);
		}

		public override void Dispose()
		{
			if (m_dataList == null)
			{
				return;
			}
			for (int i = 0; i < m_dataList.Count; i++)
			{
				T val = m_dataList[i];
				if (val != null)
				{
					if (!(val is IDisposable disposable))
					{
						break;
					}
					disposable.Dispose();
				}
			}
			m_dataList = null;
		}

		public override void RemoveAtSwapBack(int index)
		{
			int num = index * DataPerEntry + DataPerEntry - 1;
			for (int i = 0; i < DataPerEntry; i++)
			{
				int index2 = num - i;
				m_dataList.RemoveAtSwapBack(index2);
			}
		}

		public void Add(T data)
		{
			m_dataList.Add(data);
		}
	}
}
