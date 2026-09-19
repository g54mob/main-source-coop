using System;

namespace GameAnalyticsSDK.Net.Utilities.Zip
{
	public class SharpZipBaseException : Exception
	{
		public SharpZipBaseException()
		{
		}

		public SharpZipBaseException(string msg)
			: base(msg)
		{
		}

		public SharpZipBaseException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
