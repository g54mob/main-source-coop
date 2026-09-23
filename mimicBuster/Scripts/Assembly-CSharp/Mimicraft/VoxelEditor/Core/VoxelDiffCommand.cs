using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public class VoxelDiffCommand : IUndoableCommand
	{
		private readonly VoxelModel model;

		private readonly Dictionary<Vector3Int, VoxelData?> before;

		private readonly Dictionary<Vector3Int, VoxelData?> after;

		private readonly Dictionary<(Vector3Int Position, int Face), Color32?> facesBefore;

		private readonly Dictionary<(Vector3Int Position, int Face), Color32?> facesAfter;

		public string Description { get; }

		public int RetainedCells => before.Count + after.Count + (facesBefore?.Count ?? 0) + (facesAfter?.Count ?? 0);

		public VoxelDiffCommand(VoxelModel model, Dictionary<Vector3Int, VoxelData?> before, Dictionary<Vector3Int, VoxelData?> after, string description = null, Dictionary<(Vector3Int Position, int Face), Color32?> facesBefore = null, Dictionary<(Vector3Int Position, int Face), Color32?> facesAfter = null)
		{
			this.model = model;
			this.before = before;
			this.after = after;
			this.facesBefore = facesBefore;
			this.facesAfter = facesAfter;
			Description = description;
		}

		public void Undo()
		{
			Apply(before, facesBefore);
		}

		public void Redo()
		{
			Apply(after, facesAfter);
		}

		private void Apply(Dictionary<Vector3Int, VoxelData?> state, Dictionary<(Vector3Int Position, int Face), Color32?> faceState)
		{
			foreach (KeyValuePair<Vector3Int, VoxelData?> item in state)
			{
				if (item.Value.HasValue)
				{
					model.Grid.Set(item.Key, item.Value.Value);
				}
				else
				{
					model.Grid.Remove(item.Key);
				}
			}
			if (faceState != null)
			{
				foreach (KeyValuePair<(Vector3Int, int), Color32?> item2 in faceState)
				{
					if (item2.Value.HasValue)
					{
						model.Grid.SetFaceColor(item2.Key.Item1, item2.Key.Item2, item2.Value.Value);
					}
					else
					{
						model.Grid.ClearFaceColor(item2.Key.Item1, item2.Key.Item2);
					}
				}
			}
			model.RebuildMesh();
		}
	}
}
