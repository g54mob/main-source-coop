using System;
using Fusion;

namespace Features.LevelObjectSpawnModule.Scripts
{
	[Serializable]
	public class LevelObjectData
	{
		public NetworkBehaviour Item;

		public LevelObjectType Type;

		public int ItemCount;

		public bool UseSpread;

		public bool RandomizeCost = true;
	}
}
