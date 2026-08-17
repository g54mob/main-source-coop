using System;
using System.Text;

namespace Mirror.BouncyCastle.Utilities
{
	public static class Strings
	{
		public static string FromByteArray(byte[] bs)
		{
			char[] array = new char[bs.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToChar(bs[i]);
			}
			return new string(array);
		}

		public static string FromAsciiByteArray(byte[] bytes)
		{
			return Encoding.ASCII.GetString(bytes);
		}

		public static string FromUtf8ByteArray(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
