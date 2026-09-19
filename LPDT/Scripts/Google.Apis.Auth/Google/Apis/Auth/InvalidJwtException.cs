using System;

namespace Google.Apis.Auth
{
	public class InvalidJwtException : Exception
	{
		public InvalidJwtException(string message)
			: base(message)
		{
		}
	}
}
