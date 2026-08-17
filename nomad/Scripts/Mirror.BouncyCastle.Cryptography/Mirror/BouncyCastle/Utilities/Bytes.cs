namespace Mirror.BouncyCastle.Utilities
{
	public static class Bytes
	{
		public static void XorTo(int len, byte[] x, byte[] z)
		{
			for (int i = 0; i < len; i++)
			{
				z[i] ^= x[i];
			}
		}
	}
}
