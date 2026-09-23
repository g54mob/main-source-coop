using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Networking
{
	public readonly struct VoxelPieceSummary
	{
		public readonly Vector3 LocalPosition;

		public readonly Quaternion LocalRotation;

		public readonly bool HasVoxels;

		public readonly Vector3Int Min;

		public readonly Vector3Int Max;

		public readonly int Voxels;

		public readonly IReadOnlyList<VoxelIsland> Islands;

		public readonly PieceShapeData Shape;

		public VoxelPieceSummary(Vector3 localPosition, Quaternion localRotation, bool hasVoxels, Vector3Int min, Vector3Int max, int voxels, IReadOnlyList<VoxelIsland> islands = null, PieceShapeData shape = null)
		{
			LocalPosition = localPosition;
			LocalRotation = localRotation;
			HasVoxels = hasVoxels;
			Min = min;
			Max = max;
			Voxels = voxels;
			Islands = islands;
			Shape = shape;
		}
	}
}
