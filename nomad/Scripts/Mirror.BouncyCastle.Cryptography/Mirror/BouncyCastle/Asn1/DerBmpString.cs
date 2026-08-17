using System;

namespace Mirror.BouncyCastle.Asn1
{
	public class DerBmpString : DerStringBase
	{
		private readonly string m_str;

		internal DerBmpString(char[] str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			m_str = new string(str);
		}

		public override string GetString()
		{
			return m_str;
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (asn1Object is DerBmpString derBmpString)
			{
				return m_str.Equals(derBmpString.m_str);
			}
			return false;
		}

		protected override int Asn1GetHashCode()
		{
			return m_str.GetHashCode();
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			return new PrimitiveEncoding(0, 30, GetContents());
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return new PrimitiveEncoding(tagClass, tagNo, GetContents());
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new PrimitiveDerEncoding(0, 30, GetContents());
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new PrimitiveDerEncoding(tagClass, tagNo, GetContents());
		}

		private byte[] GetContents()
		{
			char[] array = m_str.ToCharArray();
			byte[] array2 = new byte[array.Length * 2];
			for (int i = 0; i != array.Length; i++)
			{
				array2[2 * i] = (byte)((int)array[i] >> 8);
				array2[2 * i + 1] = (byte)array[i];
			}
			return array2;
		}

		internal static DerBmpString CreatePrimitive(char[] str)
		{
			return new DerBmpString(str);
		}
	}
}
