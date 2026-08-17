using System.IO;

namespace Mirror.BouncyCastle.Asn1
{
	internal class BerTaggedObjectParser : IAsn1Convertible
	{
		internal readonly int m_tagClass;

		internal readonly int m_tagNo;

		internal readonly Asn1StreamParser m_parser;

		public int TagClass => m_tagClass;

		public int TagNo => m_tagNo;

		internal BerTaggedObjectParser(int tagClass, int tagNo, Asn1StreamParser parser)
		{
			m_tagClass = tagClass;
			m_tagNo = tagNo;
			m_parser = parser;
		}

		public virtual Asn1Object ToAsn1Object()
		{
			try
			{
				return m_parser.LoadTaggedIL(TagClass, TagNo);
			}
			catch (IOException ex)
			{
				throw new Asn1ParsingException(ex.Message);
			}
		}
	}
}
