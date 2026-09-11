namespace Zorro.Core
{
	public class ManagedRecord<T> : BookkeepingRecord
	{
		public PerformantList<T> PerformantList;

		public ManagedRecord(int initialCapacity)
		{
			PerformantList = new PerformantList<T>(initialCapacity);
		}

		public void Add(T value)
		{
			PerformantList.Add(value);
		}

		public override void Dispose()
		{
		}

		public override void RemoveAtSwapBack(int index)
		{
			PerformantList.RemoveAtSwapBack(index);
		}
	}
}
