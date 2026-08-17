using System;
using System.Runtime.Serialization;

namespace Mirror.BouncyCastle.Security
{
	[Serializable]
	public class SecurityUtilityException : Exception
	{
		public SecurityUtilityException()
		{
		}

		public SecurityUtilityException(string message)
			: base(message)
		{
		}

		protected SecurityUtilityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
