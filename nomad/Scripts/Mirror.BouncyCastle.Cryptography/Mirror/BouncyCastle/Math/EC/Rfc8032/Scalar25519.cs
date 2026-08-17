using Mirror.BouncyCastle.Math.Raw;

namespace Mirror.BouncyCastle.Math.EC.Rfc8032
{
	internal static class Scalar25519
	{
		private static readonly uint[] L = new uint[8] { 1559614445u, 1477600026u, 2734136534u, 350157278u, 0u, 0u, 0u, 268435456u };

		private static readonly uint[] LSq = new uint[16]
		{
			2870118761u, 3807245957u, 580428573u, 1745064566u, 3524785598u, 1036971123u, 461123738u, 2712901953u, 1268693629u, 3405925475u,
			3562992538u, 43769659u, 0u, 0u, 0u, 16777216u
		};

		internal static void Decode(byte[] k, uint[] n)
		{
			Codec.Decode32(k, 0, n, 0, 8);
		}

		internal static void ToSignedDigits(int bits, uint[] z)
		{
			Nat.CAddTo(8, (int)(~z[0] & 1), L, z);
			Nat.ShiftDownBit(8, z, 1u);
		}
	}
}
