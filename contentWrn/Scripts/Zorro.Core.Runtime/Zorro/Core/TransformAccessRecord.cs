using UnityEngine;
using UnityEngine.Jobs;

namespace Zorro.Core
{
	public class TransformAccessRecord : BookkeepingRecord
	{
		public TransformAccessArray TransformAccessArray;

		public TransformAccessRecord(int capacity)
		{
			TransformAccessArray = new TransformAccessArray(capacity);
		}

		public void Add(Transform value)
		{
			TransformAccessArray.Add(value);
		}

		public override void Dispose()
		{
			TransformAccessArray.Dispose();
		}

		public override void RemoveAtSwapBack(int index)
		{
			TransformAccessArray.RemoveAtSwapBack(index);
		}
	}
}
