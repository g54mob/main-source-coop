#define DEBUG
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Fusion
{
	internal ref struct RpcDataWriter
	{
		private readonly Span<byte> _data;

		private int _offset;

		public RpcDataWriter(Span<byte> data)
		{
			_data = data;
			_offset = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int AlignOffset(int offset)
		{
			return (offset + 3) & -4;
		}

		private Span<byte> GetAndAdvanceData(int bytes)
		{
			Span<byte> result = _data.Slice(_offset, bytes);
			_offset = AlignOffset(_offset + bytes);
			return result;
		}

		public void Write<T>(T value, int size) where T : unmanaged
		{
			FusionUnsafe.ReinterpretBytes<T>(GetAndAdvanceData(size)) = value;
		}

		public void Write<T>(T[] value, int elementSize) where T : unmanaged
		{
			Write(value.Length, 4);
			Span<byte> andAdvanceData = GetAndAdvanceData(value.Length * elementSize);
			value.CopyTo(MemoryMarshal.Cast<byte, T>(andAdvanceData));
		}

		public void Write<T>(ReadOnlySpan<T> value, int elementSize) where T : unmanaged
		{
			Write(value.Length, 4);
			Span<byte> andAdvanceData = GetAndAdvanceData(value.Length * elementSize);
			value.CopyTo(MemoryMarshal.Cast<byte, T>(andAdvanceData));
		}

		public void WriteCount<T>(T[] array)
		{
			Write(array.Length, 4);
		}

		public unsafe void Write(string value)
		{
			fixed (char* pointer = value)
			{
				int byteCount = Encoding.UTF8.GetByteCount(value);
				Write(byteCount, 4);
				int bytes = Encoding.UTF8.GetBytes(new ReadOnlySpan<char>(pointer, value.Length), GetAndAdvanceData(byteCount));
				Assert.Check(byteCount == bytes, "Expected byte count mismatch: {0} vs {1}", byteCount, bytes);
			}
		}

		public void Write(bool value)
		{
			Write((NetworkBool)value, 4);
		}

		public void Write(bool[] value)
		{
			Write(value.Length, 4);
			foreach (bool flag in value)
			{
				Write((NetworkBool)flag, 4);
			}
		}

		public void Write(string[] value)
		{
			Write(value.Length, 4);
			foreach (string value2 in value)
			{
				Write(value2);
			}
		}

		public static int GetBytePayloadSize(int size)
		{
			return (size + 3) & -4;
		}

		public static int GetBytePayloadSize(int count, int elementSize)
		{
			return (4 + count * elementSize + 3) & -4;
		}

		public static int GetPayloadSize(string str)
		{
			return (4 + (Encoding.UTF8.GetByteCount(str) + 3)) & -4;
		}

		public static int GetPayloadSize(string[] strings)
		{
			int num = 4;
			foreach (string str in strings)
			{
				num += GetPayloadSize(str);
			}
			return num;
		}

		public static int GetPayloadSize(bool value)
		{
			return 4;
		}

		public static int GetPayloadSize(bool[] value)
		{
			return 4 + value.Length * 4;
		}
	}
}
