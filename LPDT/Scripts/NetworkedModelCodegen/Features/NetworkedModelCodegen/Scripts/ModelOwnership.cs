using System;

namespace Features.NetworkedModelCodegen.Scripts
{
	[Flags]
	public enum ModelOwnership
	{
		None = 0,
		Shared = 1,
		Individual = 2
	}
}
