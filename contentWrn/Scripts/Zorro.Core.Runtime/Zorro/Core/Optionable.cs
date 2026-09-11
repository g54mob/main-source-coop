using System;

namespace Zorro.Core
{
	public readonly struct Optionable<T> : IEquatable<Optionable<T>> where T : struct
	{
		private readonly T value;

		private readonly byte hasValue;

		public T Value => value;

		public static Optionable<T> None => default(Optionable<T>);

		public bool IsSome => hasValue > 0;

		public bool IsNone => hasValue <= 0;

		public static Optionable<T> NoneWithValue(T value)
		{
			return new Optionable<T>(value, 0);
		}

		public static Optionable<T> Some(T value)
		{
			return new Optionable<T>(value, 1);
		}

		private Optionable(T value, byte hasValue)
		{
			this.value = value;
			this.hasValue = hasValue;
		}

		public T ValueOr(T other)
		{
			if (!IsSome)
			{
				return other;
			}
			return value;
		}

		public bool Equals(Optionable<T> other)
		{
			if (hasValue == other.hasValue)
			{
				return value.Equals(other.value);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Optionable<T> other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (hasValue.GetHashCode() * 397) ^ value.GetHashCode();
		}

		public static bool operator ==(Optionable<T> left, Optionable<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Optionable<T> left, Optionable<T> right)
		{
			return !left.Equals(right);
		}
	}
}
