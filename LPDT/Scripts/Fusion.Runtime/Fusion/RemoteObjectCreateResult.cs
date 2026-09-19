using System;

namespace Fusion
{
	internal enum RemoteObjectCreateResult
	{
		Allowed = 0,
		Denied = 1,
		[Obsolete]
		AllowedWithStateOverride = 2
	}
}
