using System;
using System.Net;
using System.Text;

namespace LiteNetLib.Utils
{
	public class NetDataReader
	{
		protected byte[] _data;

		protected int _position;

		protected int _dataSize;

		private int _offset;

		private static readonly UTF8Encoding _uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

		public byte[] RawData => _data;

		public int RawDataSize => _dataSize;

		public int UserDataOffset => _offset;

		public int UserDataSize => _dataSize - _offset;

		public bool IsNull => _data == null;

		public int Position => _position;

		public bool EndOfData => _position == _dataSize;

		public int AvailableBytes => _dataSize - _position;

		public void SkipBytes(int count)
		{
			_position += count;
		}

		public void SetPosition(int position)
		{
			_position = position;
		}

		public void SetSource(NetDataWriter dataWriter)
		{
			_data = dataWriter.Data;
			_position = 0;
			_offset = 0;
			_dataSize = dataWriter.Length;
		}

		public void SetSource(byte[] source)
		{
			_data = source;
			_position = 0;
			_offset = 0;
			_dataSize = source.Length;
		}

		public void SetSource(byte[] source, int offset, int maxSize)
		{
			_data = source;
			_position = offset;
			_offset = offset;
			_dataSize = maxSize;
		}

		public NetDataReader()
		{
		}

		public NetDataReader(NetDataWriter writer)
		{
			SetSource(writer);
		}

		public NetDataReader(byte[] source)
		{
			SetSource(source);
		}

		public NetDataReader(byte[] source, int offset, int maxSize)
		{
			SetSource(source, offset, maxSize);
		}

		public IPEndPoint GetNetEndPoint()
		{
			string hostStr = GetString(1000);
			int port = GetInt();
			return NetUtils.MakeEndPoint(hostStr, port);
		}

		public byte GetByte()
		{
			byte result = _data[_position];
			_position++;
			return result;
		}

		public sbyte GetSByte()
		{
			sbyte result = (sbyte)_data[_position];
			_position++;
			return result;
		}

		public bool[] GetBoolArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			bool[] array = new bool[num];
			Buffer.BlockCopy(_data, _position, array, 0, num);
			_position += num;
			return array;
		}

		public ushort[] GetUShortArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			ushort[] array = new ushort[num];
			Buffer.BlockCopy(_data, _position, array, 0, num * 2);
			_position += num * 2;
			return array;
		}

		public short[] GetShortArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			short[] array = new short[num];
			Buffer.BlockCopy(_data, _position, array, 0, num * 2);
			_position += num * 2;
			return array;
		}

		public long[] GetLongArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			long[] array = new long[num];
			Buffer.BlockCopy(_data, _position, array, 0, num * 8);
			_position += num * 8;
			return array;
		}

		public ulong[] GetULongArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			ulong[] array = new ulong[num];
			Buffer.BlockCopy(_data, _position, array, 0, num * 8);
			_position += num * 8;
			return array;
		}

		public int[] GetIntArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			int[] array = new int[num];
			Buffer.BlockCopy(_data, _position, array, 0, num * 4);
			_position += num * 4;
			return array;
		}

		public uint[] GetUIntArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			uint[] array = new uint[num];
			Buffer.BlockCopy(_data, _position, array, 0, num * 4);
			_position += num * 4;
			return array;
		}

		public float[] GetFloatArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			float[] array = new float[num];
			Buffer.BlockCopy(_data, _position, array, 0, num * 4);
			_position += num * 4;
			return array;
		}

		public double[] GetDoubleArray()
		{
			ushort num = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			double[] array = new double[num];
			Buffer.BlockCopy(_data, _position, array, 0, num * 8);
			_position += num * 8;
			return array;
		}

		public string[] GetStringArray()
		{
			ushort uShort = GetUShort();
			string[] array = new string[uShort];
			for (int i = 0; i < uShort; i++)
			{
				array[i] = GetString();
			}
			return array;
		}

		public string[] GetStringArray(int maxStringLength)
		{
			ushort uShort = GetUShort();
			string[] array = new string[uShort];
			for (int i = 0; i < uShort; i++)
			{
				array[i] = GetString(maxStringLength);
			}
			return array;
		}

		public bool GetBool()
		{
			bool result = _data[_position] > 0;
			_position++;
			return result;
		}

		public char GetChar()
		{
			return (char)GetUShort();
		}

		public ushort GetUShort()
		{
			ushort result = BitConverter.ToUInt16(_data, _position);
			_position += 2;
			return result;
		}

		public short GetShort()
		{
			short result = BitConverter.ToInt16(_data, _position);
			_position += 2;
			return result;
		}

		public long GetLong()
		{
			long result = BitConverter.ToInt64(_data, _position);
			_position += 8;
			return result;
		}

		public ulong GetULong()
		{
			ulong result = BitConverter.ToUInt64(_data, _position);
			_position += 8;
			return result;
		}

		public int GetInt()
		{
			int result = BitConverter.ToInt32(_data, _position);
			_position += 4;
			return result;
		}

		public uint GetUInt()
		{
			uint result = BitConverter.ToUInt32(_data, _position);
			_position += 4;
			return result;
		}

		public float GetFloat()
		{
			float result = BitConverter.ToSingle(_data, _position);
			_position += 4;
			return result;
		}

		public double GetDouble()
		{
			double result = BitConverter.ToDouble(_data, _position);
			_position += 8;
			return result;
		}

		public string GetString(int maxLength)
		{
			ushort uShort = GetUShort();
			if (uShort == 0)
			{
				return null;
			}
			int num = uShort - 1;
			if (num >= 32768)
			{
				return null;
			}
			ArraySegment<byte> bytesSegment = GetBytesSegment(num);
			if (maxLength <= 0 || _uTF8Encoding.GetCharCount(bytesSegment.Array, bytesSegment.Offset, bytesSegment.Count) <= maxLength)
			{
				return _uTF8Encoding.GetString(bytesSegment.Array, bytesSegment.Offset, bytesSegment.Count);
			}
			return string.Empty;
		}

		public string GetString()
		{
			ushort uShort = GetUShort();
			if (uShort == 0)
			{
				return null;
			}
			int num = uShort - 1;
			if (num >= 32768)
			{
				return null;
			}
			ArraySegment<byte> bytesSegment = GetBytesSegment(num);
			return _uTF8Encoding.GetString(bytesSegment.Array, bytesSegment.Offset, bytesSegment.Count);
		}

		public ArraySegment<byte> GetBytesSegment(int count)
		{
			ArraySegment<byte> result = new ArraySegment<byte>(_data, _position, count);
			_position += count;
			return result;
		}

		public ArraySegment<byte> GetRemainingBytesSegment()
		{
			ArraySegment<byte> result = new ArraySegment<byte>(_data, _position, AvailableBytes);
			_position = _data.Length;
			return result;
		}

		public T Get<T>() where T : INetSerializable, new()
		{
			T result = new T();
			result.Deserialize(this);
			return result;
		}

		public byte[] GetRemainingBytes()
		{
			byte[] array = new byte[AvailableBytes];
			Buffer.BlockCopy(_data, _position, array, 0, AvailableBytes);
			_position = _data.Length;
			return array;
		}

		public void GetBytes(byte[] destination, int start, int count)
		{
			Buffer.BlockCopy(_data, _position, destination, start, count);
			_position += count;
		}

		public void GetBytes(byte[] destination, int count)
		{
			Buffer.BlockCopy(_data, _position, destination, 0, count);
			_position += count;
		}

		public sbyte[] GetSBytesWithLength()
		{
			int num = GetInt();
			sbyte[] array = new sbyte[num];
			Buffer.BlockCopy(_data, _position, array, 0, num);
			_position += num;
			return array;
		}

		public byte[] GetBytesWithLength()
		{
			int num = GetInt();
			byte[] array = new byte[num];
			Buffer.BlockCopy(_data, _position, array, 0, num);
			_position += num;
			return array;
		}

		public byte PeekByte()
		{
			return _data[_position];
		}

		public sbyte PeekSByte()
		{
			return (sbyte)_data[_position];
		}

		public bool PeekBool()
		{
			return _data[_position] > 0;
		}

		public char PeekChar()
		{
			return (char)PeekUShort();
		}

		public ushort PeekUShort()
		{
			return BitConverter.ToUInt16(_data, _position);
		}

		public short PeekShort()
		{
			return BitConverter.ToInt16(_data, _position);
		}

		public long PeekLong()
		{
			return BitConverter.ToInt64(_data, _position);
		}

		public ulong PeekULong()
		{
			return BitConverter.ToUInt64(_data, _position);
		}

		public int PeekInt()
		{
			return BitConverter.ToInt32(_data, _position);
		}

		public uint PeekUInt()
		{
			return BitConverter.ToUInt32(_data, _position);
		}

		public float PeekFloat()
		{
			return BitConverter.ToSingle(_data, _position);
		}

		public double PeekDouble()
		{
			return BitConverter.ToDouble(_data, _position);
		}

		public string PeekString(int maxLength)
		{
			ushort num = PeekUShort();
			if (num == 0)
			{
				return null;
			}
			int num2 = num - 1;
			if (num2 >= 32768)
			{
				return null;
			}
			if (maxLength <= 0 || _uTF8Encoding.GetCharCount(_data, _position + 2, num2) <= maxLength)
			{
				return _uTF8Encoding.GetString(_data, _position + 2, num2);
			}
			return string.Empty;
		}

		public string PeekString()
		{
			ushort num = PeekUShort();
			if (num == 0)
			{
				return null;
			}
			int num2 = num - 1;
			if (num2 >= 32768)
			{
				return null;
			}
			return _uTF8Encoding.GetString(_data, _position + 2, num2);
		}

		public bool TryGetByte(out byte result)
		{
			if (AvailableBytes >= 1)
			{
				result = GetByte();
				return true;
			}
			result = 0;
			return false;
		}

		public bool TryGetSByte(out sbyte result)
		{
			if (AvailableBytes >= 1)
			{
				result = GetSByte();
				return true;
			}
			result = 0;
			return false;
		}

		public bool TryGetBool(out bool result)
		{
			if (AvailableBytes >= 1)
			{
				result = GetBool();
				return true;
			}
			result = false;
			return false;
		}

		public bool TryGetChar(out char result)
		{
			if (!TryGetUShort(out var result2))
			{
				result = '\0';
				return false;
			}
			result = (char)result2;
			return true;
		}

		public bool TryGetShort(out short result)
		{
			if (AvailableBytes >= 2)
			{
				result = GetShort();
				return true;
			}
			result = 0;
			return false;
		}

		public bool TryGetUShort(out ushort result)
		{
			if (AvailableBytes >= 2)
			{
				result = GetUShort();
				return true;
			}
			result = 0;
			return false;
		}

		public bool TryGetInt(out int result)
		{
			if (AvailableBytes >= 4)
			{
				result = GetInt();
				return true;
			}
			result = 0;
			return false;
		}

		public bool TryGetUInt(out uint result)
		{
			if (AvailableBytes >= 4)
			{
				result = GetUInt();
				return true;
			}
			result = 0u;
			return false;
		}

		public bool TryGetLong(out long result)
		{
			if (AvailableBytes >= 8)
			{
				result = GetLong();
				return true;
			}
			result = 0L;
			return false;
		}

		public bool TryGetULong(out ulong result)
		{
			if (AvailableBytes >= 8)
			{
				result = GetULong();
				return true;
			}
			result = 0uL;
			return false;
		}

		public bool TryGetFloat(out float result)
		{
			if (AvailableBytes >= 4)
			{
				result = GetFloat();
				return true;
			}
			result = 0f;
			return false;
		}

		public bool TryGetDouble(out double result)
		{
			if (AvailableBytes >= 8)
			{
				result = GetDouble();
				return true;
			}
			result = 0.0;
			return false;
		}

		public bool TryGetString(out string result)
		{
			if (AvailableBytes >= 2)
			{
				ushort num = PeekUShort();
				if (AvailableBytes >= num + 1)
				{
					result = GetString();
					return true;
				}
			}
			result = null;
			return false;
		}

		public bool TryGetStringArray(out string[] result)
		{
			if (!TryGetUShort(out var result2))
			{
				result = null;
				return false;
			}
			result = new string[result2];
			for (int i = 0; i < result2; i++)
			{
				if (!TryGetString(out result[i]))
				{
					result = null;
					return false;
				}
			}
			return true;
		}

		public bool TryGetBytesWithLength(out byte[] result)
		{
			if (AvailableBytes >= 4)
			{
				int num = PeekInt();
				if (num >= 0 && AvailableBytes >= num + 4)
				{
					result = GetBytesWithLength();
					return true;
				}
			}
			result = null;
			return false;
		}

		public void Clear()
		{
			_position = 0;
			_dataSize = 0;
			_data = null;
		}
	}
}
