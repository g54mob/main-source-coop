using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ElementReaderWriterUnmanaged<T, TWordCount> : IElementReaderWriter<T> where T : unmanaged where TWordCount : unmanaged, IMetaConstant
	{
		private static IElementReaderWriter<T> Instance;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IElementReaderWriter<T> GetInstance()
		{
			return Instance ?? (Instance = default(ElementReaderWriterUnmanaged<T, TWordCount>));
		}

		public unsafe T Read(byte* data, int index)
		{
			return *(T*)(data + index * sizeof(TWordCount) * 4);
		}

		public unsafe void Write(byte* data, int index, T element)
		{
			*(T*)(data + index * sizeof(TWordCount) * 4) = element;
		}

		public unsafe int GetElementWordCount()
		{
			return sizeof(TWordCount);
		}

		public unsafe ref T ReadRef(byte* data, int index)
		{
			return ref Unsafe.AsRef<T>(data + index * sizeof(TWordCount) * 4);
		}

		public int GetElementHashCode(T element)
		{
			return element.GetHashCode();
		}

		public T Read(ReadOnlySpan<byte> data)
		{
			return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(data));
		}

		public void Write(Span<byte> data, T element)
		{
			Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(data), element);
		}

		public ref T ReadRef(Span<byte> data)
		{
			return ref Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(data));
		}
	}
}
