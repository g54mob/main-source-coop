using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MessagePack
{
	internal static class SafeBitConverter
	{
		internal static long ToInt64(ReadOnlySpan<byte> value)
		{
			return Unsafe.ReadUnaligned<long>(ref Unsafe.AsRef(in MemoryMarshal.GetReference(value)));
		}

		internal static ulong ToUInt64(ReadOnlySpan<byte> value)
		{
			return (ulong)ToInt64(value);
		}

		internal static ushort ToUInt16(ReadOnlySpan<byte> value)
		{
			return Unsafe.ReadUnaligned<ushort>(ref Unsafe.AsRef(in MemoryMarshal.GetReference(value)));
		}

		internal static uint ToUInt32(ReadOnlySpan<byte> value)
		{
			return Unsafe.ReadUnaligned<uint>(ref Unsafe.AsRef(in MemoryMarshal.GetReference(value)));
		}
	}
}
