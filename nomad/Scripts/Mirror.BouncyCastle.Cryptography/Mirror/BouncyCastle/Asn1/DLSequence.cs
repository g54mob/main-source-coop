namespace Mirror.BouncyCastle.Asn1
{
	public class DLSequence : DerSequence
	{
		public new static readonly DLSequence Empty = new DLSequence();

		public static DLSequence FromVector(Asn1EncodableVector elementVector)
		{
			if (elementVector.Count >= 1)
			{
				return new DLSequence(elementVector);
			}
			return Empty;
		}

		public DLSequence()
		{
		}

		public DLSequence(Asn1Encodable element)
			: base(element)
		{
		}

		public DLSequence(Asn1EncodableVector elementVector)
			: base(elementVector)
		{
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			if (3 == encoding)
			{
				return base.GetEncoding(encoding);
			}
			return new ConstructedDLEncoding(0, 16, Asn1OutputStream.GetContentsEncodings(2, m_elements));
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			if (3 == encoding)
			{
				return base.GetEncodingImplicit(encoding, tagClass, tagNo);
			}
			return new ConstructedDLEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(2, m_elements));
		}

		internal override DerBitString ToAsn1BitString()
		{
			return new DLBitString(BerBitString.FlattenBitStrings(GetConstructedBitStrings()), check: false);
		}

		internal override DerExternal ToAsn1External()
		{
			return new DLExternal(this);
		}

		internal override Asn1Set ToAsn1Set()
		{
			return new DLSet(isSorted: false, m_elements);
		}
	}
}
