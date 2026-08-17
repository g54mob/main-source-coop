using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public sealed class X25519PublicKeyParameters : AsymmetricKeyParameter
	{
		public static readonly int KeySize = 32;

		private readonly byte[] data = new byte[KeySize];

		public X25519PublicKeyParameters(byte[] buf)
			: this(Validate(buf), 0)
		{
		}

		public X25519PublicKeyParameters(byte[] buf, int off)
			: base(privateKey: false)
		{
			Array.Copy(buf, off, data, 0, KeySize);
		}

		public byte[] GetEncoded()
		{
			return Arrays.Clone(data);
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
