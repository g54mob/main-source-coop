using System;
using System.IO;

namespace Mirror.BouncyCastle.Utilities.Encoders
{
	public class HexEncoder
	{
		private static readonly char[] CharsLower = new char[16]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'a', 'b', 'c', 'd', 'e', 'f'
		};

		private static readonly char[] CharsUpper = new char[16]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};

		protected readonly byte[] encodingTable = new byte[16]
		{
			48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
			97, 98, 99, 100, 101, 102
		};

		protected readonly byte[] decodingTable = new byte[128];

		protected void InitialiseDecodingTable()
		{
			Arrays.Fill(decodingTable, 255);
			for (int i = 0; i < encodingTable.Length; i++)
			{
				decodingTable[encodingTable[i]] = (byte)i;
			}
			decodingTable[65] = decodingTable[97];
			decodingTable[66] = decodingTable[98];
			decodingTable[67] = decodingTable[99];
			decodingTable[68] = decodingTable[100];
			decodingTable[69] = decodingTable[101];
			decodingTable[70] = decodingTable[102];
		}

		public HexEncoder()
		{
			InitialiseDecodingTable();
		}

		public int Encode(byte[] inBuf, int inOff, int inLen, byte[] outBuf, int outOff)
		{
			int num = inOff;
			int num2 = inOff + inLen;
			int num3 = outOff;
			while (num < num2)
			{
				uint num4 = inBuf[num++];
				outBuf[num3++] = encodingTable[num4 >> 4];
				outBuf[num3++] = encodingTable[num4 & 0xF];
			}
			return num3 - outOff;
		}

		public int Encode(byte[] buf, int off, int len, Stream outStream)
		{
			if (len < 0)
			{
				return 0;
			}
			byte[] array = new byte[72];
			int num = len;
			while (num > 0)
			{
				int num2 = System.Math.Min(36, num);
				int count = Encode(buf, off, num2, array, 0);
				outStream.Write(array, 0, count);
				off += num2;
				num -= num2;
			}
			return len * 2;
		}

		internal byte[] DecodeStrict(string str, int off, int len)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (off < 0 || len < 0 || off > str.Length - len)
			{
				throw new IndexOutOfRangeException("invalid offset and/or length specified");
			}
			if ((len & 1) != 0)
			{
				throw new ArgumentException("a hexadecimal encoding must have an even number of characters", "len");
			}
			int num = len >> 1;
			byte[] array = new byte[num];
			int num2 = off;
			for (int i = 0; i < num; i++)
			{
				byte b = decodingTable[(uint)str[num2++]];
				byte b2 = decodingTable[(uint)str[num2++]];
				if ((b | b2) >= 128)
				{
					throw new IOException("invalid characters encountered in Hex data");
				}
				array[i] = (byte)((b << 4) | b2);
			}
			return array;
		}
	}
}
