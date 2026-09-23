using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public interface IVoxelEditBounds
	{
		string CategoryId { get; }

		bool CanAdd(Vector3Int cell);

		bool CanRemove(Vector3Int cell);
	}
}
