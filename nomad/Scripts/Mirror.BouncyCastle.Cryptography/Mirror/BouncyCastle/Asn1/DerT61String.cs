using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public class DerT61String : DerStringBase
	{
		private readonly byte[] m_contents;

		internal DerT61String(byte[] contents, bool clone)
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
			return new PrimitiveEncoding(0, 20, m_contents);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return new PrimitiveEncoding(tagClass, tagNo, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new PrimitiveDerEncoding(0, 20, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new PrimitiveDerEncoding(tagClass, tagNo, m_contents);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (asn1Object is DerT61String derT61String)
			{
				return Arrays.AreEqual(m_contents, derT61String.m_contents);
			}
			return false;
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(m_contents);
		}

		internal static DerT61String CreatePrimitive(byte[] contents)
		{
			return new DerT61String(contents, clone: false);
		}
	}
}
