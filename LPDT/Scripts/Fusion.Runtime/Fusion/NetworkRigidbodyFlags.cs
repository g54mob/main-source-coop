using System;

namespace Fusion
{
	[Flags]
	public enum NetworkRigidbodyFlags : byte
	{
		IsKinematic = 1,
		IsSleeping = 2
	}
}
