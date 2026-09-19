using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.Sockets
{
	internal class WriteBuffer : IDisposable
	{
		public ref struct ResetPoint
		{
			private int _offset;

			private WriteBuffer _buffer;

			public readonly int Offset => _offset;

			public ResetPoint(WriteBuffer buffer)
			{
				_buffer = buffer;
				_offset = buffer._offset;
			}

			public void Use()
			{
				Assert.Always(_buffer != null, "_buffer != null");
				Assert.Always(_offset >= 0, "_offset >= 0");
				Assert.Always(_offset < _buffer._offset, "_offset < _buffer._offset");
				_buffer._offset = _offset;
				_offset = int.MaxValue;
			}
		}

		private int _offset;

		private byte[] _buffer = new byte[4096];

		public int Length => _offset;

		public int Capacity => _buffer.Length;

		public byte[] Buffer => _buffer;

		public int Offset => _offset;

		public void Reset()
		{
			_offset = 0;
			Array.Clear(_buffer, 0, _buffer.Length);
		}

		public Span<byte> GetSpan(int offset, int length)
		{
			Assert.Always(offset >= 0, "offset >= 0");
			Assert.Always(offset + length <= _offset, "offset + length <= _offset");
			return _buffer.AsSpan(offset, length);
		}

		public ResetPoint GetResetPoint()
		{
			return new ResetPoint(this);
		}

		private int Use(int length)
		{
			if (_offset + length >= _buffer.Length)
			{
				if (length > _buffer.Length)
				{
					Array.Resize(ref _buffer, _buffer.Length + length);
				}
				else
				{
					Array.Resize(ref _buffer, _buffer.Length * 2);
				}
			}
			int offset = _offset;
			_offset += length;
			return offset;
		}

		private unsafe void Write<T>(T value) where T : unmanaged
		{
			int num = Use(sizeof(T));
			Unsafe.WriteUnaligned(ref _buffer[num], value);
		}

		public void Byte(byte value)
		{
			Write(value);
		}

		public void Sbyte(sbyte value)
		{
			Write(value);
		}

		public void UShort(ushort value)
		{
			Write(value);
		}

		public void Short(short value)
		{
			Write(value);
		}

		public void UInt(uint value)
		{
			Write(value);
		}

		public void Int(int value)
		{
			Write(value);
		}

		public void Long(long value)
		{
			Write(value);
		}

		public void ULong(ulong value)
		{
			Write(value);
		}

		public void UShortVar(ushort value)
		{
			ULongVar(value);
		}

		public void ShortVar(short value)
		{
			LongVar(value);
		}

		public void UIntVar(uint value)
		{
			ULongVar(value);
		}

		public void IntVar(int value)
		{
			LongVar(value);
		}

		public void LongVar(long value)
		{
			ULongVar((ulong)Maths.ZigZagEncode(value));
		}

		public void ULongVar(ulong value)
		{
			do
			{
				byte b = (byte)(value & 0x7F);
				value >>= 7;
				if (value != 0)
				{
					b |= 0x80;
				}
				Byte(b);
			}
			while (value != 0);
		}

		public unsafe void Float(float value)
		{
			UInt(*(uint*)(&value));
		}

		public unsafe void Double(double value)
		{
			ULong(*(ulong*)(&value));
		}

		public void Span(Span<byte> data)
		{
			int num = Use(data.Length);
			Span<byte> destination = MemoryMarshal.CreateSpan(ref _buffer[num], data.Length);
			data.CopyTo(destination);
		}

		void IDisposable.Dispose()
		{
			if (_offset > 0)
			{
				Array.Clear(_buffer, 0, _offset);
			}
			_offset = 0;
		}
	}
}
