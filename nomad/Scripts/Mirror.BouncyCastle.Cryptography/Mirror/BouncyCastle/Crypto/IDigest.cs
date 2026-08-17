namespace Mirror.BouncyCastle.Crypto
{
	public interface IDigest
	{
		int GetDigestSize();

		int GetByteLength();

		void Update(byte input);

		void BlockUpdate(byte[] input, int inOff, int inLen);

		int DoFinal(byte[] output, int outOff);

		void Reset();
	}
}
