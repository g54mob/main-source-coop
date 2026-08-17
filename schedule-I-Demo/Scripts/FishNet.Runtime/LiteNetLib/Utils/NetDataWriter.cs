using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace LiteNetLib.Utils
{
	public class NetDataWriter
	{
		protected byte[] _data;

		protected int _position;

		private const int InitialSize = 64;

		private readonly bool _autoResize;

		private static readonly UTF8Encoding _uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

		public const int StringBufferMaxLength = 32768;

		private readonly byte[] _stringBuffer = new byte[32768];

		public int Capacity => _data.Length;

		public byte[] Data => _data;

		public int Length => _position;

		public NetDataWriter()
			: this(autoResize: true, 64)
		{
		}

		public NetDataWriter(bool autoResize)
			: this(autoResize, 64)
		{
		}

		public NetDataWriter(bool autoResize, int initialSize)
		{
			_data = new byte[initialSize];
			_autoResize = autoResize;
		}

		public static NetDataWriter FromBytes(byte[] bytes, bool copy)
		{
			if (copy)
			{
				NetDataWriter netDataWriter = new NetDataWriter(autoResize: true, bytes.Length);
				netDataWriter.Put(bytes);
				return netDataWriter;
			}
			return new NetDataWriter(autoResize: true, 0)
			{
				_data = bytes,
				_position = bytes.Length
			};
		}

		public static NetDataWriter FromBytes(byte[] bytes, int offset, int length)
		{
			NetDataWriter netDataWriter = new NetDataWriter(autoResize: true, bytes.Length);
			netDataWriter.Put(bytes, offset, length);
			return netDataWriter;
		}

		public static NetDataWriter FromString(string value)
		{
			NetDataWriter netDataWriter = new NetDataWriter();
			netDataWriter.Put(value);
			return netDataWriter;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResizeIfNeed(int newSize)
		{
			if (_data.Length < newSize)
			{
				Array.Resize(ref _data, Math.Max(newSize, _data.Length * 2));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnsureFit(int additionalSize)
		{
			if (_data.Length < _position + additionalSize)
			{
				Array.Resize(ref _data, Math.Max(_position + additionalSize, _data.Length * 2));
			}
		}

		public void Reset(int size)
		{
			ResizeIfNeed(size);
			_position = 0;
		}

		public void Reset()
		{
			_position = 0;
		}

		public byte[] CopyData()
		{
			byte[] array = new byte[_position];
			Buffer.BlockCopy(_data, 0, array, 0, _position);
			return array;
		}

		public int SetPosition(int position)
		{
			int position2 = _position;
			_position = position;
			return position2;
		}

		public void Put(float value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 4);
			}
			FastBitConverter.GetBytes(_data, _position, value);
			_position += 4;
		}

		public void Put(double value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 8);
			}
			FastBitConverter.GetBytes(_data, _position, value);
			_position += 8;
		}

		public void Put(long value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 8);
			}
			FastBitConverter.GetBytes(_data, _position, value);
			_position += 8;
		}

		public void Put(ulong value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 8);
			}
			FastBitConverter.GetBytes(_data, _position, value);
			_position += 8;
		}

		public void Put(int value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 4);
			}
			FastBitConverter.GetBytes(_data, _position, value);
			_position += 4;
		}

		public void Put(uint value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 4);
			}
			FastBitConverter.GetBytes(_data, _position, value);
			_position += 4;
		}

		public void Put(char value)
		{
			Put((ushort)value);
		}

		public void Put(ushort value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 2);
			}
			FastBitConverter.GetBytes(_data, _position, value);
			_position += 2;
		}

		public void Put(short value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 2);
			}
			FastBitConverter.GetBytes(_data, _position, value);
			_position += 2;
		}

		public void Put(sbyte value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 1);
			}
			_data[_position] = (byte)value;
			_position++;
		}

		public void Put(byte value)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + 1);
			}
			_data[_position] = value;
			_position++;
		}

		public void Put(byte[] data, int offset, int length)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + length);
			}
			Buffer.BlockCopy(data, offset, _data, _position, length);
			_position += length;
		}

		public void Put(byte[] data)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + data.Length);
			}
			Buffer.BlockCopy(data, 0, _data, _position, data.Length);
			_position += data.Length;
		}

		public void PutSBytesWithLength(sbyte[] data, int offset, int length)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + length + 4);
			}
			FastBitConverter.GetBytes(_data, _position, length);
			Buffer.BlockCopy(data, offset, _data, _position + 4, length);
			_position += length + 4;
		}

		public void PutSBytesWithLength(sbyte[] data)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + data.Length + 4);
			}
			FastBitConverter.GetBytes(_data, _position, data.Length);
			Buffer.BlockCopy(data, 0, _data, _position + 4, data.Length);
			_position += data.Length + 4;
		}

		public void PutBytesWithLength(byte[] data, int offset, int length)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + length + 4);
			}
			FastBitConverter.GetBytes(_data, _position, length);
			Buffer.BlockCopy(data, offset, _data, _position + 4, length);
			_position += length + 4;
		}

		public void PutBytesWithLength(byte[] data)
		{
			if (_autoResize)
			{
				ResizeIfNeed(_position + data.Length + 4);
			}
			FastBitConverter.GetBytes(_data, _position, data.Length);
			Buffer.BlockCopy(data, 0, _data, _position + 4, data.Length);
			_position += data.Length + 4;
		}

		public void Put(bool value)
		{
			Put((byte)(value ? 1u : 0u));
		}

		private void PutArray(Array arr, int sz)
		{
			ushort num = (ushort)((arr != null) ? ((ushort)arr.Length) : 0);
			sz *= num;
			if (_autoResize)
			{
				ResizeIfNeed(_position + sz + 2);
			}
			FastBitConverter.GetBytes(_data, _position, num);
			if (arr != null)
			{
				Buffer.BlockCopy(arr, 0, _data, _position + 2, sz);
			}
			_position += sz + 2;
		}

		public void PutArray(float[] value)
		{
			PutArray(value, 4);
		}

		public void PutArray(double[] value)
		{
			PutArray(value, 8);
		}

		public void PutArray(long[] value)
		{
			PutArray(value, 8);
		}

		public void PutArray(ulong[] value)
		{
			PutArray(value, 8);
		}

		public void PutArray(int[] value)
		{
			PutArray(value, 4);
		}

		public void PutArray(uint[] value)
		{
			PutArray(value, 4);
		}

		public void PutArray(ushort[] value)
		{
			PutArray(value, 2);
		}

		public void PutArray(short[] value)
		{
			PutArray(value, 2);
		}

		public void PutArray(bool[] value)
		{
			PutArray(value, 1);
		}

		public void PutArray(string[] value)
		{
			ushort num = (ushort)((value != null) ? ((ushort)value.Length) : 0);
			Put(num);
			for (int i = 0; i < num; i++)
			{
				Put(value[i]);
			}
		}

		public void PutArray(string[] value, int strMaxLength)
		{
			ushort num = (ushort)((value != null) ? ((ushort)value.Length) : 0);
			Put(num);
			for (int i = 0; i < num; i++)
			{
				Put(value[i], strMaxLength);
			}
		}

		public void Put(IPEndPoint endPoint)
		{
			Put(endPoint.Address.ToString());
			Put(endPoint.Port);
		}

		public void Put(string value)
		{
			Put(value, 0);
		}

		public void Put(string value, int maxLength)
		{
			if (value == null)
			{
				Put((ushort)0);
				return;
			}
			int charCount = ((maxLength > 0 && value.Length > maxLength) ? maxLength : value.Length);
			int bytes = _uTF8Encoding.GetBytes(value, 0, charCount, _stringBuffer, 0);
			if (bytes >= 32768)
			{
				Put((ushort)0);
				return;
			}
			Put(checked((ushort)(bytes + 1)));
			Put(_stringBuffer, 0, bytes);
		}

		public void Put<T>(T obj) where T : INetSerializable
		{
			obj.Serialize(this);
		}
	}
}
