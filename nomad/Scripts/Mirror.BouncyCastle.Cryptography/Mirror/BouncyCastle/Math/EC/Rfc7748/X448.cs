using Mirror.BouncyCastle.Math.EC.Rfc8032;

namespace Mirror.BouncyCastle.Math.EC.Rfc7748
{
	public static class X448
	{
		public static void GeneratePublicKey(byte[] k, int kOff, byte[] r, int rOff)
		{
			ScalarMultBase(k, kOff, r, rOff);
		}

		public static void ScalarMultBase(byte[] k, int kOff, byte[] r, int rOff)
		{
			uint[] array = X448Field.Create();
			uint[] y = X448Field.Create();
			Ed448.ScalarMultBaseXY(k, kOff, array, y);
			X448Field.Inv(array, array);
			X448Field.Mul(array, y, array);
			X448Field.Sqr(array, array);
			X448Field.Normalize(array);
			X448Field.Encode(array, r, rOff);
		}
	}
}
