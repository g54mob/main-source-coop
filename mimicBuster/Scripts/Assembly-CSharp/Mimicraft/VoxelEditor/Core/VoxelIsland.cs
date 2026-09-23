using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public struct VoxelIsland
	{
		public int MinX;

		public int MinY;

		public int MinZ;

		public int MaxX;

		public int MaxY;

		public int MaxZ;

		public int Voxels;

		public int ExtentX => MaxX - MinX + 1;

		public int ExtentY => MaxY - MinY + 1;

		public int ExtentZ => MaxZ - MinZ + 1;

		public bool IsSubstantial(int minExtent, int slimExtent, int minVoxels)
		{
			if (Voxels >= minVoxels)
			{
				return VoxelBodyRules.FitsMinimum(new Vector3Int(ExtentX, ExtentY, ExtentZ), minExtent, slimExtent);
			}
			return false;
		}
	}
}
