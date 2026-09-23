using System;
using System.Collections.Generic;

namespace Mimicraft.Customization
{
	[Serializable]
	public class CharacterJson
	{
		public string characterName;

		public string rigId;

		public List<CharacterPartJson> parts = new List<CharacterPartJson>();
	}
}
