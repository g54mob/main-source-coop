using UnityEngine;

namespace Mimicraft.Customization
{
	public interface IVoxelBox
	{
		string DisplayName { get; }

		Vector3Int BoxSize { get; }

		Vector3Int LimitMin { get; }

		Vector3Int LimitSize { get; }

		float VoxelSize { get; }

		bool HasRequiredCore { get; }

		bool IsInsideBox(Vector3Int cell);

		bool IsRequired(Vector3Int cell);
	}
}
