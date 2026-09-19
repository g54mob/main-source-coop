using System;

namespace Photon.Realtime
{
	public class OperationStartException : Exception
	{
		public OperationStartException(string message)
			: base(message)
		{
		}
	}
}
