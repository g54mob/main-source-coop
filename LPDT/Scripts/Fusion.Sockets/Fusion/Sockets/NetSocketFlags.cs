using System;

namespace Fusion.Sockets
{
	[Flags]
	internal enum NetSocketFlags : byte
	{
		None = 0,
		UsingLan = 2
	}
}
