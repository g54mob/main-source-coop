using System;
using System.Collections.Generic;
using System.Linq;

namespace Mirror.SimpleWeb
{
	public class Request
	{
		private static readonly char[] lineSplitChars = new char[2] { '\r', '\n' };

		private static readonly char[] headerSplitChars = new char[1] { ':' };

		public string RequestLine;

		public Dictionary<string, string> Headers = new Dictionary<string, string>();

		public Request(string message)
		{
			string[] source = message.Split(lineSplitChars, StringSplitOptions.RemoveEmptyEntries);
			RequestLine = source.First();
			Headers = (from header in source.Skip(1)
				select header.Split(headerSplitChars, 2, StringSplitOptions.RemoveEmptyEntries)).ToDictionary((string[] split) => split[0].Trim(), (string[] split) => split[1].Trim());
		}
	}
}
