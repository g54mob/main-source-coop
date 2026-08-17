using System;
using Mirror.BouncyCastle.Math.EC.Rfc7748;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public sealed class X448PrivateKeyParameters : AsymmetricKeyParameter
	{
		public static readonly int KeySize = 56;

		public static readonly int SecretSize = 56;

		private readonly byte[] data = new byte[KeySize];

		public X448PrivateKeyParameters(byte[] buf)
			: this(Validate(buf), 0)
		{
		}

		public X448PrivateKeyParameters(byte[] buf, int off)
			: base(privateKey: true)
		{
			Array.Copy(buf, off, data, 0, KeySize);
		}

		public byte[] GetEncoded()
		{
			return Arrays.Clone(data);
		}

		public X448PublicKeyParameters GeneratePublicKey()
		{
			byte[] array = new byte[56];
			X448.GeneratePublicKey(data, 0, array, 0);
			return new X448PublicKeyParameters(array, 0);
		}

		private static byte[] Validate(byte[] buf)
		{
			if (buf.Length != KeySize)
			{
				int keySize = KeySize;
				throw new ArgumentException("must have length " + keySize, "buf");
			}
			return buf;
		}
	}
}
