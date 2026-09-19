using System;
using System.Runtime.InteropServices;

namespace MessagePack.Unity.Extension
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ReverseEndianessHelperSimpleSingle : IReverseEndianessHelper
	{
		public void ReverseEndianess(Span<byte> span)
		{
			for (int i = 0; i << 1 < span.Length; i++)
			{
				ref byte reference = ref span[span.Length - 1 - i];
				ref byte reference2 = ref span[i];
				byte b = span[i];
				byte b2 = span[span.Length - 1 - i];
				reference = b;
				reference2 = b2;
			}
		}
	}
}
