namespace Fusion
{
	public readonly struct FusionUnsafeAllocInfo
	{
		public readonly string FilePath;

		public readonly int LineNumber;

		public readonly int Size;

		public readonly int Align;

		public readonly string StackTrace;

		public readonly nint Ptr;

		public FusionUnsafeAllocInfo(nint ptr, int size, int align, string filePath, int lineNumber, string stackTrace)
		{
			FilePath = filePath;
			LineNumber = lineNumber;
			Size = size;
			Align = align;
			StackTrace = stackTrace;
			Ptr = ptr;
		}
	}
}
