using System;
using Mirror.BouncyCastle.Math.EC.Rfc8032;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public sealed class Ed448PrivateKeyParameters : AsymmetricKeyParameter
	{
		public static readonly int KeySize = Ed448.SecretKeySize;

		public static readonly int SignatureSize = Ed448.SignatureSize;

		private readonly byte[] data = new byte[KeySize];

		private Ed448PublicKeyParameters cachedPublicKey;

		public Ed448PrivateKeyParameters(byte[] buf)
			: this(Validate(buf), 0)
		{
		}

		public Ed448PrivateKeyParameters(byte[] buf, int off)
			: base(privateKey: true)
		{
			Array.Copy(buf, off, data, 0, KeySize);
		}

		public byte[] GetEncoded()
		{
			return Arrays.Clone(data);
		}

		public Ed448PublicKeyParameters GeneratePublicKey()
		{
			return Objects.EnsureSingletonInitialized(ref cachedPublicKey, data, CreatePublicKey);
		}

		private static Ed448PublicKeyParameters CreatePublicKey(byte[] data)
		{
			return new Ed448PublicKeyParameters(Ed448.GeneratePublicKey(data, 0));
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
