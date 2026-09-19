using System;
using System.Runtime.CompilerServices;

namespace Fusion.Sockets
{
	public class ReadBuffer : IDisposable
	{
		private byte[] _buffer = new byte[4096];

		private int _offset;

		private int _length;

		public bool IsEmpty => _offset == 0 && _length == 0;

		public bool IsEOF => _offset >= _length;

		public int Length => _length;

		public int Offset
		{
			get
			{
				return _offset;
			}
			internal set
			{
				_offset = value;
			}
		}

		public short ShortVar()
		{
			return (short)LongVar();
		}

		public ushort UShortVar()
		{
			return (ushort)ULongVar();
		}

		public int IntVar()
		{
			return (int)LongVar();
		}

		public uint UIntVar()
		{
			return (uint)ULongVar();
		}

		public long LongVar()
		{
			return Maths.ZigZagDecode((long)ULongVar());
		}

		public ulong ULongVar()
		{
			ulong num = 0uL;
			int num2 = 0;
			byte b;
			do
			{
				b = Byte();
				num |= ((ulong)b & 0x7FuL) << num2;
				num2 += 7;
			}
			while ((b & 0x80) == 128);
			return num;
		}

		public void Skip(int bytes)
		{
			_offset += bytes;
		}

		private unsafe T Read<T>() where T : unmanaged
		{
			int num = Use(sizeof(T));
			return Unsafe.ReadUnaligned<T>(ref _buffer[num]);
		}

		public byte Byte()
		{
			return Read<byte>();
		}

		public sbyte SByte()
		{
			return Read<sbyte>();
		}

		public ushort UShort()
		{
			return Read<ushort>();
		}

		public short Short()
		{
			return Read<short>();
		}

		public int Int()
		{
			return Read<int>();
		}

		public uint UInt()
		{
			return Read<uint>();
		}

		public long Long()
		{
			return Read<long>();
		}

		public ulong ULong()
		{
			return Read<ulong>();
		}

		public unsafe float Float()
		{
			uint num = UInt();
			return *(float*)(&num);
		}

		public unsafe double Double()
		{
			ulong num = ULong();
			return *(double*)(&num);
		}

		public T Peek<T>() where T : unmanaged
		{
			return Unsafe.ReadUnaligned<T>(ref _buffer[_offset]);
		}

		public void Span(Span<byte> target)
		{
			int start = Use(target.Length);
			_buffer.AsSpan(start, target.Length).CopyTo(target);
		}

		internal unsafe void Fill(Span<byte> data)
		{
			while (_length + data.Length >= _buffer.Length)
			{
				Array.Resize(ref _buffer, _buffer.Length * 2);
			}
			fixed (byte* pointer = &_buffer[_length])
			{
				data.CopyTo(new Span<byte>(pointer, data.Length));
			}
			_length += data.Length;
		}

		private int Use(int size)
		{
			Assert.Always(_offset + size <= _length, "_offset + size <= _length | {0} + {1} <= {2}", _offset, size, _length);
			int offset = _offset;
			_offset += size;
			return offset;
		}

		void IDisposable.Dispose()
		{
			Array.Clear(_buffer, 0, _length);
			_offset = 0;
			_length = 0;
		}

		public static implicit operator ReadOnlySpan<byte>(ReadBuffer buffer)
		{
			return buffer._buffer.AsSpan(0, buffer._length);
		}
	}
}
