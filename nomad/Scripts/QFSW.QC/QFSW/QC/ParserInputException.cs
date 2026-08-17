using System;

namespace QFSW.QC
{
	public class ParserInputException : ParserException
	{
		public ParserInputException(string message)
			: base(message)
		{
		}

		public ParserInputException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
