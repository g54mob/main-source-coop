using System;

namespace QFSW.QC
{
	public class ParserException : Exception
	{
		public ParserException(string message)
			: base(message)
		{
		}

		public ParserException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
