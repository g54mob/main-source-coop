using System;
using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;

namespace Mimicraft.Customization
{
	[Serializable]
	public class CharacterPartJson
	{
		public string partId;

		public List<VoxelEntryData> voxels = new List<VoxelEntryData>();

		public List<FaceEntryData> faces;
	}
}
