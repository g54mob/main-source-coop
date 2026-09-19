#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Fusion.Analyzer;
using UnityEngine;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[DebuggerDisplay("{Value}")]
	[NetworkStructWeaved(1, true)]
	public struct NetworkString<TSize> : INetworkString, INetworkStruct, IEquatable<NetworkString<TSize>>, IEnumerable<char>, IEnumerable, IReadOnlySpanAssignable where TSize : unmanaged, IFixedStorage
	{
		[SerializeField]
		internal int _length;

		[SerializeField]
		internal TSize _data;

		public unsafe readonly int Capacity => sizeof(TSize) / 4;

		public string Value
		{
			readonly get
			{
				string cache = null;
				Get(ref cache);
				return cache;
			}
			set
			{
				Set(value);
			}
		}

		public readonly int Length => _length;

		public ref uint this[int index] => ref DataSpanMut[SafeIndex(index)];

		private readonly int SafeLength
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (_length < 0 || _length > Capacity)
				{
					throw new InvalidOperationException($"Invalid Length: {_length}");
				}
				return _length;
			}
		}

		private readonly ReadOnlySpan<uint> DataSpan => MemoryMarshal.Cast<TSize, uint>(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _data), 1));

		private Span<uint> DataSpanMut => MemoryMarshal.Cast<TSize, uint>(MemoryMarshal.CreateSpan(ref _data, 1));

		public NetworkString(string value)
		{
			this = default(NetworkString<TSize>);
			Value = value;
		}

		public NetworkString(int length, uint[] data)
		{
			this = default(NetworkString<TSize>);
			_length = length;
			_data.Set(data);
		}

		public static implicit operator NetworkString<TSize>(string str)
		{
			NetworkString<TSize> result = default(NetworkString<TSize>);
			result.Set(str);
			return result;
		}

		public static explicit operator string(NetworkString<TSize> str)
		{
			return str.Value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(NetworkString<TSize> a, NetworkString<TSize> b)
		{
			return !a.Equals(ref b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(string a, NetworkString<TSize> b)
		{
			return !b.Equals(a);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(NetworkString<TSize> a, string b)
		{
			return !a.Equals(b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(NetworkString<TSize> a, NetworkString<TSize> b)
		{
			return a.Equals(ref b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(string a, NetworkString<TSize> b)
		{
			return b.Equals(a);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(NetworkString<TSize> a, string b)
		{
			return a.Equals(b);
		}

		public readonly bool Get(ref string cache)
		{
			if (cache != null && Compare(cache) == 0)
			{
				return false;
			}
			int safeLength = SafeLength;
			if (safeLength == 0)
			{
				cache = string.Empty;
			}
			else
			{
				cache = Encoding.UTF32.GetString(MemoryMarshal.AsBytes(DataSpan.Slice(0, safeLength)));
			}
			return true;
		}

		public bool Set(string value)
		{
			value = value ?? string.Empty;
			UTF32Tools.ConversionResult conversionResult = UTF32Tools.Convert(value, DataSpanMut);
			_length = conversionResult.CodePointCount;
			return conversionResult.CharacterCount == value.Length;
		}

		public int IndexOf(char c, int startIndex = 0)
		{
			return IndexOf((uint)c, startIndex, Length - startIndex);
		}

		public int IndexOf(char c, int startIndex, int count)
		{
			return IndexOf((uint)c, startIndex, count);
		}

		public int IndexOf(uint codePoint, int startIndex = 0)
		{
			return IndexOf(codePoint, startIndex, Length - startIndex);
		}

		public int IndexOf(uint codePoint, int startIndex, int count)
		{
			int safeLength = SafeLength;
			if (startIndex < 0 || startIndex > safeLength)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (count < 0 || startIndex + count > safeLength)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			ReadOnlySpan<uint> readOnlySpan = DataSpan.Slice(startIndex, count);
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				if (readOnlySpan[i] == codePoint)
				{
					return startIndex + i;
				}
			}
			return -1;
		}

		public int IndexOf(string str, int startIndex = 0)
		{
			return IndexOf(str, startIndex, SafeLength - startIndex);
		}

		public int IndexOf(string str, int startIndex, int count)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (startIndex < 0 || startIndex > SafeLength)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (count < 0 || startIndex + count > SafeLength)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count < str.Length)
			{
			}
			int num = UTF32Tools.IndexOf(DataSpan.Slice(startIndex, count), str);
			return (num < 0) ? num : (num + startIndex);
		}

		public int IndexOf<TOtherSize>(NetworkString<TOtherSize> str, int startIndex = 0) where TOtherSize : unmanaged, IFixedStorage
		{
			return IndexOf(ref str, startIndex, SafeLength - startIndex);
		}

		public int IndexOf<TOtherSize>(NetworkString<TOtherSize> str, int startIndex, int count) where TOtherSize : unmanaged, IFixedStorage
		{
			return IndexOf(ref str, startIndex, count);
		}

		public int IndexOf<TOtherSize>(ref NetworkString<TOtherSize> str, int startIndex = 0) where TOtherSize : unmanaged, IFixedStorage
		{
			return IndexOf(ref str, startIndex, SafeLength - startIndex);
		}

		public int IndexOf<TOtherSize>(ref NetworkString<TOtherSize> str, int startIndex, int count) where TOtherSize : unmanaged, IFixedStorage
		{
			if (startIndex < 0 || startIndex > SafeLength)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (count < 0 || startIndex + count > SafeLength)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count < str.SafeLength)
			{
				return -1;
			}
			int num = UTF32Tools.IndexOf(DataSpan.Slice(startIndex, count), str.DataSpan.Slice(0, str.SafeLength));
			return (num < 0) ? num : (num + startIndex);
		}

		public bool Contains(char c)
		{
			return IndexOf(c) >= 0;
		}

		public bool Contains(uint codePoint)
		{
			return IndexOf(codePoint) >= 0;
		}

		public bool Contains(string str)
		{
			return IndexOf(str) >= 0;
		}

		public bool Contains<TOtherSize>(NetworkString<TOtherSize> str) where TOtherSize : unmanaged, IFixedStorage
		{
			return IndexOf(ref str) >= 0;
		}

		public bool Contains<TOtherSize>(ref NetworkString<TOtherSize> str) where TOtherSize : unmanaged, IFixedStorage
		{
			return IndexOf(ref str) >= 0;
		}

		public NetworkString<TSize> Substring(int startIndex)
		{
			return Substring(startIndex, SafeLength - startIndex);
		}

		public NetworkString<TSize> Substring(int startIndex, int length)
		{
			if (startIndex < 0 || startIndex >= SafeLength)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (length < 0 || startIndex + length > SafeLength)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			NetworkString<TSize> result = new NetworkString<TSize>
			{
				_length = length
			};
			DataSpan.Slice(startIndex, length).CopyTo(result.DataSpanMut);
			return result;
		}

		public NetworkString<TSize> ToLower()
		{
			NetworkString<TSize> result = new NetworkString<TSize>
			{
				_length = SafeLength
			};
			UTF32Tools.ToLowerInvariant(DataSpan.Slice(0, SafeLength), result.DataSpanMut);
			return result;
		}

		public NetworkString<TSize> ToUpper()
		{
			NetworkString<TSize> result = new NetworkString<TSize>
			{
				_length = SafeLength
			};
			UTF32Tools.ToUpperInvariant(DataSpan.Slice(0, SafeLength), result.DataSpanMut);
			return result;
		}

		public int GetCharCount()
		{
			return Encoding.UTF32.GetCharCount(MemoryMarshal.AsBytes(DataSpan.Slice(0, Length)));
		}

		public readonly int Compare(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return UTF32Tools.CompareOrdinal(s, DataSpan.Slice(0, SafeLength));
		}

		public readonly int Compare(NetworkString<TSize> s)
		{
			return UTF32Tools.CompareOrdinal(DataSpan.Slice(0, SafeLength), s.DataSpan.Slice(0, s.SafeLength), ignoreCase: false);
		}

		public readonly int Compare(ref NetworkString<TSize> s)
		{
			return UTF32Tools.CompareOrdinal(DataSpan.Slice(0, SafeLength), s.DataSpan.Slice(0, s.SafeLength), ignoreCase: false);
		}

		public readonly int Compare<TOtherSize>(NetworkString<TOtherSize> other) where TOtherSize : unmanaged, IFixedStorage
		{
			return Compare(ref other);
		}

		public readonly int Compare<TOtherSize>(ref NetworkString<TOtherSize> other) where TOtherSize : unmanaged, IFixedStorage
		{
			return UTF32Tools.CompareOrdinal(DataSpan.Slice(0, SafeLength), other.DataSpan.Slice(0, other.SafeLength), ignoreCase: false);
		}

		public readonly bool Equals(string s)
		{
			return Compare(s) == 0;
		}

		[CanMutate]
		public override bool Equals(object obj)
		{
			if (obj is INetworkString networkString)
			{
				return networkString.Equals(ref this);
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Equals(NetworkString<TSize> other)
		{
			return Compare(ref other) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Equals(ref NetworkString<TSize> other)
		{
			return Compare(ref other) == 0;
		}

		public readonly bool Equals<TOtherSize>(NetworkString<TOtherSize> other) where TOtherSize : unmanaged, IFixedStorage
		{
			return Compare(ref other) == 0;
		}

		public readonly bool Equals<TOtherSize>(ref NetworkString<TOtherSize> other) where TOtherSize : unmanaged, IFixedStorage
		{
			return Compare(ref other) == 0;
		}

		public void Assign(string value)
		{
			Value = value;
		}

		public bool StartsWith(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return UTF32Tools.StartsWithOrdinal(DataSpan.Slice(0, SafeLength), s);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool StartsWith<TOtherSize>(ref NetworkString<TOtherSize> other) where TOtherSize : unmanaged, IFixedStorage
		{
			return UTF32Tools.StartsWithOrdinal(DataSpan.Slice(0, SafeLength), other.DataSpan.Slice(0, other.SafeLength));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool EndsWith<TOtherSize>(ref NetworkString<TOtherSize> other) where TOtherSize : unmanaged, IFixedStorage
		{
			return UTF32Tools.EndsWithOrdinal(DataSpan.Slice(0, SafeLength), other.DataSpan.Slice(0, other.SafeLength));
		}

		public bool EndsWith(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return UTF32Tools.EndsWithOrdinal(DataSpan.Slice(0, SafeLength), s);
		}

		public override readonly int GetHashCode()
		{
			return UTF32Tools.GetHashDeterministic(DataSpan.Slice(0, SafeLength));
		}

		public override readonly string ToString()
		{
			return Value;
		}

		void IReadOnlySpanAssignable.Set(ReadOnlySpan<uint> values)
		{
			Assert.Check(values.Length > 0, "values.Length > 0");
			_length = (int)values[0];
			_data.Set(values.Slice(1));
		}

		public unsafe UTF32Tools.CharEnumerator GetEnumerator()
		{
			fixed (TSize* data = &_data)
			{
				return new UTF32Tools.CharEnumerator((uint*)data, Length);
			}
		}

		IEnumerator<char> IEnumerable<char>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private int SafeIndex(int index)
		{
			int safeLength = SafeLength;
			if (index < 0 || index >= safeLength)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return index;
		}
	}
	public static class NetworkString
	{
		public unsafe static int GetCapacity<TSize>() where TSize : unmanaged, IFixedStorage
		{
			return sizeof(TSize) / 4;
		}
	}
}
