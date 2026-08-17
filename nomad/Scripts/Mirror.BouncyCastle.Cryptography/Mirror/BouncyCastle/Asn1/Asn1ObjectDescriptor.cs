using System;

namespace Mirror.BouncyCastle.Asn1
{
	public sealed class Asn1ObjectDescriptor : Asn1Object
	{
		private readonly DerGraphicString m_baseGraphicString;

		public Asn1ObjectDescriptor(DerGraphicString baseGraphicString)
		{
			if (baseGraphicString == null)
			{
				throw new ArgumentNullException("baseGraphicString");
			}
			m_baseGraphicString = baseGraphicString;
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			return m_baseGraphicString.GetEncodingImplicit(encoding, 0, 7);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return m_baseGraphicString.GetEncodingImplicit(encoding, tagClass, tagNo);
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return m_baseGraphicString.GetEncodingDerImplicit(0, 7);
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return m_baseGraphicString.GetEncodingDerImplicit(tagClass, tagNo);
		}

		protected override int Asn1GetHashCode()
		{
			return ~m_baseGraphicString.CallAsn1GetHashCode();
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (asn1Object is Asn1ObjectDescriptor asn1ObjectDescriptor)
			{
				return m_baseGraphicString.Equals(asn1ObjectDescriptor.m_baseGraphicString);
			}
			return false;
		}

		internal static Asn1ObjectDescriptor CreatePrimitive(byte[] contents)
		{
			return new Asn1ObjectDescriptor(DerGraphicString.CreatePrimitive(contents));
		}
	}
}
