using Mirror.BouncyCastle.Asn1;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public class ECKeyGenerationParameters : KeyGenerationParameters
	{
		private readonly ECDomainParameters domainParams;

		private readonly DerObjectIdentifier publicKeyParamSet;

		public ECDomainParameters DomainParameters => domainParams;

		public DerObjectIdentifier PublicKeyParamSet => publicKeyParamSet;
	}
}
