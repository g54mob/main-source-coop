using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ElementReaderWriterBoolean : IElementReaderWriter<bool>
	{
		private static IElementReaderWriter<bool> Instance;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IElementReaderWriter<bool> GetInstance()
		{
			return Instance ?? (Instance = default(ElementReaderWriterBoolean));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool Read(byte* data, int index)
		{
			return ReadWriteUtilsForWeaver.ReadBoolean((int*)(data + index * 4));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref bool ReadRef(byte* data, int index)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. System.Boolean is not trivially copyable.");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write(byte* data, int index, bool val)
		{
			ReadWriteUtilsForWeaver.WriteBoolean((int*)(data + index * 4), val);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetElementWordCount()
		{
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetElementHashCode(bool val)
		{
			return val.GetHashCode();
		}

		public bool Read(ReadOnlySpan<byte> data)
		{
			return Unsafe.ReadUnaligned<int>(ref MemoryMarshal.GetReference(data)) != 0;
		}

		public void Write(Span<byte> data, bool val)
		{
			Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(data), val ? 1 : 0);
		}

		public ref bool ReadRef(Span<byte> data)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. System.Boolean is not trivially copyable.");
		}
	}
}
