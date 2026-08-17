using System.IO;

namespace Mirror.BouncyCastle.Asn1
{
	internal class DLTaggedObjectParser : BerTaggedObjectParser
	{
		private readonly bool m_constructed;

		internal DLTaggedObjectParser(int tagClass, int tagNo, bool constructed, Asn1StreamParser parser)
			: base(tagClass, tagNo, parser)
		{
			m_constructed = constructed;
		}

		public override Asn1Object ToAsn1Object()
		{
			try
			{
				return m_parser.LoadTaggedDL(base.TagClass, base.TagNo, m_constructed);
			}
			catch (IOException ex)
			{
				throw new Asn1ParsingException(ex.Message);
			}
		}
	}
}
