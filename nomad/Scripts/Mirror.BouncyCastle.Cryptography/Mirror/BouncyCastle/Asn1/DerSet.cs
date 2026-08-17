using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public class DerSet : Asn1Set
	{
		public static readonly DerSet Empty = new DerSet();

		public DerSet()
		{
		}

		internal DerSet(Asn1EncodableVector elementVector, bool doSort)
			: base(elementVector, doSort)
		{
		}

		internal DerSet(bool isSorted, Asn1Encodable[] elements)
			: base(isSorted, elements)
		{
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			IAsn1Encoding[] sortedDerEncodings = GetSortedDerEncodings();
			return new ConstructedDLEncoding(0, 17, sortedDerEncodings);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			IAsn1Encoding[] sortedDerEncodings = GetSortedDerEncodings();
			return new ConstructedDLEncoding(tagClass, tagNo, sortedDerEncodings);
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new ConstructedDerEncoding(0, 17, GetSortedDerEncodings());
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new ConstructedDerEncoding(tagClass, tagNo, GetSortedDerEncodings());
		}

		private DerEncoding[] GetSortedDerEncodings()
		{
			return Objects.EnsureSingletonInitialized(ref m_sortedDerEncodings, m_elements, CreateSortedDerEncodings);
		}

		private static DerEncoding[] CreateSortedDerEncodings(Asn1Encodable[] elements)
		{
			DerEncoding[] contentsEncodingsDer = Asn1OutputStream.GetContentsEncodingsDer(elements);
			if (contentsEncodingsDer.Length > 1)
			{
				Array.Sort(contentsEncodingsDer);
			}
			return contentsEncodingsDer;
		}
	}
}
