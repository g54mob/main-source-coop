using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public class DerVideotexString : DerStringBase
	{
		private readonly byte[] m_contents;

		internal DerVideotexString(byte[] contents, bool clone)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			m_contents = (clone ? Arrays.Clone(contents) : contents);
		}

		public override string GetString()
		{
			return Strings.FromByteArray(m_contents);
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			return new PrimitiveEncoding(0, 21, m_contents);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return new PrimitiveEncoding(tagClass, tagNo, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new PrimitiveDerEncoding(0, 21, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new PrimitiveDerEncoding(tagClass, tagNo, m_contents);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (asn1Object is DerVideotexString derVideotexString)
			{
				return Arrays.AreEqual(m_contents, derVideotexString.m_contents);
			}
			return false;
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(m_contents);
		}

		internal static DerVideotexString CreatePrimitive(byte[] contents)
		{
			return new DerVideotexString(contents, clone: false);
		}
	}
}
