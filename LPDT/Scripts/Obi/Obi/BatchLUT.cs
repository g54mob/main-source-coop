using System;
using Unity.Collections;

namespace Obi
{
	public struct BatchLUT : IDisposable
	{
		public readonly int numBatches;

		public readonly NativeArray<ushort> batchIndex;

		public BatchLUT(int numBatches)
		{
			this.numBatches = numBatches;
			batchIndex = new NativeArray<ushort>(65536, Allocator.Persistent);
			ushort num = (ushort)(numBatches - 1);
			for (ushort num2 = 0; num2 < ushort.MaxValue; num2++)
			{
				ushort num3 = num2;
				for (ushort num4 = 0; num4 < num; num4++)
				{
					if ((num3 & 1) == 0)
					{
						batchIndex[num2] = num4;
						break;
					}
					num3 >>= 1;
				}
			}
			batchIndex[65535] = num;
		}

		public void Dispose()
		{
			batchIndex.Dispose();
		}
	}
}
