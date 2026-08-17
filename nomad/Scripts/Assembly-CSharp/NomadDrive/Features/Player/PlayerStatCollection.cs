using System;
using System.Collections.Generic;

namespace NomadDrive.Features.Player
{
	[Serializable]
	public class PlayerStatCollection
	{
		public Dictionary<PlayerStatType, float> Stats = new Dictionary<PlayerStatType, float>();
	}
}
