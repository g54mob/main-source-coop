using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Customization
{
	public class WeaponEditBounds : IVoxelEditBounds
	{
		private readonly PartEditBounds box;

		private readonly VoxelModel model;

		public int MinimumVoxels { get; }

		public string CategoryId => box.CategoryId;

		public bool IsSatisfied
		{
			get
			{
				if (!(model == null) && model.Grid != null)
				{
					return model.Grid.Count >= MinimumVoxels;
				}
				return true;
			}
		}

		public WeaponEditBounds(IVoxelBox box, VoxelModel model, int minimumVoxels)
		{
			this.box = new PartEditBounds(box);
			this.model = model;
			MinimumVoxels = Mathf.Max(0, minimumVoxels);
		}

		public bool CanAdd(Vector3Int cell)
		{
			return box.CanAdd(cell);
		}

		public bool CanRemove(Vector3Int cell)
		{
			if (box.CanRemove(cell))
			{
				if (!(model == null) && model.Grid != null)
				{
					return model.Grid.Count > MinimumVoxels;
				}
				return true;
			}
			return false;
		}
	}
}
