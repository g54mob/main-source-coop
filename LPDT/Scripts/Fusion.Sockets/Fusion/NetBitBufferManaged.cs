#define DEBUG
using System;
using System.Runtime.CompilerServices;

namespace Fusion
{
	internal struct NetBitBufferManaged
	{
		private const int BITCOUNT = 64;

		private const int USEDMASK = 63;

		private const int INDEXSHIFT = 6;

		private const ulong MAXVALUE = ulong.MaxValue;

		private ulong[] _data;

		private int _offsetBits;

		private int _lengthBits;

		private int _lengthBytes;

		public ulong[] Data
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return _data;
			}
			internal set
			{
				_data = value;
			}
		}

		public int OffsetBits
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return _offsetBits;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal set
			{
				Assert.Check(value >= 0 && value <= _lengthBits, "value >= 0 && value <= _lengthBits");
				_offsetBits = value;
			}
		}

		public readonly bool IsOnEvenByte
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _offsetBits % 8 == 0;
			}
		}

		public readonly int OffsetBytes
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				Assert.Check(IsOnEvenByte, "IsOnEvenByte");
				Assert.Check(Maths.BytesRequiredForBits(_offsetBits) == _offsetBits / 8, "Maths.BytesRequiredForBits(_offsetBits) == _offsetBits / 8");
				return _offsetBits / 8;
			}
		}

		public readonly bool DoneOrOverflow
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _offsetBits >= _lengthBits;
			}
		}

		public readonly bool MoreToRead
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _offsetBits < _lengthBits;
			}
		}

		public readonly int LengthBits
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _lengthBits;
			}
		}

		public int LengthBytes
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return _lengthBytes;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal set
			{
				Assert.Check(value >= 0, "value >= 0");
				_lengthBits = value << 3;
				_lengthBytes = value;
			}
		}

		public NetBitBufferManaged(int bytes)
		{
			_offsetBits = 0;
			_lengthBits = 0;
			_lengthBytes = 0;
			_data = new ulong[(bytes + 7) / 8];
			OffsetBits = 0;
			LengthBytes = bytes;
		}

		public unsafe NetBitBufferManaged(byte[] bytes)
		{
			_offsetBits = 0;
			_lengthBits = 0;
			_lengthBytes = 0;
			_data = new ulong[(bytes.Length + 7) / 8];
			OffsetBits = 0;
			LengthBytes = bytes.Length;
			fixed (byte* source = bytes)
			{
				fixed (ulong* data = _data)
				{
					FusionUnsafe.Copy(data, source, bytes.Length);
				}
			}
		}

		public bool CanRead(int bits)
		{
			return _offsetBits + bits <= _lengthBits;
		}

		public bool ReadBoolean()
		{
			return Read(1) == 1;
		}

		public bool PeekBoolean()
		{
			return Peek(1) == 1;
		}

		public void WriteInt64VarLength(long value, int blockSize)
		{
			WriteUInt64VarLength((ulong)value, blockSize);
		}

		public void WriteInt32VarLength(int value)
		{
			WriteUInt32VarLength((uint)value);
		}

		public void WriteInt32VarLength(int value, int blockSize)
		{
			WriteUInt32VarLength((uint)value, blockSize);
		}

		public int ReadInt32VarLength()
		{
			return (int)ReadUInt32VarLength();
		}

		public long ReadInt64VarLength(int blockSize)
		{
			return (long)ReadUInt64VarLength(blockSize);
		}

		public int ReadInt32VarLength(int blockSize)
		{
			return (int)ReadUInt32VarLength(blockSize);
		}

		public uint ReadUInt32VarLength(int blockSize)
		{
			blockSize = Maths.Clamp(blockSize, 2, 16);
			int num = 1;
			while (!ReadBoolean() && !DoneOrOverflow)
			{
				num++;
			}
			if (DoneOrOverflow)
			{
				return 0u;
			}
			return (uint)ReadUInt64(num * blockSize);
		}

		public ulong ReadUInt64VarLength(int blockSize)
		{
			blockSize = Maths.Clamp(blockSize, 2, 16);
			int num = 1;
			while (!ReadBoolean() && !DoneOrOverflow)
			{
				num++;
			}
			if (DoneOrOverflow)
			{
				return 0uL;
			}
			return ReadUInt64(num * blockSize);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong ReadUInt64(int bits = 64)
		{
			Assert.Check(bits >= 0 && bits <= 64, bits);
			return Read(bits);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteInt32(int value, int bits = 32)
		{
			Assert.Check(bits >= 0 && bits <= 32, "bits >= 0 && bits <= 32");
			Write((ulong)value, bits);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ReadInt32(int bits = 32)
		{
			Assert.Check(bits >= 0 && bits <= 32, bits);
			return (int)Read(bits);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteUInt32(uint value, int bits = 32)
		{
			Assert.Check(bits >= 0 && bits <= 32, "bits >= 0 && bits <= 32");
			Write(value, bits);
		}

		public void WriteUInt32VarLength(uint value, int blockSize)
		{
			blockSize = Maths.Clamp(blockSize, 2, 16);
			int num = (Maths.BitScanReverse(value) + blockSize) / blockSize;
			WriteUInt32((uint)(1 << num - 1), num);
			WriteUInt64(value, num * blockSize);
		}

		public void WriteUInt64VarLength(ulong value, int blockSize)
		{
			blockSize = Maths.Clamp(blockSize, 2, 16);
			int num = (Maths.BitScanReverse(value) + blockSize) / blockSize;
			WriteUInt32((uint)(1 << num - 1), num);
			WriteUInt64(value, num * blockSize);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteUInt64(ulong value, int bits = 64)
		{
			Assert.Check(bits >= 0 && bits <= 64, bits);
			Write(value, bits);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte ReadByte(int bits = 8)
		{
			Assert.Check(bits >= 0 && bits <= 8, bits);
			return (byte)Read(bits);
		}

		internal uint? TryReadUInt32VarLength(int blockSize)
		{
			blockSize = Maths.Clamp(blockSize, 2, 16);
			int num = 1;
			while (CanRead(1) && !ReadBoolean() && !DoneOrOverflow)
			{
				num++;
			}
			if (DoneOrOverflow)
			{
				return null;
			}
			int bits = num * blockSize;
			if (CanRead(bits))
			{
				return (uint)Read(bits);
			}
			return null;
		}

		public unsafe void WriteUInt32VarLength(uint value)
		{
			int num = 0;
			ulong value2 = 0uL;
			byte* ptr = (byte*)(&value2);
			while (true)
			{
				ptr[num] = (byte)(value & 0x7F);
				value >>= 7;
				if (value == 0)
				{
					break;
				}
				byte* num2 = ptr + num++;
				*num2 |= 0x80;
			}
			Write(value2, (num + 1) * 8);
		}

		public bool CanReadUInt32VarLength()
		{
			int offsetBits = _offsetBits;
			for (int i = 0; i < 5; i++)
			{
				if (_lengthBits - _offsetBits < 8)
				{
					break;
				}
				byte b = ReadByte();
				if ((b & 0x80) == 0)
				{
					_offsetBits = offsetBits;
					return true;
				}
			}
			_offsetBits = offsetBits;
			return false;
		}

		public unsafe uint ReadUInt32VarLength()
		{
			Assert.Check(_offsetBits < _lengthBits, "_offsetBits < _lengthBits");
			int num = _lengthBits - _offsetBits;
			if (num > 64)
			{
				num = 64;
			}
			ulong num2 = Peek(num);
			int num3 = 0;
			uint num4 = 0u;
			byte* ptr = (byte*)(&num2);
			while (true)
			{
				Assert.Always(num3 >= 0 && num3 <= 7, "o >= 0 && o <= 7");
				uint num5 = ptr[num3];
				num4 |= (num5 & 0x7F) << 7 * num3;
				if ((num5 & 0x80) != 128)
				{
					break;
				}
				num3++;
			}
			_offsetBits += (num3 + 1) * 8;
			return num4;
		}

		public void Clear()
		{
			Array.Clear(_data, 0, _data.Length);
		}

		public void WriteSlow(ulong value, int bits)
		{
			Assert.Check(bits >= 0 && bits <= 64, bits);
			if (bits > 0)
			{
				value &= ulong.MaxValue >> 64 - bits;
				int num = Advance(bits, writing: false);
				int num2 = num >> 6;
				int num3 = num & 0x3F;
				int num4 = 64 - num3;
				int num5 = num4 - bits;
				ulong[] data = _data;
				if (num5 >= 0)
				{
					ulong num6 = (ulong.MaxValue >> num4) | (ulong)(-1L << 64 - num5);
					data[num2] = (data[num2] & num6) | (value << num3);
				}
				else
				{
					data[num2] = (data[num2] & (ulong.MaxValue >> num4)) | (value << num3);
					data[num2 + 1] = (data[num2 + 1] & (ulong)(-1L << bits - num4)) | (value >> num4);
				}
			}
		}

		private ulong Read(int bits)
		{
			Assert.Always(bits >= 0 && bits <= 64, bits);
			if (bits <= 0)
			{
				return 0uL;
			}
			int num = Advance(bits, writing: false);
			int num2 = num >> 6;
			int num3 = num & 0x3F;
			ulong num4 = _data[num2] >> num3;
			int num5 = bits - (64 - num3);
			ulong result;
			if (num5 < 1)
			{
				result = num4 & (ulong.MaxValue >> 64 - bits);
			}
			else
			{
				ulong num6 = _data[num2 + 1] & (ulong.MaxValue >> 64 - num5);
				result = num4 | (num6 << bits - num5);
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteByte(byte value, int bits = 8)
		{
			Assert.Check(bits >= 0 && bits <= 8, "bits >= 0 && bits <= 8");
			Write(value, bits);
		}

		public bool CheckBitCount(int count)
		{
			return count >= 0 && OffsetBits + count <= _lengthBits;
		}

		public void PadToByteBoundary()
		{
			if (_offsetBits % 8 != 0)
			{
				WriteByte(0, 8 - _offsetBits % 8);
			}
		}

		private ulong Peek(int bits)
		{
			Assert.Check(bits >= 0 && bits <= 64, bits);
			if (bits <= 0)
			{
				return 0uL;
			}
			if (!CheckBitCount(bits))
			{
				throw new InvalidOperationException($"Out of bounds. Bit position: {_offsetBits}, length: {bits}, capacity: {LengthBits}");
			}
			int offsetBits = _offsetBits;
			int num = offsetBits >> 6;
			int num2 = offsetBits & 0x3F;
			ulong num3 = _data[num] >> num2;
			int num4 = bits - (64 - num2);
			ulong result;
			if (num4 < 1)
			{
				result = num3 & (ulong.MaxValue >> 64 - bits);
			}
			else
			{
				ulong num5 = _data[num + 1] & (ulong.MaxValue >> 64 - num4);
				result = num3 | (num5 << bits - num4);
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal int Advance(int bits, bool writing)
		{
			int offsetBits = _offsetBits;
			_offsetBits += bits;
			if (_offsetBits > LengthBits)
			{
				if (!writing)
				{
					throw new InvalidOperationException($"Tried to read out of bounds, position: {offsetBits}, reading: {bits}, capacity: {LengthBits}");
				}
				int num = LengthBytes * 2;
				ulong[] array = new ulong[(num + 7) / 8];
				Array.Copy(_data, array, _data.Length);
				_data = array;
				LengthBytes = num;
			}
			return offsetBits;
		}

		public void Write(ulong value, int bits)
		{
			Assert.Check(bits >= 0 && bits <= 64, bits);
			if (bits > 0)
			{
				value &= ulong.MaxValue >> 64 - bits;
				Assert.Check(bits >= 0 && bits <= 64, "bits >= 0 && bits <= 64");
				int num = Advance(bits, writing: true);
				int num2 = num & 0x3F;
				int num3 = 64 - num2;
				Assert.Check(num2 + num3 == 64, "(bitsUsed + bitsFree) == 64");
				int num4 = num >> 6;
				bool arg = false;
				_data[num4] = (_data[num4] & (ulong)((1L << num2) - 1)) | (value << num2);
				if (num3 < bits)
				{
					arg = true;
					_data[num4 + 1] = value >> num3;
				}
				int offsetBits = _offsetBits;
				_offsetBits = num;
				ulong num5 = Read(bits);
				Assert.Check(num5 == value, num5, value, arg);
				_offsetBits = offsetBits;
			}
		}
	}
}
