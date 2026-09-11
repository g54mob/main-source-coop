using Unity.Collections;

namespace Zorro.Core
{
	public class MultiDataNativeRecord<T> : BookkeepingRecord where T : unmanaged
	{
		public int DataPerEntry;

		private NativeList<T> m_dataList;

		public NativeArray<T> NativeArray => m_dataList.AsArray();

		public T this[int index]
		{
			get
			{
				return m_dataList[index];
			}
			set
			{
				m_dataList[index] = value;
			}
		}

		public MultiDataNativeRecord(int dataPerEntry)
		{
			DataPerEntry = dataPerEntry;
			m_dataList = new NativeList<T>(Allocator.Persistent);
		}

		public MultiDataNativeRecord(int dataPerEntry, int defaultCapacity)
		{
			DataPerEntry = dataPerEntry;
			m_dataList = new NativeList<T>(dataPerEntry * defaultCapacity, Allocator.Persistent);
		}

		public override void Dispose()
		{
			m_dataList.Dispose();
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
			m_dataList.Add(in data);
		}
	}
}
