using System.Collections.Generic;

namespace DunGen.LockAndKey
{
	public sealed class KeySpawnParameters
	{
		public readonly List<IKeyLock> OutputSpawnedKeys = new List<IKeyLock>();

		public Key Key { get; }

		public KeyManager KeyManager { get; }

		public DungeonGenerator DungeonGenerator { get; }

		public KeySpawnParameters(Key key, KeyManager keyManager, DungeonGenerator dungeonGenerator)
		{
			Key = key;
			KeyManager = keyManager;
			DungeonGenerator = dungeonGenerator;
		}
	}
}
