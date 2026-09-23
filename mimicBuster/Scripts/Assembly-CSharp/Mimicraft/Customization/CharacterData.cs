using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;

namespace Mimicraft.Customization
{
	public class CharacterData
	{
		public string RigId;

		public readonly List<CharacterPartData> Parts = new List<CharacterPartData>();

		public CharacterVoice VoiceType;

		public CharacterData(string rigId)
		{
			RigId = rigId;
		}

		public CharacterPartData Find(string partId)
		{
			foreach (CharacterPartData part in Parts)
			{
				if (part != null && part.PartId == partId)
				{
					return part;
				}
			}
			return null;
		}

		public void Set(string partId, VoxelGrid grid)
		{
			CharacterPartData characterPartData = Find(partId);
			if (characterPartData != null)
			{
				characterPartData.Grid = grid;
			}
			else
			{
				Parts.Add(new CharacterPartData(partId, grid));
			}
		}
	}
}
