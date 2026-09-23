using System;
using System.Collections.Generic;

namespace Mimicraft.VoxelEditor.Core
{
	[Serializable]
	public class TemplatePieceData
	{
		public string name;

		public float px;

		public float py;

		public float pz;

		public float rx;

		public float ry;

		public float rz;

		public float rw = 1f;

		public List<VoxelEntryData> voxels;

		public List<FaceEntryData> faces;
	}
}
