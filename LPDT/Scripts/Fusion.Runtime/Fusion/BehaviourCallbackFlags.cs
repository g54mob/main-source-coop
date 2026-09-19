using System;

namespace Fusion
{
	[Flags]
	internal enum BehaviourCallbackFlags
	{
		None = 0,
		FixedUpdateNetwork = 1,
		Render = 2,
		PreRender = 4,
		AnyRender = 6
	}
}
