using System;

namespace Mirror.BouncyCastle.Asn1
{
	public abstract class Asn1Null : Asn1Object
	{
		internal Asn1Null()
		{
		}

		public override string ToString()
		{
			return "NULL";
		}

		internal static Asn1Null CreatePrimitive(byte[] contents)
		{
			if (contents.Length != 0)
			{
				throw new InvalidOperationException("malformed NULL encoding encountered");
			}
			return DerNull.Instance;
		}
	}
}
