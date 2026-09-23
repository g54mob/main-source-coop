using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public class ExtrudeCreateCommand : IUndoableCommand
	{
		private readonly VoxelModel sourceModel;

		private readonly GameObject splitPiece;

		private readonly Dictionary<Vector3Int, VoxelData> createdVoxels;

		private readonly Dictionary<Vector3Int, VoxelData?> beforeState;

		private readonly Dictionary<(Vector3Int Position, int Face), Color32?> beforeFaces;

		public string Description { get; }

		public int RetainedCells => (createdVoxels?.Count ?? 0) + (beforeState?.Count ?? 0) + (beforeFaces?.Count ?? 0);

		public ExtrudeCreateCommand(VoxelModel sourceModel, GameObject splitPiece, Dictionary<Vector3Int, VoxelData> createdVoxels, Dictionary<Vector3Int, VoxelData?> beforeState, string description, Dictionary<(Vector3Int Position, int Face), Color32?> beforeFaces = null)
		{
			this.sourceModel = sourceModel;
			this.splitPiece = splitPiece;
			this.createdVoxels = createdVoxels;
			this.beforeState = beforeState;
			this.beforeFaces = beforeFaces;
			Description = description;
		}

		public void Undo()
		{
			foreach (Vector3Int key in createdVoxels.Keys)
			{
				if (beforeState != null && beforeState.TryGetValue(key, out var value) && value.HasValue)
				{
					sourceModel.Grid.Set(key, value.Value);
				}
				else
				{
					sourceModel.Grid.Remove(key);
				}
			}
			if (beforeFaces != null)
			{
				foreach (KeyValuePair<(Vector3Int, int), Color32?> beforeFace in beforeFaces)
				{
					if (beforeFace.Value.HasValue)
					{
						sourceModel.Grid.SetFaceColor(beforeFace.Key.Item1, beforeFace.Key.Item2, beforeFace.Value.Value);
					}
					else
					{
						sourceModel.Grid.ClearFaceColor(beforeFace.Key.Item1, beforeFace.Key.Item2);
					}
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
			foreach (Vector3Int key in createdVoxels.Keys)
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
