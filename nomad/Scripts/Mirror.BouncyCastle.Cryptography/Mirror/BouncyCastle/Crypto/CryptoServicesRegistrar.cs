using Mirror.BouncyCastle.Security;

namespace Mirror.BouncyCastle.Crypto
{
	public static class CryptoServicesRegistrar
	{
		public static SecureRandom GetSecureRandom()
		{
			return new SecureRandom();
		}
	}
}
