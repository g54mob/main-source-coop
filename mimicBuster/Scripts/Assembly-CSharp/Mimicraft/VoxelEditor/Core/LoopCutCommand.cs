using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public class LoopCutCommand : IUndoableCommand
	{
		private readonly VoxelModel sourceModel;

		private readonly GameObject splitPiece;

		private readonly Dictionary<Vector3Int, VoxelData> movedVoxels;

		public string Description { get; }

		public int RetainedCells => movedVoxels?.Count ?? 0;

		public LoopCutCommand(VoxelModel sourceModel, GameObject splitPiece, Dictionary<Vector3Int, VoxelData> movedVoxels, string description)
		{
			this.sourceModel = sourceModel;
			this.splitPiece = splitPiece;
			this.movedVoxels = movedVoxels;
			Description = description;
		}

		public void Undo()
		{
			foreach (KeyValuePair<Vector3Int, VoxelData> movedVoxel in movedVoxels)
			{
				sourceModel.Grid.Set(movedVoxel.Key, movedVoxel.Value);
			}
			VoxelModel voxelModel = ((splitPiece != null) ? splitPiece.GetComponent<VoxelModel>() : null);
			if (voxelModel != null)
			{
				foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in voxelModel.Grid.FaceColors)
				{
					sourceModel.Grid.SetFaceColor(faceColor.Key.Item1, faceColor.Key.Item2, faceColor.Value);
				}
			}
			sourceModel.RebuildMesh();
			if (splitPiece != null)
			{
				splitPiece.SetActive(value: false);
			}
		}

		public void Redo()
		{
			foreach (Vector3Int key in movedVoxels.Keys)
			{
				sourceModel.Grid.Remove(key);
			}
			sourceModel.RebuildMesh();
			if (splitPiece != null)
			{
				splitPiece.SetActive(value: true);
			}
		}
	}
}
