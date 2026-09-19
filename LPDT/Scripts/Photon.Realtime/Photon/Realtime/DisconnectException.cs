using System;

namespace Photon.Realtime
{
	public class DisconnectException : Exception
	{
		public DisconnectCause Cause;

		public DisconnectException(DisconnectCause cause)
			: base($"DisconnectException: {cause}")
		{
			Cause = cause;
		}
	}
}
