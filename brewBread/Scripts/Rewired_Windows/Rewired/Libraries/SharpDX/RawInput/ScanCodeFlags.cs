using System;

namespace Rewired.Libraries.SharpDX.RawInput
{
	[Flags]
	internal enum ScanCodeFlags : short
	{
		Make = 0,
		Break = 1,
		E0 = 2,
		E1 = 4
	}
}
