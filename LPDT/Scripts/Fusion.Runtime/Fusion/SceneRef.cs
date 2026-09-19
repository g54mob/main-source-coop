using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	[NetworkStructWeaved(1)]
	public struct SceneRef : INetworkStruct, IEquatable<SceneRef>
	{
		[Obsolete("Use FLAG_PATH")]
		public const uint FLAG_ADDRESSABLE = 2147483648u;

		public const uint FLAG_PATH = 2147483648u;

		[FieldOffset(0)]
		public uint RawValue;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF_RAW_VALUE = 0;

		internal const int BYTE_COUNT_OF_RAW_VALUE = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static SceneRef None => default(SceneRef);

		public readonly bool IsValid => RawValue != 0;

		public readonly bool IsIndex => (RawValue & 0x80000000u) == 0;

		[IgnoreDataMember]
		public readonly int AsIndex
		{
			get
			{
				if (!IsIndex)
				{
					throw new InvalidOperationException($"SceneRef {RawValue:X8} is not an index");
				}
				return (int)(RawValue - 1);
			}
		}

		[IgnoreDataMember]
		public readonly uint AsPathHash
		{
			get
			{
				if (IsIndex)
				{
					throw new InvalidOperationException($"SceneRef {RawValue:X8} is not a path hash");
				}
				return RawValue & 0x7FFFFFFF;
			}
		}

		public bool IsPath(string path)
		{
			if (IsIndex)
			{
				return false;
			}
			return this == FromPath(path);
		}

		public static SceneRef FromIndex(int index)
		{
			if (index < 0 || index == int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			SceneRef result = default(SceneRef);
			result.RawValue = (uint)(index + 1);
			return result;
		}

		public static SceneRef FromPath(string path)
		{
			uint hashCodeDeterministic = (uint)HashCodeUtilities.GetHashCodeDeterministic(path ?? throw new ArgumentNullException("path"));
			hashCodeDeterministic &= 0x7FFFFFFF;
			SceneRef result = default(SceneRef);
			result.RawValue = 0x80000000u | hashCodeDeterministic;
			return result;
		}

		public static SceneRef FromRaw(uint rawValue)
		{
			SceneRef result = default(SceneRef);
			result.RawValue = rawValue;
			return result;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is SceneRef other && Equals(other);
		}

		public readonly bool Equals(SceneRef other)
		{
			return RawValue == other.RawValue;
		}

		public override readonly int GetHashCode()
		{
			return RawValue.GetHashCode();
		}

		public override readonly string ToString()
		{
			return ToString(brackets: true, prefix: true);
		}

		public readonly string ToString(bool brackets, bool prefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (brackets)
			{
				stringBuilder.Append('[');
			}
			if (prefix)
			{
				stringBuilder.Append("Scene:");
			}
			if (IsValid)
			{
				if (IsIndex)
				{
					stringBuilder.Append("#").Append(AsIndex);
				}
				else
				{
					stringBuilder.AppendFormat("0x{0:X8}", AsPathHash);
				}
			}
			else
			{
				stringBuilder.Append("None");
			}
			if (brackets)
			{
				stringBuilder.Append(']');
			}
			return stringBuilder.ToString();
		}

		public static SceneRef Parse(string str)
		{
			ReadOnlySpan<char> span = str.AsSpan();
			if (span.StartsWith("[".AsSpan()))
			{
				if (!span.EndsWith("]".AsSpan()))
				{
					throw new FormatException("Invalid SceneRef format: " + str);
				}
				span = span.Slice(1, span.Length - 2);
			}
			if (span.StartsWith("Scene:".AsSpan()))
			{
				span = span.Slice(6);
			}
			if (span.StartsWith("#".AsSpan()))
			{
				return FromIndex(int.Parse(span.Slice(1)));
			}
			if (span.StartsWith("0x".AsSpan()))
			{
				return FromRaw(uint.Parse(span.Slice(2), NumberStyles.HexNumber) | 0x80000000u);
			}
			if (span.SequenceEqual("None".AsSpan()))
			{
				return None;
			}
			throw new FormatException("Invalid SceneRef format: " + str);
		}

		public static bool operator ==(SceneRef a, SceneRef b)
		{
			return a.RawValue == b.RawValue;
		}

		public static bool operator !=(SceneRef a, SceneRef b)
		{
			return a.RawValue != b.RawValue;
		}
	}
}
