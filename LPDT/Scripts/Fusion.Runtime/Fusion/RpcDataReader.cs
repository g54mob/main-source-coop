using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Fusion
{
	public ref struct RpcDataReader
	{
		private readonly ReadOnlySpan<byte> _data;

		private int _offset;

		public RpcDataReader(ReadOnlySpan<byte> data)
		{
			_data = data;
			_offset = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int AlignOffset(int offset)
		{
			return (offset + 3) & -4;
		}

		private ReadOnlySpan<byte> GetAndAdvanceData(int bytes)
		{
			ReadOnlySpan<byte> result = _data.Slice(_offset, bytes);
			_offset = AlignOffset(_offset + bytes);
			return result;
		}

		public void Read<T>(out T value, int size) where T : unmanaged
		{
			value = MemoryMarshal.Read<T>(GetAndAdvanceData(size));
		}

		public void Read<T>(out T[] value, int elementSize) where T : unmanaged
		{
			Read(out int value2, 4);
			value = new T[value2];
			ReadOnlySpan<byte> andAdvanceData = GetAndAdvanceData(value2 * elementSize);
			MemoryMarshal.Cast<byte, T>(andAdvanceData).CopyTo(value);
		}

		public void Read<T>(out ReadOnlySpan<T> value, int elementSize) where T : unmanaged
		{
			Read(out int value2, 4);
			ReadOnlySpan<byte> andAdvanceData = GetAndAdvanceData(value2 * elementSize);
			value = MemoryMarshal.Cast<byte, T>(andAdvanceData);
		}

		public void ReadCount<T>(out T[] array)
		{
			Read(out int value, 4);
			array = new T[value];
		}

		public void Read(out string value)
		{
			Read(out int value2, 4);
			value = Encoding.UTF8.GetString(GetAndAdvanceData(value2));
		}

		public void Read(out string[] value)
		{
			Read(out int value2, 4);
			value = new string[value2];
			for (int i = 0; i < value2; i++)
			{
				Read(out value[i]);
			}
		}

		public void Read(out bool value)
		{
			Read(out NetworkBool value2, 4);
			value = value2;
		}

		public void Read(out bool[] value)
		{
			Read(out int value2, 4);
			value = new bool[value2];
			for (int i = 0; i < value2; i++)
			{
				Read(out NetworkBool value3, 4);
				value[i] = value3;
			}
		}
	}
}
