using System;
using System.Collections.Generic;

namespace Mimicraft.VoxelEditor.Core
{
	[Serializable]
	public class VoxelTemplateData
	{
		public string modelName;

		public string category;

		public string tag;

		public List<VoxelEntryData> voxels;

		public List<FaceEntryData> faces;

		public List<TemplatePieceData> pieces;
	}
}
