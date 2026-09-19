using System;

namespace Photon.Realtime
{
	public class OperationException : Exception
	{
		public short ErrorCode;

		public OperationException(short errorCode, string message)
			: base($"{message} (ErrorCode: {errorCode})")
		{
			ErrorCode = errorCode;
		}
	}
}
