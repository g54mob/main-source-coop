using System;

namespace MessagePack.Internal
{
	public static class UnsafeMemory
	{
		public static readonly bool Is32Bit = IntPtr.Size == 4;
	}
}
