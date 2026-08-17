namespace Mirror.BouncyCastle.Asn1
{
	public class DerSequenceParser : IAsn1Convertible
	{
		private readonly Asn1StreamParser m_parser;

		internal DerSequenceParser(Asn1StreamParser parser)
		{
			m_parser = parser;
		}

		public Asn1Object ToAsn1Object()
		{
			return DLSequence.FromVector(m_parser.ReadVector());
		}
	}
}
