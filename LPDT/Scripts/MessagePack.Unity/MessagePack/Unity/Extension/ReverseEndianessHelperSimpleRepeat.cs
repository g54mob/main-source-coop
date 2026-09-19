using System;
using System.Runtime.InteropServices;

namespace MessagePack.Unity.Extension
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ReverseEndianessHelperSimpleRepeat<T> : IReverseEndianessHelper where T : unmanaged
	{
		public unsafe void ReverseEndianess(Span<byte> span)
		{
			for (int i = 0; i < span.Length; i += sizeof(T))
			{
				for (int j = 0; j << 1 < sizeof(T); j++)
				{
					ref byte reference = ref span[i + sizeof(T) - 1 - j];
					ref byte reference2 = ref span[i + j];
					byte b = span[i + j];
					byte b2 = span[i + sizeof(T) - 1 - j];
					reference = b;
					reference2 = b2;
				}
			}
		}
	}
}
