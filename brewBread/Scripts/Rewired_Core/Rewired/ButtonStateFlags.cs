using System;

namespace Rewired
{
	[Flags]
	[CustomObfuscation(rename = false)]
	internal enum ButtonStateFlags
	{
		Off = 0,
		On = 1,
		Down = 2,
		Up = 4
	}
}
