using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public class DerUtf8String : DerStringBase
	{
		private readonly byte[] m_contents;

		internal DerUtf8String(byte[] contents, bool clone)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			m_contents = (clone ? Arrays.Clone(contents) : contents);
		}

		public override string GetString()
		{
			return Strings.FromUtf8ByteArray(m_contents);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (asn1Object is DerUtf8String derUtf8String)
			{
				return Arrays.AreEqual(m_contents, derUtf8String.m_contents);
			}
			return false;
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(m_contents);
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			return new PrimitiveEncoding(0, 12, m_contents);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return new PrimitiveEncoding(tagClass, tagNo, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new PrimitiveDerEncoding(0, 12, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new PrimitiveDerEncoding(tagClass, tagNo, m_contents);
		}

		internal static DerUtf8String CreatePrimitive(byte[] contents)
		{
			return new DerUtf8String(contents, clone: false);
		}
	}
}
