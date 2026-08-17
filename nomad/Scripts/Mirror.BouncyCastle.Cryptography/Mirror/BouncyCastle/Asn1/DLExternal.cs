namespace Mirror.BouncyCastle.Asn1
{
	public class DLExternal : DerExternal
	{
		public DLExternal(Asn1EncodableVector vector)
			: base(vector)
		{
		}

		public DLExternal(Asn1Sequence sequence)
			: base(sequence)
		{
		}

		internal override Asn1Sequence BuildSequence()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(4);
			asn1EncodableVector.AddOptional(directReference, indirectReference, dataValueDescriptor);
			asn1EncodableVector.Add(new DLTaggedObject(encoding == 0, encoding, externalContent));
			return new DLSequence(asn1EncodableVector);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			if (3 == encoding)
			{
				return base.GetEncodingImplicit(encoding, tagClass, tagNo);
			}
			return BuildSequence().GetEncodingImplicit(2, tagClass, tagNo);
		}
	}
}
