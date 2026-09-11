namespace Zorro.Core
{
	public struct BookkeperRemovalInfo
	{
		public int IndexRemoved;

		public int SwapbackIndex;

		public BookkeperRemovalInfo(int indexRemoved, int swapbackIndex)
		{
			IndexRemoved = indexRemoved;
			SwapbackIndex = swapbackIndex;
		}
	}
}
