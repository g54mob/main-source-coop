using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public readonly struct VoxelBodyPiece
	{
		public readonly bool HasVoxels;

		public readonly Vector3Int Min;

		public readonly Vector3Int Max;

		public readonly Matrix4x4 BodyFromPiece;

		public readonly int WeakIslands;

		public VoxelBodyPiece(Vector3Int min, Vector3Int max, Matrix4x4 bodyFromPiece, int weakIslands = 0)
		{
			HasVoxels = true;
			Min = min;
			Max = max;
			BodyFromPiece = bodyFromPiece;
			WeakIslands = weakIslands;
		}

		public static VoxelBodyPiece FromGrid(VoxelGrid grid, Matrix4x4 bodyFromPiece)
		{
			if (grid == null || !GridBounds.TryCompute(grid, out var min, out var max))
			{
				return default(VoxelBodyPiece);
			}
			int weakIslands = (VoxelEditorSettings.BodyRulesApply ? VoxelIslands.CountWeak(grid) : 0);
			return new VoxelBodyPiece(min, max, bodyFromPiece, weakIslands);
		}
	}
}
