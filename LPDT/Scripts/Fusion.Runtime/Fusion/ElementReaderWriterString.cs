using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ElementReaderWriterString<TCapacity> : IElementReaderWriter<string> where TCapacity : unmanaged, IMetaConstant
	{
		private static IElementReaderWriter<string> Instance;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IElementReaderWriter<string> GetInstance()
		{
			return Instance ?? (Instance = default(ElementReaderWriterString<TCapacity>));
		}

		public unsafe string Read(byte* data, int index)
		{
			ReadWriteUtilsForWeaver.ReadStringUtf32NoHash((int*)data + index * GetElementWordCount(), sizeof(TCapacity), out var result);
			return result;
		}

		public unsafe void Write(byte* data, int index, string element)
		{
			ReadWriteUtilsForWeaver.WriteStringUtf32NoHash((int*)data + index * GetElementWordCount(), sizeof(TCapacity), element);
		}

		public unsafe int GetElementWordCount()
		{
			return 1 + sizeof(TCapacity);
		}

		public unsafe ref string ReadRef(byte* data, int index)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. System.String is not trivially copyable.");
		}

		public unsafe int GetElementHashCode(string element)
		{
			return element.GetHashDeterministicInternal(Math.Min(element.Length, sizeof(TCapacity)), 352654597);
		}

		public unsafe string Read(ReadOnlySpan<byte> data)
		{
			string result;
			fixed (byte* ptr = data)
			{
				ReadWriteUtilsForWeaver.ReadStringUtf32NoHash((int*)ptr, sizeof(TCapacity), out result);
			}
			return result;
		}

		public unsafe void Write(Span<byte> data, string element)
		{
			fixed (byte* ptr = data)
			{
				ReadWriteUtilsForWeaver.WriteStringUtf32NoHash((int*)ptr, sizeof(TCapacity), element);
			}
		}

		public ref string ReadRef(Span<byte> data)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. System.String is not trivially copyable.");
		}
	}
}
