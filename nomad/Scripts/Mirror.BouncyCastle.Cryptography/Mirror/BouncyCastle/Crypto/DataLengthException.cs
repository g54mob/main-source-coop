using System;
using System.Runtime.Serialization;

namespace Mirror.BouncyCastle.Crypto
{
	[Serializable]
	public class DataLengthException : CryptoException
	{
		public DataLengthException()
		{
		}

		public DataLengthException(string message)
			: base(message)
		{
		}

		protected DataLengthException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
