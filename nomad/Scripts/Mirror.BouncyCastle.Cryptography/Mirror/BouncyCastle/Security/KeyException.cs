using System;
using System.Runtime.Serialization;

namespace Mirror.BouncyCastle.Security
{
	[Serializable]
	public class KeyException : GeneralSecurityException
	{
		public KeyException()
		{
		}

		public KeyException(string message)
			: base(message)
		{
		}

		protected KeyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
