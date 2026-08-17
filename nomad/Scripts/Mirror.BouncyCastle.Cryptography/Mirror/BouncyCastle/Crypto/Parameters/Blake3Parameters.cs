using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public sealed class Blake3Parameters
	{
		private byte[] m_theKey;

		private byte[] m_theContext;

		public byte[] GetKey()
		{
			return Arrays.Clone(m_theKey);
		}

		public byte[] GetContext()
		{
			return Arrays.Clone(m_theContext);
		}
	}
}
