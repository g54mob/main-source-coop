using System.Collections.Generic;

namespace Mimicraft.Networking
{
	public readonly struct VoxelBodySummary
	{
		public readonly float VoxelSize;

		public readonly int TotalVoxels;

		public readonly IReadOnlyList<VoxelPieceSummary> Pieces;

		public VoxelBodySummary(float voxelSize, int totalVoxels, IReadOnlyList<VoxelPieceSummary> pieces)
		{
			VoxelSize = voxelSize;
			TotalVoxels = totalVoxels;
			Pieces = pieces;
		}
	}
}
