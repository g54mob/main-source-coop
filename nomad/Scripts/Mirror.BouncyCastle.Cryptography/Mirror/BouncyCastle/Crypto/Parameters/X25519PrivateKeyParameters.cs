using System;
using Mirror.BouncyCastle.Math.EC.Rfc7748;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public sealed class X25519PrivateKeyParameters : AsymmetricKeyParameter
	{
		public static readonly int KeySize = 32;

		public static readonly int SecretSize = 32;

		private readonly byte[] data = new byte[KeySize];

		public X25519PrivateKeyParameters(byte[] buf)
			: this(Validate(buf), 0)
		{
		}

		public X25519PrivateKeyParameters(byte[] buf, int off)
			: base(privateKey: true)
		{
			Array.Copy(buf, off, data, 0, KeySize);
		}

		public byte[] GetEncoded()
		{
			return Arrays.Clone(data);
		}

		public X25519PublicKeyParameters GeneratePublicKey()
		{
			byte[] array = new byte[32];
			X25519.GeneratePublicKey(data, 0, array, 0);
			return new X25519PublicKeyParameters(array, 0);
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
