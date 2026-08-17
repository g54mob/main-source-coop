namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public class ParametersWithRandom : ICipherParameters
	{
		private readonly ICipherParameters m_parameters;

		public ICipherParameters Parameters => m_parameters;
	}
}
