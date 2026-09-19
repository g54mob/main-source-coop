using System;

namespace Fusion
{
	[Obsolete("Use LogLevel instead")]
	public enum LogType : byte
	{
		Error = 0,
		Warn = 1,
		Info = 2,
		Debug = 3,
		Trace = 4
	}
}
