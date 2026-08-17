using System.IO;

namespace Mirror.BouncyCastle.Utilities.Encoders
{
	public sealed class Hex
	{
		private static readonly HexEncoder encoder = new HexEncoder();

		public static string ToHexString(byte[] data)
		{
			return ToHexString(data, upperCase: false);
		}

		public static string ToHexString(byte[] data, bool upperCase)
		{
			return ToHexString(data, 0, data.Length, upperCase);
		}

		public static string ToHexString(byte[] data, int off, int length, bool upperCase)
		{
			string text = Strings.FromAsciiByteArray(Encode(data, off, length));
			if (upperCase)
			{
				text = text.ToUpperInvariant();
			}
			return text;
		}

		public static byte[] Encode(byte[] data, int off, int length)
		{
			MemoryStream memoryStream = new MemoryStream(length * 2);
			encoder.Encode(data, off, length, memoryStream);
			return memoryStream.ToArray();
		}

		public static byte[] DecodeStrict(string str)
		{
			return encoder.DecodeStrict(str, 0, str.Length);
		}
	}
}
