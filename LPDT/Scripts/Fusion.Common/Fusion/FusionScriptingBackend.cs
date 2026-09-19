using System;

namespace Fusion
{
	[Flags]
	public enum FusionScriptingBackend
	{
		DotNet = 1,
		Mono = 2,
		IL2CPP = 4
	}
}
