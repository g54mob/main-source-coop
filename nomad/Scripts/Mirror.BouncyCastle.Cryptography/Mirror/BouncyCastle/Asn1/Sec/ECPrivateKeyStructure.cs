using System;
using Mirror.BouncyCastle.Math;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1.Sec
{
	public class ECPrivateKeyStructure : Asn1Encodable
	{
		private readonly Asn1Sequence m_seq;

		public static ECPrivateKeyStructure GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is ECPrivateKeyStructure result)
			{
				return result;
			}
			return new ECPrivateKeyStructure(Asn1Sequence.GetInstance(obj));
		}

		private ECPrivateKeyStructure(Asn1Sequence seq)
		{
			m_seq = seq ?? throw new ArgumentNullException("seq");
		}

		public ECPrivateKeyStructure(int orderBitLength, BigInteger key, DerBitString publicKey, Asn1Encodable parameters)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (orderBitLength < key.BitLength)
			{
				throw new ArgumentException("must be >= key bitlength", "orderBitLength");
			}
			byte[] contents = BigIntegers.AsUnsignedByteArray((orderBitLength + 7) / 8, key);
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new DerInteger(1), new DerOctetString(contents));
			asn1EncodableVector.AddOptionalTagged(isExplicit: true, 0, parameters);
			asn1EncodableVector.AddOptionalTagged(isExplicit: true, 1, publicKey);
			m_seq = new DerSequence(asn1EncodableVector);
		}

		public virtual BigInteger GetKey()
		{
			Asn1OctetString asn1OctetString = (Asn1OctetString)m_seq[1];
			return new BigInteger(1, asn1OctetString.GetOctets());
		}

		public override Asn1Object ToAsn1Object()
		{
			return m_seq;
		}
	}
}
