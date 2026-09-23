namespace Mimicraft.VoxelEditor.Core
{
	public struct VoxelBox
	{
		public int MinX;

		public int MinY;

		public int MinZ;

		public int SizeX;

		public int SizeY;

		public int SizeZ;

		public int Volume => SizeX * SizeY * SizeZ;
	}
}
