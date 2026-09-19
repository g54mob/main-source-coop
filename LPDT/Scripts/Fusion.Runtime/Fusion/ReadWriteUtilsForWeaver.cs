#define DEBUG
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Scripting;

namespace Fusion
{
	public static class ReadWriteUtilsForWeaver
	{
		private const float ACCURACY = 1024f;

		private const int STRING_LENGTH_INDEX = 0;

		private const int STRING_HASHCODE_INDEX = 1;

		private const int STRING_DATA_INDEX = 2;

		private const int STRING_NOHASHCODE_DATA_INDEX = 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Preserve]
		public unsafe static bool ReadBoolean(int* data)
		{
			return (*data != 0) ? true : false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Preserve]
		public unsafe static void WriteBoolean(int* data, bool value)
		{
			*data = (value ? 1 : 0);
		}

		[Preserve]
		public unsafe static int GetByteArrayHashCode(byte* ptr, int length)
		{
			return HashCodeUtilities.GetArrayHashCode(ptr, length);
		}

		[Preserve]
		public unsafe static int WriteStringUtf8NoHash(void* destination, string str)
		{
			fixed (char* chars = str)
			{
				int byteCount = Encoding.UTF8.GetByteCount(str);
				int bytes = Encoding.UTF8.GetBytes(chars, str.Length, (byte*)destination + 4, byteCount);
				Assert.Check(byteCount == bytes, "Expected byte count mismatch {0} {1}", byteCount, bytes);
				*(int*)destination = bytes;
				return 4 + bytes;
			}
		}

		[Preserve]
		public unsafe static int ReadStringUtf8NoHash(void* source, out string result)
		{
			int num = *(int*)source;
			result = Encoding.UTF8.GetString((byte*)source + 4, num);
			return num + 4;
		}

		[Preserve]
		public static int GetByteCountUtf8NoHash(string value)
		{
			return 4 + Encoding.UTF8.GetByteCount(value);
		}

		[Preserve]
		public static int GetStringHashCode(string value, int maxLength)
		{
			int len = Math.Min(value.Length, maxLength);
			return value.GetHashDeterministicInternal(len, 352654597);
		}

		[Preserve]
		public unsafe static int WriteStringUtf32NoHash(int* ptr, int maxLength, string value)
		{
			return WriteStringUtf32NoHash(new Span<int>(ptr, maxLength), value);
		}

		[Preserve]
		public static int WriteStringUtf32NoHash(Span<int> ptr, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				ptr[0] = 0;
				return 4;
			}
			UTF32Tools.ConversionResult conversionResult = UTF32Tools.Convert(value, MemoryMarshal.Cast<int, uint>(ptr.Slice(1, ptr.Length - 1)));
			ptr[0] = conversionResult.CodePointCount;
			return (conversionResult.CodePointCount + 1) * 4;
		}

		[Preserve]
		public unsafe static int ReadStringUtf32NoHash(int* ptr, int maxLength, out string result)
		{
			return ReadStringUtf32NoHash(new ReadOnlySpan<int>(ptr, maxLength), out result);
		}

		[Preserve]
		public unsafe static int ReadStringUtf32NoHash(ReadOnlySpan<int> ptr, out string result)
		{
			ReadOnlySpan<int> readOnlySpan = ptr.Slice(1, ptr.Length - 1);
			int num = Math.Min(ptr[0], readOnlySpan.Length);
			if (num == 0)
			{
				result = "";
			}
			else
			{
				fixed (int* value = readOnlySpan)
				{
					result = new string((sbyte*)value, 0, num * 4, Encoding.UTF32);
				}
			}
			return (num + 1) * 4;
		}

		[Preserve]
		public unsafe static int WriteStringUtf32WithHash(int* ptr, int maxLength, string value, ref string cache)
		{
			return WriteStringUtf32WithHash(new Span<int>(ptr, maxLength), value, ref cache);
		}

		[Preserve]
		public static int WriteStringUtf32WithHash(Span<int> ptr, string value, ref string cache)
		{
			if (string.IsNullOrEmpty(value))
			{
				ptr[0] = 0;
				ptr[1] = 0;
				return 8;
			}
			Span<uint> dst = MemoryMarshal.Cast<int, uint>(ptr.Slice(2, ptr.Length - 2));
			UTF32Tools.ConversionResult conversionResult = UTF32Tools.Convert(value, dst);
			ptr[0] = conversionResult.CodePointCount;
			Assert.Check(conversionResult.CharacterCount <= value.Length, "res.CharacterCount <= value.Length");
			if (conversionResult.CharacterCount < value.Length)
			{
				cache = value.Substring(0, conversionResult.CharacterCount);
			}
			else
			{
				cache = value;
			}
			ptr[1] = cache.GetHashDeterministic();
			return (conversionResult.CodePointCount + 2) * 4;
		}

		[Preserve]
		public unsafe static int ReadStringUtf32WithHash(int* ptr, int maxLength, ref string cache)
		{
			return ReadStringUtf32WithHash(new ReadOnlySpan<int>(ptr, maxLength), ref cache);
		}

		[Preserve]
		public unsafe static int ReadStringUtf32WithHash(ReadOnlySpan<int> ptr, ref string cache)
		{
			int num = ptr[1];
			ReadOnlySpan<int> readOnlySpan = ptr.Slice(2, ptr.Length - 2);
			int num2 = Math.Min(ptr[0], readOnlySpan.Length);
			if (num2 == 0)
			{
				cache = "";
			}
			else
			{
				if (cache != null && num2 >= cache.Length / 2 && num2 <= cache.Length && num == cache.GetHashCode() && UTF32Tools.CompareOrdinal(cache, MemoryMarshal.Cast<int, uint>(readOnlySpan.Slice(0, num2))) == 0)
				{
					return (2 + num2) * 4;
				}
				fixed (int* value = readOnlySpan)
				{
					cache = new string((sbyte*)value, 0, num2 * 4, Encoding.UTF32);
				}
			}
			return (2 + num2) * 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Preserve]
		public static int GetWordCountString(int capacity, bool withCaching)
		{
			if (withCaching)
			{
				return 2 + capacity;
			}
			return 1 + capacity;
		}

		[Preserve]
		public static int VerifyRawNetworkUnwrap<T>(int actual, int maxBytes)
		{
			if (actual > maxBytes)
			{
				throw new InvalidOperationException($"Overflow when unwrapping {typeof(T).FullName}: expected max {maxBytes}, got {actual}");
			}
			return actual;
		}

		[Preserve]
		public static int VerifyRawNetworkWrap<T>(int actual, int maxBytes)
		{
			if (actual > maxBytes)
			{
				throw new InvalidOperationException($"Overflow when wrapping {typeof(T).FullName}: expected max {maxBytes}, got {actual}");
			}
			return actual;
		}
	}
}
