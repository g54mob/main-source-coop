using Mirror.BouncyCastle.Math.Raw;

namespace Mirror.BouncyCastle.Math.EC.Rfc8032
{
	internal static class Scalar448
	{
		private static readonly uint[] L = new uint[14]
		{
			2874688755u, 595116690u, 2378534741u, 560775794u, 2933274256u, 3293502281u, 2093622249u, 4294967295u, 4294967295u, 4294967295u,
			4294967295u, 4294967295u, 4294967295u, 1073741823u
		};

		private static readonly uint[] LSq = new uint[28]
		{
			463601321u, 3249404856u, 1239460018u, 3105617207u, 3882145813u, 1160071467u, 2729996653u, 1256291574u, 3124512708u, 4054436884u,
			2118977290u, 2449812427u, 2676112242u, 3275762323u, 1437344377u, 2445041993u, 1189267370u, 280387897u, 3614120776u, 3794234788u,
			3194294772u, 4294967295u, 4294967295u, 4294967295u, 4294967295u, 4294967295u, 4294967295u, 268435455u
		};

		internal static void Decode(byte[] k, uint[] n)
		{
			Codec.Decode32(k, 0, n, 0, 14);
		}

		internal static void ToSignedDigits(int bits, uint[] x, uint[] z)
		{
			z[14] = (uint)(1 << bits - 448) + Nat.CAdd(14, (int)(~x[0] & 1), x, L, z);
			Nat.ShiftDownBit(15, z, 0u);
		}
	}
}
