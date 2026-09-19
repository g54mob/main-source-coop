using System;

namespace Photon.Client
{
	public enum LogLevel : byte
	{
		Off = 0,
		[Obsolete]
		OFF = 0,
		Error = 1,
		[Obsolete]
		ERROR = 1,
		Warning = 2,
		[Obsolete]
		WARNING = 2,
		Info = 3,
		[Obsolete]
		INFO = 3,
		Debug = 4,
		[Obsolete]
		ALL = 4
	}
}
