using Mirror.BouncyCastle.Math;

namespace Mirror.BouncyCastle.Asn1.CryptoPro
{
	public class Gost3410ParamSetParameters : Asn1Encodable
	{
		private readonly int keySize;

		private readonly DerInteger p;

		private readonly DerInteger q;

		private readonly DerInteger a;

		public BigInteger P => p.PositiveValue;

		public BigInteger Q => q.PositiveValue;

		public BigInteger A => a.PositiveValue;

		public Gost3410ParamSetParameters(int keySize, BigInteger p, BigInteger q, BigInteger a)
		{
			this.keySize = keySize;
			this.p = new DerInteger(p);
			this.q = new DerInteger(q);
			this.a = new DerInteger(a);
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new DerInteger(keySize), p, q, a);
		}
	}
}
