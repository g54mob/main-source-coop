using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public readonly struct TemplatePiece
	{
		public readonly string Name;

		public readonly VoxelGrid Grid;

		public readonly Vector3 LocalPosition;

		public readonly Quaternion LocalRotation;

		public TemplatePiece(string name, VoxelGrid grid)
			: this(name, grid, Vector3.zero, Quaternion.identity)
		{
		}

		public TemplatePiece(string name, VoxelGrid grid, Vector3 localPosition, Quaternion localRotation)
		{
			Name = name;
			Grid = grid;
			LocalPosition = localPosition;
			LocalRotation = localRotation;
		}
	}
}
