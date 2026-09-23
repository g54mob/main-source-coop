using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public readonly struct BevelCorner
	{
		public readonly Vector3Int Cell;

		public readonly Vector3Int Signs;

		public readonly BevelEdge EdgeX;

		public readonly BevelEdge EdgeY;

		public readonly BevelEdge EdgeZ;

		public Vector3 Point => new Vector3((Signs.x > 0) ? ((float)Cell.x + 1f) : ((float)Cell.x), (Signs.y > 0) ? ((float)Cell.y + 1f) : ((float)Cell.y), (Signs.z > 0) ? ((float)Cell.z + 1f) : ((float)Cell.z));

		public BevelCorner(Vector3Int cell, Vector3Int signs, BevelEdge edgeX, BevelEdge edgeY, BevelEdge edgeZ)
		{
			Cell = cell;
			Signs = signs;
			EdgeX = edgeX;
			EdgeY = edgeY;
			EdgeZ = edgeZ;
		}

		public BevelEdge Edge(int axis)
		{
			return axis switch
			{
				1 => EdgeY, 
				0 => EdgeX, 
				_ => EdgeZ, 
			};
		}
	}
}
