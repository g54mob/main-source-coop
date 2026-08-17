using Mirror.BouncyCastle.Math.EC.Rfc8032;

namespace Mirror.BouncyCastle.Math.EC.Rfc7748
{
	public static class X25519
	{
		public static void GeneratePublicKey(byte[] k, int kOff, byte[] r, int rOff)
		{
			ScalarMultBase(k, kOff, r, rOff);
		}

		public static void ScalarMultBase(byte[] k, int kOff, byte[] r, int rOff)
		{
			int[] array = X25519Field.Create();
			int[] array2 = X25519Field.Create();
			Ed25519.ScalarMultBaseYZ(k, kOff, array, array2);
			X25519Field.Apm(array2, array, array, array2);
			X25519Field.Inv(array2, array2);
			X25519Field.Mul(array, array2, array);
			X25519Field.Normalize(array);
			X25519Field.Encode(array, r, rOff);
		}
	}
}
