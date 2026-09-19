namespace Fusion
{
	public readonly struct FusionUnsafeAllocResult
	{
		public readonly FusionUnsafeAllocInfo[] Info;

		public FusionUnsafeAllocResult(FusionUnsafeAllocInfo[] info)
		{
			Info = info;
		}
	}
}
