using System;
using Mirror.BouncyCastle.Math.EC.Rfc8032;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public sealed class Ed25519PublicKeyParameters : AsymmetricKeyParameter
	{
		public static readonly int KeySize = Ed25519.PublicKeySize;

		private readonly Ed25519.PublicPoint m_publicPoint;

		public Ed25519PublicKeyParameters(byte[] buf)
			: this(Validate(buf), 0)
		{
		}

		public Ed25519PublicKeyParameters(byte[] buf, int off)
			: base(privateKey: false)
		{
			m_publicPoint = Parse(buf, off);
		}

		public Ed25519PublicKeyParameters(Ed25519.PublicPoint publicPoint)
			: base(privateKey: false)
		{
			m_publicPoint = publicPoint ?? throw new ArgumentNullException("publicPoint");
		}

		public void Encode(byte[] buf, int off)
		{
			Ed25519.EncodePublicPoint(m_publicPoint, buf, off);
		}

		public byte[] GetEncoded()
		{
			byte[] array = new byte[KeySize];
			Encode(array, 0);
			return array;
		}

		private static Ed25519.PublicPoint Parse(byte[] buf, int off)
		{
			return Ed25519.ValidatePublicKeyPartialExport(buf, off) ?? throw new ArgumentException("invalid public key");
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
