using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Customization
{
	public class PartEditBounds : IVoxelEditBounds
	{
		private readonly IVoxelBox part;

		public string CategoryId { get; }

		public PartEditBounds(IVoxelBox part)
			: this(part, null)
		{
		}

		public PartEditBounds(IVoxelBox part, string categoryId)
		{
			this.part = part;
			CategoryId = categoryId;
		}

		public bool CanAdd(Vector3Int cell)
		{
			if (part != null)
			{
				return part.IsInsideBox(cell);
			}
			return false;
		}

		public bool CanRemove(Vector3Int cell)
		{
			if (part != null)
			{
				return !part.IsRequired(cell);
			}
			return true;
		}
	}
}
