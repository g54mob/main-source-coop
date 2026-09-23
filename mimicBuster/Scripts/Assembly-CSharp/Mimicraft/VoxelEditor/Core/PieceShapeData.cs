using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public sealed class PieceShapeData
	{
		public readonly List<VoxelBox> Boxes = new List<VoxelBox>();

		public Vector3Int Min;

		public Vector3Int Max;

		public int Volume;

		public int LargestBox;

		public bool HasVoxels => Boxes.Count > 0;

		internal void Clear()
		{
			Boxes.Clear();
			Min = (Max = default(Vector3Int));
			Volume = 0;
			LargestBox = -1;
		}

		internal void Finish()
		{
			Volume = 0;
			LargestBox = -1;
			int num = -1;
			for (int i = 0; i < Boxes.Count; i++)
			{
				int volume = Boxes[i].Volume;
				Volume += volume;
				if (volume > num)
				{
					num = volume;
					LargestBox = i;
				}
			}
		}
	}
}
