using System;
using System.Runtime.Serialization;

namespace Mirror.BouncyCastle.Security
{
	[Serializable]
	public class GeneralSecurityException : Exception
	{
		public GeneralSecurityException()
		{
		}

		public GeneralSecurityException(string message)
			: base(message)
		{
		}

		protected GeneralSecurityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
