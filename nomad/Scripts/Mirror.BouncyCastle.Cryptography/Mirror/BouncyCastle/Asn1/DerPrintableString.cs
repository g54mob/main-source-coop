using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public class DerPrintableString : DerStringBase
	{
		private readonly byte[] m_contents;

		internal DerPrintableString(byte[] contents, bool clone)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			m_contents = (clone ? Arrays.Clone(contents) : contents);
		}

		public override string GetString()
		{
			return Strings.FromAsciiByteArray(m_contents);
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			return new PrimitiveEncoding(0, 19, m_contents);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return new PrimitiveEncoding(tagClass, tagNo, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new PrimitiveDerEncoding(0, 19, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new PrimitiveDerEncoding(tagClass, tagNo, m_contents);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (asn1Object is DerPrintableString derPrintableString)
			{
				return Arrays.AreEqual(m_contents, derPrintableString.m_contents);
			}
			return false;
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(m_contents);
		}

		internal static DerPrintableString CreatePrimitive(byte[] contents)
		{
			return new DerPrintableString(contents, clone: false);
		}
	}
}
