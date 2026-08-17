using System;
using Mirror.BouncyCastle.Math.EC.Rfc8032;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public sealed class Ed25519PrivateKeyParameters : AsymmetricKeyParameter
	{
		public static readonly int KeySize = Ed25519.SecretKeySize;

		public static readonly int SignatureSize = Ed25519.SignatureSize;

		private readonly byte[] data = new byte[KeySize];

		private Ed25519PublicKeyParameters cachedPublicKey;

		public Ed25519PrivateKeyParameters(byte[] buf)
			: this(Validate(buf), 0)
		{
		}

		public Ed25519PrivateKeyParameters(byte[] buf, int off)
			: base(privateKey: true)
		{
			Array.Copy(buf, off, data, 0, KeySize);
		}

		public byte[] GetEncoded()
		{
			return Arrays.Clone(data);
		}

		public Ed25519PublicKeyParameters GeneratePublicKey()
		{
			return Objects.EnsureSingletonInitialized(ref cachedPublicKey, data, CreatePublicKey);
		}

		private static Ed25519PublicKeyParameters CreatePublicKey(byte[] data)
		{
			return new Ed25519PublicKeyParameters(Ed25519.GeneratePublicKey(data, 0));
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
