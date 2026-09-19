using Unity.Mathematics;

namespace Obi
{
	public struct BatchData
	{
		public ushort batchID;

		public int startIndex;

		public int constraintCount;

		public int activeConstraintCount;

		public int workItemSize;

		public int workItemCount;

		public bool isLast;

		public BatchData(int index, int maxBatches)
		{
			batchID = (ushort)(1 << index);
			isLast = index == maxBatches - 1;
			constraintCount = 0;
			activeConstraintCount = 0;
			startIndex = 0;
			workItemSize = 0;
			workItemCount = 0;
		}

		public void GetConstraintRange(int workItemIndex, out int start, out int end)
		{
			start = startIndex + workItemSize * workItemIndex;
			end = startIndex + math.min(constraintCount, workItemSize * (workItemIndex + 1));
		}
	}
}
