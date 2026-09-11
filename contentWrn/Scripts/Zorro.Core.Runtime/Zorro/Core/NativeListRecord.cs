using Unity.Collections;

namespace Zorro.Core
{
	public class NativeListRecord<T> : BookkeepingRecord where T : unmanaged
	{
		public NativeList<T> NativeList;

		public NativeListRecord(int initialCapacity)
		{
			NativeList = new NativeList<T>(initialCapacity, Allocator.Persistent);
		}

		public void Add(T value)
		{
			NativeList.Add(in value);
		}

		public override void Dispose()
		{
			NativeList.Dispose();
		}

		public override void RemoveAtSwapBack(int index)
		{
			NativeList.RemoveAtSwapBack(index);
		}
	}
}
