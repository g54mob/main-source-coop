namespace Mirror.BouncyCastle.Asn1
{
	public class DerSetParser : IAsn1Convertible
	{
		private readonly Asn1StreamParser m_parser;

		internal DerSetParser(Asn1StreamParser parser)
		{
			m_parser = parser;
		}

		public Asn1Object ToAsn1Object()
		{
			return DLSet.FromVector(m_parser.ReadVector());
		}
	}
}
