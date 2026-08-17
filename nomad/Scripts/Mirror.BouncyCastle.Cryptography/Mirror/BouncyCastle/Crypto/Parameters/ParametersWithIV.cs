namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public class ParametersWithIV : ICipherParameters
	{
		private readonly ICipherParameters m_parameters;

		private readonly byte[] m_iv;

		public ICipherParameters Parameters => m_parameters;

		public byte[] GetIV()
		{
			return (byte[])m_iv.Clone();
		}
	}
}
