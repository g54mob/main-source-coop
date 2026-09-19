using System;

namespace Fusion
{
	[Flags]
	internal enum NetworkRunnerFlags
	{
		AllowUnrecognizedRpcs = 1,
		[Obsolete]
		AllowStateAuthDataOverride = 2,
		CustomPlugin = 4
	}
}
