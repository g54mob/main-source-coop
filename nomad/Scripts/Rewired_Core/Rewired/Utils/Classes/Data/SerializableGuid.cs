using System;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	public struct SerializableGuid : IEquatable<SerializableGuid>
	{
		private const int length = 16;

		public static readonly SerializableGuid Empty;

		private long _a;

		private long _b;

		public SerializableGuid(Guid P_0)
		{
			byte[] value = P_0.ToByteArray();
			if (BitConverter.IsLittleEndian)
			{
				_a = BitConverter.ToInt64(value, 0);
				_b = BitConverter.ToInt64(value, 8);
			}
			else
			{
				_b = BitConverter.ToInt64(value, 0);
				_a = BitConverter.ToInt64(value, 8);
			}
		}

		public byte[] GetBytes()
		{
			byte[] array = new byte[16];
			bool isLittleEndian = BitConverter.IsLittleEndian;
			Array.Copy(BitConverter.GetBytes(isLittleEndian ? _a : _b), 0, array, 0, 8);
			Array.Copy(BitConverter.GetBytes(isLittleEndian ? _b : _a), 0, array, 8, 8);
			return array;
		}

		public Guid ToGuid()
		{
			return new Guid(GetBytes());
		}

		public override bool Equals(object obj)
		{
			if (!(obj is SerializableGuid serializableGuid))
			{
				return false;
			}
			if (serializableGuid._a == _a)
			{
				return serializableGuid._b == _b;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (17 * 29 + _a.GetHashCode()) * 29 + _b.GetHashCode();
		}

		public bool Equals(SerializableGuid other)
		{
			if (_a == other._a)
			{
				return _b == other._b;
			}
			return false;
		}

		bool IEquatable<SerializableGuid>.Equals(SerializableGuid other)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Equals
			return this.Equals(other);
		}

		public static bool operator ==(SerializableGuid a, SerializableGuid b)
		{
			if (a._a == b._a)
			{
				return a._b == b._b;
			}
			return false;
		}

		public static bool operator !=(SerializableGuid a, SerializableGuid b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			return ToGuid().ToString();
		}

		public string ToString(string format)
		{
			return ToGuid().ToString(format);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return ToGuid().ToString(format, provider);
		}
	}
}
