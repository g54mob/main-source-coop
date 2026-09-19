#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fusion
{
	public static class UTF32Tools
	{
		public struct CharEnumerator : IEnumerator<char>, IEnumerator, IDisposable
		{
			private int _index;

			private int _length;

			private char _pendingLowSurrogate;

			private unsafe uint* _ptr;

			public char Current { get; private set; }

			object IEnumerator.Current => Current;

			internal unsafe CharEnumerator(uint* utf32, int length)
			{
				_index = 0;
				Current = (_pendingLowSurrogate = '\0');
				_ptr = utf32;
				_length = length;
			}

			public void Dispose()
			{
			}

			public unsafe bool MoveNext()
			{
				if (_pendingLowSurrogate != 0)
				{
					Current = _pendingLowSurrogate;
					_pendingLowSurrogate = '\0';
					return true;
				}
				if (_index >= _length)
				{
					return false;
				}
				(Current, _pendingLowSurrogate) = ToUTF16(_ptr[_index++]);
				return true;
			}

			public void Reset()
			{
				_index = 0;
			}
		}

		public readonly struct ConversionResult
		{
			public readonly int CharacterCount;

			public readonly int CodePointCount;

			public ConversionResult(int words, int characters)
			{
				CodePointCount = words;
				CharacterCount = characters;
			}
		}

		public unsafe static ConversionResult Convert(string str, uint* dst, int dstCapacity)
		{
			if (string.IsNullOrEmpty(str))
			{
				return default(ConversionResult);
			}
			fixed (char* str2 = str)
			{
				return Convert(str2, str.Length, dst, dstCapacity);
			}
		}

		public unsafe static ConversionResult Convert(char* str, int strLength, uint* dst, int dstCapacity)
		{
			return Convert(new ReadOnlySpan<char>(str, strLength), new Span<uint>(dst, dstCapacity));
		}

		public static ConversionResult Convert(string str, Span<uint> dst)
		{
			if (string.IsNullOrEmpty(str))
			{
				return default(ConversionResult);
			}
			return Convert(str.AsSpan(), dst);
		}

		public static ConversionResult Convert(ReadOnlySpan<char> str, Span<uint> dst)
		{
			int num = 0;
			int num2 = 0;
			while (num < dst.Length && num2 < str.Length)
			{
				char c = str[num2];
				if ((uint)(c - 55296) >= 2048u)
				{
					dst[num] = c;
				}
				else
				{
					if (!char.IsHighSurrogate(c) || num2 >= str.Length - 1 || !char.IsLowSurrogate(str[num2 + 1]))
					{
						Assert.AlwaysFail($"Failed to convert character {c}");
						break;
					}
					char c2 = c;
					char c3 = str[++num2];
					dst[num] = (uint)((c2 - 55296) * 1024 + (c3 - 56320) + 65536);
				}
				num++;
				num2++;
			}
			return new ConversionResult(num, num2);
		}

		internal static int CompareOrdinal(ReadOnlySpan<uint> strA, ReadOnlySpan<uint> strB, bool ignoreCase)
		{
			int num = Math.Min(strA.Length, strB.Length);
			if (!ignoreCase)
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = (int)(strA[i] - strB[i]);
					if (num2 != 0)
					{
						return num2;
					}
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					if (!IsValidCodePoint(strA[j]))
					{
						Assert.AlwaysFail($"Failed to convert character {strA[j]}");
						continue;
					}
					if (!IsValidCodePoint(strB[j]))
					{
						Assert.AlwaysFail($"Failed to convert character {strB[j]}");
						continue;
					}
					int num3 = (int)(strA[j] - strB[j]);
					if (num3 == 0)
					{
						continue;
					}
					uint num4 = ToLowerInvariant(strA[j]);
					if (num4 != strB[j])
					{
						uint num5 = ToLowerInvariant(strB[j]);
						if (num4 != num5)
						{
							return num3;
						}
					}
				}
			}
			return strA.Length - strB.Length;
		}

		internal static int CompareOrdinal(string strA, ReadOnlySpan<uint> strB, bool ignoreCase = false)
		{
			if (strA == null)
			{
				throw new ArgumentNullException("strA");
			}
			ReadOnlySpan<char> readOnlySpan = strA.AsSpan();
			int num = 0;
			int num2 = 0;
			while (num < strB.Length && num2 < readOnlySpan.Length)
			{
				char c = readOnlySpan[num2];
				if ((uint)(c - 55296) >= 2048u)
				{
					int num3 = (int)(c - strB[num]);
					if (num3 != 0)
					{
						if (!ignoreCase)
						{
							return num3;
						}
						(char, char) tuple = ToUTF16(strB[num]);
						var (c2, _) = tuple;
						if (tuple.Item2 != 0)
						{
							return num3;
						}
						num3 = char.ToLowerInvariant(c) - char.ToLowerInvariant(c2);
						if (num3 != 0)
						{
							return num3;
						}
					}
				}
				else
				{
					if (!char.IsHighSurrogate(c) || num2 >= readOnlySpan.Length - 1 || !char.IsLowSurrogate(readOnlySpan[num2 + 1]))
					{
						Assert.AlwaysFail($"Failed to convert character {c}");
						break;
					}
					char charOrHighSurrogate = c;
					char lowSurrogate = readOnlySpan[++num2];
					uint num4 = ToUTF32(charOrHighSurrogate, lowSurrogate);
					int result = (int)(num4 - strB[num]);
					if (num4 != strB[num])
					{
						if (!ignoreCase)
						{
							return result;
						}
						uint num5 = ToLowerInvariant(num4);
						if (num5 != strB[num])
						{
							uint num6 = ToLowerInvariant(strB[num]);
							if (num5 != num6)
							{
								return result;
							}
						}
					}
				}
				num++;
				num2++;
			}
			return readOnlySpan.Length - num2 - (strB.Length - num);
		}

		internal static bool EndsWithOrdinal(ReadOnlySpan<uint> strA, ReadOnlySpan<uint> strB, bool ignoreCase = false)
		{
			if (strB.Length > strA.Length)
			{
				return false;
			}
			return CompareOrdinal(strA.Slice(strA.Length - strB.Length), strB, ignoreCase) == 0;
		}

		internal static bool EndsWithOrdinal(ReadOnlySpan<uint> strA, string strB, bool ignoreCase = false)
		{
			if (strB == null)
			{
				throw new ArgumentNullException("strB");
			}
			int byteCount = Encoding.UTF32.GetByteCount(strB);
			Assert.Check(byteCount % 4 == 0, "bytes % 4 == 0");
			int num = byteCount / 4;
			if (strA.Length < num)
			{
				return false;
			}
			return CompareOrdinal(strB, strA.Slice(strA.Length - num, num), ignoreCase) == 0;
		}

		internal static int GetHashDeterministic(ReadOnlySpan<uint> str)
		{
			int a = 352654597;
			int b = a;
			for (int i = 0; i < str.Length; i++)
			{
				(char, char) tuple = ToUTF16(str[i]);
				char item = tuple.Item1;
				char item2 = tuple.Item2;
				a = ((a << 5) + a) ^ item;
				Swap(ref a, ref b);
				if (item2 != 0)
				{
					a = ((a << 5) + a) ^ item2;
					Swap(ref a, ref b);
				}
			}
			return a + b * 1566083941;
		}

		internal static bool StartsWithOrdinal(ReadOnlySpan<uint> strA, ReadOnlySpan<uint> strB, bool ignoreCase = false)
		{
			if (strB.Length > strA.Length)
			{
				return false;
			}
			return CompareOrdinal(strA.Slice(0, strB.Length), strB, ignoreCase) == 0;
		}

		internal static bool StartsWithOrdinal(ReadOnlySpan<uint> strA, string strB, bool ignoreCase = false)
		{
			if (strB == null)
			{
				throw new ArgumentNullException("strB");
			}
			int byteCount = Encoding.UTF32.GetByteCount(strB);
			Assert.Check(byteCount % 4 == 0, "bytes % 4 == 0");
			int num = byteCount / 4;
			if (strA.Length < num)
			{
				return false;
			}
			return CompareOrdinal(strB, strA.Slice(0, num), ignoreCase) == 0;
		}

		internal unsafe static int IndexOf(ReadOnlySpan<uint> str, string pattern)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			if (str.Length == 0)
			{
				return -1;
			}
			int length = GetLength(pattern);
			if (length > str.Length)
			{
				return -1;
			}
			fixed (char* ptr = pattern)
			{
				char* end = ptr + pattern.Length;
				for (int i = 0; i + length <= str.Length; i++)
				{
					char* pstr = ptr;
					int j;
					for (j = 0; j < length; j++)
					{
						uint num = ReadNextCodePoint(ref pstr, end);
						if (str[i + j] != num)
						{
							break;
						}
					}
					if (j == length)
					{
						return i;
					}
				}
			}
			return -1;
		}

		internal static int IndexOf(ReadOnlySpan<uint> str, ReadOnlySpan<uint> pattern)
		{
			if (str.Length == 0 || pattern.Length > str.Length)
			{
				return -1;
			}
			for (int i = 0; i + pattern.Length <= str.Length; i++)
			{
				if (str.Slice(i, pattern.Length).SequenceEqual(pattern))
				{
					return i;
				}
			}
			return -1;
		}

		internal static void ToLowerInvariant(ReadOnlySpan<uint> src, Span<uint> dst)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i] = ToLowerInvariant(src[i]);
			}
		}

		internal static void ToUpperInvariant(ReadOnlySpan<uint> src, Span<uint> dst)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i] = ToUpperInvariant(src[i]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static char GetHighSurrogate(uint scalar)
		{
			return (char)((scalar - 65536) / 1024 + 55296);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetLength(string str)
		{
			int byteCount = Encoding.UTF32.GetByteCount(str);
			Assert.Check(byteCount % 4 == 0, "byteCount % 4 == 0");
			return byteCount / 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static char GetLowSurrogate(uint scalar)
		{
			return (char)((scalar - 65536) % 1024 + 56320);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsValidCodePoint(uint scalar)
		{
			return ((scalar - 1114112) ^ 0xD800) >= 4293855232u;
		}

		private unsafe static uint ReadNextCodePoint(ref char* pstr, char* end)
		{
			char c = *(pstr++);
			if (char.IsHighSurrogate(c))
			{
				Assert.Always(pstr < end, "Surrogate found at the end of the string");
				char c2 = *(pstr++);
				Assert.Check(char.IsLowSurrogate(c2), "char.IsLowSurrogate(cnext)");
				return (uint)((c - 55296) * 1024 + (c2 - 56320) + 65536);
			}
			Assert.Check(!char.IsLowSurrogate(c), "!char.IsLowSurrogate(c)");
			return c;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Swap(ref int a, ref int b)
		{
			int num = a;
			a = b;
			b = num;
		}

		private static uint ToLowerInvariant(uint value)
		{
			var (c, c2) = ToUTF16(value);
			if (c2 == '\0')
			{
				return char.ToLowerInvariant(c);
			}
			Span<char> span = stackalloc char[2];
			span[0] = c;
			span[1] = c2;
			string text = new string(span);
			string text2 = text.ToLowerInvariant();
			Assert.Check(text2.Length == 2, "converted.Length == 2");
			return ToUTF32(text2[0], text2[1]);
		}

		private static uint ToUpperInvariant(uint value)
		{
			var (c, c2) = ToUTF16(value);
			if (c2 == '\0')
			{
				return char.ToUpperInvariant(c);
			}
			Span<char> span = stackalloc char[2];
			span[0] = c;
			span[1] = c2;
			string text = new string(span);
			string text2 = text.ToUpperInvariant();
			Assert.Check(text2.Length == 2, "converted.Length == 2");
			return ToUTF32(text2[0], text2[1]);
		}

		private static (char, char) ToUTF16(uint scalar)
		{
			if (scalar >= 65536)
			{
				return (GetHighSurrogate(scalar), GetLowSurrogate(scalar));
			}
			return ((char)scalar, '\0');
		}

		private static uint ToUTF32(char charOrHighSurrogate, char lowSurrogate = '\0')
		{
			if (char.IsHighSurrogate(charOrHighSurrogate))
			{
				Assert.Check(char.IsLowSurrogate(lowSurrogate), "char.IsLowSurrogate(lowSurrogate)");
				return (uint)((charOrHighSurrogate - 55296) * 1024 + (lowSurrogate - 56320) + 65536);
			}
			Assert.Check(lowSurrogate == '\0', "lowSurrogate == 0");
			return charOrHighSurrogate;
		}
	}
}
