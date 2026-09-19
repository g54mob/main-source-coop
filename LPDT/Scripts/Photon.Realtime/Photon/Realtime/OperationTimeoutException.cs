using System;

namespace Photon.Realtime
{
	public class OperationTimeoutException : Exception
	{
		public OperationTimeoutException(string message)
			: base(message)
		{
		}
	}
}
