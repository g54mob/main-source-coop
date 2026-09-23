using System;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	[Serializable]
	public struct VoxelData
	{
		public Color32 Color;

		public VoxelData(Color32 color)
		{
			Color = color;
		}
	}
}
