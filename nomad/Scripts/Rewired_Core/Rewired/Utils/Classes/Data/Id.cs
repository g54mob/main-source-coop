using System;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	public struct Id : IEquatable<Id>, IEquatable<uint>
	{
		public const uint Default = 0u;

		public const uint First = 1u;

		public const uint Invalid = 4294967295u;

		public uint id;

		public static bool IsValid(Id id)
		{
			uint num = id.id;
			if (num == 0 || num == 4294967295u)
			{
				return false;
			}
			return true;
		}

		public static bool IsValid(uint id)
		{
			if (id == 0 || id == 4294967295u)
			{
				return false;
			}
			return true;
		}

		public Id(uint P_0)
		{
			id = P_0;
		}

		public bool Equals(Id other)
		{
			return id == other.id;
		}

		bool IEquatable<Id>.Equals(Id other)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Equals
			return this.Equals(other);
		}

		public bool Equals(uint other)
		{
			return id == other;
		}

		bool IEquatable<uint>.Equals(uint other)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Equals
			return this.Equals(other);
		}

		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			if (other is Id)
			{
				return id == ((Id)other).id;
			}
			if (other is uint)
			{
				return id == (uint)other;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}

		public static bool operator ==(Id a, Id b)
		{
			return a.id == b.id;
		}

		public static bool operator !=(Id a, Id b)
		{
			return !(a == b);
		}

		public static implicit operator uint(Id a)
		{
			return a.id;
		}

		public static implicit operator Id(uint a)
		{
			return new Id(a);
		}

		public void Increment()
		{
			id++;
			if (id == 4294967295u)
			{
				id = 1u;
			}
		}
	}
}
