using System;
using Mirror.BouncyCastle.Math.EC.Rfc8032;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public sealed class Ed448PublicKeyParameters : AsymmetricKeyParameter
	{
		public static readonly int KeySize = Ed448.PublicKeySize;

		private readonly Ed448.PublicPoint m_publicPoint;

		public Ed448PublicKeyParameters(byte[] buf)
			: this(Validate(buf), 0)
		{
		}

		public Ed448PublicKeyParameters(byte[] buf, int off)
			: base(privateKey: false)
		{
			m_publicPoint = Parse(buf, off);
		}

		public Ed448PublicKeyParameters(Ed448.PublicPoint publicPoint)
			: base(privateKey: false)
		{
			m_publicPoint = publicPoint ?? throw new ArgumentNullException("publicPoint");
		}

		public void Encode(byte[] buf, int off)
		{
			Ed448.EncodePublicPoint(m_publicPoint, buf, off);
		}

		public byte[] GetEncoded()
		{
			byte[] array = new byte[KeySize];
			Encode(array, 0);
			return array;
		}

		private static Ed448.PublicPoint Parse(byte[] buf, int off)
		{
			return Ed448.ValidatePublicKeyPartialExport(buf, off) ?? throw new ArgumentException("invalid public key");
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
