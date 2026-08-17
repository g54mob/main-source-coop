using System;
using System.Runtime.Serialization;

namespace Mirror.BouncyCastle.Crypto
{
	[Serializable]
	public class InvalidCipherTextException : CryptoException
	{
		public InvalidCipherTextException()
		{
		}

		public InvalidCipherTextException(string message)
			: base(message)
		{
		}

		protected InvalidCipherTextException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
