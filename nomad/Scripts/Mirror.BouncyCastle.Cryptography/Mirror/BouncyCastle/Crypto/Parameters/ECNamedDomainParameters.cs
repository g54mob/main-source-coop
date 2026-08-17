using Mirror.BouncyCastle.Asn1;
using Mirror.BouncyCastle.Asn1.X9;
using Mirror.BouncyCastle.Math;
using Mirror.BouncyCastle.Math.EC;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public class ECNamedDomainParameters : ECDomainParameters
	{
		private readonly DerObjectIdentifier name;

		public DerObjectIdentifier Name => name;

		public ECNamedDomainParameters(DerObjectIdentifier name, X9ECParameters x9)
			: base(x9)
		{
			this.name = name;
		}

		public ECNamedDomainParameters(DerObjectIdentifier name, ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed)
			: base(curve, g, n, h, seed)
		{
			this.name = name;
		}
	}
}
