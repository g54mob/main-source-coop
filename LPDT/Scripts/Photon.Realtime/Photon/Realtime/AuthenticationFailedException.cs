using System;

namespace Photon.Realtime
{
	public class AuthenticationFailedException : Exception
	{
		public AuthenticationFailedException(string message)
			: base(message)
		{
		}
	}
}
