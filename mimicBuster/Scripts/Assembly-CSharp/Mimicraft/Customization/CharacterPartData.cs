using Mimicraft.VoxelEditor.Core;

namespace Mimicraft.Customization
{
	public class CharacterPartData
	{
		public string PartId;

		public VoxelGrid Grid;

		public CharacterPartData(string partId, VoxelGrid grid)
		{
			PartId = partId;
			Grid = grid;
		}
	}
}
