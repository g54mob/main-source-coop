using System;
using System.Runtime.Serialization;

namespace Mirror.BouncyCastle.Crypto
{
	[Serializable]
	public class OutputLengthException : DataLengthException
	{
		public OutputLengthException()
		{
		}

		public OutputLengthException(string message)
			: base(message)
		{
		}

		protected OutputLengthException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
