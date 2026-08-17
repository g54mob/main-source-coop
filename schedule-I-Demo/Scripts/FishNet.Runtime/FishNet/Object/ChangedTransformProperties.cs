using System;
using FishNet.Documenting;

namespace FishNet.Object
{
	[Flags]
	[APIExclude]
	internal enum ChangedTransformProperties : byte
	{
		Unset = 0,
		LocalPosition = 1,
		LocalRotation = 2,
		LocalScale = 4
	}
}
