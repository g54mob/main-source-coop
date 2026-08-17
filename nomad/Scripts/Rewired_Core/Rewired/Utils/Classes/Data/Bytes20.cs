using System;
using System.Text;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	public struct Bytes20 : IEquatable<Bytes20>
	{
		public long value0;

		public long value1;

		public int value2;

		public Bytes20(byte[] P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException();
			}
			if (P_0.Length < 20)
			{
				throw new ArgumentException("bytes must be at least 20 bytes");
			}
			value0 = BitConverter.ToInt64(P_0, 0);
			value1 = BitConverter.ToInt64(P_0, 8);
			value2 = BitConverter.ToInt32(P_0, 16);
		}

		public byte[] GetBytes()
		{
			byte[] array = new byte[20];
			Array.Copy(BitConverter.GetBytes(value0), 0, array, 0, 8);
			Array.Copy(BitConverter.GetBytes(value1), 0, array, 8, 8);
			Array.Copy(BitConverter.GetBytes(value2), 0, array, 16, 4);
			return array;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Bytes20 bytes))
			{
				return false;
			}
			if (bytes.value0 == value0 && bytes.value1 == value1)
			{
				return bytes.value2 == value2;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((17 * 29 + value0.GetHashCode()) * 29 + value1.GetHashCode()) * 29 + value2.GetHashCode();
		}

		public bool Equals(Bytes20 other)
		{
			if (value0 == other.value0 && value1 == other.value1)
			{
				return value2 == other.value2;
			}
			return false;
		}

		bool IEquatable<Bytes20>.Equals(Bytes20 other)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Equals
			return this.Equals(other);
		}

		public static bool operator ==(Bytes20 a, Bytes20 b)
		{
			if (a.value0 == b.value0 && a.value1 == b.value1)
			{
				return a.value2 == b.value2;
			}
			return false;
		}

		public static bool operator !=(Bytes20 a, Bytes20 b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte[] bytes = GetBytes();
			for (int i = 0; i < bytes.Length; i++)
			{
				stringBuilder.Append(bytes[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}
	}
}
