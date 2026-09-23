namespace Mimicraft.Export
{
	public interface IVoxelSource
	{
		bool TryGetBounds(out int minX, out int minY, out int minZ, out int maxX, out int maxY, out int maxZ);

		bool IsSolid(int x, int y, int z);

		uint FaceColor(int x, int y, int z, int face);
	}
}
