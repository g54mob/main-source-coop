using System.Collections.Concurrent;
using System.Text;
using QFSW.QC.Pooling;
using UnityEngine;

namespace QFSW.QC.Utilities
{
	public static class ColorExtensions
	{
		private static readonly ConcurrentStringBuilderPool _stringBuilderPool = new ConcurrentStringBuilderPool();

		private static readonly ConcurrentDictionary<int, string> _colorLookupTable = new ConcurrentDictionary<int, string>();

		public static string ColorText(this string text, Color color)
		{
			StringBuilder stringBuilder = _stringBuilderPool.GetStringBuilder(text.Length + 10);
			stringBuilder.AppendColoredText(text, color);
			return _stringBuilderPool.ReleaseAndToString(stringBuilder);
		}

		public static void AppendColoredText(this StringBuilder stringBuilder, string text, Color color)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				stringBuilder.Append(text);
			}
			string value = Color32ToStringNonAlloc(color);
			stringBuilder.Append("<#");
			stringBuilder.Append(value);
			stringBuilder.Append('>');
			stringBuilder.Append(text);
			stringBuilder.Append("</color>");
		}

		public unsafe static string Color32ToStringNonAlloc(Color32 color)
		{
			int key = (color.r << 24) | (color.g << 16) | (color.b << 8) | color.a;
			if (_colorLookupTable.ContainsKey(key))
			{
				return _colorLookupTable[key];
			}
			char* ptr = stackalloc char[8];
			Color32ToHexNonAlloc(color, ptr);
			int length = ((color.a < 255) ? 8 : 6);
			string text = new string(ptr, 0, length);
			_colorLookupTable[key] = text;
			return text;
		}

		private unsafe static void Color32ToHexNonAlloc(Color32 color, char* buffer)
		{
			ByteToHex(color.r, out *buffer, out buffer[1]);
			ByteToHex(color.g, out buffer[2], out buffer[3]);
			ByteToHex(color.b, out buffer[4], out buffer[5]);
			ByteToHex(color.a, out buffer[6], out buffer[7]);
		}

		private static void ByteToHex(byte value, out char dig1, out char dig2)
		{
			dig1 = NibbleToHex((byte)(value >> 4));
			dig2 = NibbleToHex((byte)(value & 0xF));
		}

		private static char NibbleToHex(byte nibble)
		{
			if (nibble < 10)
			{
				return (char)(48 + nibble);
			}
			return (char)(65 + nibble - 10);
		}
	}
}
