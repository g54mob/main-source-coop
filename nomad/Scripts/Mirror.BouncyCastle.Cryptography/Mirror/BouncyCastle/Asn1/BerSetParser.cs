using System;

namespace Mirror.BouncyCastle.Asn1
{
	[Obsolete("Check for 'Asn1SetParser' instead")]
	public class BerSetParser : IAsn1Convertible
	{
		private readonly Asn1StreamParser m_parser;

		internal BerSetParser(Asn1StreamParser parser)
		{
			m_parser = parser;
		}

		public Asn1Object ToAsn1Object()
		{
			return Parse(m_parser);
		}

		internal static BerSet Parse(Asn1StreamParser sp)
		{
			return BerSet.FromVector(sp.ReadVector());
		}
	}
}
