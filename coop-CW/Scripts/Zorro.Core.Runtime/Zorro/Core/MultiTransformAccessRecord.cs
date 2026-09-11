using UnityEngine;
using UnityEngine.Jobs;

namespace Zorro.Core
{
	public class MultiTransformAccessRecord : BookkeepingRecord
	{
		public int DataPerEntry;

		public TransformAccessArray TransformAccessArray;

		public MultiTransformAccessRecord(int dataPerEntry, int capacity)
		{
			DataPerEntry = dataPerEntry;
			TransformAccessArray = new TransformAccessArray(capacity * dataPerEntry);
		}

		public override void Dispose()
		{
			TransformAccessArray.Dispose();
		}

		public void Add(Transform transform)
		{
			TransformAccessArray.Add(transform);
		}

		public override void RemoveAtSwapBack(int index)
		{
			int num = index * DataPerEntry + DataPerEntry - 1;
			for (int i = 0; i < DataPerEntry; i++)
			{
				int index2 = num - i;
				TransformAccessArray.RemoveAtSwapBack(index2);
			}
		}
	}
}
