using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Crypto.Digests
{
	public class Sha3Digest : KeccakDigest
	{
		private static int CheckBitLength(int bitLength)
		{
			switch (bitLength)
			{
			case 224:
			case 256:
			case 384:
			case 512:
				return bitLength;
			default:
				throw new ArgumentException(bitLength + " not supported for SHA-3", "bitLength");
			}
		}

		public Sha3Digest(int bitLength)
			: base(CheckBitLength(bitLength))
		{
		}

		public Sha3Digest(Sha3Digest source)
			: base(source)
		{
		}

		public override int DoFinal(byte[] output, int outOff)
		{
			AbsorbBits(2, 2);
			return base.DoFinal(output, outOff);
		}

		public override IMemoable Copy()
		{
			return new Sha3Digest(this);
		}
	}
}
