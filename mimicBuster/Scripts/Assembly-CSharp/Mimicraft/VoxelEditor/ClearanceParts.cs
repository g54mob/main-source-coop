using System;

namespace Mimicraft.VoxelEditor
{
	[Flags]
	public enum ClearanceParts
	{
		Overlap = 1,
		Depth = 2,
		Containment = 4,
		Outside = 8,
		All = 0xF
	}
}
