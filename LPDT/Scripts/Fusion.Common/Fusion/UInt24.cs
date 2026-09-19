using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 3)]
	public readonly struct UInt24 : IEquatable<UInt24>
	{
		[FieldOffset(0)]
		private readonly byte _byte0;

		[FieldOffset(1)]
		private readonly byte _byte1;

		[FieldOffset(2)]
		private readonly byte _byte2;

		public const uint MaxValue = 16777215u;

		public const uint MinValue = 0u;

		public uint Value
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (uint)(_byte0 | (_byte1 << 8) | (_byte2 << 16));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UInt24(uint value)
		{
			_byte0 = 0;
			_byte1 = 0;
			_byte2 = 0;
			EnsureInBounds(value);
			_byte0 = (byte)value;
			_byte1 = (byte)(value >> 8);
			_byte2 = (byte)(value >> 16);
		}

		private void EnsureInBounds(uint value)
		{
			if (value > 16777215)
			{
				throw new OverflowException("Value is too large to fit in a UInt24");
			}
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_byte0, _byte1, _byte2);
		}

		public bool Equals(UInt24 other)
		{
			return _byte0 == other._byte0 && _byte1 == other._byte1 && _byte2 == other._byte2;
		}

		public override bool Equals(object obj)
		{
			return obj is UInt24 other && Equals(other);
		}

		public static bool operator ==(UInt24 left, UInt24 right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(UInt24 left, UInt24 right)
		{
			return !left.Equals(right);
		}

		public static explicit operator UInt24(uint value)
		{
			return new UInt24(value);
		}

		public static implicit operator uint(UInt24 value)
		{
			return value.Value;
		}

		public override string ToString()
		{
			return $"{Value}";
		}
	}
}
